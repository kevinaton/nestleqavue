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
    public class HrdFcsController : ControllerBase
    {
        private readonly HRDContext _context;

        public HrdFcsController(HRDContext context)
        {
            _context = context;
        }

        // GET: api/HrdFcs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HrdFCViewModel>>> GetHrdFcs(int hrdId)
        {
            var query = await _context.Hrdfcs.Where(s => s.Hrdid == hrdId)
                                            .Select(s => new HrdFCViewModel
                                            {
                                                Id = s.Id,
                                                HrdId = s.Hrdid,
                                                Location = s.Location,
                                                NumberOfCases = s.NumberOfCases,
                                            }).ToListAsync();

            if (query == null)
            {
                return NotFound();
            }

            return query;
        }

        // GET: api/HrdFcs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Hrdfc>> GetHrdfc(int id)
        {
            var hrdfc = await _context.Hrdfcs.FindAsync(id);

            if (hrdfc == null)
            {
                return NotFound();
            }

            return hrdfc;
        }

        // PUT: api/HrdFcs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHrdfc(int id, Hrdfc hrdfc)
        {
            if (id != hrdfc.Id)
            {
                return BadRequest();
            }

            _context.Entry(hrdfc).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HrdfcExists(id))
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

        // POST: api/HrdFcs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Hrdfc>> PostHrdfc(Hrdfc hrdfc)
        {
            _context.Hrdfcs.Add(hrdfc);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHrdfc", new { id = hrdfc.Id }, hrdfc);
        }

        // DELETE: api/HrdFcs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHrdfc(int id)
        {
            var hrdfc = await _context.Hrdfcs.FindAsync(id);
            if (hrdfc == null)
            {
                return NotFound();
            }

            _context.Hrdfcs.Remove(hrdfc);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HrdfcExists(int id)
        {
            return _context.Hrdfcs.Any(e => e.Id == id);
        }
    }
}
