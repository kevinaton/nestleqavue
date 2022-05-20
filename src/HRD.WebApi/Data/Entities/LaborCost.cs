using System;
using System.Collections.Generic;

namespace HRD.WebApi.Data.Entities
{
    public partial class LaborCost
    {
        public string Year { get; set; }
        public decimal? LaborCostValue { get; set; }
    }
}
