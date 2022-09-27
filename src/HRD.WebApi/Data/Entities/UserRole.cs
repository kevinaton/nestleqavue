﻿
using System;
using System.Collections.Generic;

namespace HRD.WebApi.Data.Entities
{
    public partial class UserRole
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}