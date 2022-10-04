using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System.Configuration;
using System.Linq;
using Microsoft.Azure.Cosmos;

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
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            
            string userId = data?.userId;
            string productId = data?.productId;
            string locationName = data?.locationName;
            string userNotes = data?.userNotes;

            // Validate both userId and productId by calling the existing API endpoints.
            // You can find a user id to test with from the sample payload above
            var product = ProductController.Instance.GetProductAsync(productId);
            if (product == null)
                return new NotFoundObjectResult($"Product with id '{productId}' not found.");

            var user = UserController.Instance.GetUserAsync(userId);
            if (user == null)
                return new NotFoundObjectResult($"User with id '{userId}' not found.");

            // Validate that the rating field is an integer from 0 to 5
            int rating = 0;
            if (!int.TryParse(data?.rating, out rating) || rating < 0 || rating > 5)
                return new BadRequestObjectResult($"Rating is not valid. Must be a number betwen 0 and 5.");

            var iceCreamRating = new IceCreamRating
            {
                UserId = userId,
                ProductId = productId,
                LocationName = locationName,
                Rating = rating,
                UserNotes = userNotes
            };

            // Add a property called id with a GUID value
            iceCreamRating.Id = Guid.NewGuid().ToString();

            // Add a property called timestamp with the current UTC date time
            iceCreamRating.Timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ssZ");

            // Use a data service to store the ratings information to the backend
            //  - Get CosmosDB connection string
            //  - Write to CosmosDB
            var connStr = Environment.GetEnvironmentVariable("COSMOSDBCONSTR");
            
            //var cosmosClient = new CosmosClient(connStr);
            //var database = await cosmosClient.CreateDatabaseIfNotExistsAsync("IceCreamRatings");
            
            // Return the entire review JSON payload with the newly created id and timestamp            
            return new OkObjectResult(iceCreamRating);
        }
    }
}
