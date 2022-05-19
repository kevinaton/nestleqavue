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
    public class HrdNotesController : ControllerBase
    {
        private readonly HRDContext _context;

        public HrdNotesController(HRDContext context)
        {
            _context = context;
        }

        // GET: api/HrdNotes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HrdNoteViewModel>>> GetHrdNotes(int hrdId)
        {
            var query = await _context.Hrdnotes.Where(s => s.Hrdid == hrdId)
                                            .Select(s => new HrdNoteViewModel
                                            {
                                                Id = s.Id,
                                                HrdId = s.Hrdid,
                                                Category = s.Category,
                                                Description = s.Description,
                                                UserId = s.UserId,
                                                Date = s.Date,
                                                Filename = s.FileName,
                                                Path = s.Path,
                                                Size = s.Size,
                                            }).ToListAsync();

            if (query == null)
            {
                return NotFound();
            }

            return query;
        }

        // GET: api/HrdNotes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Hrdnote>> GetHrdnote(int id)
        {
            var hrdnote = await _context.Hrdnotes.FindAsync(id);

            if (hrdnote == null)
            {
                return NotFound();
            }

            return hrdnote;
        }

        // PUT: api/HrdNotes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHrdnote(int id, Hrdnote hrdnote)
        {
            if (id != hrdnote.Id)
            {
                return BadRequest();
            }

            _context.Entry(hrdnote).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HrdnoteExists(id))
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

        // POST: api/HrdNotes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Hrdnote>> PostHrdnote(Hrdnote hrdnote)
        {
            _context.Hrdnotes.Add(hrdnote);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHrdnote", new { id = hrdnote.Id }, hrdnote);
        }

        // DELETE: api/HrdNotes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHrdnote(int id)
        {
            var hrdnote = await _context.Hrdnotes.FindAsync(id);
            if (hrdnote == null)
            {
                return NotFound();
            }

            _context.Hrdnotes.Remove(hrdnote);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HrdnoteExists(int id)
        {
            return _context.Hrdnotes.Any(e => e.Id == id);
        }
    }
}
