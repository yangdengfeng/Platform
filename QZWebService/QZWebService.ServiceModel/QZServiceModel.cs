using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QZWebService.ServiceModel
{
    [Route("/QzQrcode/QzQrcodeBars", "GET")]
    public class QzQrcodeBars : IReturn<QzQrcodeBarsResponse>
    {
        public List<string> entrNums { get; set; }
    }

    public class QzQrcodeBarsResponse
    {
        public bool IsSucc { get; set; }
        public string Msg { get; set; }
        public Dictionary<string, string> Datas { get; set; }
    }
    [Route("/QzQrcode/QzQrcodeBar", "GET")]
    public class QzQrcodeBar : IReturn<QzQrcodeBarResponse>
    {
        public string entrNum { get; set; }
    }

    public class QzQrcodeBarResponse
    {
        public bool IsSucc { get; set; }
        public string Msg { get; set; }
        public string qrinfo { get; set; }
    }

    [Route("/QzQrcode/GetProgramme", "GET")]
    public class GetProgramme : IReturn<GetProgrammeResponse>
    {
        public string qrinfo { get; set; }
    }

    public class GetProgrammeResponse
    {
        public bool IsSucc { get; set; }
        public string Msg { get; set; }
        public view_programmeLiftList programe { get; set; }
    }

    [Route("/QzQrcode/GetListProgramme", "GET")]
    public class GetListProgramme : IReturn<GetListProgrammeResponse>
    {
        public string qrinfo { get; set; }
        public string customId { get; set; }
        public string projectname { get; set; }
        public string areainfo { get; set; }
        public string testpeople { get; set; }
        public bool IsReport { get; set; }
        public bool IsPhoto { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? posStart { get; set; }
        public int? count { get; set; }
    }

    public class GetListProgrammeResponse
    {
        public bool IsSucc { get; set; }
        public string Msg { get; set; }
        public List<view_programmeLiftList> programes { get; set; }
        public int totalCount { get; set; }
    }

    [Route("/QzQrcode/GetProgrammeReports", "GET")]
    public class GetProgrammeReports:IReturn<GetProgrammeReportsResponse>
    {
        public string projectnum { get; set; }
        public string checknum { get; set; }
    }

    public class GetProgrammeReportsResponse
    {
        public bool IsSucc { get; set; }
        public string Msg { get; set; }
        public List<tab_qz_report> reports { get; set; }
    }

    [Route("/QzQrcode/GetSysprimaryByReportNum", "GET")]
    public class GetSysprimaryByReportNum : IReturn<GetSysprimaryByReportNumResponse>
    {
       public string reportNum { get; set; }
        public string customId { get; set; }
    }

    public class GetSysprimaryByReportNumResponse
    {
        public bool IsSucc { get; set; }
        public string Msg { get; set; }
        public string SysPrimaryKey { get; set; }
    }

}
