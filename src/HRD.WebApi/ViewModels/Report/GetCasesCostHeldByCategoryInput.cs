using System;

namespace HRD.WebApi.ViewModels.Report
{
    public class GetCasesCostHeldByCategoryInput : DataInput
    {
        public EnumStatus Status { get; set; }
        public EnumCostGraph GraphOption { get; set; }
        public EnumWeekHeld WeekHeld { get; set; }
    }
}
