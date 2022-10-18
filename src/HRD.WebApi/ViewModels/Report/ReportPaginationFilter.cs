namespace HRD.WebApi.ViewModels.Report
{
    public class ReportPaginationFilter
    {
        public GetCasesCostByCategoryReportInput ReportFilter { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }        
        public ReportPaginationFilter()
        {
            PageNumber = 1;
            PageSize = 10;
        }

        public ReportPaginationFilter(int pageNumber, int pageSize, GetCasesCostByCategoryReportInput reportFilter)
        {
            PageNumber = pageNumber < 1 ? 1 : pageNumber;
            PageSize = pageSize < 10 ? 10 : pageSize;
            ReportFilter = reportFilter;
        }
    }
}