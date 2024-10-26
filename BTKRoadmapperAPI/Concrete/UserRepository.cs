using BTKRoadmapperAPI.Abstractions;
using BTKRoadmapperAPI.Data;
using BTKRoadmapperAPI.Entities;

namespace BTKRoadmapperAPI.Concrete
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(BTKRoadmapperDbContext context) : base(context)
        {
        }
    }
}
