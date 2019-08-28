using Pkpm.Entity;
using QZWebService.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PkpmGX.Models
{
    public class ZJSceneCheckModels
    {
    }

    public class ZJSceneCheckSearchModel
    {
        public string customId { get; set; }
        public string testpeople { get; set; }
        public string testtype { get; set; }
        public string projectname { get; set; }
        public string testequip { get; set; }
        public string areainfo { get; set; }
        public string piletype { get; set; }

        public int? posStart { get; set; }
        public int? count { get; set; }
    }

    public class ZJSceneCheckDetailsModel
    {
        /// <summary>
        /// 点击数量后点击查看按钮，上方的工程名称，检测机构名称，附件名称，路径等
        /// </summary>
        public view_programmePileList programme { get; set; }

        /// <summary>
        /// GPS信息
        /// </summary>
        public ZJSceneCheckGpsModel GpsInfo { get; set; }

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
        public  List<view_pilephoto> PhotoInfos { get; set; }

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

    public class ZJSceneCheckGpsModel
    {
        public bool IsVaild { get; set; }
        public double? GpsLatitude { get; set; }
        public double? GpsLongitude { get; set; }
    }


    public class HTYSerchViewModel
    {
        public string CheckUnitName { get; set; }
        public string CheckProject { get; set; }

        public int? posStart { get; set; }
        public int? count { get; set; }
    }

    public class ZNHTYFangAnViewModel
    {
        public t_bp_project project { get; set; }
        public tab_hty_programme programme { get; set; }
        public List<t_bp_People> people { get; set; }
        public string PLANSTARTDATE { get; set; }
        public string date { get; set; }

        public string EquipNums { get; set; }
    }


    public class HTYGjcqDetailViewModel
    {
        public HTYGjDTOViewModel Data { get; set; }
        public List<tab_hty_gjcq> SubDatas { get; set; }
    }

    /// <summary>
    /// 回弹仪构件列表页-字段转换后的
    /// </summary>
    public class HTYGjDTOViewModel
    {
        public int id { get; set; }
        public string checknum { get; set; }
        public string unitcode { get; set; }
        public string PROJECTNAME { get; set; }
        public string area { get; set; }
        public string PROJECTADDRESS { get; set; }
        public string testingpeople { get; set; }
        public string gjcqNum { get; set; }
        public string gjNo { get; set; }
        public string gjName { get; set; }
        public string tjjd { get; set; }
        public string jzm { get; set; }
        public string bsfs { get; set; }
        public string cqqxNo { get; set; }
        public int? thms { get; set; }
        public string maxTh { get; set; }
        public DateTime? checkTime { get; set; }
        public string htyType { get; set; }
        public string htyNo { get; set; }
        public decimal? minTd { get; set; }
        public decimal? gjqdTd { get; set; }
        public string Floor { get; set; }
        public string BuildingNum { get; set; }
        public decimal? hnttdz { set; get; }
        public int? hntbh { set; get; }
        public string hg { set; get; }
        public string ShiGongDuiName { set; get; }
        public decimal? avgth { set; get; }
    }
}