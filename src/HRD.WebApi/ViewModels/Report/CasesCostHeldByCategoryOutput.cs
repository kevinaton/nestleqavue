using System;

namespace HRD.WebApi.ViewModels.Report
{
    public class CasesCostHeldByCategoryOutput
    {
        public int? WeekHeld { get; set; }
        public string DayCode { get; set; }
        public DateTime? DateHeld { get; set; }
        public string Fert { get; set; }
        public string ProductDescription { get; set; }
        public string Line { get; set; }
        public string Shift { get; set; }
        public string HourCode { get; set; }
        public int? Cases { get; set; }
        public string ShortDescription { get; set; }
        public string HoldCategory { get; set; }
        public string HoldSubCategory { get; set; }
        public string Originator { get; set; }
        public string TLForU { get; set; }
        public decimal? CostofProductonHold { get; set; }
    }
}
