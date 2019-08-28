using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Entity
{
    /// <summary>
    /// 回弹仪方案
    /// </summary>
    public class tab_hty_programme
    {
        [AutoIncrement]
        public int id { get; set; }
        /// <summary>
        /// 单位编码
        /// </summary>
        public string unitcode { get; set; }
        /// <summary>
        /// 检测流水号
        /// </summary>
        public string checknum { get; set; }
        /// <summary>
        /// 人员id,多个id用英文逗号,隔开
        /// </summary>
        public string testingpeople { get; set; }
        /// <summary>
        /// 设备id,多个id用英文逗号,隔开
        /// </summary>
        public string testingequipment { get; set; }
        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime? planstartdate { get; set; }
        /// <summary>
        /// 计划结束时间
        /// </summary>
        public DateTime? planenddate { get; set; }
        /// <summary>
        /// 方案文件 路径
        /// </summary>
        public string filepath { get; set; }
        /// <summary>
        /// 方案文件 名称
        /// </summary>
        public string filename { get; set; }

        /// <summary>
        /// 会签页 附件
        /// </summary>
        public string hqfilepath { get; set; }

        /// <summary>
        /// 会签页 名称
        /// </summary>
        public string hqfilename { get; set; }
        /// <summary>
        /// 地区
        /// </summary>
        public string area { get; set; }
        /// <summary>
        /// 工程编码
        /// </summary>
        public string projectnum { get; set; }
        public string projectname { get; set; }

        public DateTime? createdate { get; set; }
    }

    public class tab_hty_gj
    {
        [AutoIncrement]
        public int id { get; set; }
        /// <summary>
        /// 方案ID
        /// </summary>
        public int progid { get; set; }
        /// <summary>
        /// 检测流水号
        /// </summary>
        public string checknum { get; set; }
        /// <summary>
        /// 构件编号
        /// </summary>
        public string gjNo { set; get; }
        /// <summary>
        /// 构件名称
        /// </summary>
        public string gjName { set; get; }
        /// <summary>
        /// 构件测区数量
        /// </summary>
        public int? gjcqNum { set; get; }

        /// <summary>
        /// 弹击角度
        /// </summary>
        public int? tjjd { set; get; }

        /// <summary>
        /// 浇筑面
        /// </summary>
        public int? jzm { set; get; }

        /// <summary>
        /// 泵送方式
        /// </summary>
        public int? bsfs { set; get; }

        /// <summary>
        /// 测强曲线编码
        /// </summary>
        public string cqqxNo { set; get; }

        /// <summary>
        /// 碳化模式
        /// </summary>
        public int? thms { set; get; }

        /// <summary>
        /// 碳化均值
        /// </summary>
        public decimal? avgth { set; get; }

        /// <summary>
        /// 碳化最大值
        /// </summary>
        public string maxTh { set; get; }

        /// <summary>
        /// 检测日期
        /// </summary>
        public DateTime? checkTime { set; get; }

        /// <summary>
        /// 回弹仪类型
        /// </summary>
        public string htyType { set; get; }

        /// <summary>
        /// 回弹仪编号
        /// </summary>
        public string htyNo { set; get; }

        /// <summary>
        /// 回弹仪率定
        /// </summary>
        public string htyLd { set; get; }

        /// <summary>
        /// 最小推定值
        /// </summary>
        public decimal? minTd { set; get; }

        /// <summary>
        /// 推定值均值
        /// </summary>
        public decimal? avgTd { set; get; }

        /// <summary>
        /// 构件强度推定值
        /// </summary>
        public decimal? gjqdTd { set; get; }

        /// <summary>
        /// 混凝土推定值
        /// </summary>
        public decimal? hnttdz { set; get; }

        /// <summary>
        /// 是否合格
        /// </summary>
        public string hg { set; get; }

        /// <summary>
        /// 混凝土标号
        /// </summary>
        public int? hntbh { set; get; }

        /// <summary>
        /// 施工队名称
        /// </summary>
        public string ShiGongDuiName { set; get; }
        /// <summary>
        /// 楼栋号
        /// </summary>
        public string BuildingNum { set; get; }

        /// <summary>
        /// 楼层
        /// </summary>
        public string Floor { set; get; }
    }

    public class tab_hty_gjcq
    {
        [AutoIncrement]
        public int id { get; set; }
        /// <summary>
        /// 方案ID
        /// </summary>
        public int progid { get; set; }
        /// <summary>
        /// 构件id
        /// </summary>
        public int gjid { get; set; }
        /// <summary>
        /// 测区编号
        /// </summary>
        public string cqno { get; set; }

        /// <summary>
        /// 点1
        /// </summary>
        public int? sub01 { set; get; }

        /// <summary>
        /// 点2
        /// </summary>
        public int? sub02 { set; get; }

        /// <summary>
        /// 点3
        /// </summary>
        public int? sub03 { set; get; }

        /// <summary>
        /// 点4
        /// </summary>
        public int? sub04 { set; get; }

        /// <summary>
        /// 点5
        /// </summary>
        public int? sub05 { set; get; }

        /// <summary>
        /// 点6
        /// </summary>
        public int? sub06 { set; get; }

        /// <summary>
        /// 点7
        /// </summary>
        public int? sub07 { set; get; }

        /// <summary>
        /// 点8
        /// </summary>
        public int? sub08 { set; get; }

        /// <summary>
        /// 点9
        /// </summary>
        public int? sub09 { set; get; }

        /// <summary>
        /// 点10
        /// </summary>
        public int? sub10 { set; get; }

        /// <summary>
        /// 点11
        /// </summary>
        public int? sub11 { set; get; }

        /// <summary>
        /// 点12
        /// </summary>
        public int? sub12 { set; get; }

        /// <summary>
        /// 点13
        /// </summary>
        public int? sub13 { set; get; }

        /// <summary>
        /// 点14
        /// </summary>
        public int? sub14 { set; get; }

        /// <summary>
        /// 点15
        /// </summary>
        public int? sub15 { set; get; }

        /// <summary>
        /// 点16
        /// </summary>
        public int? sub16 { set; get; }

        /// <summary>
        /// 碳化值1
        /// </summary>
        public decimal? thz1 { set; get; }

        /// <summary>
        /// 碳化值2
        /// </summary>
        public decimal? thz2 { set; get; }

        /// <summary>
        /// 碳化值3
        /// </summary>
        public decimal? thz3 { set; get; }

        /// <summary>
        /// 平均碳化值
        /// </summary>
        public decimal? avgthz { set; get; }

        /// <summary>
        /// 测区推定值
        /// </summary>
        public decimal? cqtdz { set; get; }
    }
   
    public class t_hty_Image
    {
        [AutoIncrement]
        public int? Id { get; set; }
        public int? ProgId { get; set; }
        public string CheckNum { get; set; }
        public string gjNo { get; set; }
        /// <summary>
        /// 标识各种环节。备用
        /// </summary>
        public int? Status { get; set; }
        public string Path { get; set; }
        public DateTime? UploadTime { get; set; }
    }
}
