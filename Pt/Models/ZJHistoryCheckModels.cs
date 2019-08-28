using QZWebService.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PkpmGX.Models
{
    public class ZJHistoryCheckSearchModel
    {
        public string customId { get; set; }
        public string testpeople { get; set; }
        public string testtype { get; set; }
        public string projectname { get; set; }
        public string testequip { get; set; }
        public string areainfo { get; set; }
        public string piletype { get; set; }
        public string hasReport { get; set; }

        public int? posStart { get; set; }
        public int? count { get; set; }
    }


    public class ZJHistoryCheckDetailsModel
    {
        /// <summary>
        /// 点击数量后点击查看按钮，上方的工程名称，检测机构名称，附件名称，路径等
        /// </summary>
        public view_programmePileList programme { get; set; }

        /// <summary>
        /// GPS信息
        /// </summary>
        public ZJHistoryCheckGpsModel GpsInfo { get; set; }

        /// <summary>
        /// 当前数据
        /// </summary>
        public string CurrentParam { get; set; }

        /// <summary>
        /// 仪器参数
        /// </summary>
        public string SourceParam { get; set; }

        public string TestLog { get; set; }

        /// <summary>
        /// 照片信息
        /// </summary>
        public List<view_pilephoto> PhotoInfos { get; set; }

        /// <summary>
        /// 汇总表
        /// </summary>
        public string hzb { get; set; }

        /// <summary>
        /// 原始记录表  
        /// </summary>
        public string ysjlb { get; set; }

        /// <summary>
        /// 加载记录表
        /// </summary>
        public string jzjlb { get; set; }

        /// <summary>
        /// 卸载记录表
        /// </summary>
        public string xzjlb { get; set; }

        /// <summary>
        /// 修改记录表
        /// </summary>
        public string xgjlb { get; set; }

        /// <summary>
        /// QS曲线图
        /// </summary>
        public byte[] QsImageBytes { get; set; }

        /// <summary>
        /// Slgt曲线图
        /// </summary>
        public byte[] SlgtImageBytes { get; set; }

        /// <summary>
        /// SlgQ曲线图
        /// </summary>
        public byte[] SlgQImageBytes { get; set; }
    }

    public class ZJHistoryCheckGpsModel
    {
        public bool IsVaild { get; set; }
        public double? GpsLatitude { get; set; }
        public double? GpsLongitude { get; set; }
    }



}