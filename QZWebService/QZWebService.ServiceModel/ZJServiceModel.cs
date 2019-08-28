using ServiceStack;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QZWebService.ServiceModel
{

    /// <summary>
    /// 桩基现场检测数据
    /// </summary>
    [Route("/ZJCheck/GetSenceSites", "GET")]
    public class SenceSiteData : IReturn<SenceSiteDataResponse>
    {
        public string customId { get; set; }
        public string customName { get; set; }
        public string projectName { get; set; }
        public string areainfo { get; set; }
        public string testingpeople { get; set; }
        public string testingequip { get; set; }
        public string piletype { get; set; }
        public string testtype { get; set; }
        public int? posStart { get; set; }
        public int? count { get; set; }
    }

    public class SenceSiteDataResponse
    {
        public bool IsSucc { get; set; }
        public string Msg { get; set; }
        public List<view_testingSite> datas { get; set; }
        public int? totalCount { get; set; }
    }


    [Route("/ZJCheck/GetPileUpdataInfo", "GET")]
    public class PileUpdataInfo : IReturn<PileUpdataInfoResponse>
    {
        public string UnitName { get; set; }
        public DateTime? UpdataStartDate { get; set; }
        public DateTime? UpdataEndDate { get; set; }
        public int? posStart { get; set; }
        public int? count { get; set; }
    }

    public class PileUpdataInfoResponse
    {
        public bool IsSucc { get; set; }
        public string Msg { get; set; }
        public List<tab_zj_updatelog> datas { get; set; }
        public int? totalCount { get; set; }
    }


    /// <summary>
    ///  根据id获取桩基检测方案信息
    /// </summary>
    /// <typeparam name="GetZJCheckByIdResponse"></typeparam>
    [Route("/ZJCheck/GetZjCheckById", "GET")]
    public class GetZJCheckById : IReturn<GetZJCheckByIdResponse>
    {
        public string id { get; set; }
    }

    public class GetZJCheckByIdResponse
    {
        public bool IsSucc { get; set; }
        public string Msg { get; set; }
        public view_programmePileList data { get; set; }

    }


    [Route("/ZJCheck/GetZjCheckGPSByArea")]
    public class GetZjCheckGPSByArea : IReturn<GetZjCheckGPSByAreaResponse>
    {
        public int? posStart { get; set; }
        public int? count { get; set; }
        public string Area { get; set; }
    }

    public class GetZjCheckGPSByAreaResponse
    {
        public bool IsSucc { get; set; }
        public string Msg { get; set; }
        public List<view_GpsPileInfo> gpsPileInfo { get; set; }
        public int? totalCount { get; set; }
    }



    /// <summary>
    /// 获取桩基检测方案列表
    /// </summary>
    [Route("/ZJCheck/GetZjList", "GET")]
    public class GetZJCheckList : IReturn<GetZJCheckListResponse>
    {
        public string CheckUnitName { get; set; }
        public string Area { get; set; }
        public string CheckEquip { get; set; }
        public string Report { get; set; }
        public string ProjectName { get; set; }
        public string CheckPeople { get; set; }
        /// <summary>
        /// 桩型
        /// </summary>
        public string ZX { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? posStart { get; set; }
        public int? count { get; set; }
    }

    public class GetZJCheckListResponse
    {
        public bool IsSucc { get; set; }
        public string Msg { get; set; }
        public List<view_programmePileList> datas { get; set; }
        public int? totalCount { get; set; }
    }

    /// <summary>
    /// 在现场检测数据列表点击数量查看
    /// </summary>
    [Route("/ZJCheck/TestSiteDetails", "GET")]
    public class TestSiteDetails : IReturn<TestSiteDetailsResponse>
    {
        public int IsTesting { get; set; }
        public string CheckNum { get; set; }
        public string CustomId { get; set; }
    }

    public class TestSiteDetailsResponse
    {
        public bool IsSucc { get; set; }
        public string Msg { get; set; }
        public List<TestSiteDetailsModel> datas { get; set; }
    }

    /// <summary>
    /// 现场检测数据列表点击数量查看之后再点击查看按钮的三图五表
    /// </summary>
    [Route("/ZJCheck/TestSiteDetailsDetails", "GET")]
    public class TestSiteDetailsDetails : IReturn<TestSiteDetailsDetailsResponse>
    {
        public string Type { get; set; }
        public int? BasicInfoId { get; set; }

    }

    public class TestSiteDetailsDetailsResponse
    {
        public bool IsSucc { get; set; }
        public string Msg { get; set; }

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

    /// <summary>
    /// 现场检测数据列表点击数量查看之后再点击查看按钮的当前数据,仪器参数,GPS位置信息
    /// </summary>
    [Route("/ZJCheck/TestSiteDetailsDetailsDatas", "GET")]
    public class TestSiteDetailsDetailsDatas : IReturn<TestSiteDetailsDetailsDatasResponse>
    {
        public int? BasicInfoId { get; set; }

    }

    public class TestSiteDetailsDetailsDatasResponse
    {
        public bool IsSucc { get; set; }
        public string Msg { get; set; }
        public int gpsisvalid { get; set; }
        public double? gpslongitude { get; set; }
        public double? gpslatitude { get; set; }
        public string sourceparam { get; set; }
        public string currentparam { get; set; }
    }

    /// <summary>
    /// 现场检测数据列表点击数量查看之后再点击查看按钮的测试日志
    /// </summary>
    [Route("/ZJCheck/TestSiteDetailsDetailsTestLog", "GET")]
    public class TestSiteDetailsDetailsTestLog : IReturn<TestSiteDetailsDetailsTestLogResponse>
    {
        public int? BasicInfoId { get; set; }

    }

    public class TestSiteDetailsDetailsTestLogResponse
    {
        public bool IsSucc { get; set; }
        public string Msg { get; set; }
        public List<Jy_TestingLogInfo> datas { get; set; }
    }

    /// <summary>
    /// 现场检测数据列表点击数量查看之后再点击查看按钮的图片信息
    /// </summary>
    [Route("/ZJCheck/TestSiteDetailsDetailsPhotoInfo", "GET")]
    public class TestSiteDetailsDetailsPhotoInfo : IReturn<TestSiteDetailsDetailsPhotoInfoResponse>
    {
        public string BasicInfoId { get; set; }
        public string PileNo { get; set; }
    }

    public class TestSiteDetailsDetailsPhotoInfoResponse
    {
        public bool IsSucc { get; set; }
        public string Msg { get; set; }
        public List<view_pilephoto> datas { get; set; }
    }

    /// <summary>
    /// 现场检测数据列表点击数量查看之后再点击查看按钮的方案信息，包括工程名称，检测单位，上报时间，流水号等信息
    /// </summary>
    [Route("/ZJCheck/TestSiteDetailsDetailsProgrammeInfo", "GET")]
    public class TestSiteDetailsDetailsProgrammeInfo : IReturn<TestSiteDetailsDetailsProgrammeInfoResponse>
    {
        public string BasicInfoId { get; set; }
        public string CheckNum { get; set; }
    }

    public class TestSiteDetailsDetailsProgrammeInfoResponse
    {
        public bool IsSucc { get; set; }
        public string Msg { get; set; }
        public view_programmePileList data { get; set; }
    }

    public class HistorySite: IReturn<HistorySiteResponse>
    {
        public string customId { get; set; }
        public string customName { get; set; }
        public string projectName { get; set; }
        public string areainfo { get; set; }
        public string testingpeople { get; set; }
        public string testingequip { get; set; }
        public string piletype { get; set; }
        public string testtype { get; set; }
        public bool? hasReport { get; set; }
        public int? posStart { get; set; }
        public int? count { get; set; }
    }

    public class HistorySiteResponse
    {
        public bool IsSucc { get; set; }
        public string Msg { get; set; }
        public List<view_testingHis> datas { get; set; }
        public int? totalCount { get; set; }
    }
}
