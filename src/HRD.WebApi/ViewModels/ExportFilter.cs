using HRD.WebApi.ViewModels.Enums;

namespace HRD.WebApi.ViewModels
{
    public class ExportFilter
    {
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }
        public string SearchString { get; set; }
        public EnumExportFormat ExportFormat { get; set; }
    }
}
