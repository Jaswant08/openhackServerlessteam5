using Newtonsoft.Json;

namespace oh5.serverless
{
    public class Product
    {
        [JsonProperty("productId")]
        public string Id { get; set; }

        [JsonProperty("productName")]
        public string Name { get; set; }

        [JsonProperty("productDescription")]
        public string Description { get; set; }
    }
}