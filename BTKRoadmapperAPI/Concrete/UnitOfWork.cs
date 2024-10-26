using BTKRoadmapperAPI.Abstractions;
using BTKRoadmapperAPI.Data;

namespace BTKRoadmapperAPI.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BTKRoadmapperDbContext _context;

        public UnitOfWork(BTKRoadmapperDbContext context)
        {
            _context = context;
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
