using BTKRoadmapperAPI.Abstractions;
using BTKRoadmapperAPI.Entities;
using Microsoft.Extensions.Caching.Distributed;

namespace BTKRoadmapperAPI.Services
{
    public class CourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IDistributedCache _redisCache;
        private const string CacheKey = "courses_with_modules";

        public CourseService(ICourseRepository courseRepository, IDistributedCache redisCache)
        {
            _courseRepository = courseRepository;
            _redisCache = redisCache;
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
    }
}
