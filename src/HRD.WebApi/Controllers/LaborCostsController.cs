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
        // [Authorize(Policy = PolicyNames.ViewHRDs)]
        public async Task<ActionResult<IEnumerable<LaborCostViewModel>>> GetLaborCosts([FromQuery]PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.SortColumn, filter.SortOrder, filter.SearchString);

            var query = _context.LaborCosts
                .Select(s => new LaborCostViewModel
                {
                    Year = s.Year,
                    LaborCost = s.LaborCostValue
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
                        ? query.OrderByDescending(o => o.LaborCost)
                        : query.OrderBy(o => o.LaborCost);
                    break;
            }

            //Search
            if (!string.IsNullOrWhiteSpace(validFilter.SearchString))
            {
                query = query.Where(f => f.Year.Contains(filter.SearchString));
            }

            var totalRecords = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalRecords / validFilter.PageSize);

            //Pagination;
            query = query.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                        .Take(validFilter.PageSize);

            var laborCostList = await query.ToListAsync();

            return Ok(new PagedResponse<List<LaborCostViewModel>>(laborCostList, validFilter.PageNumber, validFilter.PageSize, totalRecords, totalPages));
        }

        // GET: api/LaborCosts/5
        [HttpGet("{year}")]
        // [Authorize(Policy = PolicyNames.ViewHRDs)]
        public async Task<ActionResult<LaborCostViewModel>> GetLaborCost(string year)
        {
            var laborCost = await _context.LaborCosts.FindAsync(year);

            if (laborCost == null)
            {
                return NotFound();
            }

            var model = new LaborCostViewModel
            {
                Year = laborCost.Year,
                LaborCost = laborCost.LaborCostValue
            };

            return model;
        }

        // PUT: api/LaborCosts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{year}")]
        [Authorize(Policy = PolicyNames.EditHRDs)]
        public async Task<IActionResult> PutLaborCost(string year, LaborCostViewModel model)
        {
            if (year != model.Year)
            {
                return BadRequest();
            }

            var laborCost = new LaborCost
            {
                Year = model.Year,
                LaborCostValue   = model.LaborCost
            };

            _context.Entry(laborCost).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LaborCostExists(year))
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
        // [Authorize(Policy = PolicyNames.EditHRDs)]
        public async Task<ActionResult<LaborCostViewModel>> PostLaborCost(LaborCostViewModel model)
        {
            var laborCost = new LaborCost
            {
                Year = model.Year,
                LaborCostValue = model.LaborCost
            };

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

            return CreatedAtAction("GetLaborCost", new { year = model.Year }, model);
        }

        // DELETE: api/LaborCosts/5
        [HttpDelete("{year}")]
        // [Authorize(Policy = PolicyNames.EditHRDs)]
        public async Task<IActionResult> DeleteLaborCost(string year)
        {
            var laborCost = await _context.LaborCosts.FindAsync(year);
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
