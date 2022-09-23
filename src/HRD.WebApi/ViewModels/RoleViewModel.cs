using HRD.WebApi.Data.Entities;
using System.Collections.Generic;

namespace HRD.WebApi.ViewModels
{
    public class RoleViewModel
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool IsStatic { get; set; }
        public List<string> GrantedPermissionNames { get; set; }
    }
}
