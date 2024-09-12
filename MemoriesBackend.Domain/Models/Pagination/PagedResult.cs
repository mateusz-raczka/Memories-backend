using MemoriesBackend.Domain.Shared;

namespace MemoriesBackend.Domain.Models.Pagination
{
    public class PagedResult<T> : PagedResultBase where T : class
    {
        public IEnumerable<T> Results { get; set; }

        public PagedResult()
        {
            Results = new List<T>();
        }
    }

}
