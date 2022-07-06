namespace HRD.WebApi.ViewModels
{
    public class ReportPaginationFilter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }
        public ReportPaginationFilter()
        {
            this.PageNumber = 1;
            this.PageSize = 10;
        }

        public ReportPaginationFilter(int pageNumber, int pageSize, string sortColumn, string sortOrder)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize < 10 ? 10 : pageSize;
            this.SortColumn = !string.IsNullOrWhiteSpace(sortColumn) ? sortColumn.ToLower() : "";
            this.SortOrder = !string.IsNullOrWhiteSpace(sortOrder) ? sortOrder.ToLower() : "asc";
        }
    }
}