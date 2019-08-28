using Pkpm.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace PkpmGX.Models
{
    public class DistributeExpertViewModels
    {
        public Dictionary<int, string> Experts { get; set; }
        public int Id { get; set; }
        public int? PID { get; set; }
        public string DistributedExpert { get; set; }
    }



    public class DistributeExpertSearchModel
    {
        public string CheckUnitName { get; set; }
        public int? posStart { get; set; }
        public int? count { get; set; }
        public int? orderColInd { get; set; }
        public string direct { get; set; }
    }


    public class DistributeExpertUIModel
    {
        public string id { get; set; }
        public string pid { get; set; }
        public string zjsp1 { get; set; }
        public string zjsp2 { get; set; }
        public int? @static { get; set; }
    }



}


