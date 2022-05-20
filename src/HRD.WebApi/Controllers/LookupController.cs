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
    public class LookupController : ControllerBase
    {
        private readonly HRDContext _context;

        public LookupController(HRDContext context)
        {
            _context = context;
        }

        // GET: api/Lookup/types

        [HttpGet("/types")]
        public async Task<ActionResult<IEnumerable<DropDownTypeViewModel>>> GetDropDownTypes()
        {
            return await _context.DropDownTypes.Select(s => new DropDownTypeViewModel
                                        {
                                            Id = s.Id,
                                            Name = s.Name,
                                        }).ToListAsync();
        }


        // GET: api/Lookup/items

        [HttpGet("/items")]
        public async Task<ActionResult<IEnumerable<DropDownItemViewModel>>> GetDropDownItems()
        {
            return await _context.DropDownItems
                .Include(i => i.DropDownType)
                .Take(20)
                .Select(s => new DropDownItemViewModel
                {
                    Id = s.Id,
                    DropDownTypeId = s.DropDownTypeId,
                    IsActive = s.IsActive,
                    SortOrder = s.SortOrder,
                    Value = s.Value,
                    Type = new DropDownTypeViewModel
                    {
                        Id = s.DropDownType.Id,
                        Name = s.DropDownType.Name
                    }
                }).ToListAsync();
        }

        // GET: api/Lookup/5
        [HttpGet("item/{id}")]
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
        // GET: api/Lookup/5
        [HttpGet("type/{id}")]
        public async Task<ActionResult<DropDownTypeViewModel>> GetDropDownType(int id)
        {
            var dropDownType = await _context.DropDownTypes.Include(i => i.DropDownItems).FirstOrDefaultAsync(a => a.Id == id);

            if (dropDownType == null)
            {
                return NotFound();
            }

            var model = new DropDownTypeViewModel
            {
                Id = id,
                Name = dropDownType.Name,
                DropDownItems = dropDownType.DropDownItems.Select(s => new DropDownItemViewModel
                {
                    Id = s.Id,
                    DropDownTypeId = s.DropDownTypeId,
                    IsActive = s.IsActive,
                    SortOrder = s.SortOrder,
                    Value = s.Value
                }).ToList()
            };

            return model;
        }

        //Post: api/Lookup/5
        [HttpPut("item/{id}")]
        public async Task<IActionResult> PutDropDownItem(int id, DropDownItemViewModel model)
        {

            if (id != model.Id)
            {
                return BadRequest();
            }

            var dropdownitem = new DropDownItem
            {
                Id=id,
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

        // POST: api/Lookup
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
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

        // DELETE: api/Lookup/5
        [HttpDelete("item/{id}")]
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
