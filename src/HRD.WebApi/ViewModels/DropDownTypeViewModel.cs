using System.Collections.Generic;

namespace HRD.WebApi.ViewModels
{
    public class DropDownTypeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<DropDownItemViewModel> DropDownItems { get; set; }
    }
}
