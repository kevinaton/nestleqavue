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

namespace HRD.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly HRDContext _context;

        public UsersController(HRDContext context)
        {
            _context = context;
        }
        
        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserViewModel>>> GetUsers()
        {
            return await _context.Users.Select(s=> new UserViewModel { 
                                                    Id=s.Id,
                                                    Name = s.Name,
                                                    UserId = s.UserId
                                                    }).ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
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
    }
}
