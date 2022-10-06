using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using oh5.serverless;
using OpenHackTeam5.models;

namespace OpenHackTeam5
{
    public static class CreateOrders
    {
        [FunctionName("CreateOrders")]
        public static async Task Run([EventHubTrigger("poseh100622", Connection = "EHCONNSTR")] EventData[] events, ILogger log)
        {
            /*
            CosmosClientOptions options = new CosmosClientOptions { SerializerOptions = new CosmosSerializationOptions { PropertyNamingPolicy = CosmosPropertyNamingPolicy.Default } };

            CosmosClient cosmosClient = new CosmosClient(CosmosSettings.ConnectionString, options);
            Database database = await cosmosClient.CreateDatabaseIfNotExistsAsync(CosmosSettings.DatabaseId);
            Container container = await database.CreateContainerIfNotExistsAsync(CosmosSettings.EhOrdersContainerId, CosmosSettings.PartitionKeyPath);

            foreach (var e in events)
            {
                var json = Encoding.UTF8.GetString(e.EventBody);
                log.LogInformation(json);

                var order = JsonSerializer.Deserialize<Order>(json);
                order.id = Guid.NewGuid().ToString();

                ItemResponse<Order> createItemResponse = await container.CreateItemAsync(order, new PartitionKey(order.id));
                HttpStatusCode code = createItemResponse.StatusCode;

            }
            */

            CosmosClientOptions options = new CosmosClientOptions { SerializerOptions = new CosmosSerializationOptions { PropertyNamingPolicy = CosmosPropertyNamingPolicy.Default } };
            CosmosClient cosmosClient = new CosmosClient(CosmosSettings.ConnectionString, options);
            Database database = await cosmosClient.CreateDatabaseIfNotExistsAsync(CosmosSettings.DatabaseId);
            Container container = await database.CreateContainerIfNotExistsAsync(CosmosSettings.EhOrdersContainerId, CosmosSettings.PartitionKeyPath);

            string connectionString = "Endpoint=sb://posreceipts.servicebus.windows.net/;SharedAccessKeyName=SendPermission;SharedAccessKey=AXhaDH5xEGvv9ItdF0uMcY3svWtSS99x8Fd9nvna/EU=;EntityPath=receiptstopic";
            string topicName = "receiptstopic";

            await using var client = new ServiceBusClient(connectionString);

            ServiceBusSender sender = client.CreateSender(topicName);

            foreach (var e in events)
            {
                var json = Encoding.UTF8.GetString(e.EventBody);

                log.LogInformation(json);

                var order = JsonSerializer.Deserialize<Order>(json);

                order.id = Guid.NewGuid().ToString();

                ItemResponse<Order> createItemResponse = await container.CreateItemAsync(order, new PartitionKey(order.id));

                var header = order.header;

                if (!string.IsNullOrWhiteSpace(header.receiptUrl))
                {
                    var receipt = new Receipt
                    {
                        receiptUrl = header.receiptUrl,
                        salesDate = header.dateTime,
                        salesNumber = header.salesNumber,
                        storeLocation = header.locationId,
                        totalCost = Convert.ToDecimal(header.totalCost),
                        totalItems = order.details.Sum(o => Convert.ToInt32(o.quantity)),
                    };

                    var body = JsonSerializer.Serialize(receipt);
                    
                    ServiceBusMessage message = new ServiceBusMessage(body);
                    
                    await sender.SendMessageAsync(message);
                }
            }
        }
    }
}
