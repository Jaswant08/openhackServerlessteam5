using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos;
using System.Collections.Generic;

namespace oh5.serverless
{
    public static class GetRatings
    {
        [FunctionName("GetRatings")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, ILogger log)
        {
            string userId = req.Query["userId"];

            if (string.IsNullOrWhiteSpace(userId))
            {
                return new BadRequestObjectResult("User id cannot be empty.v2");
            }

            User user = await UserController.Instance.GetUserAsync(userId);

            if (user == null)
            {
                string msg = $"User with id '{userId}' not found.";

                log.LogError(msg);

                return new NotFoundObjectResult(msg);
            }

            // Use a data service to store the ratings information to the backend
            CosmosClient cosmosClient = new CosmosClient(CosmosSettings.ConnectionString, CosmosSettings.Options);
            Database database = await cosmosClient.CreateDatabaseIfNotExistsAsync(CosmosSettings.DatabaseId);
            Container container = await database.CreateContainerIfNotExistsAsync(CosmosSettings.ContainerId, CosmosSettings.PartitionKeyPath);
            QueryDefinition queryDefinition = new QueryDefinition("select * from c where c.userId = @userId").WithParameter("@userId", userId);

            List<IceCreamRating> result = new();

            using (FeedIterator<IceCreamRating> iterator = container.GetItemQueryIterator<IceCreamRating>(queryDefinition))
            {
                while (iterator.HasMoreResults)
                {
                    var ratings = await iterator.ReadNextAsync();

                    result.AddRange(ratings);
                }
            }

            return new OkObjectResult(result);
        }
    }
}
