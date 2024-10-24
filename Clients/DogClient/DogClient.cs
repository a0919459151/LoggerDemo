using LoggerDemo.Clients.Common;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace LoggerDemo.Clients.DogClient
{
    public class DogClient : HttpClientBase
    {
        public static readonly string HttpClientName = "DogClient";
        public static readonly string BaseUrl = "https://dog.ceo";

        public DogClient(IHttpClientFactory httpClientFactory)
            : base(HttpClientName, httpClientFactory)
        {
        }

        // Get Dog
        public async Task<IEnumerable<dynamic>> GetDogAsync()
        {
            var url = "api/breeds/image/random";

            return await GetAsync<IEnumerable<dynamic>>(url);
        }
    }
}
