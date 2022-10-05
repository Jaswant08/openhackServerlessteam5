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

            if (ratingId == null || ratingId == String.Empty)
            {
                return new NotFoundResult();
            }

            var rating = new IceCreamRating()
            {
                Id = ratingId,
                ProductId = "product 123",
                Rating = "4",
                Timestamp = DateTime.Now.ToLongDateString(),
                LocationName = "DC",
                UserId = "system",
                UserNotes = "It's awesome"
            };

            return new OkObjectResult(rating);
        }
    }
}
