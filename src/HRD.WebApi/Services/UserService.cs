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
        
        public async Task<IEnumerable<UserViewModel>> GetAll()
        {
            var users = await _context.Users.Select(s => new UserViewModel
            {
                Name = s.Name,
                UserId = s.UserId
            }).ToListAsync();

            return users;
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
            var userId = await _context.Users.AsNoTracking().Where(s => s.UserId.ToLower() == username.ToLower()).FirstOrDefaultAsync();
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
        public async Task<string> GetUserEmailById(int id)
        {
            var email = await _context.Users.AsNoTracking().Where(s => s.Id == id).Select(s => s.Email).FirstOrDefaultAsync();
            return email;
        }
        public async Task<string> GetUserEmailByName(string name)
        {
            var email = await _context.Users.AsNoTracking().Where(s => s.Name.ToLower() == name.ToLower()).Select(s => s.Email).FirstOrDefaultAsync();
            return email;
        }
        public async Task<string> GetUserEmailByUserId(string userId)
        {
            var email = await _context.Users.AsNoTracking().Where(s => s.UserId.ToLower() == userId.ToLower()).Select(s => s.Email).FirstOrDefaultAsync();
            return email;
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

        public async Task<IEnumerable<string>> GetUserPermissions(int userId)
        {
            var permissionList = await _context.UserRoles.Include(i => i.Role.Permissions).Where(f => f.UserId == userId)
                                .SelectMany(s => s.Role.Permissions).ToListAsync();

            var grantedPermissions = permissionList.Where(f => f.IsGranted).Select(s => s.Name).Distinct().ToList();

            return grantedPermissions;
        }

        public async Task<List<User>> GetUsersByRole(string roleName)
        {
            var users = await (from ur in _context.UserRoles
                                join r in _context.Roles on ur.RoleId equals r.Id
                                join u in _context.Users on ur.UserId equals u.Id
                                where r.Name == roleName && !string.IsNullOrEmpty(u.Email)
                                select new User
                                {
                                    Name = r.Name,
                                    Email = u.Email
                                }).ToListAsync();

            return users;
        }

        public async Task<PagedResponse<List<UserViewModel>>> GetAll(PaginationFilter filter)
        {
            

            var query = _context.Users.Select(s => new UserViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Email = s.Email,
                UserId = s.UserId
            });

            //Sorting
            switch (filter.SortColumn)
            {
                case "id":
                    query = filter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.Id)
                        : query.OrderBy(o => o.Id);
                    break;
                case "name":
                    query = filter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.Name)
                        : query.OrderBy(o => o.Name);
                    break;
                case "userid":
                    query = filter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.UserId)
                        : query.OrderBy(o => o.UserId);
                    break;
                case "email":
                    query = filter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.Email)
                        : query.OrderBy(o => o.Email);
                    break;
            }

            if (!string.IsNullOrEmpty(filter.SearchString))
            {
                query = query.Where(f => f.Name.Contains(filter.SearchString)
                                        || f.UserId.Contains(filter.SearchString)
                                        || f.Email.Contains(filter.SearchString));
            }

            var totalRecords = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalRecords / filter.PageSize);

            //Pagination
            query = query.Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize);

            var itemList = await query.ToListAsync();

            var pageResponse = new PagedResponse<List<UserViewModel>>(itemList, filter.PageNumber, filter.PageSize, totalRecords, totalPages);

            return pageResponse;
        }

        public async Task<UserViewModel> GetUser(int id)
        {
            var user = await _context.Users.Include("UserRoles.Role").FirstOrDefaultAsync(f => f.Id == id);

            var model = new UserViewModel
            {
                Id = id,
                Name = user.Name,
                UserId = user.UserId,
                Email = user.Email,
                Roles = user.UserRoles.Select(s => new RoleViewModel
                {
                    Id = s.Role.Id,
                    Name = s.Role.Name,
                    DisplayName = s.Role.DisplayName
                }).ToList()
            };

            return model;
        }

        public async Task UpdateUser(UserViewModel model)
        {
            try
            {

                var user = new User
                {
                    Id = model.Id,
                    UserId = model.UserId,
                    Email = model.Email,
                    Name = model.Name
                };

                //_context.Entry(user).State = EntityState.Modified;
                _context.Update(user);

                //Add Or Update Role
                foreach (var role in model.Roles)
                {
                    var userRoleEntity = await _context.UserRoles.FirstOrDefaultAsync(f => f.UserId == model.Id && f.RoleId == role.Id);

                    if (userRoleEntity == null)
                    {
                        var userRole = new UserRole
                        {
                            UserId = model.Id,
                            RoleId = role.Id
                        };

                        _context.UserRoles.Add(userRole);
                    }
                }

                //Delete Role
                var toDeleteRoleList = await _context.UserRoles.Where(f => f.UserId == model.Id && !model.Roles.Select(s => s.Id).Any(a => a == f.RoleId)).ToListAsync();
                foreach (var userRole in toDeleteRoleList)
                {
                    _context.Entry(userRole).State = EntityState.Deleted;
                }

                await _context.SaveChangesAsync();

            }
            catch(Exception)
            {
                throw;
            }

        }

        public async Task<bool> IsUserUsed(UserViewModel model)
        {
            var isUserUsed = await _context.Users.AnyAsync(a => a.Id != model.Id && (a.Name.ToLower() == model.Name.ToLower() || a.UserId == model.UserId));
            return isUserUsed;
        }
    }
}
