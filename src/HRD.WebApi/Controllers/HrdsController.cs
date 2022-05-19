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
    public class HrdsController : ControllerBase
    {
        private readonly HRDContext _context;

        public HrdsController(HRDContext context)
        {
            _context = context;
        }

        // GET: api/Hrds
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QAListViewModel>>> GetHrds([FromQuery] PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.SortColumn, filter.SortOrder, filter.SearchString);

            var query = _context.Hrds
                .Select(s => new QAListViewModel
                {
                    Id = s.Id,
                    DayCode = s.DayCode,
                    Type = s.CodingType, //TODO: Confirm if correct
                    Fert = String.Empty, //TODO: Not mapped
                    ProductDescription = s.ShortDescription, //TODO: Confirm if correct
                    Line = s.Line,
                    Shift = s.Shift,
                    HourCode = s.HourCode,
                    Cases = s.Cases,
                    ShortDescription = s.ShortDescription,
                    Originator = s.Originator
                });

            //Sorting
            switch (validFilter.SortColumn)
            {
                case "daycode":
                    query = validFilter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.DayCode)
                        : query.OrderBy(o => o.DayCode);
                    break;
                case "type":
                    query = validFilter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.Type)
                        : query.OrderBy(o => o.Type);
                    break;
                case "fert":
                    query = validFilter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.Fert)
                        : query.OrderBy(o => o.Fert);
                    break;
                case "productdescription":
                    query = validFilter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.ProductDescription)
                        : query.OrderBy(o => o.ProductDescription);
                    break;
                case "line":
                    query = validFilter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.Line)
                        : query.OrderBy(o => o.Line);
                    break;
                case "shift":
                    query = validFilter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.Shift)
                        : query.OrderBy(o => o.Shift);
                    break;
                case "hourcode":
                    query = validFilter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.HourCode)
                        : query.OrderBy(o => o.HourCode);
                    break;
                case "cases":
                    query = validFilter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.Cases)
                        : query.OrderBy(o => o.Cases);
                    break;
                case "shortdescription":
                    query = validFilter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.ShortDescription)
                        : query.OrderBy(o => o.ShortDescription);
                    break;
                case "originator":
                    query = validFilter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.Originator)
                        : query.OrderBy(o => o.Originator);
                    break;
            }

            //Search
            if (!string.IsNullOrWhiteSpace(validFilter.SearchString))
            {
                query = query.Where(f => f.DayCode.Contains(filter.SearchString)
                                        || f.ProductDescription.Contains(filter.SearchString)
                                        || f.ShortDescription.Contains(filter.SearchString)
                                        || f.Fert.Contains(filter.SearchString)
                                        || f.Originator.Contains(filter.SearchString));
            }

            //Pagination;
            query = query.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                        .Take(validFilter.PageSize);

            var hrdList = await query.ToListAsync();

            var totalRecords = await _context.Hrds.CountAsync();
            var totalPages = (totalRecords / validFilter.PageSize);


            return Ok(new PagedResponse<List<QAListViewModel>>(hrdList, validFilter.PageNumber, validFilter.PageSize, totalRecords, totalPages));
        }

        // GET: api/Hrds/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Hrd>> GetHrd(int id)
        {
            var hrd = await _context.Hrds.FindAsync(id);

            if (hrd == null)
            {
                return NotFound();
            }

            return hrd;
        }

        // PUT: api/Hrds/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHrd(int id, Hrd hrd)
        {
            if (id != hrd.Id)
            {
                return BadRequest();
            }

            _context.Entry(hrd).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HrdExists(id))
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

        // POST: api/Hrds
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Hrd>> PostHrd(Hrd hrd)
        {
            _context.Hrds.Add(hrd);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHrd", new { id = hrd.Id }, hrd);
        }

        // DELETE: api/Hrds/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHrd(int id)
        {
            var hrd = await _context.Hrds.FindAsync(id);
            if (hrd == null)
            {
                return NotFound();
            }

            _context.Hrds.Remove(hrd);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HrdExists(int id)
        {
            return _context.Hrds.Any(e => e.Id == id);
        }
    }
}
