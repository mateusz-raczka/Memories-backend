using System.Linq.Expressions;
using MemoriesBackend.Domain.Interfaces.Repositories;
using MemoriesBackend.Infrastructure.Contexts;
using MemoriesBackend.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace MemoriesBackend.Infrastructure.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    private bool disposed;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public IQueryable<TEntity> GetQueryable()
    {
        return _dbSet.AsQueryable();
    }

    public async Task<IEnumerable<TEntity>> GetAll(
        int? pageNumber = null,
        int? pageSize = null,
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null
    )
    {
        IQueryable<TEntity> query = _dbSet;

        if (filter != null) query = query.Where(filter);

        if (orderBy != null) query = orderBy(query);

        if (pageNumber == null && pageSize == null) return await query.ToListAsync();

        if (pageNumber != null && pageSize != null)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                throw new ApplicationException("Page number and page size must be positive integers.");

            var pagedResult = await query.GetPageAsync((int)pageNumber, (int)pageSize);

            return pagedResult.Results;
        }

        throw new ApplicationException("Both page number and page size must be provided, or neither.");
    }

    public virtual async Task<TEntity> GetById(Guid id)
    {
        var entity = await _dbSet.FindAsync(id);

        if (entity == null) throw new ApplicationException("Entity was not found");

        return entity;
    }

    public virtual async Task<TEntity> Create(TEntity entity)
    {
        await _dbSet.AddAsync(entity);

        return entity;
    }

    public virtual async Task Delete(Guid id)
    {
        var entityToDelete = await GetById(id);

        Delete(entityToDelete);
    }

    public virtual void Delete(TEntity entityToDelete)
    {
        if (_context.Entry(entityToDelete).State == EntityState.Detached) _dbSet.Attach(entityToDelete);

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

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
            if (disposing)
                _context.Dispose();
        disposed = true;
    }
}