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
    public class HrdPOsController : ControllerBase
    {
        private readonly HRDContext _context;

        public HrdPOsController(HRDContext context)
        {
            _context = context;
        }

        // GET: api/HrdPOs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HrdPoViewModel>>> GetHrdPos(int hrdId)
        {
            var query = await _context.Hrdpos.Where(s => s.Hrdid == hrdId)
                                            .Select(s => new HrdPoViewModel
                                            {
                                                Id = s.Id,
                                                HrdId = s.Hrdid,
                                                PONumber = s.Ponumber
                                            }).ToListAsync();

            if (query == null)
            {
                return NotFound();
            }

            return query;
        }

        // GET: api/HrdPOs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Hrdpo>> GetHrdpo(int id)
        {
            var hrdpo = await _context.Hrdpos.FindAsync(id);

            if (hrdpo == null)
            {
                return NotFound();
            }

            return hrdpo;
        }

        // PUT: api/HrdPOs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHrdpo(int id, Hrdpo hrdpo)
        {
            if (id != hrdpo.Id)
            {
                return BadRequest();
            }

            _context.Entry(hrdpo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HrdpoExists(id))
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

        // POST: api/HrdPOs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Hrdpo>> PostHrdpo(Hrdpo hrdpo)
        {
            _context.Hrdpos.Add(hrdpo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHrdpo", new { id = hrdpo.Id }, hrdpo);
        }

        // DELETE: api/HrdPOs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHrdpo(int id)
        {
            var hrdpo = await _context.Hrdpos.FindAsync(id);
            if (hrdpo == null)
            {
                return NotFound();
            }

            _context.Hrdpos.Remove(hrdpo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HrdpoExists(int id)
        {
            return _context.Hrdpos.Any(e => e.Id == id);
        }
    }
}
