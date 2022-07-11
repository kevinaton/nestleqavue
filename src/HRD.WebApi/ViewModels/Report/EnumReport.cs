namespace HRD.WebApi.ViewModels.Report
{
    public enum EnumWeekHeld
    {
        Select = -1,
        Sunday = 0,
        Monday = 1,
        Tuesday = 2,
        Wednesday = 3,
        Thursday = 4,
        Friday = 5,
        Saturday = 6,
    }
    public enum EnumStatus
    {
        Open = 0,
        Closed = 1,
        All = 2
    }
    public enum EnumFMCasesOptions
    {
        ByCategory = 1,
        ByInhouseVendor = 2,
        ByLine = 3,
        ByShift = 4,
    }
    public enum EnumMicrobeTypes
    {
        ByTypesOfMicrobes = 1,
        ByLine = 2,
        ByTopProducts = 3,
        ByShift = 4,
    }
    public enum EnumCostGraph
    {
        CostByCategory = 1,
        CostByAllocation = 2,
    }
}
