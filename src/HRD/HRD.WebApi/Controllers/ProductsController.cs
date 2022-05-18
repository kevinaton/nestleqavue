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
    public class ProductsController : ControllerBase
    {
        private readonly HRDContext _context;

        public ProductsController(HRDContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductsViewModel>>> GetProducts([FromQuery] PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.SortColumn, filter.SortOrder, filter.SearchString);

            var query = _context.Products
                        .Select(p => new ProductsViewModel
                        { 
                            Id = p.Id,
                            Year = p.Year,
                            Fert = string.Empty, //TODO: Not mapped
                            Gpn = p.Gpn,
                            Description = p.Description,
                            CostPerCase = p.CostPerCase,
                            Country = p.Country,
                            NoBbdate = p.NoBbdate,
                            Holiday = p.Holiday
                        });

            //Sorting
            switch (validFilter.SortColumn)
            {
                case "daycode":
                    query = validFilter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.Year)
                        : query.OrderBy(o => o.Year);
                    break;
                case "fert":
                    query = validFilter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.Fert)
                        : query.OrderBy(o => o.Fert);
                    break;
                case "description":
                    query = validFilter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.Description)
                        : query.OrderBy(o => o.Description);
                    break;
                case "costpercase":
                    query = validFilter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.CostPerCase)
                        : query.OrderBy(o => o.CostPerCase);
                    break;
                case "country":
                    query = validFilter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.Country)
                        : query.OrderBy(o => o.Country);
                    break;
                case "nobestbeforedate":
                    query = validFilter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.NoBbdate)
                        : query.OrderBy(o => o.NoBbdate);
                    break;
            }

            if (!string.IsNullOrEmpty(validFilter.SearchString))
            {
                query = query.Where(f => f.Year.Contains(filter.SearchString)
                                        || f.Fert.Contains(filter.SearchString)
                                        || f.Description.Contains(filter.SearchString)
                                        || f.Country.Contains(filter.SearchString));
            }

            //Pagination
            query = query.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize);

            var productList = await query.ToListAsync();
            var totalRecords = await _context.Products.CountAsync();
            var totalPages = (totalRecords / validFilter.PageSize);

            return Ok(new PagedResponse<List<ProductsViewModel>>(productList, validFilter.PageNumber, validFilter.PageSize, totalRecords, totalPages));
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
