using System.Collections.Generic;

namespace LibraryApp.Dtos
{
    public class PagedRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; } = "Title";
        public string SortDir { get; set; } = "asc"; 
        public string? Search { get; set; }
    }
}