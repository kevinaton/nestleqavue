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
                case "id":
                    query = validFilter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.Id)
                        : query.OrderBy(o => o.Id);
                    break;
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

        // GET: api/Lookup/items/typename/category
        [HttpGet("items/typename/{name}")]
        [Authorize(Policy = PolicyNames.ViewHRDs)]
        public async Task<ActionResult<IEnumerable<DropDownItemViewModel>>> GetDropDownItemsByType(string name)
        {
            if(string.IsNullOrEmpty(name))
            {
                return BadRequest();
            }

            var dropDownItems = await _context.DropDownItems.Where(f => f.DropDownType.Name.ToLower() == name.ToLower().Trim())
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

            var item = await _context.DropDownItems.Where(x =>

                   x.Value == model.Value &&
                   x.DropDownTypeId == model.DropDownTypeId

               ).FirstOrDefaultAsync(); 

               if(item != null)
               {
              
                   return BadRequest("Lookup already Exist");

               }

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
            var dropdownitem = await _context.DropDownItems.Include(i => i.DropDownType).FirstOrDefaultAsync(f => f.Id == id);
            if (dropdownitem == null)
            {
                return NotFound();
            }

            var typeName = dropdownitem.DropDownType.Name;
            switch (typeName)
            {
                case "Line":
                    if (await _context.Hrds.AnyAsync(a => a.Line == dropdownitem.Value))
                    {
                        return BadRequest($"Cannot delete Lookup of Type: {typeName} with value :{dropdownitem.Value}. It is being used in HRD Record");
                    }
                    break;
                case "CodingType":
                    if (await _context.Hrds.AnyAsync(a => a.CodingType == dropdownitem.Value))
                    {
                        return BadRequest($"Cannot delete Lookup of Type: {typeName} with value :{dropdownitem.Value}. It is being used in HRD Record");
                    }
                    break;
                case "Area":
                    if (await _context.Hrds.AnyAsync(a => a.Area == dropdownitem.Value))
                    {
                        return BadRequest($"Cannot delete Lookup of Type: {typeName} with value :{dropdownitem.Value}. It is being used in HRD Record");
                    }
                    break;
                case "Shift":
                    if (await _context.Hrds.AnyAsync(a => a.Shift == dropdownitem.Value))
                    {
                        return BadRequest($"Cannot delete Lookup of Type: {typeName} with value :{dropdownitem.Value}. It is being used in HRD Record");
                    }
                    break;
                case "Pest Type":
                    if (await _context.Hrds.AnyAsync(a => a.PestType == dropdownitem.Value))
                    {
                        return BadRequest($"Cannot delete Lookup of Type: {typeName} with value :{dropdownitem.Value}. It is being used in HRD Record");
                    }
                    break;
                case "FM Type":
                    if (await _context.Hrds.AnyAsync(a => a.Fmtype == dropdownitem.Value))
                    {
                        return BadRequest($"Cannot delete Lookup of Type: {typeName} with value :{dropdownitem.Value}. It is being used in HRD Record");
                    }
                    break;
                case "Equipment":
                    if (await _context.Hrds.AnyAsync(a => a.Equipment == dropdownitem.Value))
                    {
                        return BadRequest($"Cannot delete Lookup of Type: {typeName} with value :{dropdownitem.Value}. It is being used in HRD Record");
                    }
                    break;
                case "NR Category":
                    if (await _context.Hrds.AnyAsync(a => a.Nrcategory == dropdownitem.Value))
                    {
                        return BadRequest($"Cannot delete Lookup of Type: {typeName} with value :{dropdownitem.Value}. It is being used in HRD Record");
                    }
                    break;
                case "Tagged":
                    if (await _context.Hrds.AnyAsync(a => a.Tagged == dropdownitem.Value))
                    {
                        return BadRequest($"Cannot delete Lookup of Type: {typeName} with value :{dropdownitem.Value}. It is being used in HRD Record");
                    }
                    break;
                case "Hold or Concern":
                    if (await _context.Hrds.AnyAsync(a => a.HoldConcern == dropdownitem.Value))
                    {
                        return BadRequest($"Cannot delete Lookup of Type: {typeName} with value :{dropdownitem.Value}. It is being used in HRD Record");
                    }
                    break;
                case "When":
                    if (await _context.Hrds.AnyAsync(a => a.When == dropdownitem.Value))
                    {
                        return BadRequest($"Cannot delete Lookup of Type: {typeName} with value :{dropdownitem.Value}. It is being used in HRD Record");
                    }
                    break;
                case "Day of Week":
                    if (await _context.Hrds.AnyAsync(a => a.DayOfWeek == dropdownitem.Value))
                    {
                        return BadRequest($"Cannot delete Lookup of Type: {typeName} with value :{dropdownitem.Value}. It is being used in HRD Record");
                    }
                    break;
                case "Sauce Type":
                    if (await _context.Hrds.AnyAsync(a => a.SauceType == dropdownitem.Value))
                    {
                        return BadRequest($"Cannot delete Lookup of Type: {typeName} with value :{dropdownitem.Value}. It is being used in HRD Record");
                    }
                    break;
                case "Starch Type":
                    if (await _context.Hrds.AnyAsync(a => a.SauceType == dropdownitem.Value))
                    {
                        return BadRequest($"Cannot delete Lookup of Type: {typeName} with value :{dropdownitem.Value}. It is being used in HRD Record");
                    }
                    break;
                case "Organism":
                    if (await _context.HrdMicros.AnyAsync(a => a.Organism == dropdownitem.Value))
                    {
                        return BadRequest($"Cannot delete Lookup of Type: {typeName} with value :{dropdownitem.Value}. It is being used in HRD Record");
                    }
                    break;
            }

            

            



            

            _context.DropDownItems.Remove(dropdownitem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool DropDownItemExists(int id)
        {
            return _context.DropDownItems.Any(e => e.Id == id);
        }

        // GET: api/Lookup/items/typeid/5
        [HttpGet("items/typeid/{id}/{search}")]
        [Authorize(Policy = PolicyNames.ViewHRDs)]
        public async Task<ActionResult<IEnumerable<DropDownItemViewModel>>> SearchDropDownItemsByTypeId(int id, string search)
        {

            if (string.IsNullOrWhiteSpace(search) || search.Trim().Length < 3)
            {
                return BadRequest("Search string should have 3 or more characters");
            }
            var dropDownItems = await _context.DropDownItems.Where(f => f.DropDownTypeId == id && f.Value.Contains(search))
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
    }
}
