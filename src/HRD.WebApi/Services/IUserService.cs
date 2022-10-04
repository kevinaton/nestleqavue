using HRD.WebApi.Data.Entities;
using HRD.WebApi.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRD.WebApi.Services
{
    public interface IUserService
    {
        public UserViewModel Get(UserLoginViewModel userLogin);
        IEnumerable<UserViewModel> GetAll();
        Task<int> GetOrCreateUserIdByUsername(string username);
        Task<List<Role>> GetUserRoles(int id);
        Task<IEnumerable<string>> GetUserPermissions(int userId);
    }
}
