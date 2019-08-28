using Pkpm.Entity.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PkpmGX.Models
{
    public class WelcomeViewMoel
    {
        //public List<NoticeModel> Notices { get; set; }
        //public List<JobListModel> JobLists { get; set; }

        //画地图用  总报告数量   以及不合格报告数量
        public List<EchartNameValue> EchartTotalReportsCount { get; set; }
        public List<EchartNameValue> EchartUnqualifyReportsCount { get; set; }
        public List<EchartNameValue> EcharModifyReportsCount { get; set; }

        //TODO  需要加上三个到期提醒
        public List<DueRemindModel> MeasnumDate { get; set; }//检测资质有效期到期提醒
        public List<DueRemindModel> DetectnumDate { get; set; }//计量认证证书到期提醒
        public List<DueRemindModel> Checkdate { get; set; }//仪器设备检定过期提醒

        public int TotalReports { get; set; }
        public int UnqualifyReports { get; set; }
        /// <summary>
        /// 不合格百分比
        /// </summary>
        public string UnqualifyPercentage { get; set; }
        public int ModifyReports { get; set; }
        public string ModifyPercentage { get; set; }
        public int PKRReports { get; set; }
        public string PKRPercentage { get; set; }

    }

    public class DueRemindModel
    {   public string Id { get; set;}
        public int Num { get; set; }
        public string Type { get; set; }
        public string Date { get; set; }
    }

    public class MeasnumDateModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime? measnumEndDate { get; set; }
    }

    public class DetectnumDateModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime? detectnumEndDate { get; set; }
    }

    public class CheckdateModel
    {
        public string id { get; set; }
        public string EquName { get; set; }
        public DateTime? checkenddate { get; set; }
        public string customid { get; set; }
    }
}