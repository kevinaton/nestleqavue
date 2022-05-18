namespace HRD.WebApi.ViewModels
{
    public class PaginationFilter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }
        public string SearchString { get; set; }
        public PaginationFilter()
        {
            this.PageNumber = 1;
            this.PageSize = 10;
        }

        public PaginationFilter(int pageNumber, int pageSize, string sortColumn, string sortOrder, string searchString)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize < 10 ? 10 : pageSize;
            this.SortColumn = !string.IsNullOrWhiteSpace(sortColumn) ? sortColumn.ToLower() : "";
            this.SortOrder = !string.IsNullOrWhiteSpace(sortOrder) ? sortOrder.ToLower() : "asc";
            this.SearchString = searchString;
        }
    }
}