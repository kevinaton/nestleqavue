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
using HRD.WebApi.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace HRD.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LookupController : ControllerBase
    {
        private readonly HRDContext _context;

        public LookupController(HRDContext context)
        {
            _context = context;
        }

        // GET: api/Lookup/types

        [HttpGet("types")]
        [Authorize(Policy = PolicyNames.ViewHRDs)]
        public async Task<ActionResult<IEnumerable<DropDownTypeViewModel>>> GetDropDownTypes()
        {
            return await _context.DropDownTypes.Select(s => new DropDownTypeViewModel
                                        {
                                            Id = s.Id,
                                            Name = s.Name,
                                        }).ToListAsync();
        }


        // GET: api/Lookup/items

        [HttpGet("items")]
        [Authorize(Policy = PolicyNames.ViewHRDs)]
        public async Task<ActionResult<IEnumerable<DropDownItemViewModel>>> GetDropDownItems([FromQuery] PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.SortColumn, filter.SortOrder, filter.SearchString);

            var query = _context.DropDownItems
                .Include(i => i.DropDownType)
                .Select(s => new DropDownItemViewModel
                {
                    Id = s.Id,
                    DropDownTypeId = s.DropDownTypeId,
                    IsActive = s.IsActive,
                    SortOrder = s.SortOrder,
                    Value = s.Value,
                    TypeName = s.DropDownType.Name
                });

            //Sorting
            switch (validFilter.SortColumn)
            {
                case "typename":
                    query = validFilter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.TypeName)
                        : query.OrderBy(o => o.TypeName);
                    break;
                case "value":
                    query = validFilter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.Value)
                        : query.OrderBy(o => o.Value);
                    break;
            }

            if (!string.IsNullOrEmpty(validFilter.SearchString))
            {
                query = query.Where(f => f.Value.Contains(filter.SearchString)
                                        || f.TypeName.Contains(filter.SearchString));
            }

            var totalRecords = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalRecords / validFilter.PageSize);

            //Pagination
            query = query.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize);

            var itemList = await query.ToListAsync();

            return Ok(new PagedResponse<List<DropDownItemViewModel>>(itemList, validFilter.PageNumber, validFilter.PageSize, totalRecords, totalPages));
        }

        // GET: api/Lookup/items/typeid/5
        [HttpGet("items/typeid/{id}")]
        [Authorize(Policy = PolicyNames.ViewHRDs)]
        public async Task<ActionResult<IEnumerable<DropDownItemViewModel>>> GetDropDownItemsByTypeId(int id)
        {
            var dropDownItems = await _context.DropDownItems.Where(f => f.DropDownTypeId == id)
                .Select(s => new DropDownItemViewModel
                {
                    Id = s.Id,
                    DropDownTypeId = s.DropDownTypeId,
                    IsActive = s.IsActive,
                    SortOrder = s.SortOrder,
                    Value = s.Value,
                }).ToListAsync();

            if (dropDownItems == null)
            {
                return NotFound();
            }

            return dropDownItems;
        }

        // GET: api/Lookup/items/5
        [HttpGet("items/{id}")]
        [Authorize(Policy = PolicyNames.ViewHRDs)]
        public async Task<ActionResult<DropDownItemViewModel>> GetDropDownItem(int id)
        {
            var ddItem = await _context.DropDownItems.FindAsync(id);

            if (ddItem == null)
            {
                return NotFound();
            }

            var model = new DropDownItemViewModel
            {
                Id = id,
                DropDownTypeId = ddItem.DropDownTypeId,
                Value = ddItem.Value,
                SortOrder = ddItem.SortOrder,
                IsActive = ddItem.IsActive
            };

            return model;
        }

        //Put: api/Lookup/items/5
        [HttpPut("items/{id}")]
        [Authorize(Policy = PolicyNames.EditHRDs)]
        public async Task<IActionResult> PutDropDownItem(int id, DropDownItemViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            var dropdownitem = new DropDownItem
            {
                Id = id,
                DropDownTypeId = model.DropDownTypeId,
                IsActive = model.IsActive,
                SortOrder = model.SortOrder,
                Value = model.Value
            };

            _context.Entry(dropdownitem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DropDownItemExists(id))
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

        // POST: api/Lookup/items
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("items/")]
        [Authorize(Policy = PolicyNames.EditHRDs)]
        public async Task<ActionResult<DropDownItemViewModel>> PostDropDownItem(DropDownItemViewModel model)
        {
            var dropdownitem = new DropDownItem
            {
                Id = model.Id,
                DropDownTypeId = model.DropDownTypeId,
                IsActive = model.IsActive,
                SortOrder = model.SortOrder,
                Value = model.Value
            };

            _context.DropDownItems.Add(dropdownitem);
            await _context.SaveChangesAsync();

            model.Id = dropdownitem.Id;
            return CreatedAtAction("GetDropDownItem", new { id = model.Id }, model);
        }

        // DELETE: api/Lookup/items/5
        [HttpDelete("items/{id}")]
        [Authorize(Policy = PolicyNames.EditHRDs)]
        public async Task<IActionResult> DeleteDropDownItem(int id)
        {
            var dropdownitem = await _context.DropDownItems.FindAsync(id);
            if (dropdownitem == null)
            {
                return NotFound();
            }

            _context.DropDownItems.Remove(dropdownitem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool DropDownItemExists(int id)
        {
            return _context.DropDownItems.Any(e => e.Id == id);
        }
    }
}
