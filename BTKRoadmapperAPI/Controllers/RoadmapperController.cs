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
        private readonly UserService _userService;

        public RoadmapperController(GeminiService geminiService, CourseService courseService, UserService userService)
        {
            _geminiService = geminiService;
            _courseService = courseService;
            _userService = userService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<List<CourseDTO>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GenerateRoadmap([FromBody] RoadmapDTO roadmap)
        {
            var roadmapData = await _geminiService.SendPromptAsync(roadmap);
            return CreateActionResultInstance(roadmapData);
        }
        [HttpGet]
        [ProducesResponseType(typeof(UserDTO), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUserInfo([FromQuery] string mail)
        {
            var userData = await _userService.GetUserByMail(mail);
            return CreateActionResultInstance(userData);
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
