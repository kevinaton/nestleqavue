using HRD.WebApi.Data;
using HRD.WebApi.Data.Entities;
using HRD.WebApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRD.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HrdsController : ControllerBase
    {
        private readonly HRDContext _context;

        public HrdsController(HRDContext context)
        {
            _context = context;
        }

        // GET: api/Hrds
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QAListViewModel>>> GetHrds([FromQuery] PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.SortColumn, filter.SortOrder, filter.SearchString);

            var query = _context.Hrds
                .Select(s => new QAListViewModel
                {
                    Id = s.Id,
                    DayCode = s.DayCode,
                    Type = s.CodingType, //TODO: Confirm if correct
                    Fert = String.Empty, //TODO: Not mapped
                    ProductDescription = s.ShortDescription, //TODO: Confirm if correct
                    Line = s.Line,
                    Shift = s.Shift,
                    HourCode = s.HourCode,
                    Cases = s.Cases,
                    ShortDescription = s.ShortDescription,
                    Originator = s.Originator
                });

            //Sorting
            switch (validFilter.SortColumn)
            {
                case "daycode":
                    query = validFilter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.DayCode)
                        : query.OrderBy(o => o.DayCode);
                    break;
                case "type":
                    query = validFilter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.Type)
                        : query.OrderBy(o => o.Type);
                    break;
                case "fert":
                    query = validFilter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.Fert)
                        : query.OrderBy(o => o.Fert);
                    break;
                case "productdescription":
                    query = validFilter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.ProductDescription)
                        : query.OrderBy(o => o.ProductDescription);
                    break;
                case "line":
                    query = validFilter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.Line)
                        : query.OrderBy(o => o.Line);
                    break;
                case "shift":
                    query = validFilter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.Shift)
                        : query.OrderBy(o => o.Shift);
                    break;
                case "hourcode":
                    query = validFilter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.HourCode)
                        : query.OrderBy(o => o.HourCode);
                    break;
                case "cases":
                    query = validFilter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.Cases)
                        : query.OrderBy(o => o.Cases);
                    break;
                case "shortdescription":
                    query = validFilter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.ShortDescription)
                        : query.OrderBy(o => o.ShortDescription);
                    break;
                case "originator":
                    query = validFilter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.Originator)
                        : query.OrderBy(o => o.Originator);
                    break;
            }

            //Search
            if (!string.IsNullOrWhiteSpace(validFilter.SearchString))
            {
                query = query.Where(f => f.DayCode.Contains(filter.SearchString)
                                        || f.ProductDescription.Contains(filter.SearchString)
                                        || f.ShortDescription.Contains(filter.SearchString)
                                        || f.Fert.Contains(filter.SearchString)
                                        || f.Originator.Contains(filter.SearchString));
            }

            var totalRecords = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalRecords / validFilter.PageSize);


            //Pagination;
            query = query.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                        .Take(validFilter.PageSize);

            var hrdList = await query.ToListAsync();

            return Ok(new PagedResponse<List<QAListViewModel>>(hrdList, validFilter.PageNumber, validFilter.PageSize, totalRecords, totalPages));
        }

        // GET: api/Hrds/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HRDDetailViewModel>> GetHrd(int id)
        {
            var hrd = await _context.Hrds.Include(i => i.Hrddcs)
                                         .Include(i => i.Hrdfcs)
                                         .Include(i => i.Hrdnotes)
                                         .Include(i => i.Hrdpos)
                                         .FirstOrDefaultAsync(f => f.Id == id);

            if (hrd == null)
            {
                return NotFound();
            }

            var model = new HRDDetailViewModel
            {
                Id = id,
                YearHeld = hrd.YearHeld,
                DayCode = hrd.DayCode,
                Originator = hrd.Originator,
                Plant = hrd.Plant,
                Line = hrd.Line,
                Shift = hrd.Shift,
                ShortDescription = hrd.ShortDescription,
                Gstdrequired = hrd.Gstdrequired,
                HourCode = hrd.HourCode,

                HrdDc = hrd.Hrddcs.Select(s => new HrdDCViewModel { Id = s.Id, HrdId = s.Hrdid, Location = s.Location, NumberOfCases = s.NumberOfCases }).ToList(),
                HrdFc = hrd.Hrdfcs.Select(s => new HrdFCViewModel { Id = s.Id, HrdId = s.Hrdid, Location = s.Location, NumberOfCases = s.NumberOfCases }).ToList(),
                HrdNote = hrd.Hrdnotes.Select(s => new HrdNoteViewModel { Id = s.Id, HrdId = s.Hrdid, Category = s.Category, Date = s.Date, Description = s.Description, Filename = s.FileName, Path = s.Path, Size = s.Size, UserId = s.UserId }).ToList(),
                HrdPo = hrd.Hrdpos.Select(s => new HrdPoViewModel { Id = s.Id, HrdId = s.Hrdid, PONumber = s.Ponumber }).ToList(),

                QaComments = hrd.Qacomments,
                //DateCompleted = hrd.DateCompleted, //not mapped
                Clear = hrd.Clear,
                HrdcompletedBy = hrd.HrdcompletedBy,
                Scrap = hrd.Scrap,
                DateofDisposition = hrd.DateofDisposition,
                ThriftStore = hrd.ThriftStore,
                Complete = hrd.Complete,
                Cancelled = hrd.Cancelled,
                Samples = hrd.Samples,
                //NumberOfDaysHeld = hrd.numberOfDaysheld//notmappep
                //Donate = hrd.Donate, //not mapped
                AllCasesAccountedFor = hrd.AllCasesAccountedFor,
                Cases = hrd.Cases,
                OtherHrdAffected = hrd.OtherHrdaffected,
                HighRisk = hrd.HighRisk,
                OtherHrdNum = hrd.OtherHrdnum,

                FcDate = hrd.Fcdate,
                FcUser = hrd.Fcuser,
                DcDate = hrd.Dcdate,
                DcUser = hrd.Dcuser,
                Classification = hrd.Classification,
                HoldCategory = hrd.HoldCategory,
                HoldSubCategory = hrd.HoldSubCategory,
                DateHeld = hrd.DateHeld,

                //WeekHeld = hrd.WeekHeld, //not mapped
                CostofProductonHold = hrd.CostofProductonHold,
                ReworkApproved = hrd.ReworkApproved,
                //not mapped
                //NumberOfDaysToReworkApproval = hrd.NumberOfDaysToReworkApproval,
                //CaseCount = hrd.CaseCount,
                //ReasonAction = hrd.ReasonAction,
                //ApprovalRequestByQa = hrd.ApprovalRequestByQa,
                //PlantManagerAprpoval = hrd.PlantManagerAprpoval,
                //PlantControllerApproval = hrd.PlantControllerApproval,
                //Destroyed = hrd.Destroyed,
                //ApprovedByQAWho = hrd.ApprovedByQAWho,
                //ApprovedByQAWhen = hrd.ApprovedByQAWhen,
                //ApprovedByPlantManagerWho = hrd.ApprovedByPlantManagerWho,
                //ApprovedPlantManagerQAWhen = hrd.ApprovedPlantManagerQAWhen,
                //ApprovedByPlantControllerWho = hrd.ApprovedByPlantControllerWho,
                //ApprovedByPlantControllerWhen = hrd.ApprovedByPlantControllerWhen,
                //ApprovedByDistroyedWho = hrd.ApprovedByDistroyedWho,
                //ApprovedByDistroyedWhen = hrd.ApprovedByDistroyedWhen,
                //Comments = hrd.Comments,
            };


            return model;
        }

        // PUT: api/Hrds/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHrd(int id, HRDDetailViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            var hrd = new HRDDetailViewModel
            {
                Id = id,
                YearHeld = model.YearHeld,
                DayCode = model.DayCode,
                Originator = model.Originator,
                Plant = model.Plant,
                //Fert = model.Fert,//not mapped
                Line = model.Line,
                //LineSuper = model.LineSupervisor, //not mapped
                //Area = model.Area,//not mapped
                //AreaIfOther = model.AreaIfOther,//not mapped
                Shift = model.Shift,
                ShortDescription = model.ShortDescription,
                //AdditionalDescription = model.AdditionalDescription, //not mapped
                //DetailedDescription = model.DetailedDesctiption, //not mapped
                Gstdrequired = model.Gstdrequired,
                HourCode = model.HourCode,
                HrdPo = model.HrdPo,

                QaComments = model.QaComments,
                DateCompleted = model.DateCompleted,
                Clear = model.Clear,

                HrdcompletedBy = model.HrdcompletedBy,
                Scrap = model.Scrap,
                DateofDisposition = model.DateofDisposition,
                ThriftStore = model.ThriftStore,
                Complete = model.Complete,
                Cancelled = model.Cancelled,

                Samples = model.Samples,
                NumberOfDaysHeld = model.NumberOfDaysHeld,
                Donate = model.Donate,
                AllCasesAccountedFor = model.AllCasesAccountedFor,
                OtherHrdNum = model.OtherHrdNum,
                HighRisk = model.HighRisk,
                //OtherHRDNum = model.OtherHrdNum,
                HrdDc = model.HrdDc,
                HrdFc = model.HrdFc,

                FcDate = model.FcDate,
                FcUser = model.FcUser,
                DcDate = model.DcDate,
                DcUser = model.DcUser,
                Classification = model.Classification,
                HoldCategory = model.HoldCategory,
                HoldSubCategory = model.HoldSubCategory,
                DateHeld = model.DateHeld,

                //WeekHeld = model.WeekHeld, //not mapped
                CostofProductonHold = model.CostofProductonHold,
                ReworkApproved = model.ReworkApproved,

                //not mapped
                //NumberOfDaysToReworkApproval = model.NumberOfDaysToReworkApproval,
                //CaseCount = model.CaseCount,
                //ReasonAction = model.ReasonAction,
                //ApprovalRequestByQa = model.ApprovalRequestByQa,
                //PlantManagerAprpoval = model.PlantManagerAprpoval,
                //PlantControllerApproval = model.PlantControllerApproval,
                //Destroyed = model.Destroyed,
                //ApprovedByQAWho = model.ApprovedByQAWho,
                //ApprovedByQAWhen = model.ApprovedByQAWhen,
                //ApprovedByPlantManagerWho = model.ApprovedByPlantManagerWho,
                //ApprovedPlantManagerQAWhen = model.ApprovedPlantManagerQAWhen,
                //ApprovedByPlantControllerWho = model.ApprovedByPlantControllerWho,
                //ApprovedByPlantControllerWhen = model.ApprovedByPlantControllerWhen,
                //ApprovedByDistroyedWho = model.ApprovedByDistroyedWho,
                //ApprovedByDistroyedWhen = model.ApprovedByDistroyedWhen,
                //Comments = model.Comments,
            };

            _context.Entry(hrd).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HrdExists(id))
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

        // POST: api/Hrds
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Hrd>> PostHrd(Hrd hrd)
        {
            _context.Hrds.Add(hrd);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHrd", new { id = hrd.Id }, hrd);
        }

        // DELETE: api/Hrds/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHrd(int id)
        {
            var hrd = await _context.Hrds.FindAsync(id);
            if (hrd == null)
            {
                return NotFound();
            }

            _context.Hrds.Remove(hrd);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HrdExists(int id)
        {
            return _context.Hrds.Any(e => e.Id == id);
        }
    }
}
