//using HRD.WebApi.Data.Entities;
using System;
using System.Collections.Generic;

namespace HRD.WebApi.ViewModels
{
    public class HRDDetailViewModel
    {
        public int Id { get; set; }

        //Highlights
        public DateTime? Date { get; set; }
        public DateTime? TimeOfIncident { get; set; }
        public string YearHeld { get; set; }
        public string DayCode { get; set; }
        public string Originator { get; set; }
        public string Plant { get; set; }

        public string BUManager { get; set; }
        public string Type { get; set; }
        public string Fert { get; set; }
        public string FertDescription { get; set; }
        public string Line { get; set; }
        public string LineSupervisor { get; set; }
        public string Area { get; set; }
        public string AreaIfOther { get; set; }
        public string Shift { get; set; }
        public string ShortDescription { get; set; }
        public string AdditionalDescription { get; set; }
        public string DetailedDescription { get; set; }

        //DETAILS
        public bool? Gstdrequired { get; set; }
        public string HourCode { get; set; }
        public string ContinuousRun { get; set; }
        public List<HrdDCViewModel> HrdDc { get; set; }
        public List<HrdFCViewModel> HrdFc { get; set; }
        public List<HrdNoteViewModel> HrdNote { get; set; }
        public List<HrdPoViewModel> HrdPo { get; set; }
        public string QaComments { get; set; }
        public DateTime? DateCompleted { get; set; }
        public int? Clear { get; set; }
        public string HrdcompletedBy { get; set; }
        public int? Scrap { get; set; }
        public DateTime? DateofDisposition { get; set; }
        public int? ThriftStore { get; set; }
        public bool? Complete { get; set; }
        public bool? Cancelled { get; set; }
        public int? Samples { get; set; }
        public int? NumberOfDaysHeld { get; set; }
        public int? Donate { get; set; }
        public bool? AllCasesAccountedFor { get; set; }
        public int? Cases { get; set; }
        public bool? OtherHrdAffected { get; set; }
        public bool? HighRisk { get; set; }
        public string OtherHrdNum { get; set; }

        //FC = firstcheck DC = double check
        public DateTime? FcDate { get; set; }
        public string FcUser { get; set; }
        public DateTime? DcDate { get; set; }
        public string DcUser { get; set; }
        public string Classification { get; set; }
        public string HoldCategory { get; set; }
        public string HoldSubCategory { get; set; }
        public DateTime? DateHeld { get; set; }

        public string MonthHeld { get; set; }
        public int? WeekHeld { get; set; }
        public decimal? CostofProductonHold { get; set; }
        public bool? ReworkApproved { get; set; }
        public int? NumberOfDaysToReworkApproval { get; set; }
        public bool? ApprovalRequestByQa { get; set; }
        public int? CaseCount { get; set; }
        public string ReasonAction { get; set; }
        public bool? IsPlantManagerAprpoval { get; set; }
        public bool? IsPlantControllerApproval { get; set; }
        public bool? IsDestroyed { get; set; }
        public string ApprovedByQAWho { get; set; }
        public DateTime? ApprovedByQAWhen { get; set; }
        public string ApprovedByPlantManagerWho { get; set; }
        public DateTime? ApprovedPlantManagerQAWhen { get; set; }
        public string ApprovedByPlantControllerWho { get; set; }
        public DateTime? ApprovedByPlantControllerWhen { get; set; }
        public string ApprovedByDistroyedWho { get; set; }
        public DateTime? ApprovedByDistroyedWhen { get; set; }
        public string Comments { get; set; }
    }
}
