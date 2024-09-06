namespace MemoriesBackend.Domain.Interfaces.Transactions
{
    public interface ITransactionHandler
    {
        Task ExecuteAsync(Func<Task> action, Func<Task>? rollbackAction = null);
        Task<T> ExecuteAsync<T>(Func<Task<T>> action, Func<Task>? rollbackAction = null);
    }
}