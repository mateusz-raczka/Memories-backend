﻿using System.Linq.Expressions;
using MemoriesBackend.Application.Services;
using MemoriesBackend.Domain.Interfaces.Models;
using MemoriesBackend.Domain.Interfaces.Repositories;
using MemoriesBackend.Domain.Interfaces.Services;
using MemoriesBackend.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace MemoriesBackend.Infrastructure.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;
        protected readonly IUserContextService _userContextService;
        private bool disposed;

        public GenericRepository(ApplicationDbContext context, IUserContextService userContextService)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
            _userContextService = userContextService;
        }

        private IQueryable<TEntity> ApplyOwnershipFilter(IQueryable<TEntity> query)
        {
            if (typeof(IOwnerId).IsAssignableFrom(typeof(TEntity)))
            {
                var currentUserId = _userContextService.Current.UserData.Id;
                query = query.Where(e => ((IOwnerId)e).OwnerId == currentUserId);
            }
            return query;
        }

        public IQueryable<TEntity> GetQueryable(bool asNoTracking = true)
        {
            var query = asNoTracking ? _dbSet.AsNoTracking() : _dbSet.AsQueryable();
            return ApplyOwnershipFilter(query);
        }

        public IQueryable<TEntity> GetAll(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            int? pageNumber = null,
            int? pageSize = null,
            bool asNoTracking = true
        )
        {
            IQueryable<TEntity> query = GetQueryable(asNoTracking);

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            if (pageNumber != null && pageSize != null && pageNumber > 0 && pageSize > 0)
                query = query.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);

            return query;
        }

        public virtual async Task<TEntity> GetById(Guid id, bool asNoTracking = true)
        {
            var query = GetQueryable(asNoTracking).Where(e => ((IEntity)e).Id == id);
            var entity = await query.FirstOrDefaultAsync();

            if (entity == null)
                throw new ApplicationException("Entity was not found or you do not have access to it.");

            return entity;
        }

        public virtual async Task<TEntity> Create(TEntity entity)
        {
            if (typeof(IOwnerId).IsAssignableFrom(entity.GetType()))
            {
                var currentUserId = _userContextService.Current.UserData.Id;
                ((IOwnerId)entity).SetOwnerId(currentUserId);
            }

            await _dbSet.AddAsync(entity);
            return entity;
        }

        public virtual async Task Delete(Guid id)
        {
            var entityToDelete = await GetById(id, asNoTracking: false);
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
            if (typeof(IOwnerId).IsAssignableFrom(entityToUpdate.GetType()))
            {
                var currentUserId = _userContextService.Current.UserData.Id;
                if (((IOwnerId)entityToUpdate).OwnerId != currentUserId)
                {
                    throw new UnauthorizedAccessException("Cannot modify an entity that does not belong to the current user.");
                }
            }

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
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                disposed = true;
            }
        }
    }
}
