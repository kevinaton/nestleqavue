using System;
using System.Collections.Generic;

namespace HRD.WebApi.Data.Entities
{
    public partial class Hrdfc
    {
        public int Id { get; set; }
        public int Hrdid { get; set; }
        public string Location { get; set; }
        public int NumberOfCases { get; set; }

        public virtual Hrd Hrd { get; set; }
    }
}
