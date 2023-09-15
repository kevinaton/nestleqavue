using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HRD.WebApi.ViewModels;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using HRD.WebApi.Authorization;
using HRD.WebApi.Data;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using HRD.WebApi.Data.Entities;
using HRD.WebApi.Migrations;

namespace HRD.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly HRDContext _context;

        public RolesController(HRDContext context)
        {
            _context = context;
        }
    

        // GET: api/Roles
        [HttpGet]
        [Authorize(Policy = PolicyNames.ViewHRDs)]
        public async Task<ActionResult<IEnumerable<RoleViewModel>>> GetRoles([FromQuery] PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.SortColumn, filter.SortOrder, filter.SearchString);

            var query = _context.Roles
                .Select(s => new RoleViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    IsStatic = s.IsStatic
                });

            //sorting 
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
                case "isStatic":
                    query = validFilter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.IsStatic)
                        : query.OrderBy(o => o.IsStatic);
                    break;
            }

            //search
            if (!string.IsNullOrWhiteSpace(validFilter.SearchString))
            {
                query = query.Where(f => f.Name.Contains(filter.SearchString));
            }

            var totalRecords = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalRecords / validFilter.PageSize);
            //Pagination
            query = query.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize);

            var roles = await query.ToListAsync();


            return Ok(new PagedResponse<List<RoleViewModel>>(roles, validFilter.PageNumber, validFilter.PageSize, totalRecords, totalPages));
        }

        //GET: api/Roles/1
        [HttpGet("{id}")]
        [Authorize(Policy = PolicyNames.ViewHRDs)]
        public async Task<ActionResult<RoleViewModel>> GetRole(int id)
        {
            var role = await _context.Roles.Include(i => i.Permissions).FirstOrDefaultAsync(f => f.Id == id);

            if (role == null)
            {
                return NotFound();
            }

            var model = new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name,
                DisplayName = role.DisplayName,
                IsStatic = role.IsStatic,
                GrantedPermissionNames = role.Permissions.Where(s => s.IsGranted).Select(s => s.Name).ToList()
            };

            return model;
        }

        //PUT: api/Roles/1
        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.EditHRDs)]
        public async Task<ActionResult> PutRole(int id, RoleViewModel model)
        {
            if(id != model.Id)
            {
                return BadRequest();
            }

            var role = new Role
            {
                Id = model.Id,
                Name = model.Name,
                DisplayName = model.DisplayName,
                IsStatic = model.IsStatic
            };

            _context.Entry(role).State = EntityState.Modified;

            //Add Or Update Permission
            foreach(var permissionName in model.GrantedPermissionNames)
            {
                var permissionEntity = await _context.Permissions.FirstOrDefaultAsync(f => f.RoleId == model.Id && f.Name == permissionName);

                if(permissionEntity == null)
                {
                    var permission = new Permission
                    {
                        RoleId = model.Id,
                        Name = permissionName,
                        IsGranted = true
                    };

                    _context.Permissions.Add(permission);
                }
                else
                {
                    if(!permissionEntity.IsGranted)
                    {
                        permissionEntity.IsGranted = true;
                        _context.Entry(permissionEntity).State = EntityState.Modified;
                    }
                }                
            }

            //Delete Permission
            var toDeletePermissionList = await _context.Permissions.Where(f => f.RoleId == model.Id && !model.GrantedPermissionNames.Any(a => a == f.Name)).ToListAsync();
            foreach(var permission in toDeletePermissionList)
            {
                permission.IsGranted = false;
                _context.Entry(permission).State = EntityState.Modified;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleExists(id))
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

        [HttpPost]
        [Authorize(Policy = PolicyNames.EditHRDs)]
        public async Task<ActionResult<RoleViewModel>> PostRole(RoleViewModel model)
        {
            var role = new Role
            {
                Name = model.Name,
                DisplayName = model.DisplayName,
                IsStatic = model.IsStatic,
                Permissions = new List<Permission>()
            };

            foreach(var permissionName in model.GrantedPermissionNames)
            {
                var permission = new Permission
                {
                    Name = permissionName,
                    IsGranted = true,
                    RoleId = role.Id,
                };

                role.Permissions.Add(permission);
            }
            
            try
            {
                await _context.Roles.AddAsync(role);
                await _context.SaveChangesAsync();
            }
            catch(Exception)
            {
                throw;
            }
            
            model.Id = role.Id;
            return CreatedAtAction("GetRole", new { id = model.Id }, model);
        }

        // DELETE: api/Roles/1
        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyNames.EditHRDs)]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            try
            {
                var permissions = await _context.Permissions.Where(f => f.RoleId == role.Id).ToListAsync();
                _context.Permissions.RemoveRange(permissions);
                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
            

            return NoContent();
        }

        private bool RoleExists(int id)
        {
            return _context.Roles.Any(e => e.Id == id);
        }        
    }
}
