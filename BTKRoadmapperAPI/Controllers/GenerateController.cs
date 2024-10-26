using BTKRoadmapperAPI.DTOs;
using BTKRoadmapperAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BTKRoadmapperAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GenerateController : CustomBaseController
    {
        private readonly GeminiService _geminiService;

        public GenerateController(GeminiService geminiService)
        {
            _geminiService = geminiService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<List<LLMResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GenerateRoadmap()
        {
            var roadmapData = await _geminiService.SendPromptAsync();
            return CreateActionResultInstance(roadmapData);
        }
    }
}
