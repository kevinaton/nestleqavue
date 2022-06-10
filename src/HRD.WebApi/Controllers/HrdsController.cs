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

        private static string GetQAType(bool isHrd, bool isPest, bool isSmi, bool isNr, bool isFm, bool isMicro)
        {
            string type;
            if (isPest)
                type = "pest";
            else if (isSmi)
                type = "smi";
            else if (isNr)
                type = "nr";
            else if (isFm)
                type = "fm";
            else if (isMicro)
                type = "micro";
            else
                type = "hrd";

            return type;
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
                    Type = GetQAType(s.IsHrd ?? false, s.IsPest ?? false, s.IsSmi ?? false, s.IsNr ?? false, s.IsFm ?? false, s.IsMicro ?? false),
                    Fert = s.Globenum, //TODO: Not mapped
                    ProductDescription = s.ShortDescription, //TODO: Confirm if correct
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
                case "daycode":
                    query = validFilter.SortOrder == "desc"
                        ? query.OrderByDescending(o => o.DayCode)
                        : query.OrderBy(o => o.DayCode);
                    break;
                //case "type":
                //    query = validFilter.SortOrder == "desc"
                //        ? query.OrderByDescending(o => o.Type)
                //        : query.OrderBy(o => o.Type);
                //    break;
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
        [Authorize(Policy = PolicyNames.ViewHRDs)]
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

                HrdDc = hrd.Hrddcs.Select(s => new HrdDCViewModel { Id = s.Id, HrdId = s.Hrdid, Location = s.Location, NumberOfCases = s.NumberOfCases }).ToList(),
                HrdFc = hrd.Hrdfcs.Select(s => new HrdFCViewModel { Id = s.Id, HrdId = s.Hrdid, Location = s.Location, NumberOfCases = s.NumberOfCases }).ToList(),
                HrdNote = hrd.Hrdnotes.Select(s => new HrdNoteViewModel { Id = s.Id, HrdId = s.Hrdid, Category = s.Category, Date = s.Date, Description = s.Description, Filename = s.FileName, Path = s.Path, Size = s.Size, UserId = s.UserId }).ToList(),
                HrdPo = hrd.Hrdpos.Select(s => new HrdPoViewModel { Id = s.Id, HrdId = s.Hrdid, PONumber = s.Ponumber }).ToList(),

                QaComments = hrd.Qacomments,
                DateCompleted = hrd.DateCompleted, //not mapped
                Clear = hrd.Clear,
                HrdcompletedBy = hrd.HrdcompletedBy,
                Scrap = hrd.Scrap,
                DateofDisposition = hrd.DateofDisposition,
                ThriftStore = hrd.ThriftStore,
                Complete = hrd.Complete,
                Cancelled = hrd.Cancelled,
                Samples = hrd.Samples,
                NumberOfDaysHeld = hrd.NumberOfDaysHeld,//notmappep
                Donate = hrd.Donate, //not mapped
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

                MonthHeld = hrd.MonthHeld, //not mapped
                WeekHeld = hrd.WeekHeld, //not mapped
                CostofProductonHold = hrd.CostofProductonHold,
                ReworkApproved = hrd.ReworkApproved,
                //not mapped
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
            };


            return model;
        }

        //[EnableCors("AllowOrigin")]
        // PUT: api/Hrds/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("Hrd/{id}")]
        [Authorize(Policy = PolicyNames.EditHRDs)]
        public async Task<IActionResult> PutHrd(int id, HRDDetailViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            var hrd = new Hrd
            {
                Id = id,
                YearHeld = model.YearHeld,
                DayCode = model.DayCode,
                Originator = model.Originator,
                Bumanager = model.BUManager,
                CodingType = model.Type,
                Plant = model.Plant,
                Globenum = model.Fert,//not mapped
                Line = model.Line,
                LineSupervisor = model.LineSupervisor, //not mapped
                Area = model.Area,//not mapped
                AreaIfOther = model.AreaIfOther,//not mapped
                Shift = model.Shift,
                ShortDescription = model.ShortDescription,
                AdditionalDescription = model.AdditionalDescription, //not mapped
                DetailedDescription = model.DetailedDescription, //not mapped
                Gstdrequired = model.Gstdrequired,
                HourCode = model.HourCode,
                Hrdpos = model.HrdPo.Select(s => new Hrdpo { Ponumber = s.PONumber }).ToList(),

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
                HighRisk = model.HighRisk,
                OtherHrdnum = model.OtherHrdNum,

                Hrddcs = model.HrdDc.Select(s => new Hrddc { Location = s.Location, NumberOfCases = s.NumberOfCases }).ToList(),
                Hrdfcs = model.HrdDc.Select(s => new Hrdfc { Location = s.Location, NumberOfCases = s.NumberOfCases }).ToList(),
                Hrdnotes = model.HrdNote.Select(s => new Hrdnote { Category = s.Category, Date = s.Date, Description = s.Description, FileName = s.Filename, Path = s.Path, UserId = s.UserId, Size = s.Size }).ToList(),

                Fcdate = model.FcDate,
                Fcuser = model.FcUser,
                Dcdate = model.DcDate,
                Dcuser = model.DcUser,
                Classification = model.Classification,
                HoldCategory = model.HoldCategory,
                HoldSubCategory = model.HoldSubCategory,
                DateHeld = model.DateHeld,

                MonthHeld = model.MonthHeld,
                WeekHeld = model.WeekHeld, //not mapped
                CostofProductonHold = model.CostofProductonHold,
                ReworkApproved = model.ReworkApproved,

                //not mapped
                NumberOfDayToReworkApproval = model.NumberOfDaysToReworkApproval,
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
            };

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
        [Authorize(Policy = PolicyNames.EditHRDs)]
        [HttpPost("Hrd")]
        public async Task<ActionResult<HRDDetailViewModel>> PostHrd(HRDDetailViewModel model)
        {
            var hrd = new Hrd
            {
                YearHeld = model.YearHeld,
                DayCode = model.DayCode,
                Originator = model.Originator,
                Bumanager = model.BUManager,
                CodingType = model.Type, //not mapped
                Plant = model.Plant,
                Globenum = model.Fert,//not mapped
                Line = model.Line,
                LineSupervisor = model.LineSupervisor, //not mapped
                Area = model.Area,//not mapped
                AreaIfOther = model.AreaIfOther,//not mapped
                Shift = model.Shift,
                ShortDescription = model.ShortDescription,
                AdditionalDescription = model.AdditionalDescription, //not mapped
                DetailedDescription = model.DetailedDescription, //not mapped
                Gstdrequired = model.Gstdrequired,
                HourCode = model.HourCode,
                Hrdpos = model.HrdPo.Select(s => new Hrdpo { Ponumber = s.PONumber }).ToList(),

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
                HighRisk = model.HighRisk,
                OtherHrdnum = model.OtherHrdNum,
                Hrddcs = model.HrdDc.Select(s => new Hrddc { Location = s.Location, NumberOfCases = s.NumberOfCases }).ToList(),
                Hrdfcs = model.HrdDc.Select(s => new Hrdfc { Location = s.Location, NumberOfCases = s.NumberOfCases }).ToList(),
                Hrdnotes = model.HrdNote.Select(s => new Hrdnote { Category = s.Category, Date = s.Date, Description = s.Description, FileName = s.Filename, Path = s.Path, UserId = s.UserId, Size = s.Size }).ToList(),

                Fcdate = model.FcDate,
                Fcuser = model.FcUser,
                Dcdate = model.DcDate,
                Dcuser = model.DcUser,
                Classification = model.Classification,
                HoldCategory = model.HoldCategory,
                HoldSubCategory = model.HoldSubCategory,
                DateHeld = model.DateHeld,

                MonthHeld = model.MonthHeld,
                WeekHeld = model.WeekHeld, //not mapped
                CostofProductonHold = model.CostofProductonHold,
                ReworkApproved = model.ReworkApproved,

                //not mapped
                NumberOfDayToReworkApproval = model.NumberOfDaysToReworkApproval,
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
                            .FirstOrDefaultAsync(f => f.Id == id);
            
            if (hrd == null)
                return NotFound();

            if(hrd.Hrdpos.Count > 0)
                _context.Hrdpos.RemoveRange(hrd.Hrdpos);

            if (hrd.Hrddcs.Count > 0)
                _context.Hrddcs.RemoveRange(hrd.Hrddcs);

            if (hrd.Hrdfcs.Count > 0)
                _context.Hrdfcs.RemoveRange(hrd.Hrdfcs);

            if (hrd.Hrdnotes.Count > 0)
                _context.Hrdnotes.RemoveRange(hrd.Hrdnotes);
            
            if (hrd.HrdtestCosts.Count > 0)
                _context.HrdtestCosts.RemoveRange(hrd.HrdtestCosts);

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
                                         .FirstOrDefaultAsync(f => f.Id == id);

            if (qa == null)
            {
                return NotFound();
            }

            var model = new QARecordViewModel
            {
                Id = id,
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

                CasesHeld = qa.CasesHeld,
                HourCode = qa.HourCode,//not mapped

                //not mapped
                POs = qa.Pos,
                ReworkInstructions = qa.ReworkInstructions,
                PestType = qa.PestType,
                PCOContactedImmediately = qa.PcocontactedImmediately,
                ProductAdultered = qa.ProductAdultered,
                WhereFound = qa.WhereFound,
                IfYesAffectedProduct = qa.IfYesAffectedProduct,
                MaterialNumber = qa.MaterialNumber,
                RawMaterialDescription = qa.RawMaterialDescription,
                BatchLot = qa.BatchLot,
                VendorNumber = qa.VendorNumber,
                VendorName = qa.VendorName,
                VendorSiteNumber = qa.VendorSiteNumber,
                IsInspections = qa.IsInspections,
                IsXray = qa.IsXray,
                IsMetalDetector = qa.IsMetalDetector,
                FMType = qa.Fmtype,
                Size = qa.Size,
                Equipment = qa.Equipment,
                EquipmentIfOther = qa.EquipmentIfOther,
                ROHMaterial = qa.Rohmaterial,
                PiecesTotal = qa.PiecesTotal,
                RawBatchLot = qa.RawBatchLot,
                HazardousSize = qa.HazardousSize,
                Responsibility = qa.Responsibility,
                NonHazardousSize = qa.NonHazardousSize,
                DateReceived = qa.DateReceived,
                InspectorsName = qa.InspectorsName,
                NRCategory = qa.Nrcategory,
                Tagged = qa.Tagged,
                TagNumber = qa.TagNumber,
                Response = qa.Response,
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
                HrdTestCosts = qa.HrdtestCosts.Select(s => new HrdTestCostViewModel { Id = s.Id, HrdId = s.Hrdid, Cost = s.Cost, Qty = s.Qty, TestName = s.TestName}).ToList(),
            };

            return model;
        }

        // PUT: api/Hrds/Qa/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("Qa/{id}")]
        [Authorize(Policy = PolicyNames.EditHRDs)]
        public async Task<IActionResult> PutQARecord(int id, QARecordViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            var hrd = new Hrd
            {
                Id = id,
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
                HourCode = model.HourCode,//not mapped

                //not mapped
                Pos = model.POs,
                ReworkInstructions = model.ReworkInstructions,
                PestType = model.PestType,
                PcocontactedImmediately = model.PCOContactedImmediately,
                ProductAdultered = model.ProductAdultered,
                WhereFound = model.WhereFound,
                IfYesAffectedProduct = model.IfYesAffectedProduct,
                MaterialNumber = model.MaterialNumber,
                RawMaterialDescription = model.RawMaterialDescription,
                BatchLot = model.BatchLot,
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
                RawBatchLot = model.RawBatchLot,
                HazardousSize = model.HazardousSize,
                Responsibility = model.Responsibility,
                NonHazardousSize = model.NonHazardousSize,
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
            };

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
                HourCode = model.HourCode,//not mapped

                //not mapped
                Pos = model.POs,
                ReworkInstructions = model.ReworkInstructions,
                PestType = model.PestType,
                PcocontactedImmediately = model.PCOContactedImmediately,
                ProductAdultered = model.ProductAdultered,
                WhereFound = model.WhereFound,
                IfYesAffectedProduct = model.IfYesAffectedProduct,
                MaterialNumber = model.MaterialNumber,
                RawMaterialDescription = model.RawMaterialDescription,
                BatchLot = model.BatchLot,
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
                RawBatchLot = model.RawBatchLot,
                HazardousSize = model.HazardousSize,
                Responsibility = model.Responsibility,
                NonHazardousSize = model.NonHazardousSize,
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

            _context.Hrds.Remove(hrd);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
