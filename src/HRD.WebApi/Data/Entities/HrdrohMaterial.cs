namespace HRD.WebApi.Data.Entities
{
    public class HrdrohMaterial
    {
        public int Id { get; set; }
        public int Hrdid { get; set; }
        public int DropDownItemId { get; set; }
        public string Material { get; set; }
        public string Description { get; set; }
        public virtual Hrd Hrd { get; set; }
        public virtual DropDownItem DropDownItem { get; set; }
    }
}
