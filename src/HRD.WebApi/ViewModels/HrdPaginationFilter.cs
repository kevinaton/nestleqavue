namespace HRD.WebApi.ViewModels
{
    public class HrdPaginationFilter : PaginationFilter
    {
        public HrdFilterCriteria FilterCriteria { get; set; }

        public HrdPaginationFilter()
        {
            this.PageNumber = 1;
            this.PageSize = 10;
            this.FilterCriteria = new HrdFilterCriteria();
        }
            

        public HrdPaginationFilter(int pageNumber, int pageSize, string sortColumn, string sortOrder, string searchString, HrdFilterCriteria filterCriteria) : base()
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize < 10 ? 10 : pageSize;
            this.SortColumn = !string.IsNullOrWhiteSpace(sortColumn) ? sortColumn.ToLower() : "";
            this.SortOrder = !string.IsNullOrWhiteSpace(sortOrder) ? sortOrder.ToLower() : "asc";
            this.SearchString = searchString;
            FilterCriteria = filterCriteria;
            filterCriteria.Type = !string.IsNullOrWhiteSpace(filterCriteria.Type) ? filterCriteria.Type.ToLower() : string.Empty;
        }
    }
}
