using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using BTKRoadmapperAPI.DTOs;

namespace BTKRoadmapperAPI.Services
{
    public class GeminiService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpService _httpService;

        public GeminiService(IConfiguration configuration, HttpService httpService)
        {
            _configuration = configuration;
            _httpService = httpService;
        }

        public async Task<Response<List<LLMResponse>>> SendPromptAsync()
        {
            var apiKey = _configuration["GeminiAPI:ApiKey"];
            var baseUrl = _configuration["GeminiAPI:BaseUrl"];
            var url = $"{baseUrl}?key={apiKey}";
            string prompt = "Please return a list of courses with their IDs and descriptions. List items has included 4 different element.";
            var requestBody = new
            {
                contents = new[]
                {
                new
                {
                    parts = new[]
                    {
                        new { text = prompt }
                    }
                }
                }
            };



            var response = await _httpService.SendRequestAsync<object, List<LLMResponse>>  (
            HttpMethod.Post, url, requestBody);

            return Response<List<LLMResponse>>.Success(response,200);

        }
    }
}
