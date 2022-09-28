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
            }

            if (!string.IsNullOrEmpty(validFilter.SearchString))
            {
                query = query.Where(f => f.Name.Contains(filter.SearchString)
                                        || f.UserId.Contains(filter.SearchString));
            }

            var totalRecords = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalRecords / validFilter.PageSize);

            //Pagination
            query = query.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize);

            var itemList = await query.ToListAsync();

            return Ok(new PagedResponse<List<UserViewModel>>(itemList, validFilter.PageNumber, validFilter.PageSize, totalRecords, totalPages));
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        // [Authorize(Policy = PolicyNames.ViewHRDs)]
        public async Task<ActionResult<UserViewModel>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var model = new UserViewModel
            {
                Id = id,
                Name = user.Name,
                UserId = user.UserId
            };

            return model;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        // [Authorize(Policy = PolicyNames.EditUsers)]
        public async Task<IActionResult> PutUser(int id, UserViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            var user = new User
            {
                Id = model.Id,
                UserId = model.UserId,
                Name = model.Name
            };

            _context.Entry(user).State = EntityState.Modified;

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
        // [Authorize(Policy = PolicyNames.EditUsers)]
        public async Task<ActionResult<UserViewModel>> PostUser(UserViewModel model)
        {
            var user = new User
            {
                Id = model.Id,
                Name = model.Name,
                UserId = model.UserId
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            model.Id = user.Id;
            return CreatedAtAction("GetUser", new { id = model.Id }, model);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        // [Authorize(Policy = PolicyNames.EditUsers)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
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
        public IActionResult GetCurrentUser()
        {
            var currentUser = User.Identities.First().Name;
            return Ok(currentUser);
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
        public async Task<bool> HasPermission(string permissionName)
        {
            var hasPermission = false;
            // var userId = User.Identities.First().Claims.First(f => f.Type == "UserId");
            var userId = 1;

            hasPermission = await _context.UserRoles.AnyAsync(a => a.Role.Permissions.Any(a => a.Name == permissionName && a.IsGranted));
            
            return hasPermission;
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
