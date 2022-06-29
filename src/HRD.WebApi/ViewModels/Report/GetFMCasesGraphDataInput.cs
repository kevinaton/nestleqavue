namespace HRD.WebApi.ViewModels.Report
{
    public class GetFMCasesGraphDataInput : DataInput
    {
        public EnumStatus Status { get; set; }
        public EnumFMCasesOptions CasesOption { get; set; }
    }
}
