using System;

namespace HRD.WebApi.ViewModels.Report
{
    public class CasesCostHeldByCategoryOutput
    {
        public int? TotalCases { get; set; }
        public decimal? TotalCost { get; set; }
        public string Line { get; set; }
    }
}
