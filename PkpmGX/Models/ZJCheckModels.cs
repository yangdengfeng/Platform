using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PkpmGX.Models
{
    public class ZJCheckSearchModel
    {
        public string CheckUnitName { get; set; }
        public string Area { get; set; }
        public string CheckEquip { get; set; }
        public string Report { get; set; }
        public string ProjectName { get; set; }
        public string CheckPeople { get; set; }
        public string ZX { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? posStart { get; set; }
        public int? count { get; set; }
    }


    public class ZJCheckDetailsModel
    {
        public ZJCheckprogrammePileList programmePileList { get; set; }
        public ZJCheckPeople JZPeople { get; set; }
        public List<ZJCheckPeople> CheckPeople { get; set; }
    }

    public class ZJCheckPeople
    {
        public string name { get; set; }
        public string phone { get; set; }
        public string postnum { get; set; }
    }

    public partial class ZJCheckprogrammePileList
    {
        public int Id { get; set; }
        public string projectnum { get; set; }
        public string checknum { get; set; }
        public string testingpeople { get; set; }
        public string testingequipment { get; set; }
        public string plandate { get; set; }
        
        public string basictype { get; set; }
        public string structuretype { get; set; }
        public string piletype { get; set; }
        public double? elevation { get; set; }
        public double? pilenum { get; set; }
        public string eigenvalues { get; set; }
        public string pilelenght { get; set; }
        public double? areadisplacement { get; set; }
        public string pilediameter { get; set; }
        public string concretestrength { get; set; }
        public int? jzsynum { get; set; }
        public string jzsynos { get; set; }
        public string filepath { get; set; }
        public string filename { get; set; }
        public DateTime? addtime { get; set; }
        public int? stuas { get; set; }
        public string hqfilepath { get; set; }
        public string hqfilename { get; set; }
        public string customid { get; set; }
        public string witnesspeople { get; set; }
        public string projectname { get; set; }
        public string projectregnum { get; set; }
        public string areainfo { get; set; }
        public string areaname { get; set; }
        public string constructionunit { get; set; }
        public string supervisionunit { get; set; }
        public string designunit { get; set; }
        public string constructionunits { get; set; }
        public string personinchargename { get; set; }
        public string personinchargetel { get; set; }
        public string projectaddress { get; set; }
        public string customname { get; set; }
        public int? reportcount { get; set; }
    }
}