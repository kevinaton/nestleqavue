using HRD.WebApi.ViewModels.Enums;

namespace HRD.WebApi.ViewModels
{
    public class HrdFilterCriteria
    {
        public string Type { get; set; }
        public int? CompleteStatus { get; set; }
        public string Line { get; set; }
        public string Shift { get; set; }
        public string TeamLeader { get; set; }
        public string BusinessUnitManager { get; set; }
        public string Originator { get; set; }

    }
}
