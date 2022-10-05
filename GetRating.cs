using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.Azure.Cosmos;
using System.Linq;

namespace oh5.serverless
{
    public class GetRating
    {
        [FunctionName("GetRating")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            string ratingId = req.Query["ratingId"];

            log.LogInformation("Get rating received request with ratingId : {0}", ratingId);

            if (string.IsNullOrWhiteSpace(ratingId))
            {
                return new BadRequestObjectResult("Rating id cannot be empty.");
            }

            // Use a data service to store the ratings information to the backend
            CosmosClient cosmosClient = new CosmosClient(CosmosSettings.ConnectionString, CosmosSettings.Options);
            Database database = await cosmosClient.CreateDatabaseIfNotExistsAsync(CosmosSettings.DatabaseId);
            Container container = await database.CreateContainerIfNotExistsAsync(CosmosSettings.ContainerId, CosmosSettings.PartitionKeyPath);
            QueryDefinition queryDefinition = new QueryDefinition("select * from c where c.id = @ratingId").WithParameter("@ratingId", ratingId);

            IceCreamRating rating = null;
            using (FeedIterator<IceCreamRating> iterator = container.GetItemQueryIterator<IceCreamRating>(queryDefinition))
            {
                if (iterator.HasMoreResults)
                {
                    var ratings = await iterator.ReadNextAsync();

                    rating = ratings.SingleOrDefault();

                }
            }

            return rating != null ? new OkObjectResult(rating) : new BadRequestObjectResult("Rating not found");

        }
    }
}
