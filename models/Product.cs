using Newtonsoft.Json;

namespace oh5.serverless
{
    public class Product
    {
        [JsonProperty(propertyName: "productId")]
        public string Id { get; set; }

        [JsonProperty(propertyName: "productName")]
        public string Name { get; set; }

        [JsonProperty(propertyName: "productDescription")]
        public string Description { get; set; }
    }
}