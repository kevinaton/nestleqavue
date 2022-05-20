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
    public class HrdDcsController : ControllerBase
    {
        private readonly HRDContext _context;

        public HrdDcsController(HRDContext context)
        {
            _context = context;
        }

        // GET: api/HrdDc
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HrdDCViewModel>>> GetHrdDcs(int hrdId)
        {
            var query = await _context.Hrddcs.Where(a => a.Hrdid == hrdId)
                                            .Select(s => new HrdDCViewModel
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

        // GET: api/HrdDc/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Hrddc>> GetHrddc(int id)
        {
            var hrddc = await _context.Hrddcs.FindAsync(id);

            if (hrddc == null)
            {
                return NotFound();
            }

            return hrddc;
        }

        // PUT: api/HrdDc/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHrddc(int id, Hrddc hrddc)
        {
            if (id != hrddc.Id)
            {
                return BadRequest();
            }

            _context.Entry(hrddc).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HrddcExists(id))
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

        // POST: api/HrdDc
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Hrddc>> PostHrddc(Hrddc hrddc)
        {
            _context.Hrddcs.Add(hrddc);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHrddc", new { id = hrddc.Id }, hrddc);
        }

        // DELETE: api/HrdDc/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHrddc(int id)
        {
            var hrddc = await _context.Hrddcs.FindAsync(id);
            if (hrddc == null)
            {
                return NotFound();
            }

            _context.Hrddcs.Remove(hrddc);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HrddcExists(int id)
        {
            return _context.Hrddcs.Any(e => e.Id == id);
        }
    }
}
