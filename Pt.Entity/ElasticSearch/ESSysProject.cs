using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Entity.ElasticSearch
{

    public class ESTimeConverter : IsoDateTimeConverter
    {
        public ESTimeConverter()
        {
            DateTimeFormat = "yyyy-MM-dd'T'HH:mm:ss";
        }
    }

    [Serializable]
    public class ESSysProject
    {
        /// <summary>
        /// Id主键 unitCode + projectnum
        /// </summary>
        public string Id { get; set; }

        public string CUSTOMID { get; set; }

        /// <summary>
        /// 工程编号
        /// </summary> 
        public string PROJECTNUM { get; set; }

        public string PROJECTNUM_1 { get; set; }

        /// <summary>
        /// 单位工程名称
        /// </summary>
        public string PROJECTNAME { get; set; }
        //委托单位
        public string ENTRUSTUNIT { get; set; }
        //送样人
        public string SENDSAMPLEMAN { get; set; }
        //送样人电话
        public string SENDSAMPLEMANTEL { get; set; }
        //委托单位联系人
        public string ENTRUSTUNITLINKMAN { get; set; }
        //委托单位联系人电话
        public string ENTRUSTUNITLINKMANTEL { get; set; }
        //建设单位
        public string CONSTRACTUNIT { get; set; }
        //施工单位
        public string CONSTRACTIONUNIT { get; set; }
        //见证单位
        public string WITNESSUNIT { get; set; }
        //见证人
        public string WITNESSMAN { get; set; }
        //见证人证号
        public string WITNESSMANNUM { get; set; }
        //见证人电话
        public string WITNESSMANTEL { get; set; }
        //监督单位
        public string SUPERUNIT { get; set; }
        //监督人
        public string SUPERMAN { get; set; }
        //设计单位
        public string DESIGNUNIT { get; set; }
        public string INSPECTUNIT { get; set; }
        public string INSPECTMAN { get; set; }
        //勘察单位
        public string INVESTIGATEUNIT { get; set; }
        //勘察单位负责人、
        //取样人
        public string TAKESAMPLEMAN { get; set; }
        //取样人证号
        public string TAKESAMPLEMANNUM { get; set; }
        //取样人电话
        public string TAKESAMPLEMANTEL { get; set; }
        //取样单位
        public string TAKESAMPLEUNIT { get; set; }
        //工程地址
        public string PROJECTADDRESS { get; set; }
        //工程地址区域
        public string PROJECTAREA { get; set; }

        /// <summary>
        /// 建设工程项目编码（17位+3位单位工程编码）
        /// </summary>
        public string ISCUNIT { get; set; }

        /// <summary>
        /// 已作废的 
        /// </summary>
        public string ISCUNIT_1 { get; set; }

        /// <summary>
        /// 项目编码 (17位)
        /// </summary>
        public string XMBM { get; set; }

        /// <summary>
        /// 单位编码（3位）
        /// </summary> 
        public string DWBM { get; set; }

        /// <summary>
        /// 建设工程项目名称
        /// </summary>
        public string XMMC { get; set; }

        /// <summary>
        /// 子工程名称按照||||隔开（一栋,二栋)
        /// </summary>
        public string ZGCMCS { get; set; }

        /// <summary>
        /// 自工程编码数字按照空格隔开 (001 002 003 004)
        /// </summary>
        public string ZGCBMS { get; set; }

        /// <summary>
        /// 送检工程代码
        /// </summary>
        public string JCPROJECT { get; set; }
        public string PROJECTSTATUS { get; set; }
        public string PROJECTCHARGETYPE { get; set; }
        public double? ACCOUNTBALANCE { get; set; }
        public double? TOTALCONSUMEDMONEY { get; set; }
        public double? TOTALFAVOURABLEMONEY { get; set; }
        public string UNITCREDITLEVEL { get; set; }
        public string DEFAULTCONSUMETYPE { get; set; }

        [JsonConverter(typeof(ESTimeConverter))]
        public DateTime? CREATEDATE { get; set; }

        [JsonConverter(typeof(ESTimeConverter))]
        public DateTime? DESTROYDATE { get; set; }
        public string STRUTTYPE { get; set; }
        public string ISUSERICCARD { get; set; }
        public string PROJECTID { get; set; }
        public string CONTRACTNUM { get; set; }
        public string EMAIL { get; set; }
        public string AUTOSENDREPORT { get; set; }
        public string CREATEMAN { get; set; }
        public string ENTRUSTUNITADDRESS { get; set; }
        public string ENTRUSTUNITACCOUNTNUM { get; set; }
        public string ENTRUSTUNITNUM { get; set; }
        public string CONSTRACTUNITID { get; set; }
        public string DESIGNUNITID { get; set; }
        public string INVESTIGATEUNITID { get; set; }
        public string SUPERUNITID { get; set; }
        public string CONSTRACTIONUNITID { get; set; }
        public string INSPECTUNITID { get; set; }
        //地理区域编号
        public string AREAID { get; set; }
        public string ENTRUSTUNITID { get; set; }
        public string TAKESAMPLEUNITID { get; set; }
        public string WITNESSUNITID { get; set; }
        public string SUPERMANID { get; set; }
        public string INSPECTMANID { get; set; }
        public string SUPERMANTEL { get; set; }
        public string INSPECTMANTEL { get; set; }
        public string GCBH { get; set; }
        public string CONSTRACTUNITMANID { get; set; }
        public string CONSTRACTUNITMAN { get; set; }
        public string CONSTRACTUNITMANTEL { get; set; }
        public string CONSTRACTIONUNITMANID { get; set; }
        public string CONSTRACTIONUNITMAN { get; set; }
        public string CONSTRACTIONUNITMANTEL { get; set; }
        public string DESIGNUNITMANID { get; set; }
        public string DESIGNUNITMAN { get; set; }
        public string DESIGNUNITMANTEL { get; set; }
        public string INVESTIGATEUNITMANID { get; set; }
        public string INVESTIGATEUNITMAN { get; set; }
        public string INVESTIGATEUNITMANTEL { get; set; }
        public string ANQUANREGID { get; set; }

        [JsonConverter(typeof(ESTimeConverter))]
        public DateTime? PLANENDDATE { get; set; }

        [JsonConverter(typeof(ESTimeConverter))]
        public DateTime? PLANSTARTDATE { get; set; }
        public double? PROTECTSQUARE { get; set; }
        public double? SQUARE { get; set; }
        public string STRUCTLEVLES { get; set; }
        public double? TOTALPRICE { get; set; }
        public string SENDSAMPLEMANNUM { get; set; }
        public string JGLX { get; set; }

        [JsonConverter(typeof(ESTimeConverter))]
        public DateTime? CHECKTIME { get; set; }
        public string SUPERMANTEL1 { get; set; }
        public string FPROJECTNAME { get; set; }
        public string JX_PROJECTID { get; set; }
        public string JX_ITEMID { get; set; }
        public string PROJECTKEY { get; set; }
        //施工证号
        public string CONSSTRUCTIONNUM { get; set; }
        //临时证号
        public string CONSSTRUCTIONNUM_LS { get; set; }
        //施工工程名称
        public string CONSSTRUCTIONNAME { get; set; }

        /// <summary>
        /// CONSSTRUCTIONNUM_LS 取得临时施工证号
        /// CONSSTRUCTIONNUM 取得施工证号
        /// WBJ 未报监工程  
        ///  QT  其它（材料供应商送检等）  
        /// </summary>
        public string PROJECTTYPE { get; set; }

        [JsonConverter(typeof(ESTimeConverter))]
        public DateTime? UPLOADTIME { get; set; }

        [JsonConverter(typeof(ESTimeConverter))]
        public DateTime? UPDATETIME { get; set; }

        public string SLPEOPLEVALUE { get; set; }
        public string SPNPEOPLEVALUE { get; set; }
        public float? LONGITUDE { get; set; }
        public float? LATIDUTE { get; set; }

        public string REPOSITIONREASON { get; set; }
        public string REPOSITIONPHONE { get; set; }
        public string REPOSITIONUSERID { get; set; }
        [JsonConverter(typeof(ESTimeConverter))]
        public DateTime? REPOSITIONTIME { get; set; }
        /// <summary>
        /// 0变更申请，1审核通过,-1审核不通过,2已定位
        /// </summary>
        public int? REPOSITIONSTATUS { get; set; }
        [JsonConverter(typeof(ESTimeConverter))]
        public DateTime? REPOSITIONCHECKTIME { get; set; }
        public string REPOSITIONCHECKUSERID { get; set; }
    }

    [Serializable]
    public class ESSysProjectView
    {
        /// <summary>
        /// Id主键 unitCode + projectnum
        /// </summary>
        public string Id { get; set; }

        public string CUSTOMID { get; set; }

        public string CREATECUSTOMID { get; set; }

        /// <summary>
        /// 工程编号
        /// </summary> 
        public string PROJECTNUM { get; set; }

        public string PROJECTNUM_1 { get; set; }

        /// <summary>
        /// 单位工程名称
        /// </summary>
        public string PROJECTNAME { get; set; }
        //委托单位
        public string ENTRUSTUNIT { get; set; }
        //送样人
        public string SENDSAMPLEMAN { get; set; }
        //送样人电话
        public string SENDSAMPLEMANTEL { get; set; }
        //委托单位联系人
        public string ENTRUSTUNITLINKMAN { get; set; }
        //委托单位联系人电话
        public string ENTRUSTUNITLINKMANTEL { get; set; }
        //建设单位
        public string CONSTRACTUNIT { get; set; }
        //施工单位
        public string CONSTRACTIONUNIT { get; set; }
        //见证单位
        public string WITNESSUNIT { get; set; }
        //见证人
        public string WITNESSMAN { get; set; }
        //见证人证号
        public string WITNESSMANNUM { get; set; }
        //见证人电话
        public string WITNESSMANTEL { get; set; }
        //监督单位
        public string SUPERUNIT { get; set; }
        //监督人
        public string SUPERMAN { get; set; }
        //设计单位
        public string DESIGNUNIT { get; set; }
        public string INSPECTUNIT { get; set; }
        public string INSPECTMAN { get; set; }
        //勘察单位
        public string INVESTIGATEUNIT { get; set; }
        //勘察单位负责人、
        //取样人
        public string TAKESAMPLEMAN { get; set; }
        //取样人证号
        public string TAKESAMPLEMANNUM { get; set; }
        //取样人电话
        public string TAKESAMPLEMANTEL { get; set; }
        //取样单位
        public string TAKESAMPLEUNIT { get; set; }
        //工程地址
        public string PROJECTADDRESS { get; set; }
        //工程地址区域
        public string PROJECTAREA { get; set; }

        /// <summary>
        /// 建设工程项目编码（17位+3位单位工程编码）
        /// </summary>
        public string ISCUNIT { get; set; }

        /// <summary>
        /// 已作废的 
        /// </summary>
        public string ISCUNIT_1 { get; set; }

        /// <summary>
        /// 项目编码 (17位)
        /// </summary>
        public string XMBM { get; set; }

        /// <summary>
        /// 单位编码（3位）
        /// </summary> 
        public string DWBM { get; set; }

        /// <summary>
        /// 建设工程项目名称
        /// </summary>
        public string XMMC { get; set; }

        /// <summary>
        /// 子工程名称按照||||隔开（一栋,二栋)
        /// </summary>
        public string ZGCMCS { get; set; }

        /// <summary>
        /// 自工程编码数字按照空格隔开 (001 002 003 004)
        /// </summary>
        public string ZGCBMS { get; set; }

        /// <summary>
        /// 送检工程代码
        /// </summary>
        public string JCPROJECT { get; set; }
        public string PROJECTSTATUS { get; set; }
        public string PROJECTCHARGETYPE { get; set; }
        public double? ACCOUNTBALANCE { get; set; }
        public double? TOTALCONSUMEDMONEY { get; set; }
        public double? TOTALFAVOURABLEMONEY { get; set; }
        public string UNITCREDITLEVEL { get; set; }
        public string DEFAULTCONSUMETYPE { get; set; }

        
        public DateTime? CREATEDATE { get; set; }

         
        public DateTime? DESTROYDATE { get; set; }
        public string STRUTTYPE { get; set; }
        public string ISUSERICCARD { get; set; }
        public string PROJECTID { get; set; }
        public string CONTRACTNUM { get; set; }
        public string EMAIL { get; set; }
        public string AUTOSENDREPORT { get; set; }
        public string CREATEMAN { get; set; }
        public string ENTRUSTUNITADDRESS { get; set; }
        public string ENTRUSTUNITACCOUNTNUM { get; set; }
        public string ENTRUSTUNITNUM { get; set; }
        public string CONSTRACTUNITID { get; set; }
        public string DESIGNUNITID { get; set; }
        public string INVESTIGATEUNITID { get; set; }
        public string SUPERUNITID { get; set; }
        public string CONSTRACTIONUNITID { get; set; }
        public string INSPECTUNITID { get; set; }
        //地理区域编号
        public string AREAID { get; set; }
        public string ENTRUSTUNITID { get; set; }
        public string TAKESAMPLEUNITID { get; set; }
        public string WITNESSUNITID { get; set; }
        public string SUPERMANID { get; set; }
        public string INSPECTMANID { get; set; }
        public string SUPERMANTEL { get; set; }
        public string INSPECTMANTEL { get; set; }
        public string GCBH { get; set; }
        public string CONSTRACTUNITMANID { get; set; }
        public string CONSTRACTUNITMAN { get; set; }
        public string CONSTRACTUNITMANTEL { get; set; }
        public string CONSTRACTIONUNITMANID { get; set; }
        public string CONSTRACTIONUNITMAN { get; set; }
        public string CONSTRACTIONUNITMANTEL { get; set; }
        public string DESIGNUNITMANID { get; set; }
        public string DESIGNUNITMAN { get; set; }
        public string DESIGNUNITMANTEL { get; set; }
        public string INVESTIGATEUNITMANID { get; set; }
        public string INVESTIGATEUNITMAN { get; set; }
        public string INVESTIGATEUNITMANTEL { get; set; }
        public string ANQUANREGID { get; set; }

       
        public DateTime? PLANENDDATE { get; set; }

       
        public DateTime? PLANSTARTDATE { get; set; }
        public double? PROTECTSQUARE { get; set; }
        public double? SQUARE { get; set; }
        public string STRUCTLEVLES { get; set; }
        public double? TOTALPRICE { get; set; }
        public string SENDSAMPLEMANNUM { get; set; }
        public string JGLX { get; set; }

        
        public DateTime? CHECKTIME { get; set; }
        public string SUPERMANTEL1 { get; set; }
        public string FPROJECTNAME { get; set; }
        public string JX_PROJECTID { get; set; }
        public string JX_ITEMID { get; set; }
        public string PROJECTKEY { get; set; }
        //施工证号
        public string CONSSTRUCTIONNUM { get; set; }
        //临时证号
        public string CONSSTRUCTIONNUM_LS { get; set; }
        //施工工程名称
        public string CONSSTRUCTIONNAME { get; set; }

        /// <summary>
        /// CONSSTRUCTIONNUM_LS 取得临时施工证号
        /// CONSSTRUCTIONNUM 取得施工证号
        /// WBJ 未报监工程  
        ///  QT  其它（材料供应商送检等）  
        /// </summary>
        public string PROJECTTYPE { get; set; }

        
        public DateTime? UPLOADTIME { get; set; }

        [JsonConverter(typeof(ESTimeConverter))]
        public DateTime? UPDATETIME { get; set; }

        public string SLPEOPLEVALUE { get; set; }
        public string SPNPEOPLEVALUE { get; set; }
        public float? LONGITUDE { get; set; }
        public float? LATIDUTE { get; set; }

        public string REPOSITIONREASON { get; set; }
        public string REPOSITIONPHONE { get; set; }
        public string REPOSITIONUSERID { get; set; }
      
        public DateTime? REPOSITIONTIME { get; set; }
        /// <summary>
        /// 0变更申请，1审核通过,-1审核不通过,2已定位
        /// </summary>
        public int? REPOSITIONSTATUS { get; set; }
       
        public DateTime? REPOSITIONCHECKTIME { get; set; }
        public string REPOSITIONCHECKUSERID { get; set; }
    }

    [Serializable]
    public class es_t_sys_files
    {
        public string ID { get; set; }
        [JsonConverter(typeof(ESTimeConverter))]
        public DateTime? UPDATETIME { get; set; }
        public string UNITCODE { get; set; }
        public string FILETYPE { get; set; }
        public string CUSTOMID { get; set; }
        public string FILEDATA { get; set; }
        [JsonConverter(typeof(ESTimeConverter))]
        public DateTime? UPLOADTIME { get; set; }
        public string ISCLOUD { get; set; }
        public string FKEY { get; set; }
        public string SYSPRIMARYKEY { get; set; }
        public string REALDATAPATH { get; set; }
        public string PK { get; set; }
        public string FILENAME { get; set; }
        public string TABLENUM { get; set; }
    }
}
