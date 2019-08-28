using Pkpm.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PkpmGX.Models
{
    public class ApplyQualifyFiveModels
    {
    }

    public class ApplyQualifyFiveDetailsModel
    {
        public string pid { get; set; }
        public string customId { get; set; }
        public List<SysDict> checkWork { get; set; }
        //public List<ApplyQualifySevenEquipModel> Equips;
    }


    public class ApplyQualifyFiveEditSaveModel
    {
        public string PId { get; set; }
        public string CustomId { get; set; }
        public string gridStr { get; set; }
    }



    public class ApplyQualifyFiveSearchModel
    {
        public string PId { get; set; }
        public string CustomId { get; set; }
        public int? posStart { get; set; }
        public int? count { get; set; }
    }

}