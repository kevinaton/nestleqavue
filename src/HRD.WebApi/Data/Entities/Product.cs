using System;
using System.Collections.Generic;

namespace HRD.WebApi.Data.Entities
{
    public partial class Product
    {
        public int Id { get; set; }
        public string Year { get; set; }
        public string Gpn { get; set; }
        public string Description { get; set; }
        public decimal? CostPerCase { get; set; }
        public string Country { get; set; }
        public bool? NoBbdate { get; set; }
        public bool? Holiday { get; set; }
    }
}
