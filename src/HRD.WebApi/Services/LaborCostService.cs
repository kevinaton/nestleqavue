using HRD.WebApi.Data;
using HRD.WebApi.Data.Entities;
using HRD.WebApi.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRD.WebApi.Services
{
    public class LaborCostService : ILaborCostService
    {
        private readonly HRDContext _context;

        public LaborCostService(HRDContext context)
        {
            _context = context;
        }

        public async Task DeleteLaborCost(string year)
        {
            try
            {
                var laborCost = await _context.LaborCosts.FindAsync(year);

                _context.LaborCosts.Remove(laborCost);
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
            
        }

        public async Task<LaborCostViewModel> GetLaborCost(string year)
        {
            var laborCost = await _context.LaborCosts.FirstOrDefaultAsync(f => f.Year == year);

            if(laborCost != null)
            {
                var model = new LaborCostViewModel
                {
                    Year = laborCost.Year,
                    LaborCost = laborCost.LaborCostValue
                };

                return model;
            }

            return null;
        }

        public async Task UpdateLaborCost(LaborCostViewModel model)
        {
            try
            {
                var laborCost = new LaborCost
                {
                    Year = model.Year,
                    LaborCostValue = model.LaborCost
                };

                _context.Entry(laborCost).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        public async Task CreateLaborCost(LaborCostViewModel model)
        {
            try
            {
                var laborCost = new LaborCost
                {
                    Year = model.Year,
                    LaborCostValue = model.LaborCost
                };

                _context.LaborCosts.Add(laborCost);

                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw;
            }
        }        

        public async Task<PagedResponse<List<LaborCostViewModel>>> GetAll(PaginationFilter filter)
        {
            var query = _context.LaborCosts
                .Select(s => new LaborCostViewModel
                {
                    Year = s.Year,
                    LaborCost = s.LaborCostValue
                });

            //Sorting
            switch (filter.SortColumn)
            {
                case "year":
                    query = filter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.Year)
                        : query.OrderBy(o => o.Year);
                    break;
                case "laborcost":
                    query = filter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.LaborCost)
                        : query.OrderBy(o => o.LaborCost);
                    break;
            }

            //Search
            if (!string.IsNullOrWhiteSpace(filter.SearchString))
            {
                query = query.Where(f => f.Year.Contains(filter.SearchString) || f.LaborCost.ToString().Contains(filter.SearchString));
            }

            var totalRecords = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalRecords / filter.PageSize);

            //Pagination;
            query = query.Skip((filter.PageNumber - 1) * filter.PageSize)
                        .Take(filter.PageSize);

            var laborCostList = await query.ToListAsync();

            var pageResponse = new PagedResponse<List<LaborCostViewModel>>(laborCostList, filter.PageNumber, filter.PageSize, totalRecords, totalPages);

            return pageResponse;
        }

        public async Task<bool> IsLaborCostExists(string id)
        {
            return await _context.LaborCosts.AnyAsync(e => e.Year == id);
        }

        public async Task<bool> IsLaborCostYearUsed(string year)
        {
            var result = await _context.Hrds.AnyAsync(a => a.LaborHours.HasValue && a.LaborHours.Value > 0 && a.YearHeld == year);            
            return result;
        }
    }
}
