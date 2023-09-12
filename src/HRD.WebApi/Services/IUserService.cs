using HRD.WebApi.Data.Entities;
using HRD.WebApi.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRD.WebApi.Services
{
    public interface IUserService
    {
        UserViewModel Get(UserLoginViewModel userLogin);
        Task<IEnumerable<UserViewModel>> GetAll();
        Task<PagedResponse<List<UserViewModel>>> GetAll(PaginationFilter filter);
        Task<int> GetOrCreateUserIdByUsername(string username);
        Task<string> GetUserEmailById(int id);
        Task<string> GetUserEmailByName(string name);
        Task<string> GetUserEmailByUserId(string userId);
        Task<List<Role>> GetUserRoles(int id);
        Task<IEnumerable<string>> GetUserPermissions(int userId);

        Task<List<User>> GetUsersByRole(string roleName);

        Task<UserViewModel> GetUser(int id);
        Task UpdateUser(UserViewModel model);

        Task<bool> IsUserUsed(UserViewModel model);
    }
}
