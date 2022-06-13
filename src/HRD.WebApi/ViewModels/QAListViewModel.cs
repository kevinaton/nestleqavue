namespace HRD.WebApi.ViewModels
{
    public class QAListViewModel
    {
        public int Id { get; set; }
        public string DayCode { get; set; }
        public string Type { get; set; }
        public string Fert { get; set; }
        public string ProductDescription { get; set; }
        public string Line { get; set; }
        public string Shift { get; set; }
        public string HourCode { get; set; }
        public int? Cases { get; set; }
        public string ShortDescription { get; set; }
        public string Originator { get; set; }
        public bool? IsHRD { get; set; }
        public bool? IsPest { get; set; }
        public bool? IsSMI { get; set; }
        public bool? IsNR { get; set; }
        public bool? IsFM { get; set; }
        public bool? IsMicro { get; set; }

    }
}
