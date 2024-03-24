using System.ComponentModel.DataAnnotations;

namespace Memories_backend.Models.Pagination
{
    public abstract class PagedResultBase
    {
        public int CurrentPage { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Page count must be greater than 0.")]
        public int PageCount { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Page size must be greater than 0.")]
        [MaxLength(1000, ErrorMessage = "Page size cannot exceed 1000.")]
        public int PageSize { get; set; }
        public int RowCount { get; set; }

        public int FirstRowOnPage
        {

            get { return (CurrentPage - 1) * PageSize + 1; }
        }

        public int LastRowOnPage
        {
            get { return Math.Min(CurrentPage * PageSize, RowCount); }
        }
    }
}
