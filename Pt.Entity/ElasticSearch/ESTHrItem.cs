using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Entity.ElasticSearch
{
    [Serializable]
    public class ESTHrItem
    {
        /// <summary>
        /// 主键和qrinfo一致
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 唯一标识前缀
        /// </summary>
        public string QRINFOPREFIX { get; set; }

        /// <summary>
        /// 唯一标识
        /// </summary>
        public string QRINFO { get; set; }

        /// <summary>
        /// 工程名称
        /// </summary>
        public string PROJECTNAME { get; set; }

        /// <summary>
        /// 施工单位
        /// </summary>
        public string CONSTRACTIONUNIT { get; set; }

        /// <summary>
        /// 委托单位
        /// </summary>
        public string ENTRUSTUNIT { get; set; }

        /// <summary>
        /// 项目代号
        /// </summary>
        public string ITEMCODE { get; set; }

        /// <summary>
        /// 校验码
        /// </summary>
        public string VALCODE { get; set; }

        /// <summary>
        /// 样品数量
        /// </summary>
        public string SAMPLENUM { get; set; }

        /// <summary>
        /// 取样员id（人员表id）
        /// </summary>
        public int? SLID { get; set; }

        /// <summary>
        /// 取样员姓名
        /// </summary>
        public string SLNAME { get; set; }

        /// <summary>
        /// 取样员证书编号
        /// </summary>
        public string SLPOSTNUM { get; set; }

        /// <summary>
        /// 取样员手机程序openid
        /// </summary>
        public string SLPHONES { get; set; }

        /// <summary>
        /// 取样位置纬度
        /// </summary>
        public float? SLLONG { get; set; }

        /// <summary>
        /// 取样位置经度
        /// </summary>
        public float? SLLAT { get; set; }

        /// <summary>
        /// 取样二维码照片路径
        /// </summary>
        public string SLIMGQR { get; set; }

        /// <summary>
        /// 取样样品照片路径
        /// </summary>
        public string SLIMGPHONE { get; set; }

        /// <summary>
        /// 取样人员照片路径
        /// </summary>
        public string SLIMGPEOPLE { get; set; }

        /// <summary>
        /// 取样日期
        /// </summary>
        [JsonConverter(typeof(ESTimeConverter))]
        public DateTime? SLDATE { get; set; }

        /// <summary>
        /// 取样用时
        /// </summary>
        public float? SLENDDATE { get; set; }

        /// <summary>
        /// 见证员id（人员表id）
        /// </summary>
        public int? SPNID { get; set; }

        /// <summary>
        /// 见证员姓名
        /// </summary>
        public string SPNNAME { get; set; }

        /// <summary>
        /// 见证员证书编号
        /// </summary>
        public string SPNPOSTNUM { get; set; }

        /// <summary>
        /// 见证员手机程序openid
        /// </summary>
        public string SPNPHONES { get; set; }

        /// <summary>
        /// 见证位置纬度
        /// </summary>
        public float? SPNLONG { get; set; }

        /// <summary>
        /// 见证位置经度
        /// </summary>
        public float? SPNLAT { get; set; }

        /// <summary>
        /// 见证二维码照片路径
        /// </summary>
        public string SPNIMGQR { get; set; }

        /// <summary>
        /// 见证样品照片路径
        /// </summary>
        public string SPNIMGPHONE { get; set; }

        /// <summary>
        /// 见证人员照片路径
        /// </summary>
        public string SPNIMGPEOPLE { get; set; }

        /// <summary>
        /// 见证日期
        /// </summary>
        [JsonConverter(typeof(ESTimeConverter))]
        public DateTime? SPNDATE { get; set; }

        /// <summary>
        /// 见证用时
        /// </summary>
        public float? SPNENDDATE { get; set; }

        /// <summary>
        /// 见证与取样的时间差
        /// </summary>
        public float? SPNSLDATE { get; set; }

        /// <summary>
        /// 工程id
        /// </summary>
        public string PROJECTID { get; set; }

        /// <summary>
        /// 送检类别：委托送检(qy)/见证取样(jzqy)/平行检验(pxcj)/监督见证(jdjz)/监督抽样(jdcj)
        /// </summary>
        public string UTYPES { get; set; }

        /// <summary>
        /// 结构部位
        /// </summary>
        public string STRUCTPART { get; set; }

        /// <summary>
        /// 报告编号
        /// </summary>
        public string REPORTNUM { get; set; }
        /// <summary>
        /// 取样人与工程的距离
        /// </summary>
        public float? SPNDISTANCE { get; set; }

        /// <summary>
        /// 见证人与工程的距离
        /// </summary>
        public float? WITNESSDISTANCE { get; set; }

        /// <summary>
        /// 样品信息是否修改 0未修改，1已修改
        /// </summary>
        public int? ISSAMPLINGMODIFY { get; set; }
       
        /// <summary>
        /// 此标识归并与状态位（修改待审核） 因为此状态也不能生成委托单 样品信息修改是否已审核 0未审核，1已审核
        /// </summary>
        public int? ISSAMPLINGCHECK { get; set; }

        /// <summary>
        /// 样品信息是否已确认，0 未确认， 1 已确认 , 2 已作废
        /// 未确认样品不能
        /// </summary>
        public int? ISSAMPLECONFIRM { get; set; }

        /// <summary>
        /// 二维码状态 0 取样， 1 见证 ， 2 已委托,  3 收样 ， 4 已出报告 ，5已送检、6已检测 7 修改待审核 8审核完成
        /// </summary>
        public int? QRCODESTATUS { get; set; }

        /// <summary>
        /// 委托单编号
        /// </summary>
        public string EnTrustId { get; set; }

        /// <summary>
        /// 收样时间
        /// </summary>
        [JsonConverter(typeof(ESTimeConverter))]
        public DateTime? SAMPLEDATE { get; set; }

        /// <summary>
        /// 检测时间
        /// </summary>
        [JsonConverter(typeof(ESTimeConverter))]
        public DateTime? TESTDATE { get; set; }
        /// <summary>
        /// 出报告时间
        /// </summary>
        [JsonConverter(typeof(ESTimeConverter))]
        public DateTime? REPORTDATE { get; set; }
        /// <summary>
        /// 样品信息
        /// </summary>
        public string SAMPLEINFO { get; set; }

        /// <summary>
        /// COVR检测项目
        /// </summary>
        public string REPORTMODELNAME { get; set; }

        /// <summary>
        /// 大编码 工程编码 17 位
        /// </summary>
        public string XMBM { get; set; }

        /// <summary>
        /// 工程名称
        /// </summary>
        public string XMMC { get; set; }

        /// <summary>
        /// 单位编码 3位
        /// </summary>
        public string DWBM { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        public string DWMC { get; set; }

        /// <summary>
        /// 监督单位编号
        /// </summary>
        public string INSPECTMAN { get; set; }
    }
}
