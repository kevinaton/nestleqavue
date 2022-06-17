namespace HRD.WebApi.Data.Entities
{
    public class HrdMicro
    {
        public int Id { get; set; }
        public int HrdId { get; set; }
        public int Hour { get; set; }
        public int Count { get; set; }
        public string Organism { get; set; }
        public virtual Hrd Hrd { get; set; }
    }
}
