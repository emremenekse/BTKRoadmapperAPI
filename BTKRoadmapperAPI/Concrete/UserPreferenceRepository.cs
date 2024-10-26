using BTKRoadmapperAPI.Abstractions;
using BTKRoadmapperAPI.Data;
using BTKRoadmapperAPI.Entities;

namespace BTKRoadmapperAPI.Concrete
{
    public class UserPreferenceRepository : Repository<UserPreference>, IUserPreferenceRepository
    {
        public UserPreferenceRepository(BTKRoadmapperDbContext context) : base(context)
        {
        }
    }
}
