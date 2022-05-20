namespace HRD.WebApi.ViewModels
{
    public class DropDownItemViewModel
    {
        public int Id { get; set; }
        public int DropDownTypeId { get; set; }
        public string Value { get; set; }
        public short SortOrder { get; set; }
        public bool IsActive { get; set; }  
        public DropDownTypeViewModel Type { get; set; }
    }
}
