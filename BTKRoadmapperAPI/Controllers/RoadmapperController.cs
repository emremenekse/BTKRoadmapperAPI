using BTKRoadmapperAPI.DTOs;
using BTKRoadmapperAPI.Entities;
using BTKRoadmapperAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BTKRoadmapperAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RoadmapperController : CustomBaseController
    {
        private readonly GeminiService _geminiService;
        private readonly CourseService _courseService;

        public RoadmapperController(GeminiService geminiService, CourseService courseService)
        {
            _geminiService = geminiService;
            _courseService = courseService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<List<LLMResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GenerateRoadmap()
        {
            var roadmapData = await _geminiService.SendPromptAsync();
            return CreateActionResultInstance(roadmapData);
        }

        [HttpPost]
        public async Task<IActionResult> AddCourse([FromBody] CourseDTO courseDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var result = await _courseService.AddNewCourse(courseDTO);
            return CreateActionResultInstance(result);
        }
    }
}
