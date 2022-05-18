using System;
using System.Collections.Generic;

namespace HRD.WebApi.Data.Entities
{
    public partial class Hrd
    {
        public Hrd()
        {
            Hrddcs = new HashSet<Hrddc>();
            Hrdfcs = new HashSet<Hrdfc>();
            Hrdnotes = new HashSet<Hrdnote>();
            Hrdpos = new HashSet<Hrdpo>();
            HrdtestCosts = new HashSet<HrdtestCost>();
        }

        public int Id { get; set; }
        public string Plant { get; set; }
        public string Originator { get; set; }
        public bool? HrdValue { get; set; }
        public string DayCode { get; set; }
        public string Line { get; set; }
        public string Shift { get; set; }
        public string Lcl { get; set; }
        public string Schedule { get; set; }
        public string HourCode { get; set; }
        public int? Cases { get; set; }
        public bool? UnpalletizedProductHeld { get; set; }
        public bool? RejectOrNinePack { get; set; }
        public bool? ReworkOutsideNormalProduction { get; set; }
        public bool? ContactOtherPlant { get; set; }
        public string Globenum { get; set; }
        public string ShortDescription { get; set; }
        public string Problem { get; set; }
        public string ReworkInstructions { get; set; }
        public bool? ReworkApproved { get; set; }
        public string ReworkApprovedBy { get; set; }
        public DateTime? ReworkStarted { get; set; }
        public DateTime? ReworkComplete { get; set; }
        public string ReworkCompletedBy { get; set; }
        public bool? CauseMan { get; set; }
        public bool? CauseMethod { get; set; }
        public bool? CauseMaterial { get; set; }
        public bool? CauseMachine { get; set; }
        public string TlforFu { get; set; }
        public string Classification { get; set; }
        public int? Ftqcases { get; set; }
        public int? NonFtqcases { get; set; }
        public DateTime? Dcdate { get; set; }
        public string Dcuser { get; set; }
        public DateTime? Fcdate { get; set; }
        public string Fcuser { get; set; }
        public bool? OtherHrdaffected { get; set; }
        public string OtherHrdnum { get; set; }
        public DateTime? DateHeld { get; set; }
        public string HoldCategory { get; set; }
        public string HoldSubCategory { get; set; }
        public decimal? CostofProductonHold { get; set; }
        public DateTime? DateofDisposition { get; set; }
        public int? Clear { get; set; }
        public int? Scrap { get; set; }
        public int? ThriftStore { get; set; }
        public int? Samples { get; set; }
        public bool? Cancelled { get; set; }
        public bool? Complete { get; set; }
        public string HrdcompletedBy { get; set; }
        public bool? AllCasesAccountedFor { get; set; }
        public DateTime? DateReleased { get; set; }
        public decimal? TestingCost { get; set; }
        public decimal? LaborHours { get; set; }
        public bool? Gstdrequired { get; set; }
        public bool? HighRisk { get; set; }
        public bool? PhysicalIsolationNeeded { get; set; }
        public bool? Physical5DaysGstd { get; set; }
        public DateTime? LastChange { get; set; }
        public string LastChangeUser { get; set; }
        public DateTime? ReworkApprovedDate { get; set; }
        public string CodingType { get; set; }
        public string CodingDetails { get; set; }
        public string YearHeld { get; set; }
        public string Qacomments { get; set; }
        public bool? SecondaryNotification { get; set; }

        public virtual ICollection<Hrddc> Hrddcs { get; set; }
        public virtual ICollection<Hrdfc> Hrdfcs { get; set; }
        public virtual ICollection<Hrdnote> Hrdnotes { get; set; }
        public virtual ICollection<Hrdpo> Hrdpos { get; set; }
        public virtual ICollection<HrdtestCost> HrdtestCosts { get; set; }
    }
}
