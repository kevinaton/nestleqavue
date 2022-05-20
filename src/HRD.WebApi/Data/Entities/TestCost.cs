using System;
using System.Collections.Generic;

namespace HRD.WebApi.Data.Entities
{
    public partial class TestCost
    {
        public int Id { get; set; }
        public string Year { get; set; }
        public string TestName { get; set; }
        public decimal? TestCostValue { get; set; }
    }
}
