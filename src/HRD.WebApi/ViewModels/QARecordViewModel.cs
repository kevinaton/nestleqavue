using System;
using System.Collections.Generic;

namespace HRD.WebApi.ViewModels
{
    public class QARecordViewModel
    {
        public QARecordViewModel()
        {
            HrdTestCosts = new List<HrdTestCostViewModel>();
            HrdNote = new List<HrdNoteViewModel>();
            HrdMicros = new List<HRDMicroViewModel>();
        }

        public int Id { get; set; }
        public bool? IsHRD { get; set; }
        public bool? IsPest { get; set; }
        public bool? IsSMI { get; set; }
        public bool? IsNR { get; set; }
        public bool? IsFM { get; set; }
        public bool? IsMicro { get; set; }

        //Highlights
        public DateTime? Date { get; set; }        
        public DateTime? TimeOfIncident { get; set; }
        public string YearHeld { get; set; }
        public string DayCode { get; set; }
        public string Originator { get; set; }
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

        //HRD
        public string CasesHeld { get; set; }
        public string HourCode { get; set; }
        public string POs { get; set; }
        public string ReworkInstructions { get; set; }

        //PEST
        public string PestType { get; set; }
        public string PCOContactedImmediately { get; set; }
        public string ProductAdultered { get; set; }
        public string WhereFound { get; set; }
        public string IfYesAffectedProduct { get; set; }
        
        //SMI
        public string MaterialNumber { get; set; }
        public string RawMaterialDescription { get; set; }
        public string SMIVendorBatch { get; set; }
        public string VendorNumber { get; set; }
        public string VendorName { get; set; }
        public string VendorSiteNumber { get; set; }

        //FM
        public bool? IsInspections { get; set; }
        public bool? IsXray { get; set; }
        public bool? IsMetalDetector { get; set; }
        public string FMType { get; set; }
        public string Size { get; set; }
        public string Equipment { get; set; }
        public string EquipmentIfOther { get; set; }
        public string ROHMaterial { get; set; }

        public string FMMaterial { get; set; }
        public string FMDescription { get; set; }
        public string PiecesTotal { get; set; }
        public string FMVendorBatch { get; set; }
        public string FMSource { get; set; }

        //NR
        public DateTime? DateReceived { get; set; }
        public string InspectorsName { get; set; }
        public string NRCategory { get; set; }
        public string Tagged { get; set; }
        public string TagNumber { get; set; }
        public string Response { get; set; }

        //MICRO
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
        public List<HrdTestCostViewModel> HrdTestCosts { get; set; }
        public List<HrdNoteViewModel> HrdNote { get; set; }
        public List<HRDMicroViewModel> HrdMicros { get; set; }
    }
}
