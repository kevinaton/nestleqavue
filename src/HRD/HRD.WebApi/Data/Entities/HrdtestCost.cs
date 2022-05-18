using System;
using System.Collections.Generic;

namespace HRD.WebApi.Data.Entities
{
    public partial class HrdtestCost
    {
        public int Id { get; set; }
        public int Hrdid { get; set; }
        public string TestName { get; set; }
        public int? Qty { get; set; }
        public decimal? Cost { get; set; }

        public virtual Hrd Hrd { get; set; }
    }
}
