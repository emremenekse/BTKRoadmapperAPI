namespace BTKRoadmapperAPI.Services
{
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class HttpService
    {
        private readonly HttpClient _httpClient;

        public HttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<TResponse?> SendRequestAsync<TResponse>(
    HttpMethod method, string url)
        {
            return await SendRequestAsync<object, TResponse>(method, url, null);
        }

        public async Task<TResponse?> SendRequestAsync<TRequest, TResponse>(
            HttpMethod method, string url, TRequest? body)
        {
            using var request = new HttpRequestMessage(method, url);

            if (body != null)
            {
                var jsonBody = JsonSerializer.Serialize(body);
                request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            }

            using var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TResponse>(responseContent);
        }

    }

}
