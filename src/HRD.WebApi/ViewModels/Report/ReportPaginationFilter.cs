namespace HRD.WebApi.ViewModels.Report
{
    public class ReportPaginationFilter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }
        public ReportPaginationFilter()
        {
            PageNumber = 1;
            PageSize = 10;
        }

        public ReportPaginationFilter(int pageNumber, int pageSize, string sortColumn, string sortOrder)
        {
            PageNumber = pageNumber < 1 ? 1 : pageNumber;
            PageSize = pageSize < 10 ? 10 : pageSize;
            SortColumn = !string.IsNullOrWhiteSpace(sortColumn) ? sortColumn.ToLower() : "";
            SortOrder = !string.IsNullOrWhiteSpace(sortOrder) ? sortOrder.ToLower() : "asc";
        }
    }
}