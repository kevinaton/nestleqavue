using HRD.WebApi.Authorization;
using HRD.WebApi.Data;
using HRD.WebApi.Data.Entities;
using HRD.WebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Newtonsoft.Json;
using System.Globalization;

namespace HRD.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HrdsController : ControllerBase
    {
        private readonly HRDContext _context;
        protected IConfiguration Configuration { get; }

        public HrdsController(HRDContext context,
            IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        // GET: api/Hrds
        [HttpGet]
        [Authorize(Policy = PolicyNames.ViewHRDs)]
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
                                        || f.Originator.Contains(filter.SearchString)
                                        || f.Line.Contains(filter.SearchString)
                                        || f.Shift.Contains(filter.SearchString)
                                        || f.HourCode.Contains(filter.SearchString)
                                        || f.Cases.ToString().Contains(filter.SearchString));

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
        [Authorize(Policy = PolicyNames.ViewHRDs)]
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
                DateHeld = hrd.DateHeld,
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
                Clear = hrd.Clear ?? 0,
                HrdcompletedBy = hrd.HrdcompletedBy,
                Scrap = hrd.Scrap ?? 0,
                DateofDisposition = hrd.DateofDisposition,
                ThriftStore = hrd.ThriftStore ?? 0,
                Complete = hrd.Complete,
                Cancelled = hrd.Cancelled,
                Samples = hrd.Samples ?? 0,
                NumberOfDaysHeld = hrd.NumberOfDaysHeld,
                Donate = hrd.Donate ?? 0,
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

                MonthHeld = hrd.DateHeld.HasValue ? hrd.DateHeld.Value.ToString("MMMM") : string.Empty,//MonthHeld,
                WeekHeld = hrd.DateHeld.HasValue ? ISOWeek.GetWeekOfYear(hrd.DateHeld.Value) : null,
                CostofProductonHold = hrd.CostofProductonHold,
                ReworkApproved = hrd.ReworkApproved,

                NumberOfDaysToReworkApproval = hrd.NumberOfDayToReworkApproval,
                CaseCount = hrd.ScrapCaseCount,
                ReasonAction = hrd.ScrapReasonAction,
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

                HrdNotes = hrd.Hrdnotes.Select(s => new HrdNoteViewModel { Id = s.Id, HrdId = s.Hrdid, Category = s.Category, Date = s.Date, Description = s.Description, Filename = s.FileName, Size = s.Size, UserId = s.UserId }).ToList(),
                HrdDc = hrd.Hrddcs.Select(s => new HrdDCViewModel { Id = s.Id, HrdId = s.Hrdid, Location = s.Location, NumberOfCases = s.NumberOfCases }).ToList(),
                HrdFc = hrd.Hrdfcs.Select(s => new HrdFCViewModel { Id = s.Id, HrdId = s.Hrdid, Location = s.Location, NumberOfCases = s.NumberOfCases }).ToList(),
                HrdPo = hrd.Hrdpos.Select(s => new HrdPoViewModel { Id = s.Id, HrdId = s.Hrdid, PONumber = s.Ponumber }).ToList(),
            };

            return model;
        }
                
        // PUT: api/Hrds/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("Hrd/{id}")]
        [Authorize(Policy = PolicyNames.EditHRDs)]
        public async Task<IActionResult> PutHrd(int id, [FromForm] string jsonString, [FromForm] List<IFormFile> files)
        {
            var model = JsonConvert.DeserializeObject<HRDDetailViewModel>(jsonString);
            if (id != model.Id)
            {
                return BadRequest();
            }

            var hrd = _context.Hrds.Include(i => i.HrdtestCosts)
                                    .Include(i => i.HrdMicros)
                                    .Include(i => i.Hrdfcs)
                                    .Include(i => i.Hrddcs)
                                    .Include(i => i.Hrdpos)
                                    .Include(i => i.Hrdnotes)
                                    .Where(i => i.Id == id).FirstOrDefault();

            hrd.Id = id;
            hrd.DateHeld = model.DateHeld;
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

            hrd.CostofProductonHold = model.CostofProductonHold;
            hrd.ReworkApproved = model.ReworkApproved;

            //not mapped
            hrd.NumberOfDayToReworkApproval = model.NumberOfDaysToReworkApproval;
            hrd.YearOfIncident = model.YearOfIncident;
            hrd.ScrapCaseCount = model.CaseCount;
            hrd.ScrapReasonAction = model.ReasonAction;
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

            //HRD First Check
            if (model.HrdFc != null)
            {
                foreach (var item in model.HrdFc)
                {
                    if(item.Id < 1)
                    {
                        var hrdFc = new Hrdfc()
                        {
                            Hrdid = id,
                            Location = item.Location,
                            NumberOfCases = item.NumberOfCases
                        };

                        hrd.Hrdfcs.Add(hrdFc);
                    }
                }
            }

            //Delete HrdFc
            foreach (var item in hrd.Hrdfcs)
            {
                if (!model.HrdFc.Any(a => a.Id == item.Id))
                {
                    _context.Entry(item).State = EntityState.Deleted;
                }
            }

            //HRd Double Check
            if (model.HrdDc != null)
            {
                foreach (var item in model.HrdDc)
                {
                    if(item.Id < 1)
                    {
                        var hrdDc = new Hrddc()
                        {
                            Hrdid = id,
                            Location = item.Location,
                            NumberOfCases = item.NumberOfCases
                        };

                        hrd.Hrddcs.Add(hrdDc);
                    }
                }
            }


            //Delete HrdDc
            foreach (var item in hrd.Hrddcs)
            {
                if (!model.HrdDc.Any(a => a.Id == item.Id))
                {
                    _context.Entry(item).State = EntityState.Deleted;
                }
            }

            //Hrd PO Number
            if (model.HrdPo != null)
            {
                foreach (var item in model.HrdPo)
                {
                    if(!hrd.Hrdpos.Any(a => a.Ponumber == item.PONumber))
                    {
                        var po = new Hrdpo()
                        {
                            Id = item.Id,
                            Hrdid = id,
                            Ponumber = item.PONumber
                        };
                        hrd.Hrdpos.Add(po);
                    }
                }
            }

            //Delete Hrd Po
            foreach (var item in hrd.Hrdpos)
            {
                if (!model.HrdPo.Any(a => a.PONumber == item.Ponumber))
                {
                    _context.Entry(item).State = EntityState.Deleted;
                }
            }

            //Hrd Notes
            foreach (var item in model.HrdNotes)
            {
                if (item.Id < 1 && files.Any(a => a.FileName == item.Filename))
                {
                    string path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, Configuration["FilePath"].ToString()));

                    var note = new Hrdnote
                    {
                        Hrdid = id,
                        Category = item.Category,
                        Date = item.Date,
                        Description = item.Description,
                        FileName = item.Filename,
                        Path = path, //Path + file Our own filename generated.
                        UserId = User.Identities.First().Name, //item.UserId, //Current Identity User
                        Size = item.Size
                    };
                    _context.Entry(note).State = EntityState.Added;

                    hrd.Hrdnotes.Add(note);

                    //Process Save file Here
                    var isUploaded = await this.UploadFile(files.First(f => f.FileName == item.Filename));
                }
            }

            //Delete Hrd Notes
            foreach(var item in hrd.Hrdnotes)
            {
                if(!model.HrdNotes.Any(a => a.Id == item.Id))
                {
                    _context.Entry(item).State = EntityState.Deleted;

                    //Process delete file here;
                    if(System.IO.File.Exists(item.Path))
                        this.DeleteFile(item.Path);
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
        [HttpPost("Hrd")]
        [Authorize(Policy = PolicyNames.EditHRDs)]
        public async Task<ActionResult<HRDDetailViewModel>> PostHrd(HRDDetailViewModel model)
        {
            var hrd = new Hrd
            {
                DateHeld = model.DateHeld,
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

                CostofProductonHold = model.CostofProductonHold,
                ReworkApproved = model.ReworkApproved,

                //not mapped
                NumberOfDayToReworkApproval = model.NumberOfDaysToReworkApproval,
                YearOfIncident = model.YearOfIncident,
                ScrapCaseCount = model.CaseCount,
                ScrapReasonAction = model.ReasonAction,
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

                Hrdnotes = model.HrdNotes.Select(s => new Hrdnote { Category = s.Category, Date = s.Date, Description = s.Description, FileName = s.Filename, UserId = s.UserId, Size = s.Size }).ToList(),
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
        [Authorize(Policy = PolicyNames.EditHRDs)]
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
        [Authorize(Policy = PolicyNames.ViewHRDs)]
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

                DateHeld = qa.DateHeld,
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
                CasesHeld = qa.Cases,
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
                HrdNotes = qa.Hrdnotes.Select(s => new HrdNoteViewModel { Id = s.Id, HrdId = s.Hrdid, Category = s.Category, Date = s.Date, Description = s.Description, Filename = s.FileName, Size = s.Size, UserId = s.UserId }).ToList(),
                HrdMicros = qa.HrdMicros.Select(s => new HRDMicroViewModel { Id = s.Id, HrdId = s.HrdId, Hour = s.Hour, Count = s.Count, Organism = s.Organism }).ToList(),
            };

            return model;
        }

        // PUT: api/Hrds/Qa/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("Qa/{id}")]
        [Authorize(Policy = PolicyNames.EditHRDs)]
        public async Task<IActionResult> PutQARecord(int id, [FromForm] string jsonString, [FromForm] List<IFormFile> files)
        {
            var model = JsonConvert.DeserializeObject<QARecordViewModel>(jsonString);
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

            hrd.DateHeld = model.DateHeld;
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
            hrd.Cases = model.CasesHeld;
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
                    HrdMicro micro = null;
                    if(item.Id > 0)
                    {
                        micro = _context.HrdMicros.FirstOrDefault(f => f.Id == item.Id);

                        if (micro != null)
                        {
                            micro.HrdId = id;
                            micro.Hour = item.Hour;
                            micro.Count = item.Count;
                            micro.Organism = item.Organism;
                            _context.Entry(micro).State = item.IsDeleted ? EntityState.Deleted : EntityState.Modified;
                        }
                    }
                    else
                    {
                        micro = new HrdMicro
                        {
                            HrdId = id,
                            Hour = item.Hour,
                            Count = item.Count,
                            Organism = item.Organism
                        };
                        _context.Entry(micro).State = EntityState.Added;

                        hrd.HrdMicros.Add(micro);
                    }

                }
            }

            if (model.HrdTestCosts != null)
            {
                foreach (var item in model.HrdTestCosts)
                {
                    HrdtestCost testCost = null;
                    if(item.Id > 0)
                    {
                        testCost = _context.HrdtestCosts.FirstOrDefault(f => f.Id == item.Id);
                        if(testCost != null)
                        {
                            testCost.Id = testCost.Id;
                            testCost.Hrdid = id;
                            testCost.Qty = item.Qty;
                            testCost.Cost = item.Cost;
                            testCost.TestName = item.TestName;
                            _context.Entry(testCost).State = item.IsDeleted ? EntityState.Deleted : EntityState.Modified;
                        }
                    }
                    else
                    {
                        testCost = new HrdtestCost
                        {
                            Hrdid = id,
                            Qty = item.Qty,
                            Cost = item.Cost,
                            TestName = item.TestName
                        };
                        _context.Entry(testCost).State = EntityState.Added;

                        hrd.HrdtestCosts.Add(testCost);
                    }


                }
            }

            //Hrd Notes
            foreach (var item in model.HrdNotes)
            {
                if (item.Id < 1 && files.Any(a => a.FileName == item.Filename))
                {
                    string path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, Configuration["FilePath"].ToString()));

                    var note = new Hrdnote
                    {
                        Hrdid = id,
                        Category = item.Category,
                        Date = item.Date,
                        Description = item.Description,
                        FileName = item.Filename,
                        Path = path, //Path + file Our own filename generated.
                        UserId = User.Identities.First().Name, //item.UserId, //Current Identity User
                        Size = item.Size
                    };
                    _context.Entry(note).State = EntityState.Added;

                    hrd.Hrdnotes.Add(note);

                    //Process Save file Here
                    var isUploaded = await this.UploadFile(files.First(f => f.FileName == item.Filename));
                }
            }

            //Delete Hrd Notes
            foreach (var item in hrd.Hrdnotes)
            {
                if (!model.HrdNotes.Any(a => a.Id == item.Id))
                {
                    _context.Entry(item).State = EntityState.Deleted;

                    //Process delete file here;
                    if (System.IO.File.Exists(item.Path))
                        this.DeleteFile(item.Path);
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
        [Authorize(Policy = PolicyNames.EditHRDs)]
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

                DateHeld = model.DateHeld,
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

                Cases = model.CasesHeld,
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
                Hrdnotes = model.HrdNotes.Select(s => new Hrdnote { Category = s.Category, Date = s.Date, Description = s.Description, FileName = s.Filename, UserId = s.UserId, Size = s.Size }).ToList(),
                HrdMicros = model.HrdMicros.Select(s => new HrdMicro { Hour = s.Hour, Count = s.Count, Organism = s.Organism }).ToList(),
            };

            _context.Hrds.Add(hrd);
            await _context.SaveChangesAsync();

            model.Id = hrd.Id;
            return CreatedAtAction("GetQARecord", new { id = model.Id }, model);
        }

        // DELETE: api/Hrds/5
        [HttpDelete("Qa/{id}")]
        [Authorize(Policy = PolicyNames.EditHRDs)]
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

        [HttpPost("Recalculate")]
        [Authorize(Policy = PolicyNames.EditHRDs)]
        public ActionResult<RecalculateViewModel> Recalculate(RecalculateViewModel model)
        {
            var recalculate = model.Clear + model.Sample + model.Scrap + model.ThriftStore + model.Donate;

            return Ok(recalculate);
        }

        [HttpPost("UploadFiles"), DisableRequestSizeLimit]
        public async Task<bool> UploadFiles(List<IFormFile> files)
        {
            try
            {
                if (files.Count > 0)
                {
                    string path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, Configuration["FilePath"].ToString()));
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    foreach (var file in files)
                    {
                        using var fileStream = new FileStream(Path.Combine(path, file.FileName), FileMode.Create);
                        await file.CopyToAsync(fileStream);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("File Upload Failed", ex);
            }
        }

        private async Task<bool> UploadFile(IFormFile file)
        {
            try
            {
                string path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, Configuration["FilePath"].ToString()));
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                using var fileStream = new FileStream(Path.Combine(path, file.FileName), FileMode.Create);
                await file.CopyToAsync(fileStream);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("File Upload Failed", ex);
            }
        }

        private bool DeleteFile(string path)
        {
            try
            {
                System.IO.File.Delete(path);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("File Delete Failed", ex);
            }
        }

        [HttpGet("DownloadFile")]
        public async Task<ActionResult> DownloadFile(string filename)
        {
            // ... code for validation and get the file
            string path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, Configuration["FilePath"].ToString() + "//" + filename));

            if (!System.IO.File.Exists(path))
                return NotFound();
            else
            {
                var provider = new FileExtensionContentTypeProvider();
                if (!provider.TryGetContentType(path, out var contentType))
                {
                    contentType = "application/octet-stream";
                }

                var bytes = await System.IO.File.ReadAllBytesAsync(path);
                return File(bytes, contentType, Path.GetFileName(path));
            }
        }
    }
}
