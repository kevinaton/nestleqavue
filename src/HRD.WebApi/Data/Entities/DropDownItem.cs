using System;
using System.Collections.Generic;

namespace HRD.WebApi.Data.Entities
{
    public partial class DropDownItem
    {
        public int Id { get; set; }
        public int DropDownTypeId { get; set; }
        public string Value { get; set; }
        public short SortOrder { get; set; }
        public bool IsActive { get; set; }

        public virtual DropDownType DropDownType { get; set; }
    }
}
