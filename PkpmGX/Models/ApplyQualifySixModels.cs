using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PkpmGX.Models
{
    public class QualifysixSearchModel
    {
        public int? posStart { get; set; }
        public int? count { get; set; }
        public string pid { get; set; }
    }

    public class ApplyQualifSixData
    {
        public string xm { get; set; }
        public string xb { get; set; }
        public string nl { get; set; }
        public string zw { get; set; }
        public string xl { get; set; }
        public string zy { get; set; }
        public string zc { get; set; }
        public string sfzhm { get; set; }
        public string csjflb { get; set; }
        public string csjfnx { get; set; }
        public string shbxzh { get; set; }
        public string xlpath { get; set; }
        public string sgzsh { get; set; }
        public string sgzshpath { get; set; }
        public string sfzhmpath { get; set; }
        public string zcpath { get; set; }

    }

    public class ApplyQualifySixSaveModel
    {
        public string PId { get; set; }
        public string CustomId { get; set; }
        public string updatapath { get; set; }
        public string gridStr { get; set; }
    }

    public class PeopleSearchModel
    {
        public int id { get; set; }
        public string educationpath { get; set; }
        public string selfnumPath { get; set; }
        public string titlepath { get; set; }
        public string PostPath { get; set; }
    }
}