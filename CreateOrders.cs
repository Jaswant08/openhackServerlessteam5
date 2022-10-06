using System;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
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
            CosmosClient cosmosClient = new CosmosClient(CosmosSettings.ConnectionString, CosmosSettings.Options);
            Database database = await cosmosClient.CreateDatabaseIfNotExistsAsync(CosmosSettings.DatabaseId);
            Container container = await database.CreateContainerIfNotExistsAsync(CosmosSettings.EhOrdersContainerId, CosmosSettings.PartitionKeyPath);

            foreach(var e in events)
            {
                var json = Encoding.UTF8.GetString(e.EventBody);
                log.LogInformation(json);

                var order = JsonSerializer.Deserialize<Order>(json);
                order.Id = Guid.NewGuid().ToString();

                ItemResponse<Order> createItemResponse = await container.CreateItemAsync(order, new PartitionKey(order.Id));
                HttpStatusCode code = createItemResponse.StatusCode;

            }
        }
    }
}
