using Memories_backend.Models.Pagination;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Memories_backend.Extensions
{
    /// <summary>
    /// Extension methods for querying collections.
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Retrieves a paged result of items from the IQueryable collection synchronously.
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="query">The IQueryable collection to page.</param>
        /// <param name="page">The page number (1-based index).</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A paged result containing items from the collection.</returns>
        public static async Task<PagedResult<T>> GetPageAsync<T>(this IQueryable<T> query, int page, int pageSize) where T : class
        {
            PagedResult<T> result = InitializeResult<T>(query, page, pageSize);

            int skip = (page - 1) * pageSize;
            result.Results = await query.Skip(skip).Take(pageSize).ToListAsync();

            return result;
        }

        /// <summary>
        /// Retrieves a paged result of items from the query synchronously.
        /// </summary>
        /// <typeparam name="T">The type of items in the query.</typeparam>
        /// <param name="query">The queryable collection.</param>
        /// <param name="page">The page number (1-based).</param>
        /// <param name="pageSize">The maximum number of items per page.</param>
        /// <returns>The paged result.</returns>
        public static PagedResult<T> GetPage<T>(this IQueryable<T> query, int page, int pageSize) where T : class
        {
            PagedResult<T> result = InitializeResult<T>(query, page, pageSize);

            int skip = (page - 1) * pageSize;
            result.Results = query.Skip(skip).Take(pageSize).ToList();

            return result;
        }

        /// <summary>
        /// Initializes the common properties of the paged result.
        /// </summary>
        /// <typeparam name="T">The type of items in the query.</typeparam>
        /// <param name="query">The queryable collection.</param>
        /// <param name="page">The page number (1-based).</param>
        /// <param name="pageSize">The maximum number of items per page.</param>
        /// <returns>The initialized paged result.</returns>
        private static PagedResult<T> InitializeResult<T>(IQueryable<T> query, int page, int pageSize) where T : class
        {
            PagedResult<T> result = new PagedResult<T>();
            result.CurrentPage = page;
            result.PageSize = pageSize;
            result.RowCount = query.Count();

            double pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            return result;
        }
    }
}
