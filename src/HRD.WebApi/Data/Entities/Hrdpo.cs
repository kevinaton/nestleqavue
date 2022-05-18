using System;
using System.Collections.Generic;

namespace HRD.WebApi.Data.Entities
{
    public partial class Hrdpo
    {
        public int Id { get; set; }
        public int Hrdid { get; set; }
        public string Ponumber { get; set; }

        public virtual Hrd Hrd { get; set; }
    }
}
