using System.Collections.Generic;

namespace LibraryApp.Dtos
{
    public class PagedResult<T>
    {
        public IReadOnlyList<T> Items { get; set; } = Array.Empty<T>();
        public int Total { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)Total / PageSize);
        public string SortBy { get; set; } = "Title";
        public string SortDir { get; set; } = "asc"; 
        public string? Search { get; set; }

        public PagedResult()
        {
            PageNumber = 1;
            PageSize = 20;
        }

        public PagedResult(IReadOnlyList<T> items, int total, PagedRequest request)
        {
            Items = items;
            Total = total;
            PageNumber = request.PageNumber;
            PageSize = request.PageSize;
            SortBy = request.SortBy;
            SortDir = request.SortDir;
            Search = request.Search;
        }
    }
}