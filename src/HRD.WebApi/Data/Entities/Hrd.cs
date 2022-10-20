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
            HrdMicros = new HashSet<HrdMicro>();
        }

        public int Id { get; set; }
        public string Plant { get; set; }
        public string Originator { get; set; }
        public bool? IsHrd { get; set; }
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
        public bool? IsPest { get; set; }
        public bool? IsSmi { get; set; }
        public bool? IsNr { get; set; }
        public bool? IsFm { get; set; }
        public bool? IsMicro { get; set; }
        public DateTime? TimeOfIncident { get; set; }
        public string Bumanager { get; set; }
        public string FertDescription { get; set; }
        public string LineSupervisor { get; set; }
        public string Area { get; set; }
        public string AreaIfOther { get; set; }
        public string AdditionalDescription { get; set; }
        public string DetailedDescription { get; set; }
        public string ContinuousRun { get; set; }
        public DateTime? DateCompleted { get; set; }
        public int? NumberOfDaysHeld { get; set; }
        public int? Donate { get; set; }
        public int? Total { get; set; }
        public int? NumberOfDayToReworkApproval { get; set; }
        public int? ScrapCaseCount { get; set; }
        public string ScrapReasonAction { get; set; }
        public bool? IsApprovalRequestByQa { get; set; }
        public bool? IsPlantManagerAprpoval { get; set; }
        public bool? IsPlantControllerApproval { get; set; }
        public bool? IsDestroyed { get; set; }
        public string ApprovedByQawho { get; set; }
        public DateTime? ApprovedByQawhen { get; set; }
        public string ApprovedByPlantManagerWho { get; set; }
        public DateTime? ApprovedPlantManagerQawhen { get; set; }
        public string ApprovedByPlantControllerWho { get; set; }
        public DateTime? ApprovedByPlantControllerWhen { get; set; }
        public string ApprovedByDistroyedWho { get; set; }
        public DateTime? ApprovedByDistroyedWhen { get; set; }
        public string Comments { get; set; }
        public string PestType { get; set; }
        public string PcocontactedImmediately { get; set; }
        public string ProductAdultered { get; set; }
        public string WhereFound { get; set; }
        public string IfYesAffectedProduct { get; set; }
        public string MaterialNumber { get; set; }
        public string RawMaterialDescription { get; set; }
        public string SMIVendorBatch { get; set; }
        public string VendorNumber { get; set; }
        public string VendorName { get; set; }
        public string VendorSiteNumber { get; set; }
        public bool? IsInspections { get; set; }
        public bool? IsXray { get; set; }
        public bool? IsMetalDetector { get; set; }
        public string Fmtype { get; set; }
        public string Size { get; set; }
        public string Equipment { get; set; }
        public string EquipmentIfOther { get; set; }
        public string Rohmaterial { get; set; }
        public string PiecesTotal { get; set; }
        public string FMVendorBatch { get; set; }
        public string FMSource { get; set; }
        public DateTime? DateReceived { get; set; }
        public string InspectorsName { get; set; }
        public string Nrcategory { get; set; }
        public string Tagged { get; set; }
        public string TagNumber { get; set; }
        public string Response { get; set; }
        public string HoldConcern { get; set; }
        public string DayOfWeek { get; set; }
        public string When { get; set; }
        public string WhenOther { get; set; }
        public DateTime? DateOfResample { get; set; }
        public string MeatComponent { get; set; }
        public string VeggieComponent { get; set; }
        public string SauceType { get; set; }
        public string StarchType { get; set; }
        public string AdditionalComments { get; set; }
        public string YearOfIncident { get; set; }
        public virtual ICollection<Hrddc> Hrddcs { get; set; }
        public virtual ICollection<Hrdfc> Hrdfcs { get; set; }
        public virtual ICollection<Hrdnote> Hrdnotes { get; set; }
        public virtual ICollection<Hrdpo> Hrdpos { get; set; }
        public virtual ICollection<HrdtestCost> HrdtestCosts { get; set; }
        public virtual ICollection<HrdMicro> HrdMicros { get; set; }
    }
}
