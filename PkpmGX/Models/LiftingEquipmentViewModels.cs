
using QZWebService.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PkpmGX.Models
{
    public class LiftingEquipmentViewModels
    {
        public view_programmeLiftList programme { get; set; }
        public List<LiftingEquipmentPeopleModel> CheckPeoples { get; set; }
        public List<LiftingEquipmentPeopleModel> WitnessPeoples { get; set; }
    }

    public class LiftingEquipmentPeopleModel
    {
        //[{"name":"33","phone":"15676731331","postnum":"554544"},{"name":"","phone":"","postnum":""}]
        public string name { get; set; }
        public string phone { get; set; }
        public string postnum { get; set; }
    }

    public class LiftingEquipmentSearchModel
    {
        public string customId { get; set; }
        public string projectname { get; set; }
        public string areainfo { get; set; }
        public string testpeople { get; set; }
        public bool? IsReport { get; set; }
        public bool? IsPhoto { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? posStart { get; set; }
        public int? count { get; set; }
    }

    public class LiftingEquipmentGetSysPrimaryKeyModel
    {
        public string sysPrimaryKey { get; set; }
    }
}