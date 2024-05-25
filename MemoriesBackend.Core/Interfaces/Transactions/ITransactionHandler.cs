﻿namespace MemoriesBackend.Application.Interfaces.Transactions
{
    public interface ITransactionHandler
    {
        Task ExecuteAsync(Func<Task> action);
        Task<T> ExecuteAsync<T>(Func<Task<T>> action);
    }
}
