using HRD.WebApi.Authorization;
using HRD.WebApi.Data;
using HRD.WebApi.Data.Entities;
using HRD.WebApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HRD.WebApi.Services
{
    public class UserService : IUserService
    {
        private HRDContext _context;

        public UserService(HRDContext context)
        {
            _context = context;
        }
        public IEnumerable<UserViewModel> GetAll()
        {
            var user = _context.Users.Select(s => new UserViewModel
            {
                Name = s.Name,
                UserId = s.UserId
            });

            return user;
        }
        public UserViewModel Get(UserLoginViewModel userLogin)
        {
            var query = _context.Users.Where(s => s.UserId.ToLower() == userLogin.UserName.ToLower()).FirstOrDefault();
            if (query != null)
            {
                return new UserViewModel { UserId = query.UserId, Name = query.Name };
            }

            return null;
        }
        public async Task<int> GetOrCreateUserIdByUsername(string username)
        {
            var userId = await _context.Users.Where(s => s.UserId.ToLower() == username.ToLower()).FirstOrDefaultAsync();
            if (userId != null)
            {
                return userId.Id;
            }

            var user = new User
            {
                UserId = username,
                Name = username
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user.Id;
        }
        public async Task<List<Role>> GetUserRoles(int id)
        {
            var result = await (from ur in _context.UserRoles
                          join r in _context.Roles on ur.RoleId equals r.Id
                          where ur.UserId == id
                          select new Role
                          {
                              Name = r.Name
                          }).ToListAsync();

            return result;
        }
    }
}
