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
using HRD.WebApi.Services;

namespace HRD.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly HRDContext _context;
        protected IAuthorizationService AuthorizationService { get; }
        private readonly IUserService _service;
        protected IConfiguration Configuration { get; }

        public UsersController(HRDContext context,
            IAuthorizationService authorizationService,
            IConfiguration configuration, IUserService service)
        {
            _context = context;
            AuthorizationService = authorizationService;
            Configuration = configuration;
            _service = service;
        }

        // GET: api/Users
        [HttpGet]
        // [Authorize(Policy = PolicyNames.ViewHRDs)]
        public async Task<ActionResult<IEnumerable<UserViewModel>>> GetUsers([FromQuery] PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.SortColumn, filter.SortOrder, filter.SearchString);

            var pagedResponse = await _service.GetAll(validFilter);

            return Ok(pagedResponse);
        }

        [HttpGet("GetAll")]
        // [Authorize(Policy = PolicyNames.ViewHRDs)]
        public async Task<ActionResult<IEnumerable<UserLookupDto>>> GetAll()
        {

            var results = await _service.GetAll();

            return Ok(results);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        // [Authorize(Policy = PolicyNames.ViewHRDs)]
        public async Task<ActionResult<UserViewModel>> GetUser(int id)
        {
            var user = await _service.GetUser(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
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

            if (await _service.IsUserUsed(model))
            {
                return BadRequest($"User name: {model.Name} with userId {model.UserId} already exists.");
            }

            await _service.UpdateUser(model);

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
