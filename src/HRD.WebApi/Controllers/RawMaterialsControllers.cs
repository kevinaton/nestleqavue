using HRD.WebApi.Authorization;
using HRD.WebApi.Data;
using HRD.WebApi.Data.Entities;
using HRD.WebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace HRD.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RawMaterialsController : ControllerBase
    {
        private readonly HRDContext _context;

        public RawMaterialsController(HRDContext context)
        {
            _context = context;
        }

        // GET: api/RawMaterials
        [HttpGet]
        [Authorize(Policy = PolicyNames.ViewHRDs)]
        public async Task<ActionResult<IEnumerable<RawMaterialViewModel>>> GetRawMaterials([FromQuery] PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.SortColumn, filter.SortOrder, filter.SearchString);

            var query = _context.RawMaterials
                .Select(s => new RawMaterialViewModel
                {
                    Id = s.Id,
                    Description = s.Description
                });

            //sorting 
            switch (validFilter.SortColumn)
            {
                case "id":
                    query = validFilter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.Id)
                        : query.OrderBy(o => o.Id);
                    break;
                case "description":
                    query = validFilter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.Id)
                        : query.OrderBy(o =>o.Id);
                    break;
            }

            //search
            if (!string.IsNullOrWhiteSpace(validFilter.SearchString))
            {
                query = query.Where(f => f.Id.Contains(filter.SearchString) || f.Description.Contains(filter.SearchString));
            }

            var totalRecords = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalRecords / validFilter.PageSize);
            //Pagination
            query = query.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize);

            var Description = await query.ToListAsync();


            return Ok( new PagedResponse<List<RawMaterialViewModel>>(Description, validFilter.PageNumber, validFilter.PageSize, totalRecords, totalPages));
        }

        //GET: api/RawMaterials/5
        [HttpGet("{Id}")]
        [Authorize(Policy = PolicyNames.ViewHRDs)]
        public async Task<ActionResult<RawMaterialViewModel>> GetRawMaterial(string Id)
        {
            var rawMaterials = await _context.RawMaterials.FindAsync(Id);

            if(rawMaterials == null)
            {
                return NotFound();
            }

            var model = new RawMaterialViewModel
            {
                Id = rawMaterials.Id,
                Description = rawMaterials.Description
            };

            return model;
        }

        //PUT: api/RawMaterial/5
        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.EditHRDs)]
        public async Task<IActionResult> PutRawMaterial(string id, RawMaterialViewModel model)
        {
            model.Id = model.Id.TrimEnd();
            if (id != model.Id)
            {
                return BadRequest();
            }

            RawMaterial rawMaterials = new RawMaterial
            {
                Id = model.Id,
                Description = model.Description
            };

            _context.Entry(rawMaterials).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }

            catch (DbUpdateConcurrencyException)
            {
                if (!RawMaterialExists(id))
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

        //POST: api?RawMaterial/5
        [HttpPost]
        [Authorize(Policy = PolicyNames.EditHRDs)]
        public async Task<ActionResult<RawMaterialViewModel>> PostRawMaterial(RawMaterialViewModel model)
        {


            var Item = await _context.RawMaterials.Where(x =>
                x.Id == model.Id &&
                x.Description == model.Description
                ).FirstOrDefaultAsync();
           
            if(Item != null)
            {
                return BadRequest("Raw Materials already Exist");
            }


            var rawMaterial = new RawMaterial
            {
                Id = model.Id,
                Description = model.Description
            };

            try
            {
                _context.RawMaterials.Add(rawMaterial);

                if (rawMaterial.Id.Length > 8)
                {
                    throw new InvalidOperationException("Exceed 8 characters");
                }
                else if (string.IsNullOrEmpty(rawMaterial.Id))
                {
                    throw new InvalidOperationException("Field is empty");
                }
                else
                {
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return CreatedAtAction("GetRawMaterial", new { Id = model.Id }, model);
        }

        //DELETE: api/Rawmaterial/5
        [HttpDelete("{Id}")]
        [Authorize(Policy = PolicyNames.EditHRDs)]
        public async Task<IActionResult> DeleteRawMaterial(string Id)
        {
            var description = await _context.RawMaterials.FindAsync(Id);

            if (description == null)
            {
                return NotFound();
            }

            _context.RawMaterials.Remove(description);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool RawMaterialExists(string id)
        {
            return _context.RawMaterials.Any(c => c.Id == id);  
        }

        [HttpGet("Search/{id}")]
        [Authorize(Policy = PolicyNames.ViewHRDs)]
        public async Task<ActionResult<IEnumerable<RawMaterialViewModel>>> SearchRawMaterials(string id)
        {

            var rawMaterials = new List<RawMaterialViewModel>();
            rawMaterials = await _context.RawMaterials.Where(f => f.Id.Contains(id))
                                .Select(s => new RawMaterialViewModel
                                {
                                    Id = s.Id,
                                    Description = s.Description
                                }).ToListAsync();
            
            
                
            return rawMaterials; 
        }

        [HttpGet("Search")]
        [Authorize(Policy = PolicyNames.ViewHRDs)]
        public async Task<ActionResult<IEnumerable<RawMaterialViewModel>>> SearchRawMaterials()
        {

            
             var rawMaterials = await _context.RawMaterials.Select(s => new RawMaterialViewModel
                                {
                                    Id = s.Id,
                                    Description = s.Description
                                }).ToListAsync();


            return rawMaterials;
        }
    }
}
