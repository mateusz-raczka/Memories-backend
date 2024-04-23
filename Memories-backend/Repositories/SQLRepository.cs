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
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null
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

            if(pageNumber != null && pageSize != null)
            {
                PagedResult<TEntity> pagedResult = await query.GetPageAsync<TEntity>((int)pageNumber, (int)pageSize);

                return pagedResult.Results;
            }

            return query;
        }

        public async virtual Task<TEntity> GetById(Guid id)
        {
            TEntity entity = await _dbSet.FindAsync(id);

            return entity;
        }

        public async virtual Task<TEntity> Create(TEntity entity)
        {
            await _dbSet.AddAsync(entity);

            return entity;
        }

        public async virtual Task Delete(Guid id)
        {
            TEntity entityToDelete = await GetById(id);

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

        public async virtual Task Save()
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
