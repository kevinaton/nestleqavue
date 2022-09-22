using System.Collections.Generic;

namespace HRD.WebApi.ViewModels
{
    public class CreateOrUpdateRoleInput
    {
        public RoleViewModel Role { get; set; }
        public List<string> GrantedPermissionNames { get; set; }
    }
}
