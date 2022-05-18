using System;
using System.Collections.Generic;

namespace HRD.WebApi.Data.Entities
{
    public partial class Security
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public bool? Administrator { get; set; }
        public bool? Qamanager { get; set; }
        public bool? Qatgrp { get; set; }
        public bool? QaprodRelGrp { get; set; }
        public bool? ProdTlgrp { get; set; }
        public bool? ReworkMembersGrp { get; set; }
        public bool? PhysicalGrp { get; set; }
        public bool? Physical5Grp { get; set; }
        public bool? GstdnotificationGrp { get; set; }
        public bool? DeleteHrd { get; set; }
        public bool? EditHrdapproveRework { get; set; }
        public bool? ProdMaintenanceList { get; set; }
        public bool? Gstdgrp { get; set; }
        public bool? Bumgrp { get; set; }
        public bool? LaborCostList { get; set; }
        public bool? Notification { get; set; }
    }
}
