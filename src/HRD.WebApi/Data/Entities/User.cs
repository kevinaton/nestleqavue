using System;
using System.Collections.Generic;

namespace HRD.WebApi.Data.Entities
{
    public partial class User
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
