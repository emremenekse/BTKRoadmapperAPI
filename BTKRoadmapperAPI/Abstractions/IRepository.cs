using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BTKRoadmapperAPI.Abstractions
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllWithIncludesAsync(params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> GetAllWithIncludesManyAsync(params Expression<Func<T, object>>[] includes);
        DbSet<T> Table { get; }
        Task<T> GetByIdAsync(Guid id);
        Task<T> GetByIdWithIncludesAsync(Guid id, params Expression<Func<T, object>>[] includes);
        Task<T> GetByIdWithIncludesManyAsync(Guid id, params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> FindAsyncWithPaginationWithAll(
    Expression<Func<T, bool>> predicate,
    int page,
    int pageSize,
    params Expression<Func<T, object>>[] includes);
        Task<T> GetByIdWithIncludesAsync(int id, params Expression<Func<T, object>>[] includes);
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> FindAsyncWithMany(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> FindAsyncWithPagination(
    Expression<Func<T, bool>> predicate,
    int page,
    int pageSize,
    params Expression<Func<T, object>>[] includes);
        Task<T> GetByIdCategoryAsync(int id);
        Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes);
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task UpdateAsync(T entity, params Expression<Func<T, object>>[] updatedProperties);
        Task DeleteAsync(T entity);
        Task UpdateFieldAsync<TField>(object id, Expression<Func<T, TField>> fieldSelector, TField newValue);
        Task<IEnumerable<T>> FindAsyncWithPagination(Expression<Func<T, bool>> predicate, int page,
    int pageSize);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        Task UpdateFieldAsync<TField>(int id, Expression<Func<T, TField>> fieldSelector, TField newValue);
    }
}
