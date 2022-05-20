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
    public class LaborCostsController : ControllerBase
    {
        private readonly HRDContext _context;

        public LaborCostsController(HRDContext context)
        {
            _context = context;
        }

        // GET: api/LaborCosts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LaborCostViewModel>>> GetLaborCosts([FromQuery]PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.SortColumn, filter.SortOrder, filter.SearchString);

            var query = _context.LaborCosts
                .Select(s => new LaborCostViewModel
                {
                    Year = s.Year,
                    LaborCosts = s.LaborCostValue
                });

            //Sorting
            switch (validFilter.SortColumn)
            {
                case "year":
                    query = validFilter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.Year)
                        : query.OrderBy(o => o.Year);
                    break;
                case "laborcost":
                    query = validFilter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.LaborCosts)
                        : query.OrderBy(o => o.LaborCosts);
                    break;
            }

            //Search
            if (!string.IsNullOrWhiteSpace(validFilter.SearchString))
            {
                query = query.Where(f => f.Year.Contains(filter.SearchString));
            }

            //Pagination;
            query = query.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                        .Take(validFilter.PageSize);

            var laborCostList = await query.ToListAsync();

            var totalRecords = await _context.Hrds.CountAsync();
            var totalPages = (totalRecords / validFilter.PageSize);


            return Ok(new PagedResponse<List<LaborCostViewModel>>(laborCostList, validFilter.PageNumber, validFilter.PageSize, totalRecords, totalPages));
        }

        // GET: api/LaborCosts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LaborCost>> GetLaborCost(string id)
        {
            var laborCost = await _context.LaborCosts.FindAsync(id);

            if (laborCost == null)
            {
                return NotFound();
            }

            return laborCost;
        }

        // PUT: api/LaborCosts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLaborCost(string id, LaborCost laborCost)
        {
            if (id != laborCost.Year)
            {
                return BadRequest();
            }

            _context.Entry(laborCost).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LaborCostExists(id))
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

        // POST: api/LaborCosts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LaborCost>> PostLaborCost(LaborCost laborCost)
        {
            _context.LaborCosts.Add(laborCost);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (LaborCostExists(laborCost.Year))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetLaborCost", new { id = laborCost.Year }, laborCost);
        }

        // DELETE: api/LaborCosts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLaborCost(string id)
        {
            var laborCost = await _context.LaborCosts.FindAsync(id);
            if (laborCost == null)
            {
                return NotFound();
            }

            _context.LaborCosts.Remove(laborCost);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LaborCostExists(string id)
        {
            return _context.LaborCosts.Any(e => e.Year == id);
        }
    }
}
