using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace oh5.serverless
{
    public class UserController
    {
        private static readonly Lazy<UserController> lazy =
            new Lazy<UserController>(() => new UserController());

        public static UserController Instance { get { return lazy.Value; } }

        private UserController()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private readonly HttpClient client;
        private const string uri = "https://serverlessohapi.azurewebsites.net/api/GetUser";

        public async Task<User> GetUserAsync(string productId)
        {
            string path = uri + "?userId=" + productId;
            User user = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                user = await response.Content.ReadAsAsync<User>();
            }
            return user;
        }
    }
}