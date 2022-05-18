using System;
using System.Collections.Generic;

namespace HRD.WebApi.Data.Entities
{
    public partial class Hrdnote
    {
        public int Id { get; set; }
        public int? Hrdid { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public DateTime? Date { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public string Size { get; set; }

        public virtual Hrd Hrd { get; set; }
    }
}
