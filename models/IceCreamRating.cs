using Newtonsoft.Json;

namespace oh5.serverless
{
    public class IceCreamRating
    {
        [JsonProperty(propertyName: "id")]
        public string Id {get;set;}

        [JsonProperty(propertyName: "userId")]
        public string UserId {get;set;}

        [JsonProperty(propertyName: "productId")]
        public string ProductId {get;set;}

        [JsonProperty(propertyName: "timestamp")]
        public string Timestamp {get;set;}

        [JsonProperty(propertyName: "locationName")]
        public string LocationName {get;set;}

        [JsonProperty(propertyName: "rating")]
        public int Rating {get;set;}

        [JsonProperty(propertyName: "productId")]
        public string UserNotes {get;set;}
    }
}
