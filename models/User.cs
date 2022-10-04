using Newtonsoft.Json;

namespace oh5.serverless
{
    public class User
    {
        [JsonProperty(propertyName: "userId")]
        public string Id { get; set; }

        [JsonProperty(propertyName: "userName")]
        public string Name { get; set; }

        [JsonProperty(propertyName: "fullName")]
        public string FullName { get; set; }
    }
}