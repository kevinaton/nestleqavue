using System;
namespace HRD.WebApi.ViewModels
{
    public class HrdTestCostViewModel
    {
        public int Id { get; set; }
        public int HrdId { get; set; }
        public string TestName { get; set; }
        public int? Qty { get; set; }
        public decimal? Cost { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
