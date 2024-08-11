using Microsoft.EntityFrameworkCore;
using MemoriesBackend.Infrastructure.Contexts;
using MemoriesBackend.Domain.Interfaces.Transactions;

namespace MemoriesBackend.Infrastructure.Transactions
{
    public class TransactionHandler : ITransactionHandler
    {
        private readonly ApplicationDbContext _context;

        public TransactionHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task ExecuteAsync(Func<Task> action)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    await action();
                    await transaction.CommitAsync();
                }
            });
        }

        public async Task<T> ExecuteAsync<T>(Func<Task<T>> action)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    var result = await action();
                    await transaction.CommitAsync();
                    return result;
                }
            });
        }
    }
}