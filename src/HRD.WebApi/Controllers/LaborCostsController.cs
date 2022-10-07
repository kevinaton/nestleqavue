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
using HRD.WebApi.Services;

namespace HRD.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LaborCostsController : ControllerBase
    {
        private readonly ILaborCostService _service;

        public LaborCostsController(HRDContext context, ILaborCostService service)
        {
            _service = service;
        }

        // GET: api/LaborCosts
        [HttpGet]
        [Authorize(Policy = PolicyNames.ViewHRDs)]
        public async Task<ActionResult<IEnumerable<LaborCostViewModel>>> GetLaborCosts([FromQuery]PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.SortColumn, filter.SortOrder, filter.SearchString);

            var pagedResponse = await _service.GetAll(validFilter);

            return Ok(pagedResponse);
        }

        // GET: api/LaborCosts/5
        [HttpGet("{year}")]
        [Authorize(Policy = PolicyNames.ViewHRDs)]
        public async Task<ActionResult<LaborCostViewModel>> GetLaborCost(string year)
        {
            var laborCost = await _service.GetLaborCost(year);

            if (laborCost == null)
            {
                return NotFound();
            }

            return laborCost;
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

            if (!await LaborCostExists(year))
            {
                return NotFound();
            }

            await _service.UpdateLaborCost(model);

            return NoContent();
        }

        // POST: api/LaborCosts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Policy = PolicyNames.EditHRDs)]
        public async Task<ActionResult<LaborCostViewModel>> PostLaborCost(LaborCostViewModel model)
        {
            
            try
            {
                await _service.CreateLaborCost(model);
            }
            catch (DbUpdateException)
            {
                if (await LaborCostExists(model.Year))
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
        [Authorize(Policy = PolicyNames.EditHRDs)]
        public async Task<IActionResult> DeleteLaborCost(string year)
        {
            var laborCost = await _service.GetLaborCost(year);
            if (laborCost == null)
            {
                return NotFound();
            }

            await _service.DeleteLaborCost(laborCost);

            return NoContent();
        }

        private async Task<bool> LaborCostExists(string id)
        {
            return await _service.IsLaborCostExists(id);
        }
    }
}
