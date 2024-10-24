using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LoggerDemo.Clients.Common
{
    public abstract class HttpClientBase
    {
        private readonly string _httpClientName;

        private readonly IHttpClientFactory _httpClientFactory;

        protected JsonSerializerOptions JsonSerializerOptions { get; set; }

        protected HttpClientBase(
            string httpClientName,
            IHttpClientFactory httpClientFactory)
        {
            JsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            _httpClientFactory = httpClientFactory
                ?? throw new ArgumentException(nameof(_httpClientFactory));

            _httpClientName = !string.IsNullOrWhiteSpace(httpClientName)
                ? httpClientName
                : throw new ArgumentException(nameof(_httpClientName));
        }


        // Get Async
        protected async Task<T> GetAsync<T>(string url)
        {
            var client = CreateClient();

            using var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(content, JsonSerializerOptions) ?? throw new HttpRequestException("Response content is null.");
            }
            else
            {
                throw new HttpRequestException($"Response status code does not indicate success: {(int)response.StatusCode} ({response.ReasonPhrase}).");
            }
        }

        // Post Async
        protected async Task<T> PostAsync<T>(string url, object data)
        {
            var client = CreateClient();

            var jsonContent = JsonSerializer.Serialize(data);

            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(responseContent, JsonSerializerOptions) ?? throw new HttpRequestException("Response content is null.");
            }
            else
            {
                throw new HttpRequestException($"Response status code does not indicate success: {(int)response.StatusCode} ({response.ReasonPhrase}).");
            }
        }

        // Delete Async
        protected async Task DeleteAsync(string url)
        {
            var client = CreateClient();

            var response = await client.DeleteAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Response status code does not indicate success: {(int)response.StatusCode} ({response.ReasonPhrase}).");
            }
        }

        // create client
        private HttpClient CreateClient()
        {
            return _httpClientFactory.CreateClient(_httpClientName);
        }
    }
}
