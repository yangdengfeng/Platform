using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Entity.ElasticSearch
{
    /// <summary>
    /// 二维码委托单信息
    /// </summary>
    [Serializable]
    public class ESHrEntrust
    {
        /// <summary>
        /// 委托流水号 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 生成委托单用户id
        /// </summary>
        public int ENTRUSUSERID { get; set; }

        /// <summary>
        /// 生成委托单时间
        /// </summary>
        [JsonConverter(typeof(ESTimeConverter))]
        public DateTime? ENTRUSTDATE { get; set; }

        /// <summary>
        /// 检测项目
        /// </summary>
        public string ITEMCODE { get; set; }

        /// <summary>
        /// 样品数量
        /// </summary>
        public int? SAMPLECOUNT { get; set; }

        /// <summary>
        /// 二维码,多个已,隔开如（
        /// </summary>
        public string QRINFOS { get; set; }
       
        /// <summary>
        /// 机构编码
        /// </summary>
        public string CUSTOMID { get; set; }

        /// <summary>
        /// 工程id
        /// </summary>
        public string PROJECTID { get; set; }

        /// <summary>
        /// 工程名称
        /// </summary>
        public string PROJECTNAME { get; set; } 

        /// <summary>
        /// 取样员id（人员表id）
        /// </summary>
        public int? SLID { get; set; }

        /// <summary>
        /// 取样员姓名
        /// </summary>
        public string SLNAME { get; set; }

        /// <summary>
        /// 取样人电话
        /// </summary>
        public string SLPHONE { get; set; }

        /// <summary>
        /// 见证员id（人员表id）
        /// </summary>
        public int? SPNID { get; set; }

        /// <summary>
        /// 见证员姓名
        /// </summary>
        public string SPNNAME { get; set; }

        /// <summary>
        /// 见证员电话
        /// </summary>
        public string SPPHONE { get; set; }

        /// <summary>
        /// 送检类别：委托送检(qy)/见证取样(jzqy)/平行检验(pxcj)/监督见证(jdjz)/监督抽样(jdcj)
        /// </summary>
        public string UTYPES { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string REMARK { get; set; }

        /// <summary>
        /// 委托状态 0 未打印， 1 已打印
        /// </summary>
        public int? ENTRUSTSTATUS { get; set; }
    }
}
