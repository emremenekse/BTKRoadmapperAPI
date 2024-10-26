using BTKRoadmapperAPI.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace BTKRoadmapperAPI.Concrete
{
    public class Repository<T> : IRepository<T> where T : class

    {
        protected readonly DbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllWithIncludesAsync(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }
        public async Task<IEnumerable<T>> GetAllWithIncludesManyAsync(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }

        public DbSet<T> Table => _context.Set<T>();

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }
        public async Task<T> GetByIdWithIncludesManyAsync(Guid id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            ParameterExpression parameter = Expression.Parameter(typeof(T), "entity");
            MemberExpression property = Expression.Property(parameter, "Id");
            ConstantExpression constant = Expression.Constant(id, typeof(Guid));
            BinaryExpression equality = Expression.Equal(property, constant);
            Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(equality, parameter);

            return await query.FirstOrDefaultAsync(lambda);
        }


        public async Task<T> GetByIdWithIncludesAsync(Guid id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            ParameterExpression parameter = Expression.Parameter(typeof(T), "entity");
            MemberExpression property = Expression.Property(parameter, "Id");
            ConstantExpression constant = Expression.Constant(id, typeof(Guid));
            BinaryExpression equality = Expression.Equal(property, constant);
            Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(equality, parameter);

            return await query.FirstOrDefaultAsync(lambda);
        }
        public async Task<T> GetByIdWithIncludesAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            ParameterExpression parameter = Expression.Parameter(typeof(T), "entity");
            MemberExpression property = Expression.Property(parameter, "Id");
            ConstantExpression constant = Expression.Constant(id, typeof(int));
            BinaryExpression equality = Expression.Equal(property, constant);
            Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(equality, parameter);

            return await query.FirstOrDefaultAsync(lambda);
        }


        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }
        public async Task<T> GetByIdCategoryAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> FindAsyncWithPagination(Expression<Func<T, bool>> predicate, int page,
    int pageSize)
        {
            var query = _dbSet.Where(predicate);
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return items;
        }
        public async Task<IEnumerable<T>> FindAsyncWithPagination(
    Expression<Func<T, bool>> predicate,
    int page,
    int pageSize,
    params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet.Where(predicate);

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return items;
        }

        public async Task<IEnumerable<T>> FindAsyncWithPaginationWithAll(
    Expression<Func<T, bool>> predicate,
    int page,
    int pageSize,
    params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet.Where(predicate);

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return items;
        }

        public async Task UpdateFieldAsync<TField>(int id, Expression<Func<T, TField>> fieldSelector, TField newValue)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException("Entity with the specified ID not found.");
            }

            MemberExpression memberExpression = fieldSelector.Body as MemberExpression;
            if (memberExpression == null)
            {
                if (fieldSelector.Body is UnaryExpression unaryExpression)
                {
                    memberExpression = unaryExpression.Operand as MemberExpression;
                }
            }

            if (memberExpression == null || !(memberExpression.Member is PropertyInfo propertyInfo))
            {
                throw new ArgumentException("The fieldSelector expression does not refer to a valid property.");
            }

            propertyInfo.SetValue(entity, newValue, null);
            _context.Entry(entity).Property(propertyInfo.Name).IsModified = true;

        }


        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }
        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet.Where(predicate);

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }
        public async Task<IEnumerable<T>> FindAsyncWithMany(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet.Where(predicate);

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }
        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public async Task UpdateAsync(T entity, params Expression<Func<T, object>>[] updatedProperties)
        {
            var dbEntityEntry = _context.Entry(entity);

            if (updatedProperties.Any())
            {
                foreach (var property in updatedProperties)
                {
                    dbEntityEntry.Property(property).IsModified = true;
                }
            }
            else
            {
                dbEntityEntry.State = EntityState.Modified;
            }

        }
        public async Task UpdateFieldAsync<TField>(object id, Expression<Func<T, TField>> fieldSelector, TField newValue)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException("Entity with the specified ID not found.");
            }

            MemberExpression memberExpression = fieldSelector.Body as MemberExpression;
            if (memberExpression == null)
            {
                if (fieldSelector.Body is UnaryExpression unaryExpression)
                {
                    memberExpression = unaryExpression.Operand as MemberExpression;
                }
            }

            if (memberExpression == null || !(memberExpression.Member is PropertyInfo propertyInfo))
            {
                throw new ArgumentException("The fieldSelector expression does not refer to a valid property.");
            }

            propertyInfo.SetValue(entity, newValue, null);
            _context.Entry(entity).Property(propertyInfo.Name).IsModified = true;

            await _context.SaveChangesAsync();
        }


        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
        }

    }
}
