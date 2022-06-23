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
    public class TestCostsController : ControllerBase
    {
        private readonly HRDContext _context;

        public TestCostsController(HRDContext context)
        {
            _context = context;
        }

        // GET: api/TestCosts
        [HttpGet]
        [Authorize(Policy = PolicyNames.ViewHRDs)]
        public async Task<ActionResult<IEnumerable<TestCostViewModel>>> GetTestCosts([FromQuery] PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.SortColumn, filter.SortOrder, filter.SearchString);

            var query = _context.TestCosts
                .Select(s => new TestCostViewModel
                {
                    Id = s.Id,
                    Year = s.Year,
                    TestCost = s.TestCostValue,
                    TestName = s.TestName
                });

            //Sorting
            switch (validFilter.SortColumn)
            {
                case "id":
                    query = validFilter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.Id)
                        : query.OrderBy(o => o.Id);
                    break;
                case "year":
                    query = validFilter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.Year)
                        : query.OrderBy(o => o.Year);
                    break;
                case "testname":
                    query = validFilter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.TestName)
                        : query.OrderBy(o => o.TestName);
                    break;
                case "testcost":
                    query = validFilter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.TestCost)
                        : query.OrderBy(o => o.TestCost);
                    break;
            }

            //Search
            if (!string.IsNullOrWhiteSpace(validFilter.SearchString))
            {
                query = query.Where(f => f.Year.Contains(filter.SearchString) || f.TestName.Contains(filter.SearchString));
            }

            var totalRecords = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalRecords / validFilter.PageSize);

            //Pagination;
            query = query.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                        .Take(validFilter.PageSize);

            var testCostList = await query.ToListAsync();

            return Ok(new PagedResponse<List<TestCostViewModel>>(testCostList, validFilter.PageNumber, validFilter.PageSize, totalRecords, totalPages));
        }

        // GET: api/TestCosts/5
        [HttpGet("{id}")]
        [Authorize(Policy = PolicyNames.ViewHRDs)]
        public async Task<ActionResult<TestCostViewModel>> GetTestCost(int id)
        {
            var testCost = await _context.TestCosts.FindAsync(id);

            if (testCost == null)
            {
                return NotFound();
            }

            var model = new TestCostViewModel
            {
                Id = id,
                TestCost = testCost.TestCostValue,
                TestName = testCost.TestName,
                Year = testCost.Year,
            };

            return model;
        }

        // PUT: api/TestCosts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.EditHRDs)]
        public async Task<IActionResult> PutTestCost(int id, TestCostViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            var testCost = new TestCost
            {
                Id = id,
                TestName = model.TestName,
                TestCostValue = model.TestCost,
                Year = model.Year
            };

            _context.Entry(testCost).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TestCostExists(id))
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

        // POST: api/TestCosts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Policy = PolicyNames.EditHRDs)]
        public async Task<ActionResult<TestCostViewModel>> PostTestCost(TestCostViewModel model)
        {
            var testCost = new TestCost
            {
                Id = model.Id,
                TestName = model.TestName,
                TestCostValue = model.TestCost,
                Year = model.Year
            };

            _context.TestCosts.Add(testCost);
            await _context.SaveChangesAsync();

            model.Id = testCost.Id;
            return CreatedAtAction("GetTestCost", new { id = model.Id }, model);
        }

        // DELETE: api/TestCosts/5
        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyNames.EditHRDs)]
        public async Task<IActionResult> DeleteTestCost(int id)
        {
            var testCost = await _context.TestCosts.FindAsync(id);
            if (testCost == null)
            {
                return NotFound();
            }

            _context.TestCosts.Remove(testCost);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TestCostExists(int id)
        {
            return _context.TestCosts.Any(e => e.Id == id);
        }
    }
}
