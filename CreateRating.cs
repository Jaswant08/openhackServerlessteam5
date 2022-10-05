using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace oh5.serverless
{
    public static class CreateRating
    {
        [FunctionName("CreateRating")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("CreateRating triggered");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            IceCreamRating rating = JsonConvert.DeserializeObject<IceCreamRating>(requestBody);

            // Validate the productId by calling the existing API endpoint
            Product product = await ProductController.Instance.GetProductAsync(rating.ProductId);
            if (product == null)
            {
                string msg = $"Product with id '{rating.ProductId}' not found.";
                log.LogError(msg);
                return new NotFoundObjectResult(msg);
            }

            // Validate the userId by calling the existing API endpoint
            User user = await UserController.Instance.GetUserAsync(rating.UserId);
            if (user == null)
            {
                string msg = $"User with id '{rating.UserId}' not found.";
                log.LogError(msg);
                return new NotFoundObjectResult(msg);
            }

            // Validate that the rating is an integer from 0 to 5            
            if (!int.TryParse(rating.Rating, out int r) || r < 0 || r > 5)
            {
                string msg = $"Rating '{rating.Rating}' is not valid. Must be a number betwen 0 and 5.";
                log.LogError(msg);
                return new BadRequestObjectResult(msg);
            }

            // Add a property called id with a GUID value
            rating.Id = Guid.NewGuid().ToString();

            // Add a property called timestamp with the current UTC date time
            rating.Timestamp = DateTime.UtcNow.ToString(CosmosSettings.TimestampFormat);

            // Use a data service to store the ratings information to the backend
            CosmosClient cosmosClient = new CosmosClient(CosmosSettings.ConnectionString, CosmosSettings.Options);
            Database database = await cosmosClient.CreateDatabaseIfNotExistsAsync(CosmosSettings.DatabaseId);
            Container container = await database.CreateContainerIfNotExistsAsync(CosmosSettings.ContainerId, CosmosSettings.PartitionKeyPath);
            ItemResponse<IceCreamRating> createItemResponse =
                await container.CreateItemAsync<IceCreamRating>(rating, new PartitionKey(rating.Id));
            HttpStatusCode code = createItemResponse.StatusCode;
            if (code != HttpStatusCode.Created)
            {
                string msg = $"Received code {code} ({(int)code}) while creating item in database";
                log.LogError(msg);
                return new ConflictObjectResult(msg);
            }

            // Return the entire review JSON payload with the newly created id and timestamp            
            return new OkObjectResult(rating);
        }
    }
}
