using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using BTKRoadmapperAPI.DTOs;
using k8s.Models;
using System.Text.Json.Nodes;

namespace BTKRoadmapperAPI.Services
{
    public class GeminiService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpService _httpService;
        private readonly CourseService _courseService;

        public GeminiService(IConfiguration configuration, HttpService httpService, CourseService courseService)
        {
            _configuration = configuration;
            _httpService = httpService;
            _courseService = courseService;
        }

        public async Task<Response<List<LLMResponse>>> SendPromptAsync(RoadmapDTO roadmapDTO)
        {
            var apiKey = _configuration["GeminiAPI:ApiKey"];
            var baseUrl = _configuration["GeminiAPI:BaseUrl"];
            string userLevel = roadmapDTO.InterestedFieldSkillLevel.ToString();
            string personalInterest = roadmapDTO.InterestedFields;
            string educationLevel = roadmapDTO.EducationLevel.ToString();
            string targetFields = roadmapDTO.TargetField.ToString();

            var url = $"{baseUrl}?key={apiKey}";
            var courses = await _courseService.GetAllCoursesWithModulesAsync();
            var coursesJson = JsonConvert.SerializeObject(courses.Select(course => new
            {
                CourseId = course.Id,
                CourseName = course.CourseName,
            }));

            string prompt = $@"
Below is a list of available courses. Based on the user's interests, please select and return only the most relevant courses from this list. 
Only include courses from the list provided and only return each course's IDs  in a JSON array.

User’s interests are as follows:
- Education Level: {educationLevel}
- User Level: {userLevel}
- Personal Interest: {personalInterest}
- Target Fields: {targetFields}

Course List:
{coursesJson}

Expected Response Format:
[
    {{ ""CourseId"": 1 }},
    {{ ""CourseId"": 2 }}
]

Instructions:
- Do not return an empty response. If you cannot find relevant courses, still include a couple of example courses as placeholders.
- Only use the provided courses and format your response exactly as shown above.
- Do not include courses that are very similar to each other in the response. Instead, select only one from similar courses.
";

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



            var response = await _httpService.SendRequestAsync<object, ResponseModel>  (
            HttpMethod.Post, url, requestBody);

            var courseIds = new List<int>();
            if (response?.Candidates != null)
            {
                foreach (var candidate in response.Candidates)
                {
                    foreach (var part in candidate.Content.Parts)
                    {
                        var courseArray = JsonNode.Parse(part.Text).AsArray();
                        foreach (var item in courseArray)
                        {
                            if (item["CourseId"] != null)
                            {
                                courseIds.Add((int)item["CourseId"]);
                            }
                        }
                    }
                }
            }


            if (courseIds != null)
            {
                var returnedCourses = await _courseService.GetCoursesWithModulesByIdsAsync(courseIds);
                var courseDetails = returnedCourses.Select(course => new
                {
                    CourseId = course.Id,
                    CourseName = course.CourseName,
                    ModuleTitles = course.Modules.Select(module => module.Title).ToList()
                }).ToList();

                var coursesJsonSecond = JsonConvert.SerializeObject(courseDetails, Formatting.Indented);

                string promptSecond = $@"
Below is a list of available courses with their modules. Based on the user's interests, please evaluate each course to see if it aligns with the user's preferences.
If a course is not suitable based on its modules, exclude it from the final list. For the remaining courses, organize them in the recommended order for the user to start learning.

User’s interests are as follows:
- Education Level: {educationLevel}
- User Level: {userLevel}
- Personal Interest: {personalInterest}
- Target Fields: {targetFields}

Course List:
{coursesJsonSecond}

Expected Response Format:
[
    {{ ""CourseId"": 1, ""RecommendedOrder"": 1 }},
    {{ ""CourseId"": 2, ""RecommendedOrder"": 2 }}
]

Instructions:
- Carefully analyze each course based on its modules to determine if it fits the user's interests.
- Exclude courses that are not suitable for the user's preferences based on the module content.
- For the remaining courses, provide a recommended learning order in ascending order of priority.
- Ensure that the response is strictly in the specified format and that only relevant courses are included.
";

                var requestBodySecond = new
                {
                    contents = new[]
                {
                new
                {
                    parts = new[]
                    {
                        new { text = promptSecond }
                    }
                }
                }
                };



                var responseSecond = await _httpService.SendRequestAsync<object, ResponseModel>(
                HttpMethod.Post, url, requestBodySecond);

                var courseOrders = new List<CourseOrder>();
                if (responseSecond?.Candidates != null)
                {
                    foreach (var candidate in responseSecond.Candidates)
                    {
                        foreach (var part in candidate.Content.Parts)
                        {
                            var cleanedJson = part.Text
    .Replace("```json", "")
    .Replace("```", "")
    .Trim();
                            var courseArray = JsonNode.Parse(cleanedJson).AsArray();
                            foreach (var item in courseArray)
                            {
                                if (item["CourseId"] != null && item["RecommendedOrder"] != null)
                                {
                                    var courseOrder = new CourseOrder
                                    {
                                        CourseId = (int)item["CourseId"],
                                        RecommendedOrder = (int)item["RecommendedOrder"]
                                    };
                                    courseOrders.Add(courseOrder);
                                }
                            }
                        }
                    }
                }

            }

            return null;
        }
    }
}
