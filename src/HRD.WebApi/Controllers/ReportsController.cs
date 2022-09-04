using HRD.WebApi.Authorization;
using HRD.WebApi.Data;
using HRD.WebApi.Extensions;
using HRD.WebApi.ViewModels;
using HRD.WebApi.ViewModels.Enums;
using HRD.WebApi.ViewModels.Report;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace HRD.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly HRDContext _context;

        public ReportsController(HRDContext context)
        {
            _context = context;
        }

        public class ActivePeriod
        {
            public DateTime? PeriodFrom { get; set; }
            public DateTime PeriodTo { get; set; }
            public int Number { get; set; }
        }
        public class FMCasesOnDate
        {
            public DateTime? DateTime { get; set; }
            public decimal FmCases { get; set; }
        }

        // GET: api/Reports
        [HttpGet("CasesCostByCategory")]
        // [Authorize(Policy = PolicyNames.ViewHRDs)]
        public async Task<ActionResult<IEnumerable<CasesCostHeldByCategoryOutput>>> GetCasesCostByCategoryReport([FromQuery] ReportPaginationFilter filter)
        {
            var validFilter = new ReportPaginationFilter(filter.PageNumber, filter.PageSize, filter.SortColumn, filter.SortOrder);

            var query = _context.Hrds
                            .GroupBy(g => new
                            {
                                g.DateHeld,
                                g.YearHeld,
                                g.HoldCategory,
                                g.Line,
                                g.Complete
                            })
                            .Select(s => new CasesCostHeldByCategoryOutput
                            {
                                TotalCases = s.Sum(s => s.Cases),
                                TotalCost = s.Sum(s => s.CostofProductonHold),
                                Line = s.Key.Line,
                            });

            //Sorting
            switch (validFilter.SortColumn)
            {
                case "line":
                    query = validFilter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.Line)
                        : query.OrderBy(o => o.Line);
                    break;
                case "totalcases":
                    query = validFilter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.TotalCases)
                        : query.OrderBy(o => o.TotalCases);
                    break;
                case "totalcost":
                    query = validFilter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.TotalCost)
                        : query.OrderBy(o => o.TotalCost);
                    break;
            }

            var totalRecords = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalRecords / validFilter.PageSize);

            //Pagination
            query = query.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize);

            var allCasesCostHeld = await query.ToListAsync();

            return Ok(new PagedResponse<List<CasesCostHeldByCategoryOutput>>(allCasesCostHeld, validFilter.PageNumber, validFilter.PageSize, totalRecords, totalPages));
        }

        // GET: api/Reports
        [HttpGet("CostHeldByCategory")]
        // [Authorize(Policy = PolicyNames.ViewHRDs)]
        public async Task<ActionResult> GetCostHeldByCategoryGraphData([FromQuery] GetCasesCostHeldByCategoryInput input)
        {
            var query = _context.Hrds.Where(x => x.DateHeld >= input.PeriodBegin && x.DateHeld <= input.PeriodEnd && !string.IsNullOrWhiteSpace(x.HoldCategory)
                                                    && (input.Status == EnumStatus.All
                                                        || (input.Status == EnumStatus.Closed && x.Complete.HasValue && x.Complete.Value)
                                                        || (input.Status == EnumStatus.Open && (!x.Complete.HasValue || !x.Complete.Value)))
                                                    && (!string.IsNullOrEmpty(input.Line) && x.Line == input.Line)
                                                    && (!string.IsNullOrEmpty(input.WeekHeld.ToString()) || input.WeekHeld == EnumWeekHeld.Select
                                                            || (x.DateHeld.HasValue && input.WeekHeld.ToString() == x.DateHeld.Value.DayOfWeek.ToString("F"))));

            switch (input.CostGraphOption)
            {
                case EnumCostGraph.CostByCategory:
                    var queryByCategory = await query
                                                    .GroupBy(g => new
                                                    {
                                                        g.HoldCategory,
                                                    }) 
                                                    .Select(s => new
                                                    {
                                                        s.Key.HoldCategory,
                                                        TotalCost = s.Sum(a => a.CostofProductonHold),
                                                    }).ToListAsync();
                    return Ok(queryByCategory);

                case EnumCostGraph.CostByAllocation:
                    var queryByAllocation = await query
                                                    .GroupBy(g => new
                                                    {
                                                        g.HoldCategory,
                                                    })
                                                    .Select(s => new
                                                    {
                                                        s.Key.HoldCategory,
                                                        TotalCost = s.Sum(a => a.CostofProductonHold),
                                                    }).ToListAsync();
                    return Ok(queryByAllocation);
            }

            return Ok();
        }

        // GET: api/Reports
        [HttpGet("CasesHeldByCategory")]
        // [Authorize(Policy = PolicyNames.ViewHRDs)]
        public async Task<ActionResult> GetCasesHeldByCategoryGraphData([FromQuery] GetCasesCostHeldByCategoryInput input)
        {
            var query = _context.Hrds.Where(x => x.DateHeld >= input.PeriodBegin && x.DateHeld <= input.PeriodEnd && !string.IsNullOrWhiteSpace(x.HoldCategory)
                                                    && (input.Status == EnumStatus.All
                                                        || (input.Status == EnumStatus.Closed && x.Complete.HasValue && x.Complete.Value)
                                                        || (input.Status == EnumStatus.Open && (!x.Complete.HasValue || !x.Complete.Value)))
                                                    && (!string.IsNullOrEmpty(input.Line) && x.Line == input.Line)
                                                    && (!string.IsNullOrEmpty(input.WeekHeld.ToString()) || input.WeekHeld == EnumWeekHeld.Select
                                                            || (x.DateHeld.HasValue && input.WeekHeld.ToString() == x.DateHeld.Value.DayOfWeek.ToString("F"))));
            switch (input.CostGraphOption)
            {
                case EnumCostGraph.CostByCategory:
                    var queryByCategory = await query
                                                    .GroupBy(g => new
                                                    {
                                                        MonthHeld = g.DateHeld.Value.Month
                                                    })
                                                    .Select(s => new
                                                    {
                                                        MonthHeld = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(s.Key.MonthHeld),
                                                        TotalCost = s.Sum(a => a.Cases),
                                                    }).ToListAsync();
                    return Ok(queryByCategory);

                case EnumCostGraph.CostByAllocation:
                    var queryByAllocation = await query
                                                    .GroupBy(g => new
                                                    {
                                                        MonthHeld = g.DateHeld.Value.Month
                                                    })
                                                    .Select(s => new
                                                    {
                                                        MonthHeld = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(s.Key.MonthHeld),
                                                        TotalCost = s.Sum(a => a.Cases),
                                                    }).ToListAsync();
                    return Ok(queryByAllocation);
            }

            return Ok();
        }

        // GET: api/Reports
        [HttpGet("FMCases")]
        // [Authorize(Policy = PolicyNames.ViewHRDs)]
        public async Task<ActionResult> GetFMCasesGraphData([FromQuery]GetFMCasesGraphDataInput input)
        {
            var query = _context.Hrds.Where(x => x.DateHeld >= input.PeriodBegin && x.DateHeld <= input.PeriodEnd && !string.IsNullOrWhiteSpace(x.Fmtype)
                                                    && (input.Status == EnumStatus.All
                                                            || (input.Status == EnumStatus.Closed && x.Complete.HasValue && x.Complete.Value)
                                                            || (input.Status == EnumStatus.Open && (!x.Complete.HasValue || !x.Complete.Value))));
            switch (input.CasesOption)
            {
                case EnumFMCasesOptions.ByCategory:
                    var queryByCategory = await query
                                                .GroupBy(g => new
                                                {
                                                    g.Fmtype,
                                                })
                                                .Select(s => new
                                                {
                                                    Type = s.Key.Fmtype,
                                                    NumberOfCases = s.Sum(a => a.Cases),
                                                }).ToListAsync();
                    return Ok(queryByCategory);

                case EnumFMCasesOptions.ByInhouseVendor:
                    var queryByInHouse = await query
                                                .GroupBy(g => new
                                                {
                                                    g.FMSource,
                                                })
                                                .Select(s => new
                                                {
                                                    Type = s.Key.FMSource,
                                                    NumberOfCases = s.Sum(a => a.Cases),
                                                }).ToListAsync();
                    return Ok(queryByInHouse);

                case EnumFMCasesOptions.ByLine:
                    var queryByLine = await query
                                                .GroupBy(g => new
                                                {
                                                    g.Line,
                                                })
                                                .Select(s => new
                                                {
                                                    Type = s.Key.Line,
                                                    NumberOfCases = s.Sum(a => a.Cases),
                                                }).ToListAsync();
                    return Ok(queryByLine);

                case EnumFMCasesOptions.ByShift:
                    var queryByShift = await query
                                                .GroupBy(g => new
                                                {
                                                    g.Shift,
                                                })
                                                .Select(s => new
                                                {
                                                    Type = s.Key.Shift,
                                                    NumberOfCases = s.Sum(a => a.Cases),
                                                }).ToListAsync();
                    return Ok(queryByShift);
            }
            
            return Ok();
        }

        // GET: api/Reports
        [HttpGet("PestLog")]
        // [Authorize(Policy = PolicyNames.ViewHRDs)]
        public async Task<ActionResult> GetPestLogGraphData([FromQuery] GetPestLogGraphDataInput input)
        {
            var queryByPest = await _context.Hrds.Where(x => x.DateHeld >= input.PeriodBegin && x.DateHeld <= input.PeriodEnd && !string.IsNullOrEmpty(x.PestType))
                                                .GroupBy(g => new
                                                {
                                                    g.PestType
                                                })
                                                .Select(s => new
                                                {
                                                    Type = s.Key.PestType,
                                                    NumberOfPestLog = s.Count()
                                                }).ToListAsync();

            return Ok(queryByPest);
        }

        // GET: api/Reports
        [HttpGet("Microbe")]
        // [Authorize(Policy = PolicyNames.ViewHRDs)]
        public async Task<ActionResult> GetMicrobeGraphData([FromQuery] GetMicrobeGraphDataInput input)
        {
            var query = _context.Hrds.Where(x => x.DateHeld >= input.PeriodBegin && x.DateHeld <= input.PeriodEnd && 
                                                    (input.Status == EnumStatus.All
                                                        || (input.Status == EnumStatus.Closed && x.Complete.HasValue && x.Complete.Value)
                                                        || (input.Status == EnumStatus.Open && x.Complete.HasValue && x.Complete.Value)));

            switch (input.Types)
            {
                case EnumMicrobeTypes.ByTypesOfMicrobes:
                    var queryByMicrobe = await query.Where(x => !string.IsNullOrEmpty(x.HoldConcern))
                                                            .GroupBy(g => new {
                                                                g.HoldConcern
                                                            })
                                                            .Select(s => new
                                                            {
                                                                Type = s.Key.HoldConcern,
                                                                NumberOfCases = s.Sum(a => a.Cases)
                                                            }).ToListAsync();

                    return Ok(queryByMicrobe);

                case EnumMicrobeTypes.ByLine:
                    var queryByLine = await query.Where(x => !string.IsNullOrEmpty(x.Line))
                                                        .GroupBy(g => new
                                                        {
                                                            g.Line
                                                        })
                                                        .Select(s => new
                                                        {
                                                            Type = s.Key.Line,
                                                            NumberOfCases = s.Sum(a => a.Cases)
                                                        }).ToListAsync();
                    return Ok(queryByLine);

                case EnumMicrobeTypes.ByTopProducts:

                    var queryByProduct = await (from s in query
                                                join p in _context.Products on s.Globenum equals p.Gpn
                                                where (!string.IsNullOrEmpty(s.Globenum))
                                                group new { s, p } by new
                                                {
                                                    p.Description,
                                                    p.Gpn
                                                } into cases
                                                select new
                                                {
                                                    Type = cases.Key.Description,
                                                    NumberOfCases = cases.Sum(s => s.p.CostPerCase)
                                                }).Take(10).ToListAsync();
                                     
                    return Ok(queryByProduct);

                case EnumMicrobeTypes.ByShift:
                    var queryByShift = await query.Where(x => !string.IsNullOrEmpty(x.Shift))
                                                    .GroupBy(g => new
                                                    {
                                                        g.Shift
                                                    })
                                                    .Select(s => new
                                                    {
                                                        Type = s.Key.Shift,
                                                        NumberOfCases = s.Sum(a => a.Cases)
                                                    }).ToListAsync();
                    return Ok(queryByShift);
            }
            return Ok();
        }
    }
}