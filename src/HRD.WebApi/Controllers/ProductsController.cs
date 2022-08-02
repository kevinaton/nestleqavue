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
    public class ProductsController : ControllerBase
    {
        private readonly HRDContext _context;

        public ProductsController(HRDContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        [Authorize(Policy = PolicyNames.ViewHRDs)]
        public async Task<ActionResult<IEnumerable<ProductViewModel>>> GetProducts([FromQuery] PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.SortColumn, filter.SortOrder, filter.SearchString);

            var query = _context.Products
                        .Select(p => new ProductViewModel
                        { 
                            Id = p.Id,
                            Year = p.Year,
                            Fert = p.Gpn,
                            Description = p.Description,
                            CostPerCase = p.CostPerCase,
                            Country = p.Country,
                            NoBbdate = p.NoBbdate,
                            Holiday = p.Holiday
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

            var totalRecords = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalRecords / validFilter.PageSize);

            //Pagination
            query = query.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize);

            var productList = await query.ToListAsync();

            return Ok(new PagedResponse<List<ProductViewModel>>(productList, validFilter.PageNumber, validFilter.PageSize, totalRecords, totalPages));
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        [Authorize(Policy = PolicyNames.ViewHRDs)]
        public async Task<ActionResult<ProductViewModel>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            var model = new ProductViewModel
            {
                Id = id,
                Year = product.Year,
                Fert = product.Gpn,
                Description = product.Description,
                Country = product.Country,
                CostPerCase = product.CostPerCase,
                Holiday = product.Holiday,
                NoBbdate = product.NoBbdate,
            };

            return model;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.EditHRDs)]
        public async Task<IActionResult> PutProduct(int id, ProductViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            var product = new Product
            {
                Id = id,
                Year = model.Year,
                Gpn = model.Fert,
                Description = model.Description,
                Country = model.Country,
                CostPerCase = model.CostPerCase,
                Holiday = model.Holiday,
                NoBbdate = model.NoBbdate,
            };

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
        [Authorize(Policy = PolicyNames.EditHRDs)]
        public async Task<ActionResult<ProductViewModel>> PostProduct(ProductViewModel model)
        {
            var product = new Product
            {
                Year = model.Year,
                Gpn = model.Fert,
                Description = model.Description,
                Country = model.Country,
                CostPerCase = model.CostPerCase,
                Holiday = model.Holiday,
                NoBbdate = model.NoBbdate,
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            model.Id = product.Id;
            return CreatedAtAction("GetProduct", new { id = model.Id }, model);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyNames.EditHRDs)]
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
