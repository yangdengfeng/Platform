using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PkpmGX.Models
{
    public class ReportInfoViewModels
    {
    }
    public class witnesspeopleModels
    {
        public string name { get; set; }
        public string phone { get; set; }
        public string postnum { get; set; }
    }
    public class ProjectInfoDetailModels : witnesspeopleModels
    {
        public int Id { get; set; }
        public string projectnum { get; set; }
        public string projectname { get; set; }
        public string projectregnum { get; set; }
        public string witnesspeople { get; set; }
        public string customid { get; set; }
        public string areainfo { get; set; }
        public string constructionunit { get; set; }
        public string supervisionunit { get; set; }
        public string designunit { get; set; }
        public string constructionunits { get; set; }
        public string personinchargename { get; set; }
        public string personinchargetel { get; set; }
        public string projectaddress { get; set; }
        public string checknum { get; set; }
        public string areaname { get; set; }
        public string customname { get; set; }
        public List<witnesspeopleModels> JZPeople { get; set; }
    }
    public class programmeSecneListDetailModels : witnesspeopleModels
    {
        public DateTime? addtime { get; set; }
        public string areainfo { get; set; }
        public string checknum { get; set; }
        public string checktype { get; set; }
        public string constructionunit { get; set; }
        public string constructionunits { get; set; }
        public string customid { get; set; }
        public string customname { get; set; }
        public string designunit { get; set; }
        public string filename { get; set; }
        public string filepath { get; set; }
        public string hqfilename { get; set; }
        public string hqfilepath { get; set; }
        public int Id { get; set; }
        public decimal? latitude { get; set; }
        public decimal? longitude { get; set; }
        public string peoplepath { get; set; }
        public string personinchargename { get; set; }
        public string personinchargetel { get; set; }
        public int? photoid { get; set; }
        public string photopath { get; set; }
        public string planenddate { get; set; }
        public string planstartdate { get; set; }
        public string projectaddress { get; set; }
        public string projectname { get; set; }
        public string projectnum { get; set; }
        public string projectregnum { get; set; }
        public int reportcount { get; set; }
        public string structpart { get; set; }
        public int? stuas { get; set; }
        public string supervisionunit { get; set; }
        public string testingpeople { get; set; }
        public string witnesspeople { get; set; }
        public List<witnesspeopleModels> JZPeople { get; set; }
        public List<witnesspeopleModels> JCPeople { get; set; }
    }

    public class tab_xc_reportModel
    {
        public string sysprimarykey { get; set; }
    }
}