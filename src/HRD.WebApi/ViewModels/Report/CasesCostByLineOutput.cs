using System;

namespace HRD.WebApi.ViewModels.Report
{
    public class CasesCostByLineOutput
    {
        public string Line { get; set; }
        public int? TotalCases { get; set; }
        public decimal? TotalCost { get; set; }
    }
}
