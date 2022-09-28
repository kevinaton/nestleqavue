using System;
using System.Collections.Generic;

namespace HRD.WebApi.Data.Entities
{
    public partial class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool IsStatic { get; set; } = true;
        public virtual ICollection<Permission> Permissions { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}