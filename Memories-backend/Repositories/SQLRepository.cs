using Memories_backend.Contexts;
using Memories_backend.Models.Pagination;
using Memories_backend.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Memories_backend.Repositories
{
    public class SQLRepository<TEntity> : ISQLRepository<TEntity> where TEntity : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public SQLRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetAll(
            int? pageNumber = null,
            int? pageSize = null,
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null
            )
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (pageNumber == null && pageSize == null)
            {
                return await query.ToListAsync();
            }
            else if (pageNumber != null && pageSize != null)
            {
                if (pageNumber <= 0 || pageSize <= 0)
                {
                    throw new ApplicationException("Page number and page size must be positive integers.");
                }

                PagedResult<TEntity> pagedResult = await query.GetPageAsync<TEntity>((int)pageNumber, (int)pageSize);

                return pagedResult.Results;
            }
            else
            {
                throw new ApplicationException("Both page number and page size must be provided, or neither.");
            }
        }

        public virtual async Task<TEntity> GetById(Guid id)
        {
            TEntity? entity = await _dbSet.FindAsync(id);

            if(entity == null)
            {
                throw new ApplicationException("Failed to fetch - entity was not found");
            }

            return entity;
        }

        public virtual async Task<TEntity> Create(TEntity entity)
        {
            await _dbSet.AddAsync(entity);

            return entity;
        }

        public virtual async Task Delete(Guid id)
        {
            TEntity? entityToDelete = await GetById(id);

            if(entityToDelete == null)
            {
                throw new ApplicationException("Failed to delete - entity was not found");
            }

            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }

            _dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            _dbSet.Attach(entityToUpdate);

            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
