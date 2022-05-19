using System;

namespace HRD.WebApi.ViewModels
{
    public class HrdNoteViewModel
    {
        public int Id { get; set; }
        public int HrdId { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public DateTime? Date { get; set; }
        public string Filename { get; set; }
        public string Path { get; set; }
        public string Size { get; set; }
    }
}
