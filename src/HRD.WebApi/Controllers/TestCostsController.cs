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
    public class TestCostsController : ControllerBase
    {
        private readonly HRDContext _context;

        public TestCostsController(HRDContext context)
        {
            _context = context;
        }

        // GET: api/TestCosts
        [HttpGet]
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

            //Pagination;
            query = query.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                        .Take(validFilter.PageSize);

            var testCostList = await query.ToListAsync();

            var totalRecords = await _context.TestCosts.CountAsync();
            var totalPages = (totalRecords / validFilter.PageSize);


            return Ok(new PagedResponse<List<TestCostViewModel>>(testCostList, validFilter.PageNumber, validFilter.PageSize, totalRecords, totalPages));
        }

        // GET: api/TestCosts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TestCost>> GetTestCost(int id)
        {
            var testCost = await _context.TestCosts.FindAsync(id);

            if (testCost == null)
            {
                return NotFound();
            }

            return testCost;
        }

        // PUT: api/TestCosts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTestCost(int id, TestCost testCost)
        {
            if (id != testCost.Id)
            {
                return BadRequest();
            }

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
        public async Task<ActionResult<TestCost>> PostTestCost(TestCost testCost)
        {
            _context.TestCosts.Add(testCost);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTestCost", new { id = testCost.Id }, testCost);
        }

        // DELETE: api/TestCosts/5
        [HttpDelete("{id}")]
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
