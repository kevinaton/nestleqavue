namespace HRD.WebApi.ViewModels.Report
{
    public enum EnumWeekHeld
    {
        Monday = 1,
        Tuesday = 2,
        Wednesday = 3,
        Thursday = 4,
        Friday = 5,
        Saturday = 6,
        Sunday = 7
    }
    public enum EnumStatus
    {
        Open = 1,
        Closed = 2,
        All = 3
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
