using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PkpmGX.Models
{
    public class ApplyQualifySevenModels
    {
    }

    public class ApplyQualifySevenEquipModel
    {
        // {"jcxm":"地基基础工程检测","zyyqsb":"09-14","clfw":"09","zqddj":"10","jdxzjg":"机构","yxrq":"2016-04-13","zjxxm":"自检项目","zjxgfmcjbh":"自检编号","bz":"1"}
        public string jcxm { get; set; }
        public string zyyqsb { get; set; }
        public string clfw { get; set; }
        public string zqddj { get; set; }
        public string jdxzjg { get; set; }
        public DateTime? yxrq { get; set; }
        public string zjxxm { get; set; }
        public string zjxgfmcjbh { get; set; }
        public string bz { get; set; }
    }

    public class ApplyQualifySevenDetailsModel
    {
        public string pid { get; set; }
        public string customId { get; set; }
        //public List<ApplyQualifySevenEquipModel> Equips;
    }


    public class ApplyQualifySevenSearchModel
    {
        public string PId { get; set; }
        public string CustomId { get; set; }
        public int? posStart { get; set; }
        public int? count { get; set; }
    }

    public class ApplyQualifySevenEditSaveModel
    {
        public string PId { get; set; }
        public string CustomId { get; set; }
        public string gridStr { get; set; }
    }
}