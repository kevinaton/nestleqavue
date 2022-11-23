using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HRD.WebApi.Data;
using HRD.WebApi.Data.Entities;
using HRD.WebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using HRD.WebApi.Authorization;
using Microsoft.Extensions.Configuration;

namespace HRD.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly HRDContext _context;
        protected IAuthorizationService AuthorizationService { get; }
        protected IConfiguration Configuration { get; }

        public UsersController(HRDContext context,
            IAuthorizationService authorizationService,
            IConfiguration configuration)
        {
            _context = context;
            AuthorizationService = authorizationService;
            Configuration = configuration;
        }

        // GET: api/Users
        [HttpGet]
        // [Authorize(Policy = PolicyNames.ViewHRDs)]
        public async Task<ActionResult<IEnumerable<UserViewModel>>> GetUsers([FromQuery] PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.SortColumn, filter.SortOrder, filter.SearchString);

            var query = _context.Users.Select(s => new UserViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Email = s.Email,
                UserId = s.UserId
            });

            //Sorting
            switch (validFilter.SortColumn)
            {
                case "id":
                    query = validFilter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.Id)
                        : query.OrderBy(o => o.Id);
                    break;
                case "name":
                    query = validFilter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.Name)
                        : query.OrderBy(o => o.Name);
                    break;
                case "userid":
                    query = validFilter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.UserId)
                        : query.OrderBy(o => o.UserId);
                    break;
                case "email":
                    query = validFilter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.Email)
                        : query.OrderBy(o => o.Email);
                    break;
            }

            if (!string.IsNullOrEmpty(validFilter.SearchString))
            {
                query = query.Where(f => f.Name.Contains(filter.SearchString)
                                        || f.UserId.Contains(filter.SearchString)
                                        || f.Email.Contains(filter.SearchString));
            }

            var totalRecords = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalRecords / validFilter.PageSize);

            //Pagination
            query = query.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize);

            var itemList = await query.ToListAsync();

            return Ok(new PagedResponse<List<UserViewModel>>(itemList, validFilter.PageNumber, validFilter.PageSize, totalRecords, totalPages));
        }

        [HttpGet("GetAll")]
        // [Authorize(Policy = PolicyNames.ViewHRDs)]
        public async Task<ActionResult<IEnumerable<UserLookupDto>>> GetAll()
        {
            var query = _context.Users.OrderBy(o => o.Name).Select(s => new UserLookupDto
            {
                UserId = s.UserId,
                Name = s.Name
            });

            var results = await query.ToListAsync();

            return Ok(results);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        // [Authorize(Policy = PolicyNames.ViewHRDs)]
        public async Task<ActionResult<UserViewModel>> GetUser(int id)
        {
            var user = await _context.Users.Include("UserRoles.Role").FirstOrDefaultAsync(f => f.Id == id);

            if (user == null)
            {
                return NotFound();
            }

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

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        // [Authorize(Policy = PolicyNames.EditHRDs)]
        public async Task<IActionResult> PutUser(int id, UserViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (await _context.Users.AnyAsync(a => a.Id != model.Id && (a.Name.ToLower() == model.Name.ToLower() || a.UserId == model.UserId)))
            {
                return BadRequest($"User name: {model.Name} with userId {model.UserId} already exists.");
            }

            var user = new User
            {
                Id = model.Id,
                UserId = model.UserId,
                Email = model.Email,
                Name = model.Name
            };

            _context.Entry(user).State = EntityState.Modified;

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

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        // [Authorize(Policy = PolicyNames.EditHRDs)]
        public async Task<ActionResult<UserViewModel>> PostUser(UserViewModel model)
        {
            if (await _context.Users.AnyAsync(a => a.Name.ToLower() == model.Name.ToLower() || a.UserId == model.UserId))
            {
                return BadRequest($"User name: {model.Name} with userId {model.UserId} already exists.");
            }

            var user = new User
            {
                Id = model.Id,
                Name = model.Name,
                Email = model.Email,
                UserId = model.UserId,
                UserRoles = model.Roles.Select(s => new UserRole { RoleId = s.Id }).ToList()
            };
            _context.Users.Add(user);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }

            model.Id = user.Id;
            return CreatedAtAction("GetUser", new { id = model.Id }, model);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        // [Authorize(Policy = PolicyNames.EditHRDs)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            if (await _context.Hrds.AnyAsync(a => a.Bumanager == user.UserId || a.Originator == user.UserId || a.LineSupervisor == user.UserId ||
                            a.HrdcompletedBy == user.UserId || a.ApprovedByDistroyedWho == user.UserId || a.ApprovedByPlantControllerWho == user.UserId ||
                            a.ApprovedByPlantManagerWho == user.UserId || a.ApprovedByQawho == user.UserId || a.ReworkApprovedBy == user.UserId ||
                            a.ReworkCompletedBy == user.UserId || a.Fcuser == user.UserId || a.Dcuser == user.UserId))
            {
                return BadRequest($"Cannot delete Role: {user.Name}. It is being used in HRD Record");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        [HttpGet("GetCurrentUser")]
        public async Task<IActionResult> GetCurrentUser()
        {
            
            // var userId = Convert.ToInt32(User.Identities.First().Claims.First(f => f.Type == "UserId").Value);
            var userId = 113;
            var user = await _context.Users.FirstAsync(e => e.Id == userId);
            var result = new CurrentUserDto {
                            UserId = user.UserId,
                            Name = user.Name,
                        };
            return Ok(result);
        }

        [HttpGet("CheckPermission")]
        public async Task<IActionResult> CheckUserHasPermission()
        {
            var hasViewAccess = await HasPermissionAsync(PolicyNames.ViewHRDs);

            if (!hasViewAccess)
            {
                return Ok("You do not currently have access to the HRD application. Send an email to " + Configuration["RequestAccessAdminEmail"].ToString() + " requesting access.");
            }

            return Ok();
        }
        [HttpGet("HasPermission/{permissionName}")]
        public async Task<ActionResult<bool>> HasPermission(string permissionName)
        {
            var hasPermission = false;
            // var userId = Convert.ToInt32(User.Identities.First().Claims.First(f => f.Type == "UserId").Value);
            var userId = 113;

            hasPermission = await _context.UserRoles.AnyAsync(a => a.UserId == userId && a.Role.Permissions.Any(a => a.Name == permissionName && a.IsGranted));
            
            return Ok(hasPermission);
        }

        [HttpGet("GetCurrentUserPermissions")]
        public async Task<ActionResult<IEnumerable<string>>> GetCurrentUserPermissions()
        {
            // var userId = Convert.ToInt32(User.Identities.First().Claims.First(f => f.Type == "UserId").Value);
            var userId = 113;
            
            var permissionList = await _context.UserRoles.Include(i => i.Role.Permissions).Where(f => f.UserId == userId)
                                .SelectMany(s => s.Role.Permissions).ToListAsync();
            
            var permissions = permissionList.Where(f => f.IsGranted).Select(s => s.Name).Distinct().ToList();
            return Ok(permissions);
        }

        [HttpGet("GetUsersByPermission")]
        public async Task<ActionResult<IEnumerable<UserLookupDto>>> GetUsersByPermission(string name)
        {
            var userRoles = await _context.UserRoles.Include(i => i.User).Where(f => f.Role.Permissions.Any(a => a.Name == name)).ToListAsync();
            var users = userRoles.Select(s => new UserLookupDto
                        {
                            UserId = s.User.UserId,
                            Name = s.User.Name
                        }).DistinctBy(d => d.UserId).OrderBy(o => o.Name);

            return Ok(users);
        }

        [HttpGet("GetUsersByRole/{role}")]
        // [Authorize(Policy = PolicyNames.ViewHRDs)]
        public async Task<ActionResult<IEnumerable<UserLookupDto>>> GetUsersByRole(string role)
        {
            var userRoles = await _context.UserRoles.Include(i => i.User).Where(f => f.Role.Name == role).ToListAsync();
            var users = userRoles.Select(s => new UserLookupDto
            {
                UserId = s.User.UserId,
                Name = s.User.Name
            }).OrderBy(o => o.Name);

            return Ok(users);
        }



        protected async Task<bool> HasPermissionAsync(params string[] permissionsToCheck)
        {
            foreach (var permissionToCheck in permissionsToCheck)
            {
                var succeed = (await AuthorizationService.AuthorizeAsync(User, permissionToCheck)).Succeeded;
                if (succeed)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
