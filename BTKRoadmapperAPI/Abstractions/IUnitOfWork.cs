namespace BTKRoadmapperAPI.Abstractions
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
    }
}
