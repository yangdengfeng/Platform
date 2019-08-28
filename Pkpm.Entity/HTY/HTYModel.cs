using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Entity
{
    public class ZNHTYGjDataModel : tab_hty_gj
    {
        public List<tab_hty_gjcq> gjcqDatas { get; set; }
    }

    /// <summary>
    /// 回弹仪构件查询
    /// </summary>
    public class HTYGjSearchModel
    {
        public string ProjectName { get; set; }
        public string AreaName { get; set; }
        public string CustomId { get; set; }
        public int? gjid { get; set; }

        public int? posStart { get; set; }
        public int? count { get; set; }
    }
    /// <summary>
    /// 回弹仪构件列表页
    /// </summary>
    public class HTYGjViewModel
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
        public int? tjjd { get; set; }
        public int? jzm { get; set; }
        public int? bsfs { get; set; }
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
        /// <summary>
        /// 图片数量
        /// </summary>
        public int? imgcount { get; set; }
    }

    public class HTYFangAnWebServiceModel
    {
        public bool IsSucc { get; set; }
        public string Msg { get; set; }
        public int Count { get; set; }
        public List<ZNHTYWebServiceModel> Data { get; set; }
    }

    public class ZNHTYWebServiceModel : tab_hty_programme
    {
        public List<ZNHTYGjDataModel> gjDatas { get; set; }
    }

    public class HTYFangAnWebWithchecknumServiceModel
    {
        public bool IsSucc { get; set; }
        public string Msg { get; set; }
        public ZNHTYWebServiceModel Data { get; set; }
    }

    public class HTYFangAnModel
    {
        public bool IsSucc { get; set; }
        public string Msg { get; set; }
        public int Count { get; set; }
        public List<tab_hty_programme> Datas { get; set; }
    }

    public class HTYGjImagesModel
    {
        public bool IsSucc { get; set; }
        public string Msg { get; set; }
        public int Count { get; set; }
        /// <summary>
        /// 图片的base64字符串
        /// </summary>
        public List<HTYGjImage> Datas { get; set; }
    }
    public class HTYGjImage
    {
        public string ImageData { get; set; }
    }
}
