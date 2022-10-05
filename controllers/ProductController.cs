using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace oh5.serverless
{
    public class ProductController
    {
        private static readonly Lazy<ProductController> lazy =
            new Lazy<ProductController>(() => new ProductController());

        public static ProductController Instance { get { return lazy.Value; } }

        private ProductController()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private readonly HttpClient client;
        private const string uri = "https://serverlessohapi.azurewebsites.net/api/GetProduct";

        public async Task<Product> GetProductAsync(string productId)
        {
            string path = uri + "?productId=" + productId;
            Product product = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                product = await response.Content.ReadAsAsync<Product>();
            }
            return product;
        }
    }
}