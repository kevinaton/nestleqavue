using System;

namespace HRD.WebApi.ViewModels
{
    public class HrdReworkViewModel
    {
        public int Id { get; set; }
        public DateTime? ReworkStarted { get; set; }
        public DateTime? ReworkComplete { get; set; }
    }
}