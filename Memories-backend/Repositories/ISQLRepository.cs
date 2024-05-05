using System.Linq.Expressions;

namespace Memories_backend.Repositories
{
    public interface ISQLRepository<TEntity> : IDisposable where TEntity: class
    {
        Task<TEntity> GetById(Guid id);
        Task<IEnumerable<TEntity>> GetAll(
            int? pageNumber = null,
            int? pageSize = null,
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null
            );
        Task<TEntity> Create(TEntity entity);
        Task Delete(Guid id);
        void Delete(TEntity entityToDelete);
        void Update(TEntity entityToUpdate);
        Task Save();
    }
}
