using BTKRoadmapperAPI.Abstractions;
using BTKRoadmapperAPI.Data;
using BTKRoadmapperAPI.Entities;

namespace BTKRoadmapperAPI.Concrete
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        public CourseRepository(BTKRoadmapperDbContext context) : base(context)
        {
        }
    }
}
