﻿using System;
using System.Collections.Generic;

namespace HRD.WebApi.Data.Entities
{
    public partial class Setting
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
    }
}