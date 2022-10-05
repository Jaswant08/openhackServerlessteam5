using Newtonsoft.Json;

namespace oh5.serverless
{
    public class IceCreamRating
    {
        //[JsonProperty("id")]
        public string Id { get; set; }

        //[JsonProperty("userId")]
        public string UserId { get; set; }

        //[JsonProperty("productId")]
        public string ProductId { get; set; }

        //[JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        //[JsonProperty("locationName")]
        public string LocationName { get; set; }

        //[JsonProperty("rating")]
        public string Rating { get; set; }

        //[JsonProperty("productId")]
        public string UserNotes { get; set; }
    }
}
