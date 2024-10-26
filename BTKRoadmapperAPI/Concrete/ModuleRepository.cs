using BTKRoadmapperAPI.Abstractions;
using BTKRoadmapperAPI.Data;
using BTKRoadmapperAPI.Entities;

namespace BTKRoadmapperAPI.Concrete
{
    public class ModuleRepository : Repository<Module>, IModuleRepository
    {
        public ModuleRepository(BTKRoadmapperDbContext context) : base(context)
        {
        }
    }
}
