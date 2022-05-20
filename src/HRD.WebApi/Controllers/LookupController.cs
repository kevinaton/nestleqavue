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

        // GET: api/Lookup
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DropDownTypeViewModel>>> GetDropDownTypes()
        {
            return await _context.DropDownTypes.Select(s => new DropDownTypeViewModel
                                        {
                                            Id = s.Id,
                                            Name = s.Name,
                                        }).ToListAsync();
        }

        // GET: api/Lookup/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<DropDownItemViewModel>>> GetDropDownItem(int id)
        {
            var dropDownItem = await _context.DropDownItems.Where(a => a.DropDownTypeId == id)
                                                            .Select(s => new DropDownItemViewModel { 
                                                                        Id = s.Id,
                                                                        DropDownTypeId = s.DropDownTypeId,
                                                                        Value = s.Value,
                                                                        SortOrder = s.SortOrder,
                                                                        IsActive = s.IsActive
                                                            })
                                                            .OrderBy(o => o.SortOrder)
                                                            .ToListAsync();

            if (dropDownItem == null)
            {
                return NotFound();
            }

            return dropDownItem;
        }
    }
}
