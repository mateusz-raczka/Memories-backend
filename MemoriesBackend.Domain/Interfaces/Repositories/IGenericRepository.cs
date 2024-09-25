using System.Linq.Expressions;

namespace MemoriesBackend.Domain.Interfaces.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetQueryable(bool asNoTracking = true);

        IQueryable<TEntity> GetAll(
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            int? pageNumber = null,
            int? pageSize = null,
            bool asNoTracking = true);

        Task<TEntity> GetById(Guid id, bool asNoTracking = true);

        Task<TEntity> Create(TEntity entity);

        Task Delete(Guid id);

        void Delete(TEntity entityToDelete);

        void Update(TEntity entityToUpdate);

        Task Save();
    }
}
