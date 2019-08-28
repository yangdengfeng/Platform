using Pkpm.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PkpmGX.Models
{
    public class STCheckEquipModels
    {
        public string CheckUnitName { get; set; }
        public string EquName { get; set; }
        public string EquType { get; set; }
        public string Status { get; set; }
        public int? posStart { get; set; }
        public int? count { get; set; }
    }

    public class STCheckUIModel
    {
        public string id { get; set; }
        public string customid { get; set; }
        public string EquName { get; set; }
        public string Equtype { get; set; }
        public string Equspec { get; set; }
        public DateTime? timestart { get; set; }
        public DateTime? timeend { get; set; }
        public string approvalstatus { get; set; }
       
    }

    public  class STCheckEquipViewModel
    {
        public List<SysDict> Status { get; set; }
    }

    public class STCheckEquipModel
    {
        public string CustomName { get; set; }
        public string EquipName { get; set; }
        public string EquipSpec { get; set; }
        public string CheckTime { get; set; }
        public string EquipType { get; set; }
        public string BuyTime { get; set; }
        public string TimeStart { get; set; }
        public string TimeEnd { get; set; }
        public string id { get; set; }
        public string CustomId { get; set; }
        public string Time { get; set; }
    }

    public class STCheckEquipEditModel
    {
        public STCheckEquipModel STCheckEquip { get; set; }
        public InstShortInfos allSTUnit { get; set; }
    }

    public class STCheckEquipApplyChangeViewModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }

    public class STCheckEquipApplyChange
    {
        public string SubmitName { get; set; }
        public string SubmitText { get; set; }
        public string SubmitId { get; set; }
        public string Status { get; set; }
        public bool Result { get; set; }
    }

    public class STCheckPeopleFileModel
    {
        public Dictionary<string,int> File { get; set; }
        public int Number { get; set; }
    }

    public class STCheckPeopleEditAttachFileModel
    {
        public string path { get; set; }//最后的路径返回用
        public Dictionary<int, string> paths { get; set; }
        public string Id { get; set; }
    }

}