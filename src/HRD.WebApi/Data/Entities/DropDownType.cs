using System;
using System.Collections.Generic;

namespace HRD.WebApi.Data.Entities
{
    public partial class DropDownType
    {
        public DropDownType()
        {
            DropDownItems = new HashSet<DropDownItem>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<DropDownItem> DropDownItems { get; set; }
    }
}
