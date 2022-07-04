using HRD.WebApi.Authorization;
using HRD.WebApi.Data;
using HRD.WebApi.Data.Entities;
using HRD.WebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
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
        // [Authorize(Policy = PolicyNames.ViewHRDs)]
        public async Task<ActionResult<IEnumerable<QAListViewModel>>> GetHrds([FromQuery] PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, filter.SortColumn, filter.SortOrder, filter.SearchString);

            var query = _context.Hrds
                .Select(s => new QAListViewModel
                {
                    Id = s.Id,
                    DayCode = s.DayCode,
                    IsHRD = s.IsHrd,
                    IsPest = s.IsPest,
                    IsSMI = s.IsSmi,
                    IsNR = s.IsNr,
                    IsFM = s.IsFm,
                    IsMicro = s.IsMicro,
                    Fert = s.Globenum, //TODO: Not mapped
                    ProductDescription = _context.Products.Where(x => x.Gpn == s.Globenum).Select(s => s.Description).First(),//s.ShortDescription, //TODO: Confirm if correct
                    Line = s.Line,
                    Shift = s.Shift,
                    HourCode = s.HourCode,
                    Cases = s.Cases,
                    ShortDescription = s.ShortDescription,
                    Originator = s.Originator,
                });

            //Sorting
            switch (validFilter.SortColumn)
            {
                case "id":
                    query = validFilter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.Id)
                        : query.OrderBy(o => o.Id);
                    break;
                case "daycode":
                    query = validFilter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.DayCode)
                        : query.OrderBy(o => o.DayCode);
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
        [HttpGet("Hrd/{id}")]
        // [Authorize(Policy = PolicyNames.ViewHRDs)]
        public async Task<ActionResult<HRDDetailViewModel>> GetHrd(int id)
        {
            var hrd = await _context.Hrds.Include(i => i.Hrddcs)
                                         .Include(i => i.Hrdfcs)
                                         .Include(i => i.Hrdpos)
                                         .Include(i => i.Hrdnotes)
                                         .FirstOrDefaultAsync(f => f.Id == id);

            if (hrd == null)
            {
                return NotFound();
            }

            var model = new HRDDetailViewModel
            {
                Id = id,
                Date = hrd.Date,
                TimeOfIncident = hrd.TimeOfIncident,
                YearHeld = hrd.YearHeld,
                DayCode = hrd.DayCode,
                Originator = hrd.Originator,
                Plant = hrd.Plant,
                BUManager = hrd.Bumanager,
                Type = hrd.CodingType,
                Fert = hrd.Globenum,
                FertDescription = hrd.FertDescription,
                Line = hrd.Line,
                LineSupervisor = hrd.LineSupervisor,
                Area = hrd.Area,
                AreaIfOther = hrd.AreaIfOther,
                Shift = hrd.Shift,
                ShortDescription = hrd.ShortDescription,
                AdditionalDescription = hrd.AdditionalDescription,
                DetailedDescription = hrd.DetailedDescription,
                Gstdrequired = hrd.Gstdrequired,
                HourCode = hrd.HourCode,
                ContinuousRun = hrd.ContinuousRun,

                QaComments = hrd.Qacomments,
                DateCompleted = hrd.DateCompleted,
                Clear = hrd.Clear,
                HrdcompletedBy = hrd.HrdcompletedBy,
                Scrap = hrd.Scrap,
                DateofDisposition = hrd.DateofDisposition,
                ThriftStore = hrd.ThriftStore,
                Complete = hrd.Complete,
                Cancelled = hrd.Cancelled,
                Samples = hrd.Samples,
                NumberOfDaysHeld = hrd.NumberOfDaysHeld,
                Donate = hrd.Donate,
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

                MonthHeld = hrd.DateHeld.Value.ToString("MMMM"),//MonthHeld,
                WeekHeld = hrd.DateHeld.Value.DayOfWeek.ToString("F"),//hrd.WeekHeld,
                CostofProductonHold = hrd.CostofProductonHold,
                ReworkApproved = hrd.ReworkApproved,

                NumberOfDaysToReworkApproval = hrd.NumberOfDayToReworkApproval,
                CaseCount = hrd.CaseCount,
                ReasonAction = hrd.ReasonAction,
                ApprovalRequestByQa = hrd.IsApprovalRequestByQa,
                IsPlantManagerAprpoval = hrd.IsPlantManagerAprpoval,
                IsPlantControllerApproval = hrd.IsPlantControllerApproval,
                IsDestroyed = hrd.IsDestroyed,
                ApprovedByQAWho = hrd.ApprovedByQawho,
                ApprovedByQAWhen = hrd.ApprovedByQawhen,
                ApprovedByPlantManagerWho = hrd.ApprovedByPlantManagerWho,
                ApprovedPlantManagerQAWhen = hrd.ApprovedPlantManagerQawhen,
                ApprovedByPlantControllerWho = hrd.ApprovedByPlantControllerWho,
                ApprovedByPlantControllerWhen = hrd.ApprovedByPlantControllerWhen,
                ApprovedByDistroyedWho = hrd.ApprovedByDistroyedWho,
                ApprovedByDistroyedWhen = hrd.ApprovedByDistroyedWhen,
                Comments = hrd.Comments,
                YearOfIncident = hrd.YearOfIncident,

                HrdDcTotalCases = hrd.Hrddcs.Sum(s => s.NumberOfCases),
                HrdFcTotalCases = hrd.Hrdfcs.Sum(s => s.NumberOfCases),

                HrdNote = hrd.Hrdnotes.Select(s => new HrdNoteViewModel { Id = s.Id, HrdId = s.Hrdid, Category = s.Category, Date = s.Date, Description = s.Description, Filename = s.FileName, Path = s.Path, Size = s.Size, UserId = s.UserId }).ToList(),
                HrdDc = hrd.Hrddcs.Select(s => new HrdDCViewModel { Id = s.Id, HrdId = s.Hrdid, Location = s.Location, NumberOfCases = s.NumberOfCases }).ToList(),
                HrdFc = hrd.Hrdfcs.Select(s => new HrdFCViewModel { Id = s.Id, HrdId = s.Hrdid, Location = s.Location, NumberOfCases = s.NumberOfCases }).ToList(),
                HrdPo = hrd.Hrdpos.Select(s => new HrdPoViewModel { Id = s.Id, HrdId = s.Hrdid, PONumber = s.Ponumber }).ToList(),
            };

            return model;
        }

        //[EnableCors("AllowOrigin")]
        // PUT: api/Hrds/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("Hrd/{id}")]
        // [Authorize(Policy = PolicyNames.EditHRDs)]
        public async Task<IActionResult> PutHrd(int id, HRDDetailViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            var hrd = _context.Hrds.Include(i => i.HrdtestCosts)
                                    .Include(i => i.HrdMicros)
                                    .Include(i => i.Hrdnotes)
                                    .Where(i => i.Id == id).FirstOrDefault();

            hrd.Id = id;
            hrd.Date = model.Date;
            hrd.TimeOfIncident = model.TimeOfIncident;
            hrd.YearHeld = model.YearHeld;
            hrd.DayCode = model.DayCode;
            hrd.Originator = model.Originator;
            hrd.Bumanager = model.BUManager;
            hrd.CodingType = model.Type;
            hrd.Plant = model.Plant;
            hrd.Globenum = model.Fert;
            hrd.FertDescription = model.FertDescription;
            hrd.Line = model.Line;
            hrd.LineSupervisor = model.LineSupervisor;
            hrd.Area = model.Area;
            hrd.AreaIfOther = model.AreaIfOther;
            hrd.Shift = model.Shift;
            hrd.ShortDescription = model.ShortDescription;
            hrd.AdditionalDescription = model.AdditionalDescription;
            hrd.DetailedDescription = model.DetailedDescription;
            hrd.Gstdrequired = model.Gstdrequired;
            hrd.HourCode = model.HourCode;
            hrd.ContinuousRun = model.ContinuousRun;

            hrd.Qacomments = model.QaComments;
            hrd.DateCompleted = model.DateCompleted;
            hrd.Clear = model.Clear;

            hrd.HrdcompletedBy = model.HrdcompletedBy;
            hrd.Scrap = model.Scrap;
            hrd.DateofDisposition = model.DateofDisposition;
            hrd.ThriftStore = model.ThriftStore;
            hrd.Complete = model.Complete;
            hrd.Cancelled = model.Cancelled;

            hrd.Samples = model.Samples;
            hrd.NumberOfDaysHeld = model.NumberOfDaysHeld;
            hrd.Donate = model.Donate;
            hrd.AllCasesAccountedFor = model.AllCasesAccountedFor;
            hrd.OtherHrdaffected = model.OtherHrdAffected;
            hrd.HighRisk = model.HighRisk;
            hrd.OtherHrdnum = model.OtherHrdNum;

            hrd.Fcdate = model.FcDate;
            hrd.Fcuser = model.FcUser;
            hrd.Dcdate = model.DcDate;
            hrd.Dcuser = model.DcUser;
            hrd.Classification = model.Classification;
            hrd.HoldCategory = model.HoldCategory;
            hrd.HoldSubCategory = model.HoldSubCategory;
            hrd.DateHeld = model.DateHeld;

            //hrd.MonthHeld = model.MonthHeld;
            //hrd.WeekHeld = model.WeekHeld;
            hrd.CostofProductonHold = model.CostofProductonHold;
            hrd.ReworkApproved = model.ReworkApproved;

            //not mapped
            hrd.NumberOfDayToReworkApproval = model.NumberOfDaysToReworkApproval;
            hrd.YearOfIncident = model.YearOfIncident;
            hrd.CaseCount = model.CaseCount;
            hrd.ReasonAction = model.ReasonAction;
            hrd.IsApprovalRequestByQa = model.ApprovalRequestByQa;
            hrd.IsPlantManagerAprpoval = model.IsPlantManagerAprpoval;
            hrd.IsPlantControllerApproval = model.IsPlantControllerApproval;
            hrd.IsDestroyed = model.IsDestroyed;
            hrd.ApprovedByQawho = model.ApprovedByQAWho;
            hrd.ApprovedByQawhen = model.ApprovedByQAWhen;
            hrd.ApprovedByPlantManagerWho = model.ApprovedByPlantManagerWho;
            hrd.ApprovedPlantManagerQawhen = model.ApprovedPlantManagerQAWhen;
            hrd.ApprovedByPlantControllerWho = model.ApprovedByPlantControllerWho;
            hrd.ApprovedByPlantControllerWhen = model.ApprovedByPlantControllerWhen;
            hrd.ApprovedByDistroyedWho = model.ApprovedByDistroyedWho;
            hrd.ApprovedByDistroyedWhen = model.ApprovedByDistroyedWhen;
            hrd.Comments = model.Comments;

            if (model.HrdDc != null)
            {
                foreach (var item in model.HrdDc)
                {
                    var hrdDc = _context.Hrddcs.FirstOrDefault(f => f.Id == item.Id);
                    if (hrdDc != null)
                    {
                        hrdDc.Id = hrdDc.Id;
                        hrdDc.Hrdid = id;
                        hrdDc.Location = item.Location;
                        hrdDc.NumberOfCases = item.NumberOfCases;
                    }
                    else
                    {
                        hrdDc = new Hrddc();
                        hrdDc.Hrdid = id;
                        hrdDc.Location = item.Location;
                        hrdDc.NumberOfCases = item.NumberOfCases;
                    }

                    hrd.Hrddcs.Add(hrdDc);
                }
            }

            if (model.HrdFc != null)
            {
                foreach (var item in model.HrdFc)
                {
                    var hrdFc = _context.Hrdfcs.FirstOrDefault(f => f.Id == item.Id);
                    if (hrdFc != null)
                    {
                        hrdFc.Id = hrdFc.Id;
                        hrdFc.Hrdid = id;
                        hrdFc.Location = item.Location;
                        hrdFc.NumberOfCases = item.NumberOfCases;
                    }
                    else
                    {
                        hrdFc = new Hrdfc();
                        hrdFc.Hrdid = id;
                        hrdFc.Location = item.Location;
                        hrdFc.NumberOfCases = item.NumberOfCases;
                    }

                    hrd.Hrdfcs.Add(hrdFc);
                }
            }

            if (model.HrdPo != null)
            {
                foreach (var item in model.HrdPo)
                {
                    var po = _context.Hrdpos.FirstOrDefault(f => f.Id == item.Id);
                    if (po != null)
                    {
                        po.Id = po.Id;
                        po.Hrdid = id;
                        po.Ponumber = item.PONumber;
                    }
                    else
                    {
                        po = new Hrdpo();
                        po.Hrdid = id;
                        po.Ponumber = item.PONumber;
                    }

                    hrd.Hrdpos.Add(po);
                }
            }

            if (model.HrdNote != null)
            {
                foreach (var item in model.HrdNote)
                {
                    var note = _context.Hrdnotes.FirstOrDefault(f => f.Id == item.Id);

                    if (note != null)
                    {
                        note.Id = item.Id;
                        note.Hrdid = id;
                        note.Category = item.Category;
                        note.Date = item.Date;
                        note.Description = item.Description;
                        note.FileName = item.Filename;
                        note.Path = item.Path;
                        note.UserId = item.UserId;
                        note.Size = item.Size;
                        _context.Entry(note).State = EntityState.Modified;
                    }
                    else
                    {
                        note = new Hrdnote();
                        note.Hrdid = id;
                        note.Category = item.Category;
                        note.Date = item.Date;
                        note.Description = item.Description;
                        note.FileName = item.Filename;
                        note.Path = item.Path;
                        note.UserId = item.UserId;
                        note.Size = item.Size;
                        _context.Entry(note).State = EntityState.Added;
                    }

                    hrd.Hrdnotes.Add(note);
                }
            }

            _context.Entry(hrd).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecordExists(id))
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

        //[EnableCors("AllowOrigin")]
        // POST: api/Hrds
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Hrd")]
        // [Authorize(Policy = PolicyNames.EditHRDs)]
        public async Task<ActionResult<HRDDetailViewModel>> PostHrd(HRDDetailViewModel model)
        {
            var hrd = new Hrd
            {
                Date = model.Date,
                TimeOfIncident = model.TimeOfIncident,
                YearHeld = model.YearHeld,
                DayCode = model.DayCode,
                Originator = model.Originator,
                Bumanager = model.BUManager,
                CodingType = model.Type,
                Plant = model.Plant,
                Globenum = model.Fert,
                FertDescription = model.FertDescription,
                Line = model.Line,
                LineSupervisor = model.LineSupervisor,
                Area = model.Area,
                AreaIfOther = model.AreaIfOther,
                Shift = model.Shift,
                ShortDescription = model.ShortDescription,
                AdditionalDescription = model.AdditionalDescription,
                DetailedDescription = model.DetailedDescription,
                Gstdrequired = model.Gstdrequired,
                HourCode = model.HourCode,
                ContinuousRun = model.ContinuousRun,

                Qacomments = model.QaComments,
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
                OtherHrdaffected = model.OtherHrdAffected,
                HighRisk = model.HighRisk,
                OtherHrdnum = model.OtherHrdNum,

                Fcdate = model.FcDate,
                Fcuser = model.FcUser,
                Dcdate = model.DcDate,
                Dcuser = model.DcUser,
                Classification = model.Classification,
                HoldCategory = model.HoldCategory,
                HoldSubCategory = model.HoldSubCategory,
                DateHeld = model.DateHeld,

                //MonthHeld = model.MonthHeld,
                //WeekHeld = model.WeekHeld,
                CostofProductonHold = model.CostofProductonHold,
                ReworkApproved = model.ReworkApproved,

                //not mapped
                NumberOfDayToReworkApproval = model.NumberOfDaysToReworkApproval,
                YearOfIncident = model.YearOfIncident,
                CaseCount = model.CaseCount,
                ReasonAction = model.ReasonAction,
                IsApprovalRequestByQa = model.ApprovalRequestByQa,
                IsPlantManagerAprpoval = model.IsPlantManagerAprpoval,
                IsPlantControllerApproval = model.IsPlantControllerApproval,
                IsDestroyed = model.IsDestroyed,
                ApprovedByQawho = model.ApprovedByQAWho,
                ApprovedByQawhen = model.ApprovedByQAWhen,
                ApprovedByPlantManagerWho = model.ApprovedByPlantManagerWho,
                ApprovedPlantManagerQawhen = model.ApprovedPlantManagerQAWhen,
                ApprovedByPlantControllerWho = model.ApprovedByPlantControllerWho,
                ApprovedByPlantControllerWhen = model.ApprovedByPlantControllerWhen,
                ApprovedByDistroyedWho = model.ApprovedByDistroyedWho,
                ApprovedByDistroyedWhen = model.ApprovedByDistroyedWhen,
                Comments = model.Comments,

                Hrdnotes = model.HrdNote.Select(s => new Hrdnote { Category = s.Category, Date = s.Date, Description = s.Description, FileName = s.Filename, Path = s.Path, UserId = s.UserId, Size = s.Size }).ToList(),
                Hrdpos = model.HrdPo.Select(s => new Hrdpo { Ponumber = s.PONumber }).ToList(),
                Hrddcs = model.HrdDc.Select(s => new Hrddc { Location = s.Location, NumberOfCases = s.NumberOfCases }).ToList(),
                Hrdfcs = model.HrdDc.Select(s => new Hrdfc { Location = s.Location, NumberOfCases = s.NumberOfCases }).ToList(),
            };

            _context.Hrds.Add(hrd);
            await _context.SaveChangesAsync();

            model.Id = hrd.Id;
            return CreatedAtAction("GetHrd", new { id = model.Id }, model);
        }

        // DELETE: api/Hrds/5
        [HttpDelete("Hrd/{id}")]
        // [Authorize(Policy = PolicyNames.EditHRDs)]
        public async Task<IActionResult> DeleteHrd(int id)
        {
            var hrd = await _context.Hrds
                            .Include(i => i.Hrdpos)
                            .Include(i => i.Hrddcs)
                            .Include(i => i.Hrdfcs)
                            .Include(i => i.Hrdnotes)
                            .Include(i => i.HrdtestCosts)
                            .Include(i => i.HrdMicros)
                            .FirstOrDefaultAsync(f => f.Id == id);

            if (hrd == null)
                return NotFound();

            if (hrd.Hrdpos.Count > 0)
                _context.Hrdpos.RemoveRange(hrd.Hrdpos);

            if (hrd.Hrddcs.Count > 0)
                _context.Hrddcs.RemoveRange(hrd.Hrddcs);

            if (hrd.Hrdfcs.Count > 0)
                _context.Hrdfcs.RemoveRange(hrd.Hrdfcs);

            if (hrd.Hrdnotes.Count > 0)
                _context.Hrdnotes.RemoveRange(hrd.Hrdnotes);

            if (hrd.HrdtestCosts.Count > 0)
                _context.HrdtestCosts.RemoveRange(hrd.HrdtestCosts);

            if (hrd.HrdMicros.Count > 0)
                _context.HrdMicros.RemoveRange(hrd.HrdMicros);

            _context.Hrds.Remove(hrd);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RecordExists(int id)
        {
            return _context.Hrds.Any(e => e.Id == id);
        }

        // GET: api/Hrds/5
        [HttpGet("Qa/{id}")]
        // [Authorize(Policy = PolicyNames.ViewHRDs)]
        public async Task<ActionResult<QARecordViewModel>> GetQARecord(int id)
        {
            var qa = await _context.Hrds.Include(i => i.HrdtestCosts)
                                        .Include(i => i.Hrdnotes)
                                        .Include(i => i.HrdMicros)
                                        .FirstOrDefaultAsync(f => f.Id == id);

            if (qa == null)
            {
                return NotFound();
            }

            var model = new QARecordViewModel
            {
                Id = id,
                IsHRD = qa.IsHrd,
                IsPest = qa.IsPest,
                IsSMI = qa.IsSmi,
                IsNR = qa.IsNr,
                IsFM = qa.IsFm,
                IsMicro = qa.IsMicro,

                Date = qa.Date,
                TimeOfIncident = qa.TimeOfIncident,
                YearHeld = qa.YearHeld,
                DayCode = qa.DayCode,
                Originator = qa.Originator,
                BUManager = qa.Bumanager,
                Type = qa.CodingType,
                Fert = qa.Globenum,
                FertDescription = qa.FertDescription,
                Line = qa.Line,
                LineSupervisor = qa.LineSupervisor,
                Area = qa.Area,
                AreaIfOther = qa.AreaIfOther,
                Shift = qa.Shift,
                ShortDescription = qa.ShortDescription,
                AdditionalDescription = qa.AdditionalDescription,
                DetailedDescription = qa.DetailedDescription,

                //HRD                                  
                CasesHeld = qa.CasesHeld,
                HourCode = qa.HourCode,
                POs = qa.Pos,
                ReworkInstructions = qa.ReworkInstructions,

                //PEST                                     
                PestType = qa.PestType,
                PCOContactedImmediately = qa.PcocontactedImmediately,
                ProductAdultered = qa.ProductAdultered,
                WhereFound = qa.WhereFound,
                IfYesAffectedProduct = qa.IfYesAffectedProduct,

                //SMI                                               
                MaterialNumber = qa.MaterialNumber,
                RawMaterialDescription = qa.RawMaterialDescription,
                SMIVendorBatch = qa.SMIVendorBatch,
                VendorNumber = qa.VendorNumber,
                VendorName = qa.VendorName,
                VendorSiteNumber = qa.VendorSiteNumber,

                //FM                                                
                IsInspections = qa.IsInspections,
                IsXray = qa.IsXray,
                IsMetalDetector = qa.IsMetalDetector,
                FMType = qa.Fmtype,
                Size = qa.Size,
                Equipment = qa.Equipment,
                EquipmentIfOther = qa.EquipmentIfOther,
                ROHMaterial = qa.Rohmaterial,

                PiecesTotal = qa.PiecesTotal,
                FMVendorBatch = qa.FMVendorBatch,
                FMSource = qa.FMSource,

                //NR                                                
                DateReceived = qa.DateReceived,
                InspectorsName = qa.InspectorsName,
                NRCategory = qa.Nrcategory,
                Tagged = qa.Tagged,
                TagNumber = qa.TagNumber,
                Response = qa.Response,

                //MICRO                                             
                HoldConcern = qa.HoldConcern,
                DayOfWeek = qa.DayOfWeek,
                When = qa.When,
                WhenOther = qa.WhenOther,
                DateOfResample = qa.DateOfResample,
                MeatComponent = qa.MeatComponent,
                VeggieComponent = qa.VeggieComponent,
                SauceType = qa.SauceType,
                StarchType = qa.StarchType,
                AdditionalComments = qa.AdditionalComments,
                HrdTestCosts = qa.HrdtestCosts.Select(s => new HrdTestCostViewModel { Id = s.Id, HrdId = s.Hrdid, Cost = s.Cost, Qty = s.Qty, TestName = s.TestName }).ToList(),
                HrdNote = qa.Hrdnotes.Select(s => new HrdNoteViewModel { Id = s.Id, HrdId = s.Hrdid, Category = s.Category, Date = s.Date, Description = s.Description, Filename = s.FileName, Path = s.Path, Size = s.Size, UserId = s.UserId }).ToList(),
                HrdMicros = qa.HrdMicros.Select(s => new HRDMicroViewModel { Id = s.Id, HrdId = s.HrdId, Hour = s.Hour, Count = s.Count, Organism = s.Organism }).ToList(),
            };

            return model;
        }

        // PUT: api/Hrds/Qa/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("Qa/{id}")]
        // [Authorize(Policy = PolicyNames.EditHRDs)]
        public async Task<IActionResult> PutQARecord(int id, QARecordViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            var hrd = _context.Hrds.Include(i => i.Hrdfcs)
                                    .Include(i => i.Hrddcs)
                                    .Include(i => i.Hrdpos)
                                    .Include(i => i.Hrdnotes)
                                    .Where(i => i.Id == id).FirstOrDefault();

            hrd.Id = id;
            hrd.IsHrd = model.IsHRD;
            hrd.IsPest = model.IsPest;
            hrd.IsSmi = model.IsSMI;
            hrd.IsNr = model.IsNR;
            hrd.IsFm = model.IsFM;
            hrd.IsMicro = model.IsMicro;

            hrd.Date = model.Date;
            hrd.TimeOfIncident = model.TimeOfIncident;
            hrd.YearHeld = model.YearHeld;
            hrd.DayCode = model.DayCode;
            hrd.Originator = model.Originator;
            hrd.Bumanager = model.BUManager;
            hrd.CodingType = model.Type;
            hrd.Globenum = model.Fert;
            hrd.FertDescription = model.FertDescription;
            hrd.Line = model.Line;
            hrd.LineSupervisor = model.LineSupervisor;
            hrd.Area = model.Area;
            hrd.AreaIfOther = model.AreaIfOther;
            hrd.Shift = model.Shift;
            hrd.ShortDescription = model.ShortDescription;
            hrd.AdditionalDescription = model.AdditionalDescription;
            hrd.DetailedDescription = model.DetailedDescription;

            //HRD                                  
            hrd.CasesHeld = model.CasesHeld;
            hrd.HourCode = model.HourCode;
            hrd.Pos = model.POs;
            hrd.ReworkInstructions = model.ReworkInstructions;

            //PEST                                     
            hrd.PestType = model.PestType;
            hrd.PcocontactedImmediately = model.PCOContactedImmediately;
            hrd.ProductAdultered = model.ProductAdultered;
            hrd.WhereFound = model.WhereFound;
            hrd.IfYesAffectedProduct = model.IfYesAffectedProduct;

            //SMI                                               
            hrd.MaterialNumber = model.MaterialNumber;
            hrd.RawMaterialDescription = model.RawMaterialDescription;
            hrd.SMIVendorBatch = model.SMIVendorBatch;
            hrd.VendorNumber = model.VendorNumber;
            hrd.VendorName = model.VendorName;
            hrd.VendorSiteNumber = model.VendorSiteNumber;

            //FM                                                
            hrd.IsInspections = model.IsInspections;
            hrd.IsXray = model.IsXray;
            hrd.IsMetalDetector = model.IsMetalDetector;
            hrd.Fmtype = model.FMType;
            hrd.Size = model.Size;
            hrd.Equipment = model.Equipment;
            hrd.EquipmentIfOther = model.EquipmentIfOther;
            hrd.Rohmaterial = model.ROHMaterial;

            hrd.PiecesTotal = model.PiecesTotal;
            hrd.FMVendorBatch = model.FMVendorBatch;
            hrd.FMSource = model.FMSource;

            hrd.DateReceived = model.DateReceived;
            hrd.InspectorsName = model.InspectorsName;
            hrd.Nrcategory = model.NRCategory;
            hrd.Tagged = model.Tagged;
            hrd.TagNumber = model.TagNumber;
            hrd.Response = model.Response;

            hrd.HoldConcern = model.HoldConcern;
            hrd.DayOfWeek = model.DayOfWeek;
            hrd.When = model.When;
            hrd.WhenOther = model.WhenOther;
            hrd.DateOfResample = model.DateOfResample;
            hrd.MeatComponent = model.MeatComponent;
            hrd.VeggieComponent = model.VeggieComponent;
            hrd.SauceType = model.SauceType;
            hrd.StarchType = model.StarchType;
            hrd.AdditionalComments = model.AdditionalComments;

            if (model.HrdMicros != null)
            {
                foreach (var item in model.HrdMicros)
                {
                    var micro = _context.HrdMicros.FirstOrDefault(f => f.Id == item.Id);

                    if (micro != null)
                    {
                        micro.Id = micro.Id;
                        micro.HrdId = id;
                        micro.Hour = item.Hour;
                        micro.Count = item.Count;
                        micro.Organism = item.Organism;
                        _context.Entry(micro).State = EntityState.Modified;
                    }
                    else
                    {
                        micro = new HrdMicro();
                        micro.HrdId = id;
                        micro.Hour = item.Hour;
                        micro.Count = item.Count;
                        micro.Organism = item.Organism;
                        _context.Entry(micro).State = EntityState.Added;
                    }

                    hrd.HrdMicros.Add(micro);
                }
            }

            if (model.HrdTestCosts != null)
            {
                foreach (var item in model.HrdTestCosts)
                {
                    var testCost = _context.HrdtestCosts.FirstOrDefault(f => f.Id == item.Id);

                    if (testCost != null)
                    {
                        testCost.Id = testCost.Id;
                        testCost.Hrdid = id;
                        testCost.Qty = item.Qty;
                        testCost.Cost = item.Cost;
                        testCost.TestName = item.TestName;
                        _context.Entry(testCost).State = EntityState.Modified;
                    }
                    else
                    {
                        testCost = new HrdtestCost();
                        testCost.Hrdid = id;
                        testCost.Qty = item.Qty;
                        testCost.Cost = item.Cost;
                        testCost.TestName = item.TestName;
                        _context.Entry(testCost).State = EntityState.Added;
                    }

                    hrd.HrdtestCosts.Add(testCost);
                }
            }

            if (model.HrdNote != null)
            {
                foreach (var item in model.HrdNote)
                {
                    var note = _context.Hrdnotes.FirstOrDefault(f => f.Id == item.Id);

                    if (note != null)
                    {
                        note.Id = item.Id;
                        note.Hrdid = id;
                        note.Category = item.Category;
                        note.Date = item.Date;
                        note.Description = item.Description;
                        note.FileName = item.Filename;
                        note.Path = item.Path;
                        note.UserId = item.UserId;
                        note.Size = item.Size;
                        _context.Entry(note).State = EntityState.Modified;
                    }
                    else
                    {
                        note = new Hrdnote();
                        note.Hrdid = id;
                        note.Category = item.Category;
                        note.Date = item.Date;
                        note.Description = item.Description;
                        note.FileName = item.Filename;
                        note.Path = item.Path;
                        note.UserId = item.UserId;
                        note.Size = item.Size;
                        _context.Entry(note).State = EntityState.Added;
                    }

                    hrd.Hrdnotes.Add(note);
                }
            }

            _context.Entry(hrd).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecordExists(id))
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
        [HttpPost("Qa")]
        // [Authorize(Policy = PolicyNames.EditHRDs)]
        public async Task<ActionResult<QARecordViewModel>> PostQARecord(QARecordViewModel model)
        {
            var hrd = new Hrd
            {
                IsHrd = model.IsHRD,
                IsPest = model.IsPest,
                IsSmi = model.IsSMI,
                IsNr = model.IsNR,
                IsFm = model.IsFM,
                IsMicro = model.IsMicro,

                Date = model.Date,
                TimeOfIncident = model.TimeOfIncident,
                YearHeld = model.YearHeld,
                DayCode = model.DayCode,
                Originator = model.Originator,
                Bumanager = model.BUManager,
                CodingType = model.Type,
                Globenum = model.Fert,
                FertDescription = model.FertDescription,
                Line = model.Line,
                LineSupervisor = model.LineSupervisor,
                Area = model.Area,
                AreaIfOther = model.AreaIfOther,
                Shift = model.Shift,
                ShortDescription = model.ShortDescription,
                AdditionalDescription = model.AdditionalDescription,
                DetailedDescription = model.DetailedDescription,

                CasesHeld = model.CasesHeld,
                HourCode = model.HourCode,
                Pos = model.POs,
                ReworkInstructions = model.ReworkInstructions,

                PestType = model.PestType,
                PcocontactedImmediately = model.PCOContactedImmediately,
                ProductAdultered = model.ProductAdultered,
                WhereFound = model.WhereFound,
                IfYesAffectedProduct = model.IfYesAffectedProduct,
                MaterialNumber = model.MaterialNumber,
                RawMaterialDescription = model.RawMaterialDescription,
                SMIVendorBatch = model.SMIVendorBatch,
                VendorNumber = model.VendorNumber,
                VendorName = model.VendorName,
                VendorSiteNumber = model.VendorSiteNumber,
                IsInspections = model.IsInspections,
                IsXray = model.IsXray,
                IsMetalDetector = model.IsMetalDetector,
                Fmtype = model.FMType,
                Size = model.Size,
                Equipment = model.Equipment,
                EquipmentIfOther = model.EquipmentIfOther,
                Rohmaterial = model.ROHMaterial,
                PiecesTotal = model.PiecesTotal,
                FMVendorBatch = model.FMVendorBatch,
                FMSource = model.FMSource,
                DateReceived = model.DateReceived,
                InspectorsName = model.InspectorsName,
                Nrcategory = model.NRCategory,
                Tagged = model.Tagged,
                TagNumber = model.TagNumber,
                Response = model.Response,
                HoldConcern = model.HoldConcern,
                DayOfWeek = model.DayOfWeek,
                When = model.When,
                WhenOther = model.WhenOther,
                DateOfResample = model.DateOfResample,
                MeatComponent = model.MeatComponent,
                VeggieComponent = model.VeggieComponent,
                SauceType = model.SauceType,
                StarchType = model.StarchType,
                AdditionalComments = model.AdditionalComments,
                HrdtestCosts = model.HrdTestCosts.Select(s => new HrdtestCost { Cost = s.Cost, Qty = s.Qty, TestName = s.TestName }).ToList(),
                Hrdnotes = model.HrdNote.Select(s => new Hrdnote { Category = s.Category, Date = s.Date, Description = s.Description, FileName = s.Filename, Path = s.Path, UserId = s.UserId, Size = s.Size }).ToList(),
                HrdMicros = model.HrdMicros.Select(s => new HrdMicro { Hour = s.Hour, Count = s.Count, Organism = s.Organism }).ToList(),
            };

            _context.Hrds.Add(hrd);
            await _context.SaveChangesAsync();

            model.Id = hrd.Id;
            return CreatedAtAction("GetQARecord", new { id = model.Id }, model);
        }

        // DELETE: api/Hrds/5
        [HttpDelete("Qa/{id}")]
        // [Authorize(Policy = PolicyNames.EditHRDs)]
        public async Task<IActionResult> DeleteQARecord(int id)
        {
            var hrd = await _context.Hrds
                            .Include(i => i.Hrdpos)
                            .Include(i => i.Hrddcs)
                            .Include(i => i.Hrdfcs)
                            .Include(i => i.Hrdnotes)
                            .Include(i => i.HrdtestCosts)
                            .Include(i => i.HrdMicros)
                            .FirstOrDefaultAsync(f => f.Id == id);

            if (hrd == null)
                return NotFound();

            if (hrd.Hrdpos.Count > 0)
                _context.Hrdpos.RemoveRange(hrd.Hrdpos);

            if (hrd.Hrddcs.Count > 0)
                _context.Hrddcs.RemoveRange(hrd.Hrddcs);

            if (hrd.Hrdfcs.Count > 0)
                _context.Hrdfcs.RemoveRange(hrd.Hrdfcs);

            if (hrd.Hrdnotes.Count > 0)
                _context.Hrdnotes.RemoveRange(hrd.Hrdnotes);

            if (hrd.HrdMicros.Count > 0)
                _context.HrdMicros.RemoveRange(hrd.HrdMicros);

            if (hrd.HrdtestCosts.Count > 0)
                _context.HrdtestCosts.RemoveRange(hrd.HrdtestCosts);

            _context.Hrds.Remove(hrd);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
