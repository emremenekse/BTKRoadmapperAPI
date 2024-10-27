using BTKRoadmapperAPI.Abstractions;
using BTKRoadmapperAPI.DTOs;
using BTKRoadmapperAPI.Entities;
using Microsoft.Extensions.Caching.Distributed;

namespace BTKRoadmapperAPI.Services
{
    public class CourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDistributedCache _redisCache;
        private const string CacheKey = "courses_with_modules";

        public CourseService(ICourseRepository courseRepository, IDistributedCache redisCache, IUnitOfWork unitOfWork)
        {
            _courseRepository = courseRepository;
            _redisCache = redisCache;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Course>> GetAllCoursesWithModulesAsync()
        {
            var cachedData = await _redisCache.GetStringAsync(CacheKey);
            if (!string.IsNullOrEmpty(cachedData))
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<Course>>(cachedData)!;
            }

            var courses = (await _courseRepository.GetAllWithIncludesAsync(c => c.Modules)).ToList();

            var serializedData = Newtonsoft.Json.JsonConvert.SerializeObject(courses);
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7) 
            };
            await _redisCache.SetStringAsync(CacheKey, serializedData, cacheOptions);

            return courses;
        }

        public async Task<Response<bool>> AddNewCourse(CourseDTO courseDTO)
        {
            var course = new Course
            {
                CourseName = courseDTO.CourseName,
                Category = (Category)courseDTO.Category,
                TotalRequeiredTimeInSeconds = courseDTO.TotalRequeiredTimeInSeconds,
                Level = courseDTO.Level,
                Description = courseDTO.Description,
                Modules = courseDTO.Modules.Select(m => new Module
                {
                    Title = m.Title,
                    LessonCount = m.LessonCount
                }).ToList()
            };
            await _courseRepository.AddAsync(course);
            await _unitOfWork.CommitAsync();
            return Response<bool>.Success(true,201);

        }
    }
}
