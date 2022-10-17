using HRD.WebApi.ViewModels.Enums;
using System;

namespace HRD.WebApi.ViewModels.Report
{
    public class GetCasesCostByCategoryReportInput : DataInput
    {
        public EnumStatus Status { get; set; }
        public string Line { get; set; }
    }
}
