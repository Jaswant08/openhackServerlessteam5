using Newtonsoft.Json;

namespace oh5.serverless
{
    public class User
    {
        [JsonProperty("userId")]
        public string Id { get; set; }

        [JsonProperty("userName")]
        public string Name { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }
    }
}