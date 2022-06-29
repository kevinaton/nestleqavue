namespace HRD.WebApi.ViewModels.Report
{
    public class GetMicrobeGraphDataInput : DataInput
    {
        public EnumStatus Status { get; set; }
        public EnumMicrobeTypes Types { get; set; }
    }
}
