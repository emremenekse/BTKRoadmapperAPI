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

        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<List<LLMResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GenerateRoadmap([FromBody] RoadmapDTO roadmap)
        {
            var roadmapData = await _geminiService.SendPromptAsync(roadmap);
            return CreateActionResultInstance(roadmapData);
        }

        [HttpPost]
        public async Task<IActionResult> AddCourse([FromBody] List<CourseDTO> courseDTOList)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _courseService.AddNewCourse(courseDTOList);
            return CreateActionResultInstance(result);
        }
    }
}
