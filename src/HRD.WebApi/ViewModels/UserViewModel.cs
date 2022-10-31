using System.Collections.Generic;

namespace HRD.WebApi.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public List<RoleViewModel> Roles { get; set; }
    }
}
