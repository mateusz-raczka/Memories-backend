using Microsoft.EntityFrameworkCore;
using MemoriesBackend.Infrastructure.Contexts;
using MemoriesBackend.Domain.Interfaces.Transactions;
using System;
using System.Threading.Tasks;

namespace MemoriesBackend.Infrastructure.Transactions
{
    public class TransactionHandler : ITransactionHandler
    {
        private readonly ApplicationDbContext _context;

        public TransactionHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task ExecuteAsync(Func<Task> action, Func<Task>? rollbackAction = null)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        await action();
                        await transaction.CommitAsync();
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        if (rollbackAction != null)
                        {
                            await rollbackAction();
                        }
                        throw;
                    }
                }
            });
        }

        public async Task<T> ExecuteAsync<T>(Func<Task<T>> action, Func<Task>? rollbackAction = null)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var result = await action();
                        await transaction.CommitAsync();
                        return result;
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        if (rollbackAction != null)
                        {
                            await rollbackAction();
                        }
                        throw;
                    }
                }
            });
        }
    }
}
