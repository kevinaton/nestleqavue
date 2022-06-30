using HRD.WebApi.Authorization;
using HRD.WebApi.Data;
using HRD.WebApi.Extensions;
using HRD.WebApi.ViewModels;
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
        [Authorize(Policy = PolicyNames.ViewHRDs)]
        public async Task<ActionResult<IEnumerable<CasesCostHeldByCategoryOutput>>> GetCasesCostByCategoryReport([FromQuery] PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.SortColumn, filter.SortOrder, filter.SearchString);

            var query = _context.Hrds
                            .GroupBy(g => new
                            {
                                g.DateHeld,
                                g.YearHeld,
                                g.HoldCategory,
                                g.Line,
                                g.WeekHeld,
                                g.Complete
                            })
                            .Select(s => new CasesCostHeldByCategoryOutput
                            {
                                MonthHeld = s.Key.DateHeld,
                                HoldCategory = s.Key.HoldCategory,
                                TotalCases = s.Sum(s => s.Cases),
                                TotalCost = s.Sum(s => s.CostofProductonHold),
                                Line = s.Key.Line,
                                YearHeld = s.Key.YearHeld,
                                WeekHeld = s.Key.WeekHeld,
                                IsComplete = s.Key.Complete
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

            if (!string.IsNullOrEmpty(validFilter.SearchString))
            {
                query = query.Where(f => f.Line.Contains(filter.SearchString));
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
        [HttpGet("FMCases")]
        [Authorize(Policy = PolicyNames.ViewHRDs)]
        public async Task<ActionResult> GetFMCasesGraphData([FromQuery]GetFMCasesGraphDataInput input)
        {

            switch (input.CasesOption)
            {
                case EnumFMCasesOptions.ByCategory:
                    var queryByCategory = await _context.Hrds.Where(x => x.DateHeld >= input.PeriodBegin && x.DateHeld <= input.PeriodEnd && !string.IsNullOrWhiteSpace(x.Fmtype)
                                                    && (input.Status == EnumStatus.All
                                                            || (input.Status == EnumStatus.Closed && (x.Complete.HasValue && x.Complete.Value))
                                                            || (input.Status == EnumStatus.Open && (!x.Complete.HasValue || !x.Complete.Value))))
                                        .GroupBy(g => new
                                        {
                                            g.Fmtype,
                                        })
                                        .Select(s => new
                                        {
                                            s.Key.Fmtype,
                                            NumberOfCases = s.Sum(a => a.Cases),
                                        }).ToListAsync();
                    return Ok(queryByCategory);

                case EnumFMCasesOptions.ByInhouseVendor:
                    var queryByInHouse = await _context.Hrds.Where(x => x.DateHeld >= input.PeriodBegin && x.DateHeld <= input.PeriodEnd && !string.IsNullOrWhiteSpace(x.Fmtype)
                                                    && (input.Status == EnumStatus.All
                                                            || (input.Status == EnumStatus.Closed && (x.Complete.HasValue && x.Complete.Value))
                                                            || (input.Status == EnumStatus.Open && (!x.Complete.HasValue || !x.Complete.Value))))
                                        .GroupBy(g => new
                                        {
                                            g.FMSource,
                                        })
                                        .Select(s => new
                                        {
                                            s.Key.FMSource,
                                            NumberOfCases = s.Sum(a => a.Cases),
                                        }).ToListAsync();
                    return Ok(queryByInHouse);

                case EnumFMCasesOptions.ByLine:
                    var queryByLine = await _context.Hrds.Where(x => x.DateHeld >= input.PeriodBegin && x.DateHeld <= input.PeriodEnd && !string.IsNullOrWhiteSpace(x.Fmtype)
                                                    && (input.Status == EnumStatus.All
                                                            || (input.Status == EnumStatus.Closed && (x.Complete.HasValue && x.Complete.Value))
                                                            || (input.Status == EnumStatus.Open && (!x.Complete.HasValue || !x.Complete.Value))))
                                        .GroupBy(g => new
                                        {
                                            g.Line,
                                        })
                                        .Select(s => new
                                        {
                                            s.Key.Line,
                                            NumberOfCases = s.Sum(a => a.Cases),
                                        }).ToListAsync();
                    return Ok(queryByLine);

                case EnumFMCasesOptions.ByShift:
                    var queryByShift = await _context.Hrds.Where(x => x.DateHeld >= input.PeriodBegin && x.DateHeld <= input.PeriodEnd && !string.IsNullOrWhiteSpace(x.Fmtype)
                                                    && (input.Status == EnumStatus.All
                                                            || (input.Status == EnumStatus.Closed && (x.Complete.HasValue && x.Complete.Value))
                                                            || (input.Status == EnumStatus.Open && (!x.Complete.HasValue || !x.Complete.Value))))
                                        .GroupBy(g => new
                                        {
                                            g.Shift,
                                        })
                                        .Select(s => new
                                        {
                                            s.Key.Shift,
                                            NumberOfCases = s.Sum(a => a.Cases),
                                        }).ToListAsync();
                    return Ok(queryByShift);
            }
            


            return Ok();
        }

        // GET: api/Reports
        [HttpGet("PestLog")]
        [Authorize(Policy = PolicyNames.ViewHRDs)]
        public async Task<ActionResult> GetPestLogGraphData([FromQuery] GetPestLogGraphDataInput input)
        {
            var queryByPest = await _context.Hrds.Where(x => x.DateHeld >= input.PeriodBegin && x.DateHeld <= input.PeriodEnd && !string.IsNullOrEmpty(x.PestType))
                                                .GroupBy(g => new
                                                {
                                                    g.PestType
                                                })
                                                .Select(s => new
                                                {
                                                    s.Key.PestType,
                                                    NumberOfPestLog = s.Count()
                                                }).ToListAsync();

            return Ok(queryByPest);
        }

        // GET: api/Reports
        [HttpGet("Microbe")]
        [Authorize(Policy = PolicyNames.ViewHRDs)]
        public async Task<ActionResult> GetMicrobeGraphData([FromQuery] GetMicrobeGraphDataInput input)
        {
            switch (input.Types)
            {
                case EnumMicrobeTypes.ByTypesOfMicrobes:
                    var queryByMicrobe = await _context.Hrds.Where(x => x.DateHeld >= input.PeriodBegin && x.DateHeld <= input.PeriodEnd && !string.IsNullOrEmpty(x.HoldConcern)
                                                                        && (input.Status == EnumStatus.All
                                                                                || (input.Status == EnumStatus.Closed && x.Complete.HasValue && x.Complete.Value)
                                                                                || (input.Status == EnumStatus.Open && x.Complete.HasValue && x.Complete.Value)))
                                                            .GroupBy(g => new {
                                                                g.HoldConcern
                                                            })
                                                            .Select(s => new
                                                            {
                                                                s.Key.HoldConcern,
                                                                MicrobeCases = s.Sum(a => a.Cases)
                                                            }).ToListAsync();

                    return Ok(queryByMicrobe);

                case EnumMicrobeTypes.ByLine:
                    var queryByLine = await _context.Hrds.Where(x => x.DateHeld >= input.PeriodBegin && x.DateHeld <= input.PeriodEnd && !string.IsNullOrEmpty(x.Line)
                                                                     && (input.Status == EnumStatus.All
                                                                        || (input.Status == EnumStatus.Closed && x.Complete.HasValue && x.Complete.Value)
                                                                        || (input.Status == EnumStatus.Open && (!x.Complete.HasValue || !x.Complete.Value))))
                                                        .GroupBy(g => new
                                                        {
                                                            g.Line
                                                        })
                                                        .Select(s => new
                                                        {
                                                            s.Key.Line,
                                                            lineCases = s.Sum(a => a.Cases)
                                                        }).ToListAsync();
                    return Ok(queryByLine);

                case EnumMicrobeTypes.ByTopProducts:

                    var queryByProduct = await (from s in _context.Hrds
                                                join p in _context.Products on s.Globenum equals p.Gpn
                                                where (s.DateHeld >= input.PeriodBegin && s.DateHeld <= input.PeriodEnd && !string.IsNullOrEmpty(s.Globenum)
                                                        && (input.Status == EnumStatus.All
                                                                || (input.Status == EnumStatus.Closed && s.Complete.HasValue && s.Complete.Value)
                                                                || (input.Status == EnumStatus.Open && (!s.Complete.HasValue || !s.Complete.Value))))
                                                group new { s, p } by new
                                                {
                                                    p.Description,
                                                    p.Gpn
                                                } into cases
                                                select new
                                                {
                                                    cases.Key.Description,
                                                    NoCases = cases.Sum(s => s.p.CostPerCase)
                                                }).Take(10).ToListAsync();
                                     
                    return Ok(queryByProduct);

                case EnumMicrobeTypes.ByShift:
                    var queryByShift = await _context.Hrds.Where(x => x.DateHeld >= input.PeriodBegin && x.DateHeld <= input.PeriodEnd && !string.IsNullOrEmpty(x.Shift)
                                                                        && input.Status == EnumStatus.All
                                                                            || (input.Status == EnumStatus.Closed && x.Complete.HasValue && x.Complete.Value)
                                                                            || (input.Status == EnumStatus.Open && (!x.Complete.HasValue || !x.Complete.Value)))
                                                    .GroupBy(g => new
                                                    {
                                                        g.Shift
                                                    })
                                                    .Select(s => new
                                                    {
                                                        s.Key.Shift,
                                                        shiftCases = s.Sum(a => a.Cases)
                                                    }).ToListAsync();
                    return Ok(queryByShift);
            }
            return Ok();
        }
    }
}
    