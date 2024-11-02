using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using BTKRoadmapperAPI.DTOs;
using k8s.Models;
using System.Text.Json.Nodes;
using IdentityModel.OidcClient;

namespace BTKRoadmapperAPI.Services
{
    public class GeminiService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpService _httpService;
        private readonly CourseService _courseService;
        private readonly UserService _userService;

        public GeminiService(IConfiguration configuration, HttpService httpService, CourseService courseService, UserService userService)
        {
            _configuration = configuration;
            _httpService = httpService;
            _courseService = courseService;
            _userService = userService;
        }

        public async Task<Response<List<CourseDTO>>> SendPromptAsync(RoadmapDTO roadmapDTO)
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
-Reponse format is extremely critical be careful.
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



            var response = await RetryAsync(() => _httpService.SendRequestAsync<object, ResponseModel>(
        HttpMethod.Post, url, requestBody), maxRetries: 20);

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

            var courseOrders = new List<CourseOrder>();
            List<CourseDTO> result = new();
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
- Only list suitable courses in the specified format.
- Do not include any explanations or additional details.
-Reponse format is extremely critical be careful.
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



                
                var responseSecond = await RetryAsync(() => _httpService.SendRequestAsync<object, ResponseModel>(
            HttpMethod.Post, url, requestBodySecond), maxRetries: 30);


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
                if (courseOrders != null)
                {
                    var newCourseIds = courseOrders.Select(result => result.CourseId);
                    var courseFinalListResponse = await _courseService.GetCoursesWithManyByIdsAsync(newCourseIds.ToList());
                    foreach (var item in courseFinalListResponse)
                    {
                        item.RecommendedOrder = courseOrders.Where(x => x.CourseId == item.Id).FirstOrDefault().RecommendedOrder;
                    }
                    result = courseFinalListResponse.ToList();
                }
                

            }
            if (!roadmapDTO.IsUser)
            {
                var newUser = new UserDTO()
                {
                    Email= roadmapDTO.Email,
                    Role=roadmapDTO.Role,
                    Name=roadmapDTO.Name,
                    AvailableHoursPerDaily = roadmapDTO.DailyAvailableTime,
                    EducationLevel = roadmapDTO.EducationLevel,
                    InterestedFields = roadmapDTO.InterestedFields,
                    InterestedFieldSkillLevel = roadmapDTO.InterestedFieldSkillLevel,
                    TargetField = roadmapDTO.TargetField

                };
                await _userService.AddUser(newUser);
            }
            if (roadmapDTO.HasUserInfoChange)
            {
                var newUser = new UserDTO()
                {
                    Email = roadmapDTO.Email,
                    Role = roadmapDTO.Role,
                    Name = roadmapDTO.Name,
                    AvailableHoursPerDaily=roadmapDTO.DailyAvailableTime,
                    EducationLevel=roadmapDTO.EducationLevel,
                    InterestedFields=roadmapDTO.InterestedFields,
                    InterestedFieldSkillLevel = roadmapDTO.InterestedFieldSkillLevel,
                    TargetField = roadmapDTO.TargetField

                };
                await _userService.UpdateUser(newUser);
            }

            return Response<List< CourseDTO>>.Success(result, 200);
        }

        private async Task<T> RetryAsync<T>(Func<Task<T>> action, int maxRetries)
        {
            int retries = 0;
            while (true)
            {
                try
                {
                    return await action();
                }
                catch (Exception ex)
                {
                    retries++;
                    if (retries >= maxRetries)
                    {
                        throw new Exception($"Max retries reached: {ex.Message}", ex);
                    }
                    await Task.Delay(1000); // 1 saniye bekle
                }
            }
        }
    }
}
