namespace HRD.WebApi.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Year { get; set; }
        public string Fert { get; set; }
        public string Description { get; set; }
        public decimal? CostPerCase { get; set; }
        public string Country { get; set; }
        public bool? NoBbdate { get; set; }
        public bool? Holiday { get; set; }
    }
}
