using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QZWebService.ServiceModel
{
   
        /// <summary>
        /// 现场检测工程信息
        /// </summary>
        [Route("/XCCheckInfo/GetXCProjectInfos", "GET")]
        public class GetXCProjectInfos : IReturn<GetXCProjectInfosResponse>
        {
            public string projectname { get; set; }
            public string areainfo { get; set; }
            public string customid { get; set; }
            public string customname { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public int? posStart { get; set; }
            public int? count { get; set; }
        }
        public class GetXCProjectInfosResponse
        {
            public bool IsSucc { get; set; }
            public string Msg { get; set; }
            public List<view_projectinfo> datas { get; set; }
            public int totalCount { get; set; }

        }

        [Route("/XCCheckInfo/GetProjectInfos", "GET")]
        public class GetProjectInfos : IReturn<GetProjectInfosResponse>
        {
            public int id { get; set; }
        }
        public class GetProjectInfosResponse
        {
            public bool IsSucc { get; set; }
            public string Msg { get; set; }
            public view_projectinfo project { get; set; }
        }

    [Route("/XCCheckInfo/GetprogrammeSecneLists", "GET")]
    public class GetprogrammeSecneLists : IReturn<GetprogrammeSecneListsResponse>
    {
        public string customname { get; set; }
        public string projectname { get; set; }
        public string areainfo { get; set; }
        public string testingpeople { get; set; }
        public string IsReport { get; set; }
        public string IsPhoto { get; set; }
        public int? posStart { get; set; }
        public int? count { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }
    public class GetprogrammeSecneListsResponse
    {
        public bool IsSucc { get; set; }
        public string Msg { get; set; }
        public List<view_programmeSecneList> datas { get; set; }
        public int totalCount { get; set; }
    }

    [Route("/XCCheckInfo/GetPSLs", "GET")]
    public class GetPSLs : IReturn<GetPSLsResponse>
    {
        public int id { get; set; }
    }
    public class GetPSLsResponse
    {
        public bool IsSucc { get; set; }
        public string Msg { get; set; }
        public view_programmeSecneList project { get; set; }
    }

    [Route("/XCCheckInfo/GetReports", "GET")]
    public class GetReports : IReturn<GetReportsResponse>
    {
        public string projectnum { get; set; }
        public string checknum { get; set; }
    }
    public class GetReportsResponse
    {
        public bool IsSucc { get; set; }
        public string Msg { get; set; }
        public List<tab_xc_report> Reports { get; set; }
    }

    [Route("/XCCheckInfo/GetSysprimaryByReportNum", "GET")]
    public class XCGetSysprimaryByReportNum : IReturn<XCGetSysprimaryByReportNumResponse>
    {
        public string reportNum { get; set; }
        public string customId { get; set; }
    }

    public class XCGetSysprimaryByReportNumResponse
    {
        public bool IsSucc { get; set; }
        public string Msg { get; set; }
        public string SysPrimaryKey { get; set; }
    }
}
