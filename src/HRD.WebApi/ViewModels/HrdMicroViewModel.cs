namespace HRD.WebApi.ViewModels
{
    public class HRDMicroViewModel
    {
        public int Id { get; set; }
        public int HrdId { get; set; }
        public char Hour { get; set; }
        public int Count { get; set; }
        public string Organism { get; set; }
        public bool IsDeleted { get; set; } = false;

    }
}
