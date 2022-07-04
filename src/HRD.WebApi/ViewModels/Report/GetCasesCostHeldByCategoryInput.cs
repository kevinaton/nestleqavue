﻿using System;

namespace HRD.WebApi.ViewModels.Report
{
    public class GetCasesCostHeldByCategoryInput : DataInput
    {
        public EnumStatus Status { get; set; }
        public EnumCostGraph CostGraphOption { get; set; }
        public string WeekHeld { get; set; }
        public string Line { get; set; }
    }
}
