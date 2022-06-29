using System;

namespace HRD.WebApi.ViewModels.Report
{
    public class CasesCostHeldByCategoryOutput
    {
        public DateTime? MonthHeld { get; set; }
        public string HoldCategory { get; set; }
        public int? TotalCases { get; set; }
        public decimal? TotalCost { get; set; }
        public string Line { get; set; }
        public string YearHeld { get; set; }
        public int? WeekHeld { get; set; }
        public bool? IsComplete { get; set; }
    }
}
