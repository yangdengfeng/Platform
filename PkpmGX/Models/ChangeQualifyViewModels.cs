using Pkpm.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace PkpmGX.Models
{


    public class ChangeQualifySearchModel
    {
        public string CheckUnitName { get; set; }
        public string status { get; set; }
        public int? posStart { get; set; }
        public int? count { get; set; }
        public int? orderColInd { get; set; }
        public string direct { get; set; }
    }


    public class ChangeQualifyUIModel
    {
        public int id { get; set; }
        public string unitname { get; set; }
        public string area { get; set; }
        public string bgnr { get; set; }
        public string bgq { get; set; }
        public string bgh { get; set; }
        public DateTime? time { get; set; }
        public string unitCode { get; set; }
        public int? @static { get; set; }
        public string fh { get; set; }
        public string sp { get; set; }
        public string outstaticinfo { get; set; }
        public string bgclpath { get; set; }
        public string cbr { get; set; }
        public string SQname { get; set; }
        public string SQTel { get; set; }
        public string YZZPath { get; set; }
        public string EndTime { get; set; }

        public List<t_sys_region> Region { get; set; }
        public string ImageUrl { get; set; }
    }

    public class ChangeQualifyCreateViewModel
    {
        public List<t_sys_region> Region { get; set; }
        public string UnitCode { get; set; }
        public string UnitName { get; set; }
    }





}


