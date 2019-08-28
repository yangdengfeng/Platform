﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Entity.ST
{

    [Serializable]
    public class es_t_bp_item
    {
        public string CUSTOMID { get; set; }
        public string SYSPRIMARYKEY { get; set; }
        public string PROJECTNUM { get; set; }
        public string SENDSAMPLEMAN { get; set; }
        public string SENDSAMPLEMANTEL { get; set; }
        public string SAMPLENUM { get; set; }
        public string ENTRUSTNUM { get; set; }
        public string REPORTNUM { get; set; }
        public string SAMPLENAME { get; set; }
        public string STANDARDNAME { get; set; }
        public string CHECKTYPE { get; set; }
        public string STRUCTPART { get; set; }
        public string SAMPLEDISPOSEPHASE { get; set; }
        public string SAMPLEDISPOSEPHASEORIGIN { get; set; }
        public string SAMPLECHARGETYPE { get; set; }
        public string PRODUCEFACTORY { get; set; }
        public DateTime? REPORTCONSENTDATE { get; set; }
        public DateTime? ENTRUSTDATE { get; set; }
        public DateTime? CHECKDATE { get; set; }
        public DateTime? AUDITINGDATE { get; set; }
        public DateTime? APPROVEDATE { get; set; }
        public DateTime? PRINTDATE { get; set; }
        public string CHECKCONCLUSION { get; set; }
        public string CONCLUSIONCODE { get; set; }
        public string ACCEPTSAMPLEMAN { get; set; }
        public string FIRSTCHECKMAN { get; set; }
        public string SECONDCHECKMAN { get; set; }
        public string VERIFYMAN { get; set; }
        public string AUDITINGMAN { get; set; }
        public string APPROVEMAN { get; set; }
        public string PRINTMAN { get; set; }
        public string EXTENDMAN { get; set; }
        public string INSTRUMENTNUM { get; set; }
        public string INSTRUMENTNAME { get; set; }
        public string EXPLAIN { get; set; }
        public string REPORTCONSENTDAYS { get; set; }
        public string WORKSTATION { get; set; }
        public string NEEDCHARGEMONEY { get; set; }
        public string COLLATEMAN { get; set; }
        public DateTime? COLLATEDATE { get; set; }
        public DateTime? VERIFYDATE { get; set; }
        public DateTime? EXTENDDATE { get; set; }
        public string BEFORESTATUS { get; set; }
        public string AFTERSTATUS { get; set; }
        public string TEMPERATURE { get; set; }
        public string HUMIDITY { get; set; }
        public string PDSTANDARDNAME { get; set; }
        public string TAKESAMPLEMAN { get; set; }
        public DateTime? FACTCHECKDATE { get; set; }
        public string PROJECTNAME { get; set; }
        public string REPORTSTYLESTYPES { get; set; }
        public string SENDTOWEB { get; set; }
        public string CONSTRACTUNIT { get; set; }
        public string ENTRUSTUNIT { get; set; }
        public string SUPERUNIT { get; set; }
        public string ITEMNAME { get; set; }
        public string CODEBAR { get; set; }
        public string QRCODEBAR { get; set; }
        public int? HAVEACS { get; set; }
        public int? HAVELOG { get; set; }
        public int? ISCREPORT { get; set; }
        public int? HAVREPORT { get; set; }
        public string SUBITEMLIST { get; set; }
        public DateTime? UPLOADTIME { get; set; }
        public DateTime? UPDATETIME { get; set; }
        public DateTime? ACSTIME { get; set; }
        public string TYPECODE { get; set; }
        public string SUBITEMCODE { get; set; }
        public string PARMCODE { get; set; }
        public int? REPSEQNO { get; set; }
        public string REPORMNUMWITHOUTSEQ { get; set; }
        public string CHUJIANID { get; set; }
        public string INSPECTMAN { get; set; }
        public string ITEMCHNAME { get; set; }
        /// <summary>
        /// 是否已处理 1已处理
        /// </summary>
        public int? ISDEAL { get; set; }
        /// <summary>
        /// 处理过程
        /// </summary>
        public string DEALPROCESS { get; set; }
    }

    [Serializable]
    public class es_t_bp_modify_log
    {
        public string CUSTOMID { get; set; }
        public string MODIFYPRIMARYKEY { get; set; }
        public string QUESTIONPRIMARYKEY { get; set; }
        public string SYSPRIMARYKEY { get; set; }
        public int? MODIFYTIMES { get; set; }
        public string SAMPLENUM { get; set; }
        public string REPORTNUM { get; set; }
        public string ENTRUSTNUM { get; set; }
        public string PROJECTNUM { get; set; }
        public string ITEMTABLENAME { get; set; }
        public string MODIFYMAN { get; set; }
        public string FIELDNAME { get; set; }
        public DateTime? MODIFYDATETIME { get; set; }
        public string BEFOREMODIFYVALUES { get; set; }
        public string AFTERMODIFYVALUES { get; set; }
        public string SENDFLAGS { get; set; }
        public string COMPUTERNAME { get; set; }
        public string MACADDRESS { get; set; }
        public string PK { get; set; }
        public DateTime? UPLOADTIME { get; set; }
        public DateTime? UPDATETIME { get; set; }
    }

    [Serializable]
    public class es_t_bp_question
    {
        public string CUSTOMID { get; set; }
        public string QUESTIONPRIMARYKEY { get; set; }
        public string SYSPRIMARYKEY { get; set; }
        public string ITEMTABLENAME { get; set; }
        public string SAMPLENUM { get; set; }
        public string RECORDMAN { get; set; }
        public DateTime? RECORDTIME { get; set; }
        public string RECORDINGPHASE { get; set; }
        public string AUDITINGMAN { get; set; }
        public DateTime? AUDITINGTIME { get; set; }
        public string ISAUDITING { get; set; }
        public string APPROVEMAN { get; set; }
        public DateTime? APPROVETIME { get; set; }
        public string ISAPPROVE { get; set; }
        public string NEEDPROCMAN { get; set; }
        public string ISPROCED { get; set; }
        public DateTime? NEEDPROCTIME { get; set; }
        public string QUESTIONTYPES { get; set; }
        public string CONTEXT { get; set; }
        public string AUDITINGCONTEXT { get; set; }
        public string APPROVECONTEXT { get; set; }
        public int? MODIFYRANGE { get; set; }
        public string ISMODIFIED { get; set; }
        public string ISREFUNDMONEY { get; set; }
        public int? ISCONSOLEAPPROVED { get; set; }
        public string PK { get; set; }
        public DateTime? UPLOADTIME { get; set; }
        public DateTime? UPDATETIME { get; set; }
    }

    [Serializable]
    public class es_t_bp_imagerecognition
    {
        public string QRCODE { get; set; }
        public string QIMG { get; set; }
        public string SIMG { get; set; }
        public string QXIMG { get; set; }
        public string SHOWINFO { get; set; }
    }

    [Serializable]
    public class es_covrlist
    {
        public string UNITCODE { get; set; }
        public string SYSPRIMARYKEY { get; set; }
        public int? ID { get; set; }
        public string CODE { get; set; }
        public string PROJECTSERIALNO { get; set; }
        public string PROJECTNAME { get; set; }
        public string CUSTOMID { get; set; }
        public string CHECKITEMTYPE { get; set; }
        public string CHECKITEMNAME { get; set; }
        public DateTime? DECLAREDATE { get; set; }
        public DateTime? BEGINDATE { get; set; }
        public DateTime? ENDDATE { get; set; }
        public string TIMEPERIOD { get; set; }
        public int? STATUS { get; set; }
        public string TESTERS { get; set; }
        public string DECLARANT { get; set; }
        public string DECLARANTIDNO { get; set; }
        public DateTime? FINISHTIME { get; set; }
        public string FINISHER { get; set; }
        public string FINISHIDNO { get; set; }
        public string REMARK { get; set; }
        public string COORDINATES { get; set; }
        public string IMAGES { get; set; }
        public string VIDEOS { get; set; }
        public string WITNESS { get; set; }
        public string CHECKTYPECODE { get; set; }
        public string CHECKTYPENAME { get; set; }
        public string CHECKITEMCODE { get; set; }
        public string CHECKPARAMNAME { get; set; }
        public DateTime? UPLOADTIME { get; set; }
    }

    [Serializable]
    public class es_t_bp_acs
    {
        public string CUSTOMID { get; set; }
        public string UNITCODE { get; set; }
        public string SYSPRIMARYKEY { get; set; }
        public string COLUMNNAME { get; set; }
        public string ITEMTABLENAME { get; set; }
        public string SAMPLENUM { get; set; }
        public int? MAXLC { get; set; }
        public string TIMES { get; set; }
        public string MAXVALUE { get; set; }
        public string QFVALUE { get; set; }
        public DateTime? ACSTIME { get; set; }
        public string DATATYPES { get; set; }
        public string A { get; set; }
        public string B { get; set; }
        public string A1 { get; set; }
        public string B1 { get; set; }
        public string A2 { get; set; }
        public string B2 { get; set; }
        public string YSN { get; set; }
        public string CJSJ { get; set; }
        public string CHECKMAN { get; set; }
        public string INSTRUMENTNUM { get; set; }
        public string INSTRUMENTNAME { get; set; }
        public string SPEED { get; set; }
        public string ACSDATAPATH { get; set; }
        public string PK { get; set; }
        public DateTime? UPLOADTIME { get; set; }
        public DateTime? UPDATETIME { get; set; }
    }

    [Serializable]
    public class es_t_bp_wordreport
    {
        public string CUSTOMID { get; set; }
        public string ITEMTABLENAME { get; set; }
        public string SYSPRIMARYKEY { get; set; }
        public string REPORTTYPES { get; set; }
        public string WORDREPORTPATH { get; set; }
        public string ISLOCALSTORE { get; set; }
        public DateTime? UPLOADTIME { get; set; }
        public DateTime? UPDATETIME { get; set; }
    }

    [Serializable]
    public class es_extReportMange
    {
        public string CUSTOMID { get; set; }
        public string IDENTKEY { get; set; }
        public string ITEMTABLENAME { get; set; }
        public string SYSPRIMARYKEY { get; set; }
        public string REPORTNUM { get; set; }
        public string FILETYPE { get; set; }
        public string REPORTNAME { get; set; }
        public string WORDREPORTPATH { get; set; }
        public string ISLOCALSTORE { get; set; }
        public DateTime? UPLOADTIME { get; set; }
        public DateTime? UPDATETIME { get; set; }
    }

    [Serializable]
    public class es_t_bp_project
    {
        public string CUSTOMID { get; set; }
        public string PROJECTNUM { get; set; }
        public string PROJECTNAME { get; set; }
        public string ENTRUSTUNIT { get; set; }
        public string SENDSAMPLEMAN { get; set; }
        public string SENDSAMPLEMANTEL { get; set; }
        public string ENTRUSTUNITLINKMAN { get; set; }
        public string ENTRUSTUNITLINKMANTEL { get; set; }
        public string CONSTRACTUNIT { get; set; }
        public string CONSTRACTIONUNIT { get; set; }
        public string WITNESSUNIT { get; set; }
        public string WITNESSMAN { get; set; }
        public string WITNESSMANNUM { get; set; }
        public string WITNESSMANTEL { get; set; }
        public string SUPERUNIT { get; set; }
        public string SUPERMAN { get; set; }
        public string DESIGNUNIT { get; set; }
        public string INSPECTUNIT { get; set; }
        public string INSPECTMAN { get; set; }
        public string INVESTIGATEUNIT { get; set; }
        public string TAKESAMPLEMAN { get; set; }
        public string TAKESAMPLEMANNUM { get; set; }
        public string TAKESAMPLEMANTEL { get; set; }
        public string TAKESAMPLEUNIT { get; set; }
        public string PROJECTADDRESS { get; set; }
        public string PROJECTAREA { get; set; }
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
        public string INSPECTUNITNUM { get; set; }
        public string CONTRACTIONPERMITNUM { get; set; }
        public string INSPECTPROJECTNUM { get; set; }
        public string HAINANXIANGMUBIAOSHI { get; set; }
        public string HAINANXIANGMUBIANHAO { get; set; }
        public string HAINANXIANGMUMINGCHEN { get; set; }
        public string HAINANGONGCHENGBIAOSHI { get; set; }
        public string HAINANGONGCHENGBIANHAO { get; set; }
        public string HAINANSHIGONGDANWEIBIAOSHI { get; set; }
        public int? HAINANISVARIFYINFO { get; set; }
        public string HAINANDANWEIGONGCHENGMING { get; set; }
        public string RECKONER { get; set; }
        public string DEPOSITRATE { get; set; }
        public string USERDEFINEBAR { get; set; }
        public long? ICARD { get; set; }
        public string SALEMAN { get; set; }
        public string PROJECTKEY { get; set; }
        public DateTime? UPLOADTIME { get; set; }
        public DateTime? UPDATETIME { get; set; }
    }


    [Serializable]
    public class es_t_bp_item_list
    {
        public string CUSTOMID { get; set; }
        public string UNITCODE { get; set; }
        public string ITEMTABLENAME { get; set; }
        public string ITEMCHNAME { get; set; }
        public string ITEMTYPES { get; set; }
        public string ITEMSTANDARDS { get; set; }
        public int? REPORTCONLINES { get; set; }
        public int? RECORDCONLINES { get; set; }
        public int? ENTRFORMCONLINES { get; set; }
        public int? REPORTORGI { get; set; }
        public int? RECORDORGI { get; set; }
        public string TESTEQUIPMENTNUM { get; set; }
        public string TESTEQUIPMENTNAME { get; set; }
        public int? REPORTCONSENTDAYS { get; set; }
        public int? REPORTCONSENTCWORKDAYS { get; set; }
        public string KEYS { get; set; }
        public int? USEFREQ { get; set; }
        public int? CHECKMANCOUNT { get; set; }
        public string PRINTDATEMODE { get; set; }
        public string STANDARDNAMEMODE { get; set; }
        public int? ISHAVEGJ { get; set; }
        public int? ONLYMANAGEREPORT { get; set; }
        public int? CHECKDATESTYLE { get; set; }
        public string ISUSE { get; set; }
        public int? CHECKDAYS { get; set; }
        public int? REPORTDAYS { get; set; }
        public string JX_LBDM { get; set; }
        public string JX_ITEMCODE { get; set; }
        public string JX_LB { get; set; }
        public string JX_ITEMCHNAME { get; set; }
        public string HEBEI_ITEMCODE { get; set; }
        public string HEBEI_TABLENAME { get; set; }
        public string HEBEI_ITEMCHNAME { get; set; }
        public DateTime? STARTTIME { get; set; }
        public DateTime? VALIDDATE { get; set; }
        public string PDCOLUMNS { get; set; }
        public DateTime? UPLOADTIME { get; set; }
        public DateTime? UPDATETIME { get; set; }
    }

    [Serializable]
    public class es_sub_item_pb
    {
        public string SYSPRIMARYKEY { get; set; }
        public string SUBITEMCOLUMN { get; set; }
        public string SUBITEMNAME { get; set; }
        public string PDJG { get; set; }
        public DateTime? UPLOADTIME { get; set; }
        public DateTime? UPDATETIME { get; set; }
    }

    [Serializable]
    public class es_t_pkpm_binaryReport
    {
        public string CUSTOMID { get; set; }
        public string REPORTNUM { get; set; }
        public string REPORTTYPE { get; set; }
        public int? ORDERID { get; set; }
        public int? STATUS { get; set; }
        public string REPORTPATH { get; set; }
        public DateTime? UPLOADTIME { get; set; }
        public DateTime? UPDATETIME { get; set; }
    }


    [Serializable]
    public class es_tono_cur
    {
        public string SP_MTTTT { get; set; }
        public string CUSTOMID { get; set; }
        public string SYSPRIMARYKEY { get; set; }
        public string PROJECTNUM { get; set; }
        public string SENDSAMPLEMAN { get; set; }
        public string SENDSAMPLEMANTEL { get; set; }
        public string SAMPLENUM { get; set; }
        public string ENTRUSTNUM { get; set; }
        public string REPORTNUM { get; set; }
        public string SAMPLENAME { get; set; }
        public string STANDARDNAME { get; set; }
        public string CHECKTYPE { get; set; }
        public string STRUCTPART { get; set; }
        public string DEPUTYBATCH { get; set; }
        public string SAMPLEAMOUNT { get; set; }
        public string SAMPLESTATUS { get; set; }
        public string SAMPLEDISPOSEMODE { get; set; }
        public string SAMPLEDISPOSEPHASE { get; set; }
        public string SAMPLECHARGETYPE { get; set; }
        public string PRODUCEFACTORY { get; set; }
        public DateTime? PRODUCEDATE { get; set; }
        public DateTime? REPORTCONSENTDATE { get; set; }
        public DateTime? ENTRUSTDATE { get; set; }
        public DateTime? CHECKDATE { get; set; }
        public DateTime? AUDITINGDATE { get; set; }
        public DateTime? APPROVEDATE { get; set; }
        public DateTime? PRINTDATE { get; set; }
        public string CHECKCONCLUSION { get; set; }
        public string CONCLUSIONCODE { get; set; }
        public string ACCEPTSAMPLEMAN { get; set; }
        public string FIRSTCHECKMAN { get; set; }
        public string SECONDCHECKMAN { get; set; }
        public string VERIFYMAN { get; set; }
        public string AUDITINGMAN { get; set; }
        public string APPROVEMAN { get; set; }
        public string PRINTMAN { get; set; }
        public string EXTENDMAN { get; set; }
        public string INSTRUMENTNUM { get; set; }
        public string INSTRUMENTNAME { get; set; }
        public string CHECKENVIRONMENT { get; set; }
        public string EXPLAIN { get; set; }
        public int? REPORTCONSENTDAYS { get; set; }
        public string WORKSTATION { get; set; }
        public string AUTODEFINENOTE { get; set; }
        public string CHECKITEM1 { get; set; }
        public float? NEEDCHARGEMONEY { get; set; }
        public string COLLATEMAN { get; set; }
        public DateTime? COLLATEDATE { get; set; }
        public DateTime? VERIFYDATE { get; set; }
        public DateTime? EXTENDDATE { get; set; }
        public string BEFORESTATUS { get; set; }
        public string AFTERSTATUS { get; set; }
        public string TEMPERATURE { get; set; }
        public string HUMIDITY { get; set; }
        public string PDSTANDARDNAME { get; set; }
        public string BACKUPCOLUMN1 { get; set; }
        public string BACKUPCOLUMN2 { get; set; }
        public string BACKUPCOLUMN3 { get; set; }
        public string BACKUPCOLUMN4 { get; set; }
        public string BACKUPCOLUMN5 { get; set; }
        public string BACKUPCOLUMN6 { get; set; }
        public string BACKUPCOLUMN7 { get; set; }
        public string BACKUPCOLUMN8 { get; set; }
        public string BACKUPCOLUMN9 { get; set; }
        public string BACKUPCOLUMN10 { get; set; }
        public string BACKUPCOLUMN11 { get; set; }
        public string BACKUPCOLUMN12 { get; set; }
        public string BACKUPCOLUMN13 { get; set; }
        public string BACKUPCOLUMN14 { get; set; }
        public string BACKUPCOLUMN15 { get; set; }
        public string BACKUPCOLUMN16 { get; set; }
        public string BACKUPCOLUMN17 { get; set; }
        public string BACKUPCOLUMN18 { get; set; }
        public string BACKUPCOLUMN19 { get; set; }
        public string BACKUPCOLUMN20 { get; set; }
        public string BACKUPCOLUMN21 { get; set; }
        public string BACKUPCOLUMN22 { get; set; }
        public string BACKUPCOLUMN23 { get; set; }
        public string BACKUPCOLUMN24 { get; set; }
        public string BACKUPCOLUMN25 { get; set; }
        public string BACKUPCOLUMN26 { get; set; }
        public string BACKUPCOLUMN27 { get; set; }
        public string BACKUPCOLUMN28 { get; set; }
        public string BACKUPCOLUMN29 { get; set; }
        public string BACKUPCOLUMN30 { get; set; }
        public string WITNESSUNIT { get; set; }
        public string WITNESSMAN { get; set; }
        public string WITNESSMANNUM { get; set; }
        public string WITNESSMANTEL { get; set; }
        public string SUPERMAN { get; set; }
        public string SUPERUNIT { get; set; }
        public string TAKESAMPLEUNIT { get; set; }
        public string TAKESAMPLEMANNUM { get; set; }
        public string TAKESAMPLEMAN { get; set; }
        public string TAKESAMPLEMANTEL { get; set; }
        public string INSPECTMAN { get; set; }
        public string CONSTRACTIONUNIT { get; set; }
        public DateTime? FACTCHECKDATE { get; set; }
        public string PROJECTNAME { get; set; }
        public string REPORTSTYLESTYPES { get; set; }
        public string SENDTOWEB { get; set; }
        public string CONSTRACTUNIT { get; set; }
        public string ENTRUSTUNIT { get; set; }
        public string TONENO_OLD { get; set; }
        public DateTime? PEIZ_DATE { get; set; }
        public float? MIN_GJD { get; set; }
        public float? MIN_MCC { get; set; }
        public string CDZB { get; set; }
        public string YANGHU { get; set; }
        public string CHXHF { get; set; }
        public float? LINQI { get; set; }
        public float? KZQD { get; set; }
        public string PEILX { get; set; }
        public string JZHF { get; set; }
        public string PJHF { get; set; }
        public string C_DJ { get; set; }
        public string KSDJ { get; set; }
        public string T_HJTJ { get; set; }
        public string TTD { get; set; }
        public string JBFH { get; set; }
        public float? TLD30MIN { get; set; }
        public float? TLD60MIN { get; set; }
        public float? C_BZC { get; set; }
        public float? C_PEIQD { get; set; }
        public string C_CNO { get; set; }
        public string C_CVARIETY { get; set; }
        public string C_CDJ { get; set; }
        public string C_CPRODUCE { get; set; }
        public float? C_CXISU { get; set; }
        public float? C_CMIDU { get; set; }
        public float? C_CFMPA { get; set; }
        public float? C_CZMPAT { get; set; }
        public float? C_CZMPATE { get; set; }
        public float? C_CMPAT { get; set; }
        public float? C_CMPATE { get; set; }
        public string C_SAFERST { get; set; }
        public string SZ_NO1 { get; set; }
        public string SZ_SPRODUCE1 { get; set; }
        public string SZ_SGRIDDLE1 { get; set; }
        public float? SZ_SCGMASS1 { get; set; }
        public float? SZ_SCXIDU1 { get; set; }
        public float? SZ_SAPPDENSITY1 { get; set; }
        public float? SZ_SDENSITY1 { get; set; }
        public float? SZ_SCMUD1 { get; set; }
        public string SZ_VARIETY1 { get; set; }
        public float? SZ_CL1 { get; set; }
        public string SZ_SPEC1 { get; set; }
        public string SZ_NO2 { get; set; }
        public string SZ_SPRODUCE2 { get; set; }
        public string SZ_SGRIDDLE2 { get; set; }
        public float? SZ_SCGMASS2 { get; set; }
        public float? SZ_SCXIDU2 { get; set; }
        public float? SZ_SAPPDENSITY2 { get; set; }
        public float? SZ_SDENSITY2 { get; set; }
        public float? SZ_SCMUD2 { get; set; }
        public string SZ_VARIETY2 { get; set; }
        public float? SZ_CL2 { get; set; }
        public string SZ_SPEC2 { get; set; }
        public string SI_INO1 { get; set; }
        public string SI_IPRODUCE1 { get; set; }
        public string SI_IPINZ1 { get; set; }
        public string SI_ISPCE1 { get; set; }
        public float? SI_IAPPDENSITY1 { get; set; }
        public float? SI_IDENSITY1 { get; set; }
        public float? SI_ICMUD1 { get; set; }
        public float? SI_IZPKL1 { get; set; }
        public float? SI_CL1 { get; set; }
        public float? SI_MAXLJ1 { get; set; }
        public float? SI_DENSITYL1 { get; set; }
        public string SI_INO2 { get; set; }
        public string SI_IPRODUCE2 { get; set; }
        public string SI_IPINZ2 { get; set; }
        public string SI_ISPCE2 { get; set; }
        public float? SI_IAPPDENSITY2 { get; set; }
        public float? SI_IDENSITY2 { get; set; }
        public float? SI_ICMUD2 { get; set; }
        public float? SI_IZPKL2 { get; set; }
        public float? SI_CL2 { get; set; }
        public float? SI_MAXLJ2 { get; set; }
        public float? SI_DENSITYL2 { get; set; }
        public string MT_PINZ { get; set; }
        public string MT_DJ { get; set; }
        public string MT_SYFH { get; set; }
        public float? MT_QDP { get; set; }
        public float? MT_MD { get; set; }
        public float? MT_CLXS { get; set; }
        public string MT_WATER { get; set; }
        public string MT_PRODUCE { get; set; }
        public string WJ_PINZ1 { get; set; }
        public float? WJ_CANL1 { get; set; }
        public float? WJ_JSL1 { get; set; }
        public string WJ_SYFH1 { get; set; }
        public string WJ_PRODUNIT1 { get; set; }
        public string WJ_PINZ2 { get; set; }
        public float? WJ_CANL2 { get; set; }
        public float? WJ_JSL2 { get; set; }
        public string WJ_SYFH2 { get; set; }
        public string WJ_PRODUNIT2 { get; set; }
        public float? LT_SDYXS { get; set; }
        public float? LT_FMIMD { get; set; }
        public float? LT_SHB { get; set; }
        public float? LT_SDSP { get; set; }
        public float? LT_MIDUM { get; set; }
        public float? LT_MIDUM1 { get; set; }
        public float? CT_MIDUM1 { get; set; }
        public float? CT_MIDUM2 { get; set; }
        public float? CT_MIDUM3 { get; set; }
        public float? LT_SHUIM { get; set; }
        public float? LT_M { get; set; }
        public float? LT_CEMT1 { get; set; }
        public float? LT_SZI11 { get; set; }
        public float? LT_SZI21 { get; set; }
        public float? LT_WATER1 { get; set; }
        public float? LT_SII11 { get; set; }
        public float? LT_SII21 { get; set; }
        public float? LT_MT1 { get; set; }
        public float? LT_WJ11 { get; set; }
        public float? LT_WJ21 { get; set; }
        public float? LT_CEMT2 { get; set; }
        public float? LT_SZI12 { get; set; }
        public float? LT_SZI22 { get; set; }
        public float? LT_WATER2 { get; set; }
        public float? LT_SII12 { get; set; }
        public float? LT_SII22 { get; set; }
        public float? LT_MT2 { get; set; }
        public float? LT_WJ12 { get; set; }
        public float? LT_WJ22 { get; set; }
        public float? CT_BZL { get; set; }
        public float? CT_BZL1 { get; set; }
        public float? CT_BZL2 { get; set; }
        public float? CT_BZL3 { get; set; }
        public float? CT_CEMT { get; set; }
        public float? CT_SZI1 { get; set; }
        public float? CT_SZI2 { get; set; }
        public float? CT_WATER { get; set; }
        public float? CT_SII1 { get; set; }
        public float? CT_SII2 { get; set; }
        public float? CT_MT { get; set; }
        public float? CT_WJ1 { get; set; }
        public float? CT_WJ2 { get; set; }
        public float? CT_SHB1 { get; set; }
        public float? CT_SP1 { get; set; }
        public float? CT_WATER1 { get; set; }
        public float? CT_CEMT1 { get; set; }
        public float? CT_SZI11 { get; set; }
        public float? CT_SZI21 { get; set; }
        public float? CT_SII11 { get; set; }
        public float? CT_SII21 { get; set; }
        public float? CT_MT1 { get; set; }
        public float? CT_WJ11 { get; set; }
        public float? CT_WJ21 { get; set; }
        public float? CT_SHB2 { get; set; }
        public float? CT_SP2 { get; set; }
        public float? CT_WATER2 { get; set; }
        public float? CT_CEMT2 { get; set; }
        public float? CT_SZI12 { get; set; }
        public float? CT_SZI22 { get; set; }
        public float? CT_SII12 { get; set; }
        public float? CT_SII22 { get; set; }
        public float? CT_MT2 { get; set; }
        public float? CT_WJ12 { get; set; }
        public float? CT_WJ22 { get; set; }
        public float? CT_SHB3 { get; set; }
        public float? CT_SP3 { get; set; }
        public float? CT_WATER3 { get; set; }
        public float? CT_CEMT3 { get; set; }
        public float? CT_SZI13 { get; set; }
        public float? CT_SZI23 { get; set; }
        public float? CT_SII13 { get; set; }
        public float? CT_SII23 { get; set; }
        public float? CT_MT3 { get; set; }
        public float? CT_WJ13 { get; set; }
        public float? CT_WJ23 { get; set; }
        public float? CTP_CEMT1 { get; set; }
        public float? CTP_WATER1 { get; set; }
        public float? CTP_MT1 { get; set; }
        public float? CTP_SZI1 { get; set; }
        public float? CTP_SII1 { get; set; }
        public float? CTP_WJ1 { get; set; }
        public float? CTP_TTD1 { get; set; }
        public string CTP_HYX1 { get; set; }
        public string CTP_GZD1 { get; set; }
        public float? CTP_ZLMD1 { get; set; }
        public float? CTP_MPAT1 { get; set; }
        public float? CTP_MPATE1 { get; set; }
        public float? CTP_MPA1 { get; set; }
        public float? CTP_CEMT2 { get; set; }
        public float? CTP_WATER2 { get; set; }
        public float? CTP_MT2 { get; set; }
        public float? CTP_SZI2 { get; set; }
        public float? CTP_SII2 { get; set; }
        public float? CTP_WJ2 { get; set; }
        public float? CTP_TTD2 { get; set; }
        public string CTP_HYX2 { get; set; }
        public string CTP_GZD2 { get; set; }
        public float? CTP_ZLMD2 { get; set; }
        public float? CTP_MPAT2 { get; set; }
        public float? CTP_MPATE2 { get; set; }
        public float? CTP_MPA2 { get; set; }
        public float? CTP_CEMT3 { get; set; }
        public float? CTP_WATER3 { get; set; }
        public float? CTP_MT3 { get; set; }
        public float? CTP_SZI3 { get; set; }
        public float? CTP_SII3 { get; set; }
        public float? CTP_WJ3 { get; set; }
        public float? CTP_TTD3 { get; set; }
        public string CTP_HYX3 { get; set; }
        public string CTP_GZD3 { get; set; }
        public float? CTP_ZLMD3 { get; set; }
        public float? CTP_MPAT3 { get; set; }
        public float? CTP_MPATE3 { get; set; }
        public float? CTP_MPA3 { get; set; }
        public float? CTP_XISU { get; set; }
        public string CTP_DAY { get; set; }
        public float? SP_SHB { get; set; }
        public float? SP_WATER { get; set; }
        public float? SP_CEMT { get; set; }
        public float? SP_SZI1 { get; set; }
        public string SP_SZI2 { get; set; }
        public float? SP_SII1 { get; set; }
        public string SP_SII2 { get; set; }
        public float? SP_MT { get; set; }
        public float? SP_WJ1 { get; set; }
        public float? SP_WJ2 { get; set; }
        public float? SPJ_CEMT { get; set; }
        public float? SPJ_WATER { get; set; }
        public float? SPJ_MT { get; set; }
        public float? SPJ_SZI { get; set; }
        public float? SPJ_SII { get; set; }
        public float? SPJ_WJ { get; set; }
        public float? SP_SJB { get; set; }
        public float? SP_SP { get; set; }
        public float? SP_TTD { get; set; }
        public float? SP_BGMD { get; set; }
        public float? SP_XISU { get; set; }
        public string SP_KSDJ { get; set; }
        public float? SP_ZMPAT { get; set; }
        public float? SP_ZMPATE { get; set; }
        public float? SP_MPAT { get; set; }
        public float? SP_MPATE { get; set; }
        public float? SP_MPA { get; set; }
        public float? SP_MIDUM { get; set; }
        public DateTime? CHX_DATE { get; set; }
        public DateTime? PXS_DATE { get; set; }
        public DateTime? PXE_DATE { get; set; }
        public string SIZE { get; set; }
        public float? KY_YLS11 { get; set; }
        public float? KY_YLS12 { get; set; }
        public float? KY_YLS13 { get; set; }
        public float? KY_YLS21 { get; set; }
        public float? KY_YLS22 { get; set; }
        public float? KY_YLS23 { get; set; }
        public float? KY_YLS31 { get; set; }
        public float? KY_YLS32 { get; set; }
        public float? KY_YLS33 { get; set; }
        public float? KY_QDS11 { get; set; }
        public float? KY_QDS12 { get; set; }
        public float? KY_QDS13 { get; set; }
        public float? KY_QDS21 { get; set; }
        public float? KY_QDS22 { get; set; }
        public float? KY_QDS23 { get; set; }
        public float? KY_QDS31 { get; set; }
        public float? KY_QDS32 { get; set; }
        public float? KY_QDS33 { get; set; }
        public float? KY_QDSZ1 { get; set; }
        public float? KY_QDSZ2 { get; set; }
        public float? KY_QDSZ3 { get; set; }
        public float? KY_YLE11 { get; set; }
        public float? KY_YLE12 { get; set; }
        public float? KY_YLE13 { get; set; }
        public float? KY_YLE21 { get; set; }
        public float? KY_YLE22 { get; set; }
        public float? KY_YLE23 { get; set; }
        public float? KY_YLE31 { get; set; }
        public float? KY_YLE32 { get; set; }
        public float? KY_YLE33 { get; set; }
        public float? KY_QDE11 { get; set; }
        public float? KY_QDE12 { get; set; }
        public float? KY_QDE13 { get; set; }
        public float? KY_QDE21 { get; set; }
        public float? KY_QDE22 { get; set; }
        public float? KY_QDE23 { get; set; }
        public float? KY_QDE31 { get; set; }
        public float? KY_QDE32 { get; set; }
        public float? KY_QDE33 { get; set; }
        public float? KY_QDEZ1 { get; set; }
        public float? KY_QDEZ2 { get; set; }
        public float? KY_QDEZ3 { get; set; }
        public float? SPJ_WJ1 { get; set; }
        public float? SPJ_WJ2 { get; set; }
        public float? SPJ_SII1 { get; set; }
        public string SPJ_SII2 { get; set; }
        public float? SPJ_SZI1 { get; set; }
        public string SPJ_SZI2 { get; set; }
        public string MTT_PINZ { get; set; }
        public string MTT_DJ { get; set; }
        public string MTT_SYFH { get; set; }
        public float? MTT_QDP { get; set; }
        public float? MTT_MD { get; set; }
        public float? MTT_CLXS { get; set; }
        public string MTT_PRODUCE { get; set; }
        public float? LT_MTT1 { get; set; }
        public float? LT_MTT2 { get; set; }
        public float? CT_MTT { get; set; }
        public float? CT_MTT1 { get; set; }
        public float? CT_MTT2 { get; set; }
        public float? CT_MTT3 { get; set; }
        public float? SP_MTT { get; set; }
        public float? SPJ_MTT { get; set; }
        public float? CTP_ZMPAT1 { get; set; }
        public float? CTP_ZMPAT2 { get; set; }
        public float? CTP_ZMPAT3 { get; set; }
        public float? CTP_ZMPATE1 { get; set; }
        public float? CTP_ZMPATE2 { get; set; }
        public float? CTP_ZMPATE3 { get; set; }
        public string CTP_KSDJ1 { get; set; }
        public string CTP_KSDJ2 { get; set; }
        public string CTP_KSDJ3 { get; set; }
        public float? CTP_MTT1 { get; set; }
        public float? CTP_MTT2 { get; set; }
        public float? CTP_MTT3 { get; set; }
        public float? CTP_SIIT1 { get; set; }
        public float? CTP_SIIT2 { get; set; }
        public float? CTP_SIIT3 { get; set; }
        public float? CTP_SZIT1 { get; set; }
        public float? CTP_SZIT2 { get; set; }
        public float? CTP_SZIT3 { get; set; }
        public float? CTP_WJT1 { get; set; }
        public float? CTP_WJT2 { get; set; }
        public float? CTP_WJT3 { get; set; }
        public string TLD30MIN1 { get; set; }
        public string TLD30MIN2 { get; set; }
        public string TLD60MIN1 { get; set; }
        public string TLD60MIN2 { get; set; }
        public float? SHB_A { get; set; }
        public float? SHB_B { get; set; }
        public float? SHB_FCUO { get; set; }
        public float? SHB_F { get; set; }
        public float? C0 { get; set; }
        public float? W0 { get; set; }
        public float? S0 { get; set; }
        public float? G0 { get; set; }
        public float? KY_YLK11 { get; set; }
        public float? KY_YLK12 { get; set; }
        public float? KY_YLK13 { get; set; }
        public float? KY_YLK21 { get; set; }
        public float? KY_YLK22 { get; set; }
        public float? KY_YLK23 { get; set; }
        public float? KY_YLK31 { get; set; }
        public float? KY_YLK32 { get; set; }
        public float? KY_YLK33 { get; set; }
        public float? KY_QDK11 { get; set; }
        public float? KY_QDK12 { get; set; }
        public float? KY_QDK13 { get; set; }
        public float? KY_QDK21 { get; set; }
        public float? KY_QDK22 { get; set; }
        public float? KY_QDK23 { get; set; }
        public float? KY_QDK31 { get; set; }
        public float? KY_QDK32 { get; set; }
        public float? KY_QDK33 { get; set; }
        public float? KY_QDKZ1 { get; set; }
        public float? KY_QDKZ2 { get; set; }
        public float? KY_QDKZ3 { get; set; }
        public DateTime? PXK_DATE { get; set; }
        public string KDDJ { get; set; }
        public string SCKDDJ { get; set; }
        public float? SZ_CPUGPA1 { get; set; }
        public float? SZ_CPUGPA2 { get; set; }
        public float? SI_CPUGPA1 { get; set; }
        public float? SI_CPUGPA2 { get; set; }
        public string JLX { get; set; }
        public string BSL { get; set; }
        public string KZX { get; set; }
        public string QTYQ { get; set; }
        public float? LIMEP1 { get; set; }
        public float? LIMEP2 { get; set; }
        public string QT_PINZ { get; set; }
        public float? QT_CL { get; set; }
        public string QT_PRODUNIT { get; set; }
        public string QT_SPEC { get; set; }
        public float? SP_QT { get; set; }
        public float? SPJ_QT { get; set; }
        public float? SP_ZMPA { get; set; }
        public string SP_KSMPA { get; set; }
        public string SP_KSMPAT { get; set; }
        public string SP_KSMPATE { get; set; }
        public string YHFS { get; set; }
        public string SW { get; set; }
        public string NJX { get; set; }
        public string BSX { get; set; }
        public string BHDSFS { get; set; }
        public string MT_NO1 { get; set; }
        public string MT_NO2 { get; set; }
        public string WJ_NO1 { get; set; }
        public string WJ_NO2 { get; set; }
        public string QT_NO1 { get; set; }
        public string SM { get; set; }
        public string tasknum { get; set; }
        public string LYTBAZH { get; set; }
        public string DHFL { get; set; }
        public string LYZS { get; set; }
        public float? SP_CEMT_JS { get; set; }
        public float? SP_SZI1_JS { get; set; }
        public string SP_SZI2_JS { get; set; }
        public float? SP_SII1_JS { get; set; }
        public string SP_SII2_JS { get; set; }
        public float? SP_WATER_JS { get; set; }
        public float? SP_MT_JS { get; set; }
        public float? SP_MTT_JS { get; set; }
        public float? SP_WJ1_JS { get; set; }
        public float? SP_WJ2_JS { get; set; }
        public string sclwdh { get; set; }
        public string scdw { get; set; }
        public string scdd { get; set; }
        public float? hsl_s1 { get; set; }
        public float? hsl_s2 { get; set; }
        public float? hsl_s { get; set; }
        public float? hsl_z1 { get; set; }
        public float? hsl_z { get; set; }
        public string WATER_NO { get; set; }
        public string WJ_SPEC1 { get; set; }
        public string WJ_SPEC2 { get; set; }
        public string MT_NO3 { get; set; }
        public string MTTT_PINZ { get; set; }
        public string MTTT_DJ { get; set; }
        public float? MTTT_MD { get; set; }
        public float? MTTT_QDP { get; set; }
        public float? MTTT_CLXS { get; set; }
        public string MTTT_SYFH { get; set; }
        public string MTTT_PRODUCE { get; set; }
        public float? SPJ_MTTT { get; set; }
        public float? SP_MTTT { get; set; }


        public string ITM_STATE { get; set; }
        public string TASKNUM { get; set; }
        public DateTime? UPLOADTIME { get; set; }
        public DateTime? UPDATETIME { get; set; }
    }

    [Serializable]
    public class es_ut_rpm
    {
        /// <summary>
        /// 由itemname+id构成
        /// </summary>
        public string ID { get; set; }
        public string PK { get; set; }
        public string CUSTOMID { get; set; }
        public string ITEMNAME { get; set; }
        /// <summary>
        /// 平台自用项目编码
        /// </summary>
        public string ITEMCUSTOMNAME { get; set; }
        public string SYSPRIMARYKEY { get; set; }
        public string REPORTNUM { get; set; }
        public string C_PAIHAO { get; set; }
        public string C_GRADE { get; set; }
        public string SAMPLENAME { get; set; }
        public string DJ { get; set; }
        public string SZ_VARIETY { get; set; }
        public string SPEC { get; set; }
        public string SAMPLETYPE { get; set; }
        public string GRADE { get; set; }
        public string BIAOHAO { get; set; }
        public string ENTRYNO { get; set; }
        public string PACKAGE { get; set; }
        public string PRODUCEFACTORY { get; set; }
        public string GHDW { get; set; }
        public string CD { get; set; }
        public string HGZH { get; set; }
        public string JYPC { get; set; }
        public DateTime? PRODUCEDATE { get; set; }
        public DateTime? ADDTIME { get; set; }
        public float? ENTRYAMOUNT { get; set; }
        public string RECORDMAN { get; set; }
        public float? MZ { get; set; }
        public float? PZ { get; set; }
        public float? JZ { get; set; }
        public float? LJJZ { get; set; }
        public float? KS { get; set; }
        public string SAMPLENUM { get; set; }
        public string CONCLUSIONCODE { get; set; }
        public DateTime? ENTRYDATE { get; set; }
        public float? JCCOUNT { get; set; }
        public DateTime? UPLOADTIME { get; set; }
        public DateTime? UPDATETIME { get; set; }

        public string STATUS { get; set; }
        public string CUSTOMNAME { get; set; }
        public string SYJG { get; set; }
        public string UNIT { get; set; }
        public string ADDMAN { get; set; }

    }

    [Serializable]
    public class es_t_hnt_data
    {
        public string PK { get; set; }
        public string CUSTOMID { get; set; }
        public int? F_ID { get; set; }
        public DateTime? DATE { get; set; }
        public int? F_HTDID { get; set; }
        public int? F_SBBH { get; set; }
        public string WORKSTATIONID { get; set; }
        public string F_WORKSTATIONID { get; set; }
        public string F_WORKSTATIONNAME { get; set; }
        public string F_SCXBH { get; set; }
        public string F_GCMC { get; set; }
        public string F_GCBW { get; set; }
        public string F_CPH { get; set; }
        public string F_PBBH { get; set; }
        public string F_QDDJ { get; set; }
        public string F_KSDJ { get; set; }
        public string F_TLD { get; set; }
        public float? F_FL { get; set; }
        public string F_CHECI { get; set; }
        public DateTime? F_CLSJ { get; set; }
        public DateTime? F_SCSJ { get; set; }
        public string F_GONGCHENGMINGCHENG { get; set; }
        public string F_GONGCHENGBUWEI { get; set; }
        public float? F_SHEJI_SN { get; set; }
        public float? F_SHEJI_LQ { get; set; }
        public float? F_SHEJI_SHUI { get; set; }
        public float? F_SHEJI_GL1 { get; set; }
        public float? F_SHEJI_GL2 { get; set; }
        public float? F_SHEJI_GL3 { get; set; }
        public float? F_SHEJI_GL4 { get; set; }
        public float? F_SHEJI_GL5 { get; set; }
        public float? F_SHEJI_GL6 { get; set; }
        public float? F_SHEJI_FMH { get; set; }
        public float? F_SHEJI_KF { get; set; }
        public float? F_SHEJI_WJJ { get; set; }
        public float? F_SHEJI_WJJ3 { get; set; }
        public float? F_SHEJI_FL1 { get; set; }
        public float? F_SHEJI_FL2 { get; set; }
        public float? F_SHEJI_FL3 { get; set; }
        /// <summary>
        /// 实际水泥
        /// </summary>
        public float? F_SHIJI_SN { get; set; }
        public float? F_SHIJI_LQ { get; set; }
        /// <summary>
        /// 实际水
        /// </summary>
        public float? F_SHIJI_SHUI { get; set; }
        /// <summary>
        /// 实际砂1
        /// </summary>
        public float? F_SHIJI_GL1 { get; set; }
        /// <summary>
        /// 实际砂2
        /// </summary>
        public float? F_SHIJI_GL2 { get; set; }
        /// <summary>
        /// 实际砂3
        /// </summary>
        public float? F_SHIJI_GL3 { get; set; }
        /// <summary>
        /// 实际石1
        /// </summary>
        public float? F_SHIJI_GL4 { get; set; }
        /// <summary>
        /// 实际石2
        /// </summary>
        public float? F_SHIJI_GL5 { get; set; }
        /// <summary>
        /// 实际石3
        /// </summary>
        public float? F_SHIJI_GL6 { get; set; }
        /// <summary>
        /// 实际粉煤灰
        /// </summary>
        public float? F_SHIJI_FMH { get; set; }
        /// <summary>
        /// 实际矿粉
        /// </summary>
        public float? F_SHIJI_KF { get; set; }
        /// <summary>
        /// 实际外加剂1
        /// </summary>
        public float? F_SHIJI_WJJ { get; set; }
        /// <summary>
        /// 实际膨胀剂
        /// </summary>
        public float? F_SHIJI_WJJ3 { get; set; }
        /// <summary>
        /// 实际粉料1
        /// </summary>
        public float? F_SHIJI_FL1 { get; set; }
        /// <summary>
        /// 实际粉料2
        /// </summary>
        public float? F_SHIJI_FL2 { get; set; }
        /// <summary>
        /// 实际石灰石粉
        /// </summary>
        public float? F_SHIJI_FL3 { get; set; }
        public string F_CLWD { get; set; }
        public string F_WENDU1 { get; set; }
        public string F_WENDU2 { get; set; }
        public string F_WENDU3 { get; set; }
        public DateTime? F_ADDTIME { get; set; }
        public float? F_SHIJI_RZCL { get; set; }
        public float? F_SHIJI_LJZCL { get; set; }
        public int F_BHGTPSFSC { get; set; }
        public string F_BHGTPURL { get; set; }
        public float? F_SHEJI_WJJ2 { get; set; }
        public string F_RWBH { get; set; }
        public string F_KHMC { get; set; }
        public string F_JZFS { get; set; }
        public string F_CHEBH { get; set; }
        public float? F_SHIJI_WJJ2 { get; set; }
        public string F_SGDW { get; set; }
        public DateTime? UPLOADTIME { get; set; }
        public DateTime? UPDATETIME { get; set; }

        public string F_JBSJ { get; set; }

        #region 单方配比
        public float? F_SHEJI_SN_DFPB { get; set; }
        public float? F_SHEJI_LQ_DFPB { get; set; }
        public float? F_SHEJI_SHUI_DFPB { get; set; }
        public float? F_SHEJI_GL1_DFPB { get; set; }
        public float? F_SHEJI_GL2_DFPB { get; set; }
        public float? F_SHEJI_GL3_DFPB { get; set; }
        public float? F_SHEJI_GL4_DFPB { get; set; }
        public float? F_SHEJI_GL5_DFPB { get; set; }
        public float? F_SHEJI_GL6_DFPB { get; set; }
        public float? F_SHEJI_FMH_DFPB { get; set; }
        public float? F_SHEJI_KF_DFPB { get; set; }
        public float? F_SHEJI_WJJ_DFPB { get; set; }
        public float? F_SHEJI_WJJ2_DFPB { get; set; }
        public float? F_SHEJI_WJJ3_DFPB { get; set; }
        public float? F_SHEJI_FL1_DFPB { get; set; }
        public float? F_SHEJI_FL2_DFPB { get; set; }
        public float? F_SHEJI_FL3_DFPB { get; set; }
        #endregion
        #region 偏差
        public float? F_SHIJI_SN_PC { get; set; }
        public float? F_SHIJI_LQ_PC { get; set; }
        public float? F_SHIJI_SHUI_PC { get; set; }
        public float? F_SHIJI_GL1_PC { get; set; }
        public float? F_SHIJI_GL2_PC { get; set; }
        public float? F_SHIJI_GL3_PC { get; set; }
        public float? F_SHIJI_GL4_PC { get; set; }
        public float? F_SHIJI_GL5_PC { get; set; }
        public float? F_SHIJI_GL6_PC { get; set; }
        public float? F_SHIJI_FMH_PC { get; set; }
        public float? F_SHIJI_KF_PC { get; set; }
        public float? F_SHIJI_WJJ_PC { get; set; }
        public float? F_SHIJI_WJJ2_PC { get; set; }
        public float? F_SHIJI_WJJ3_PC { get; set; }
        public float? F_SHIJI_FL1_PC { get; set; }
        public float? F_SHIJI_FL2_PC { get; set; }
        public float? F_SHIJI_FL3_PC { get; set; }
        #endregion
    }

    [Serializable]
    public class es_hnks_cur
    {
        /// <summary>
        /// 企业ID
        /// </summary>
        public string PK { get; set; }
        public string CUSTOMID { get; set; }
        public string SYSPRIMARYKEY { get; set; }
        /// <summary>
        /// 工程编号
        /// </summary>
        public string PROJECTNUM { get; set; }
        public string SENDSAMPLEMAN { get; set; }
        public string SENDSAMPLEMANTEL { get; set; }
        public string SAMPLENUM { get; set; }
        public string ENTRUSTNUM { get; set; }
        public string REPORTNUM { get; set; }
        public string SAMPLENAME { get; set; }
        public string STANDARDNAME { get; set; }
        public string CHECKTYPE { get; set; }
        public string STRUCTPART { get; set; }
        public string DEPUTYBATCH { get; set; }
        public string SAMPLEAMOUNT { get; set; }
        public string SAMPLESTATUS { get; set; }
        public string SAMPLEDISPOSEMODE { get; set; }
        public string SAMPLEDISPOSEPHASE { get; set; }
        public string SAMPLECHARGETYPE { get; set; }
        public string PRODUCEFACTORY { get; set; }
        public DateTime? PRODUCEDATE { get; set; }
        public DateTime? REPORTCONSENTDATE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? ENTRUSTDATE { get; set; }
        public DateTime? CHECKDATE { get; set; }
        public DateTime? AUDITINGDATE { get; set; }
        public DateTime? APPROVEDATE { get; set; }
        public DateTime? PRINTDATE { get; set; }
        public DateTime? UPDATETIME { get; set; }
        public string CHECKCONCLUSION { get; set; }
        /// <summary>
        /// 检测结论
        /// </summary>
        public string CONCLUSIONCODE { get; set; }
        public string ACCEPTSAMPLEMAN { get; set; }
        public string FIRSTCHECKMAN { get; set; }
        public string SECONDCHECKMAN { get; set; }
        public string VERIFYMAN { get; set; }
        public string AUDITINGMAN { get; set; }
        public string APPROVEMAN { get; set; }
        public string PRINTMAN { get; set; }
        public string EXTENDMAN { get; set; }
        public string INSTRUMENTNUM { get; set; }
        public string INSTRUMENTNAME { get; set; }
        public string CHECKENVIRONMENT { get; set; }
        public string EXPLAIN { get; set; }
        public int? REPORTCONSENTDAYS { get; set; }
        public string WORKSTATION { get; set; }
        public string AUTODEFINENOTE { get; set; }
        public DateTime? CHXDATE { get; set; }
        /// <summary>
        /// 检测龄期
        /// </summary>
        public int? LINGQI { get; set; }
        public float? KSBH { get; set; }

        /// <summary>
        /// 设计抗渗等级
        /// </summary>
        public float? SJKSDJ { get; set; }

        /// <summary>
        /// 设计强度等级
        /// </summary>
        public float? SJQDDJ { get; set; }

        public float? PDKSDJ { get; set; }
        public string SNSCCJ { get; set; }
        public string SNPH { get; set; }
        public string SNDJ { get; set; }
        public string SHACHD { get; set; }
        public string SHAXDML { get; set; }
        public string SHIPZ { get; set; }
        public string SHILIJING { get; set; }
        public string HHCPZ { get; set; }
        public string HHCCL { get; set; }
        public string JSJPZ { get; set; }
        public string JSJCL { get; set; }
        public DateTime? DATE1 { get; set; }
        public DateTime? DATE2 { get; set; }
        public DateTime? DATE3 { get; set; }
        public DateTime? DATE4 { get; set; }
        public DateTime? DATE5 { get; set; }
        public DateTime? DATE6 { get; set; }
        public DateTime? DATE7 { get; set; }
        public DateTime? DATE8 { get; set; }
        public DateTime? DATE9 { get; set; }
        public DateTime? DATE10 { get; set; }
        public DateTime? DATE11 { get; set; }
        public DateTime? DATE12 { get; set; }
        public DateTime? DATE13 { get; set; }
        public DateTime? DATE14 { get; set; }
        public DateTime? DATE15 { get; set; }
        public DateTime? DATE16 { get; set; }
        public DateTime? DATE17 { get; set; }
        public DateTime? DATE18 { get; set; }
        public DateTime? DATE19 { get; set; }
        public DateTime? DATE20 { get; set; }
        public string TIME1 { get; set; }
        public string TIME2 { get; set; }
        public string TIME3 { get; set; }
        public string TIME4 { get; set; }
        public string TIME5 { get; set; }
        public string TIME6 { get; set; }
        public string TIME7 { get; set; }
        public string TIME8 { get; set; }
        public string TIME9 { get; set; }
        public string TIME10 { get; set; }
        public string TIME11 { get; set; }
        public string TIME12 { get; set; }
        public string TIME13 { get; set; }
        public string TIME14 { get; set; }
        public string TIME15 { get; set; }
        public string TIME16 { get; set; }
        public string TIME17 { get; set; }
        public string TIME18 { get; set; }
        public string TIME19 { get; set; }
        public string TIME20 { get; set; }
        public float? MPA1 { get; set; }
        public float? MPA2 { get; set; }
        public float? MPA3 { get; set; }
        public float? MPA4 { get; set; }
        public float? MPA5 { get; set; }
        public float? MPA6 { get; set; }
        public float? MPA7 { get; set; }
        public float? MPA8 { get; set; }
        public float? MPA9 { get; set; }
        public float? MPA10 { get; set; }
        public float? MPA11 { get; set; }
        public float? MPA12 { get; set; }
        public float? MPA13 { get; set; }
        public float? MPA14 { get; set; }
        public float? MPA15 { get; set; }
        public float? MPA16 { get; set; }
        public float? MPA17 { get; set; }
        public float? MPA18 { get; set; }
        public float? MPA19 { get; set; }
        public float? MPA20 { get; set; }
        public string S111 { get; set; }
        public string S112 { get; set; }
        public string S113 { get; set; }
        public string S114 { get; set; }
        public string S115 { get; set; }
        public string S116 { get; set; }
        public string S117 { get; set; }
        public string S118 { get; set; }
        public string S119 { get; set; }
        public string S120 { get; set; }
        public string S121 { get; set; }
        public string S122 { get; set; }
        public string S123 { get; set; }
        public string S124 { get; set; }
        public string S125 { get; set; }
        public string S126 { get; set; }
        public string S127 { get; set; }
        public string S128 { get; set; }
        public string S129 { get; set; }
        public string S130 { get; set; }
        public string S211 { get; set; }
        public string S212 { get; set; }
        public string S213 { get; set; }
        public string S214 { get; set; }
        public string S215 { get; set; }
        public string S216 { get; set; }
        public string S217 { get; set; }
        public string S218 { get; set; }
        public string S219 { get; set; }
        public string S220 { get; set; }
        public string S221 { get; set; }
        public string S222 { get; set; }
        public string S223 { get; set; }
        public string S224 { get; set; }
        public string S225 { get; set; }
        public string S226 { get; set; }
        public string S227 { get; set; }
        public string S228 { get; set; }
        public string S229 { get; set; }
        public string S230 { get; set; }
        public string S311 { get; set; }
        public string S312 { get; set; }
        public string S313 { get; set; }
        public string S314 { get; set; }
        public string S315 { get; set; }
        public string S316 { get; set; }
        public string S317 { get; set; }
        public string S318 { get; set; }
        public string S319 { get; set; }
        public string S320 { get; set; }
        public string S321 { get; set; }
        public string S322 { get; set; }
        public string S323 { get; set; }
        public string S324 { get; set; }
        public string S325 { get; set; }
        public string S326 { get; set; }
        public string S327 { get; set; }
        public string S328 { get; set; }
        public string S329 { get; set; }
        public string S330 { get; set; }
        public string S411 { get; set; }
        public string S412 { get; set; }
        public string S413 { get; set; }
        public string S414 { get; set; }
        public string S415 { get; set; }
        public string S416 { get; set; }
        public string S417 { get; set; }
        public string S418 { get; set; }
        public string S419 { get; set; }
        public string S420 { get; set; }
        public string S421 { get; set; }
        public string S422 { get; set; }
        public string S423 { get; set; }
        public string S424 { get; set; }
        public string S425 { get; set; }
        public string S426 { get; set; }
        public string S427 { get; set; }
        public string S428 { get; set; }
        public string S429 { get; set; }
        public string S430 { get; set; }
        public string S511 { get; set; }
        public string S512 { get; set; }
        public string S513 { get; set; }
        public string S514 { get; set; }
        public string S515 { get; set; }
        public string S516 { get; set; }
        public string S517 { get; set; }
        public string S518 { get; set; }
        public string S519 { get; set; }
        public string S520 { get; set; }
        public string S521 { get; set; }
        public string S522 { get; set; }
        public string S523 { get; set; }
        public string S524 { get; set; }
        public string S525 { get; set; }
        public string S526 { get; set; }
        public string S527 { get; set; }
        public string S528 { get; set; }
        public string S529 { get; set; }
        public string S530 { get; set; }
        public string S611 { get; set; }
        public string S612 { get; set; }
        public string S613 { get; set; }
        public string S614 { get; set; }
        public string S615 { get; set; }
        public string S616 { get; set; }
        public string S617 { get; set; }
        public string S618 { get; set; }
        public string S619 { get; set; }
        public string S620 { get; set; }
        public string S621 { get; set; }
        public string S622 { get; set; }
        public string S623 { get; set; }
        public string S624 { get; set; }
        public string S625 { get; set; }
        public string S626 { get; set; }
        public string S627 { get; set; }
        public string S628 { get; set; }
        public string S629 { get; set; }
        public string S630 { get; set; }
        public float? GAO1 { get; set; }
        public float? GAO2 { get; set; }
        public float? GAO3 { get; set; }
        public float? GAO4 { get; set; }
        public float? GAO5 { get; set; }
        public float? GAO6 { get; set; }
        public string SSSY1 { get; set; }
        public string SSSY2 { get; set; }
        public string SSSY3 { get; set; }
        public string SSSY4 { get; set; }
        public string SSSY5 { get; set; }
        public string SSSY6 { get; set; }
        public float? SSSY { get; set; }
        public float? MAXKN { get; set; }
        public float? GAOMIN { get; set; }
        public float? GAOMAX { get; set; }
        public float? GAOAVG { get; set; }
        public string CHECKITEM1 { get; set; }
        public float? NEEDCHARGEMONEY { get; set; }
        public string COLLATEMAN { get; set; }
        public DateTime? COLLATEDATE { get; set; }
        public DateTime? VERIFYDATE { get; set; }
        public DateTime? EXTENDDATE { get; set; }
        public string BEFORESTATUS { get; set; }
        public string AFTERSTATUS { get; set; }
        public string TEMPERATURE { get; set; }
        public string HUMIDITY { get; set; }
        public string PDSTANDARDNAME { get; set; }
        public string BACKUPCOLUMN1 { get; set; }
        public string BACKUPCOLUMN2 { get; set; }
        public string BACKUPCOLUMN3 { get; set; }
        public string BACKUPCOLUMN4 { get; set; }
        public string BACKUPCOLUMN5 { get; set; }
        public string BACKUPCOLUMN6 { get; set; }
        public string BACKUPCOLUMN7 { get; set; }
        public string BACKUPCOLUMN8 { get; set; }
        public string BACKUPCOLUMN9 { get; set; }
        public string BACKUPCOLUMN10 { get; set; }
        public string BACKUPCOLUMN11 { get; set; }
        public string BACKUPCOLUMN12 { get; set; }
        public string BACKUPCOLUMN13 { get; set; }
        public string BACKUPCOLUMN14 { get; set; }
        public string BACKUPCOLUMN15 { get; set; }
        public string BACKUPCOLUMN16 { get; set; }
        public string BACKUPCOLUMN17 { get; set; }
        public string BACKUPCOLUMN18 { get; set; }
        public string BACKUPCOLUMN19 { get; set; }
        public string BACKUPCOLUMN20 { get; set; }
        public string BACKUPCOLUMN21 { get; set; }
        public string BACKUPCOLUMN22 { get; set; }
        public string BACKUPCOLUMN23 { get; set; }
        public string BACKUPCOLUMN24 { get; set; }
        public string BACKUPCOLUMN25 { get; set; }
        public string BACKUPCOLUMN26 { get; set; }
        public string BACKUPCOLUMN27 { get; set; }
        public string BACKUPCOLUMN28 { get; set; }
        public string BACKUPCOLUMN29 { get; set; }
        public string BACKUPCOLUMN30 { get; set; }
        public string WITNESSUNIT { get; set; }
        public string WITNESSMAN { get; set; }
        public string WITNESSMANNUM { get; set; }
        public string WITNESSMANTEL { get; set; }
        public string SUPERMAN { get; set; }
        public string SUPERUNIT { get; set; }
        public string TAKESAMPLEUNIT { get; set; }
        public string TAKESAMPLEMANNUM { get; set; }
        public string TAKESAMPLEMAN { get; set; }
        public string TAKESAMPLEMANTEL { get; set; }
        public string INSPECTMAN { get; set; }
        public string VIEWMAN1 { get; set; }
        public string VIEWMAN2 { get; set; }
        public string VIEWMAN3 { get; set; }
        public string VIEWMAN4 { get; set; }
        public string VIEWMAN5 { get; set; }
        public string VIEWMAN6 { get; set; }
        public string VIEWMAN7 { get; set; }
        public string VIEWMAN8 { get; set; }
        public string VIEWMAN9 { get; set; }
        public string VIEWMAN10 { get; set; }
        public string VIEWMAN11 { get; set; }
        public string VIEWMAN12 { get; set; }
        public string VIEWMAN13 { get; set; }
        public string VIEWMAN14 { get; set; }
        public string VIEWMAN15 { get; set; }
        public string VIEWMAN16 { get; set; }
        public string VIEWMAN17 { get; set; }
        public string VIEWMAN18 { get; set; }
        public string VIEWMAN19 { get; set; }
        public string VIEWMAN20 { get; set; }
        public string CONSTRACTIONUNIT { get; set; }
        public DateTime? FACTCHECKDATE { get; set; }
        public string PROJECTNAME { get; set; }
        public string REPORTSTYLESTYPES { get; set; }
        public string SENDTOWEB { get; set; }
        public string CONSTRACTUNIT { get; set; }
        public string SSJG1 { get; set; }
        public string SSJG2 { get; set; }
        public string SSJG3 { get; set; }
        public string SSJG4 { get; set; }
        public string SSJG5 { get; set; }
        public string SSJG6 { get; set; }
        public string IMPORTDATANUM { get; set; }
        public string ENTRUSTUNIT { get; set; }
        /// <summary>
        /// 试件尺寸
        /// </summary>
        public string SPEC { get; set; }
        public string SSXS { get; set; }
        public string SJYBH { get; set; }
        public string YANGHUTIAOJIAN { get; set; }
        public float? YQLQ { get; set; }
        public string LYTBAZH { get; set; }
        public string TASKNUM { get; set; }
        public string CODEBAR { get; set; }
        public string SIZE { get; set; }
        /// <summary>
        /// 是否已处理 1已处理
        /// </summary>
        public int? ISDEAL { get; set; }
        /// <summary>
        /// 处理过程
        /// </summary>
        public string DEALPROCESS { get; set; }
    }


    [Serializable]
    public class es_hnky_cur
    {
        public string SYSPRIMARYKEY { get; set; }
        public string PROJECTNUM { get; set; }
        public string SENDSAMPLEMAN { get; set; }
        public string SENDSAMPLEMANTEL { get; set; }
        public string CUSTOMID { get; set; }
        public string SAMPLENUM { get; set; }
        public string ENTRUSTNUM { get; set; }
        public string REPORTNUM { get; set; }
        public string SAMPLENAME { get; set; }
        public string STANDARDNAME { get; set; }
        public string CHECKTYPE { get; set; }
        public string STRUCTPART { get; set; }
        public string DEPUTYBATCH { get; set; }
        public string SAMPLEAMOUNT { get; set; }
        public string SAMPLESTATUS { get; set; }
        public string SAMPLEDISPOSEMODE { get; set; }
        public string SAMPLEDISPOSEPHASE { get; set; }
        public string SAMPLECHARGETYPE { get; set; }
        public string PRODUCEFACTORY { get; set; }
        public DateTime? PRODUCEDATE { get; set; }
        public DateTime? REPORTCONSENTDATE { get; set; }
        public DateTime? ENTRUSTDATE { get; set; }
        public DateTime? CHECKDATE { get; set; }
        public DateTime? AUDITINGDATE { get; set; }
        public DateTime? APPROVEDATE { get; set; }
        public DateTime? PRINTDATE { get; set; }
        public DateTime? UPLOADTIME { get; set; }
        public string CHECKCONCLUSION { get; set; }
        public string CONCLUSIONCODE { get; set; }
        public string ACCEPTSAMPLEMAN { get; set; }
        public string FIRSTCHECKMAN { get; set; }
        public string SECONDCHECKMAN { get; set; }
        public string VERIFYMAN { get; set; }
        public string AUDITINGMAN { get; set; }
        public string APPROVEMAN { get; set; }
        public string PRINTMAN { get; set; }
        public string EXTENDMAN { get; set; }
        public string INSTRUMENTNUM { get; set; }
        public string INSTRUMENTNAME { get; set; }
        public string CHECKENVIRONMENT { get; set; }
        public string EXPLAIN { get; set; }
        public int? REPORTCONSENTDAYS { get; set; }
        public string WORKSTATION { get; set; }
        public string CHECKITEM1 { get; set; }
        public string SHEJIDENGJI { get; set; }
        public string XINGZHUANG { get; set; }
        public string CHICUN { get; set; }
        public string YANGHUTIAOJIAN { get; set; }
        public float? YANGHUXISHU { get; set; }
        public DateTime? CHENGXINGRIQI { get; set; }
        public int? LINQI { get; set; }
        public string CHENGXINGFANGFA { get; set; }
        public string JIANYANPIMINGCHEN { get; set; }
        public int? CHANG1 { get; set; }
        public int? CHANG2 { get; set; }
        public int? CHANG3 { get; set; }
        public int? KUAN1 { get; set; }
        public int? KUAN2 { get; set; }
        public int? KUAN3 { get; set; }
        public int? GAO1 { get; set; }
        public int? GAO2 { get; set; }
        public int? GAO3 { get; set; }
        public float? CHICUNXISHU { get; set; }
        public float? JIXIANGHEZAI1 { get; set; }
        public float? JIXIANGHEZAI2 { get; set; }
        public float? JIXIANGHEZAI3 { get; set; }
        public float? QIANGDU3 { get; set; }
        public float? QIANGDU2 { get; set; }
        public float? QIANGDU1 { get; set; }
        public string QIANGDUDAIBIAOZHI { get; set; }
        public float? BAIFENGBI { get; set; }
        public float? NEEDCHARGEMONEY { get; set; }
        public string COLLATEMAN { get; set; }
        public DateTime? COLLATEDATE { get; set; }
        public DateTime? VERIFYDATE { get; set; }
        public DateTime? EXTENDDATE { get; set; }
        public string BEFORESTATUS { get; set; }
        public string AFTERSTATUS { get; set; }
        public string TEMPERATURE { get; set; }
        public string HUMIDITY { get; set; }
        public string PDSTANDARDNAME { get; set; }
        public string BACKUPCOLUMN1 { get; set; }
        public string BACKUPCOLUMN2 { get; set; }
        public string BACKUPCOLUMN3 { get; set; }
        public string BACKUPCOLUMN4 { get; set; }
        public string BACKUPCOLUMN5 { get; set; }
        public string BACKUPCOLUMN6 { get; set; }
        public string BACKUPCOLUMN7 { get; set; }
        public string BACKUPCOLUMN8 { get; set; }
        public string BACKUPCOLUMN9 { get; set; }
        public string BACKUPCOLUMN10 { get; set; }
        public string BACKUPCOLUMN11 { get; set; }
        public string BACKUPCOLUMN12 { get; set; }
        public string BACKUPCOLUMN13 { get; set; }
        public string BACKUPCOLUMN14 { get; set; }
        public string BACKUPCOLUMN15 { get; set; }
        public string BACKUPCOLUMN16 { get; set; }
        public string BACKUPCOLUMN17 { get; set; }
        public string BACKUPCOLUMN18 { get; set; }
        public string BACKUPCOLUMN19 { get; set; }
        public string BACKUPCOLUMN20 { get; set; }
        public string BACKUPCOLUMN21 { get; set; }
        public string BACKUPCOLUMN22 { get; set; }
        public string BACKUPCOLUMN23 { get; set; }
        public string BACKUPCOLUMN24 { get; set; }
        public string BACKUPCOLUMN25 { get; set; }
        public string BACKUPCOLUMN26 { get; set; }
        public string BACKUPCOLUMN27 { get; set; }
        public string BACKUPCOLUMN28 { get; set; }
        public string BACKUPCOLUMN29 { get; set; }
        public string BACKUPCOLUMN30 { get; set; }
        public string WITNESSUNIT { get; set; }
        public string WITNESSMAN { get; set; }
        public string WITNESSMANNUM { get; set; }
        public string WITNESSMANTEL { get; set; }
        public string SUPERMAN { get; set; }
        public string SUPERUNIT { get; set; }
        public string TAKESAMPLEUNIT { get; set; }
        public string TAKESAMPLEMANNUM { get; set; }
        public string TAKESAMPLEMAN { get; set; }
        public string TAKESAMPLEMANTEL { get; set; }
        public string INSPECTMAN { get; set; }
        public string CONSTRACTIONUNIT { get; set; }
        public DateTime? FACTCHECKDATE { get; set; }
        public string PROJECTNAME { get; set; }
        public string REPORTSTYLESTYPES { get; set; }
        public string SENDTOWEB { get; set; }
        public string CONSTRACTUNIT { get; set; }
        public string QIANGDUZHI { get; set; }
        public int? YQLQ { get; set; }
        public string IMPORTDATANUM { get; set; }
        public string ENTRUSTUNIT { get; set; }
        public string LJWD { get; set; }
        public string QIANGDUGUOCHENGZHI { get; set; }
        public float? SHEJIQIANGDU { get; set; }
        public string LAYERCODE { get; set; }
        public int? DATATYPE { get; set; }
        public string TASKNUM { get; set; }
        public string LYTBAZH { get; set; }
        public float? QIANGDUAVG { get; set; }
        public string C_CVARIETY { get; set; }
        public string C_CDJ { get; set; }
        public string C_SORT { get; set; }
        public string MT_PINZ { get; set; }
        public string MTT_PINZ { get; set; }
        public string WJ_PINZ1 { get; set; }
        public string WJ_PINZ2 { get; set; }
        public string WJ_PINZ3 { get; set; }
        public float? SP_CEMT { get; set; }
        public float? SP_WATER { get; set; }
        public float? SP_SZI1 { get; set; }
        public float? SP_SZI2 { get; set; }
        public float? SP_SII1 { get; set; }
        public float? SP_SII2 { get; set; }
        public float? SP_MT { get; set; }
        public float? SP_MTT { get; set; }
        public float? SP_WJ1 { get; set; }
        public float? SP_WJ2 { get; set; }
        public float? SP_QT { get; set; }
        public float? SP_SHB { get; set; }
        public float? SPJ_WATER { get; set; }
        public float? SPJ_SZI1 { get; set; }
        public float? SPJ_SZI2 { get; set; }
        public float? SPJ_SII1 { get; set; }
        public float? SPJ_SII2 { get; set; }
        public float? SPJ_MT { get; set; }
        public float? SPJ_MTT { get; set; }
        public float? SPJ_WJ1 { get; set; }
        public float? SPJ_WJ2 { get; set; }
        public float? SPJ_QT { get; set; }
        public float? SP_WJ3 { get; set; }
        public float? SPJ_WJ3 { get; set; }
        public string PK { get; set; }
        /// <summary>
        /// 是否已处理 1已处理
        /// </summary>
        public int? ISDEAL { get; set; }
        /// <summary>
        /// 处理过程
        /// </summary>
        public string DEALPROCESS { get; set; }
    }


    [Serializable]
    public class es_hnkz_cur
    {
        public string SYSPRIMARYKEY { get; set; }
        public string PROJECTNUM { get; set; }
        public string SENDSAMPLEMAN { get; set; }
        public string SENDSAMPLEMANTEL { get; set; }
        public string SAMPLENUM { get; set; }
        public string ENTRUSTNUM { get; set; }
        public string REPORTNUM { get; set; }
        public string SAMPLENAME { get; set; }
        public string STANDARDNAME { get; set; }
        public string CHECKTYPE { get; set; }
        public string STRUCTPART { get; set; }
        public string DEPUTYBATCH { get; set; }
        public string SAMPLEAMOUNT { get; set; }
        public string SAMPLESTATUS { get; set; }
        public string SAMPLEDISPOSEMODE { get; set; }
        public string SAMPLEDISPOSEPHASE { get; set; }
        public string SAMPLECHARGETYPE { get; set; }
        public string PRODUCEFACTORY { get; set; }
        public DateTime? PRODUCEDATE { get; set; }
        public DateTime? REPORTCONSENTDATE { get; set; }
        public DateTime? ENTRUSTDATE { get; set; }
        public DateTime? CHECKDATE { get; set; }
        public DateTime? AUDITINGDATE { get; set; }
        public DateTime? APPROVEDATE { get; set; }
        public DateTime? PRINTDATE { get; set; }

        public DateTime? timestamp { get; set; }

        public string CHECKCONCLUSION { get; set; }
        public string CONCLUSIONCODE { get; set; }
        public string ACCEPTSAMPLEMAN { get; set; }
        public string FIRSTCHECKMAN { get; set; }
        public string SECONDCHECKMAN { get; set; }
        public string VERIFYMAN { get; set; }
        public string AUDITINGMAN { get; set; }
        public string APPROVEMAN { get; set; }
        public string PRINTMAN { get; set; }
        public string EXTENDMAN { get; set; }
        public string INSTRUMENTNUM { get; set; }
        public string INSTRUMENTNAME { get; set; }
        public string CHECKENVIRONMENT { get; set; }
        public string EXPLAIN { get; set; }
        public int? REPORTCONSENTDAYS { get; set; }
        public string WORKSTATION { get; set; }
        public string AUTODEFINENOTE { get; set; }
        public float? SHEJIDENGJI { get; set; }
        public string SIZE { get; set; }
        public string SHUINIPINZHONG { get; set; }
        public string SHUINIDENGJI { get; set; }
        public string SHUINICHANGJIA { get; set; }
        public string YANGHUTIAOJIAN { get; set; }
        public DateTime? CHENGXINGDATE { get; set; }
        public int? LINGQI { get; set; }
        public string CHENGXINGFANGFA { get; set; }
        public float? YANGHUXISHU { get; set; }
        public float? CHICUNXISHU { get; set; }
        public int? CHANG1 { get; set; }
        public int? CHANG2 { get; set; }
        public int? CHANG3 { get; set; }
        public int? KUAN1 { get; set; }
        public int? KUAN2 { get; set; }
        public int? KUAN3 { get; set; }
        public int? GAO1 { get; set; }
        public int? GAO2 { get; set; }
        public int? GAO3 { get; set; }
        public float? JIXIANHEZAI1 { get; set; }
        public float? JIXIANHEZAI2 { get; set; }
        public float? JIXIANHEZAI3 { get; set; }
        public float? QIANGDU1 { get; set; }
        public float? QIANGDU2 { get; set; }
        public float? QIANGDU3 { get; set; }
        public string DUANLIEWEIZHI1 { get; set; }
        public string DUANLIEWEIZHI2 { get; set; }
        public string DUANLIEWEIZHI3 { get; set; }
        public float? QIANGDUBAIFENBI { get; set; }
        public string QIANGDUDAIBIAOZHI { get; set; }
        public float? KUAJU1 { get; set; }
        public float? KUAJU2 { get; set; }
        public float? KUAJU3 { get; set; }
        public float? DUANHOUJULI1 { get; set; }
        public float? DUANHOUJULI2 { get; set; }
        public float? DUANHOUJULI3 { get; set; }
        public string CHECKITEM1 { get; set; }
        public float? NEEDCHARGEMONEY { get; set; }
        public string COLLATEMAN { get; set; }
        public DateTime? COLLATEDATE { get; set; }
        public DateTime? VERIFYDATE { get; set; }
        public DateTime? EXTENDDATE { get; set; }
        public string BEFORESTATUS { get; set; }
        public string AFTERSTATUS { get; set; }
        public string TEMPERATURE { get; set; }
        public string HUMIDITY { get; set; }
        public string PDSTANDARDNAME { get; set; }
        public string BACKUPCOLUMN1 { get; set; }
        public string BACKUPCOLUMN2 { get; set; }
        public string BACKUPCOLUMN3 { get; set; }
        public string BACKUPCOLUMN4 { get; set; }
        public string BACKUPCOLUMN5 { get; set; }
        public string BACKUPCOLUMN6 { get; set; }
        public string BACKUPCOLUMN7 { get; set; }
        public string BACKUPCOLUMN8 { get; set; }
        public string BACKUPCOLUMN9 { get; set; }
        public string BACKUPCOLUMN10 { get; set; }
        public string BACKUPCOLUMN11 { get; set; }
        public string BACKUPCOLUMN12 { get; set; }
        public string BACKUPCOLUMN13 { get; set; }
        public string BACKUPCOLUMN14 { get; set; }
        public string BACKUPCOLUMN15 { get; set; }
        public string BACKUPCOLUMN16 { get; set; }
        public string BACKUPCOLUMN17 { get; set; }
        public string BACKUPCOLUMN18 { get; set; }
        public string BACKUPCOLUMN19 { get; set; }
        public string BACKUPCOLUMN20 { get; set; }
        public string BACKUPCOLUMN21 { get; set; }
        public string BACKUPCOLUMN22 { get; set; }
        public string BACKUPCOLUMN23 { get; set; }
        public string BACKUPCOLUMN24 { get; set; }
        public string BACKUPCOLUMN25 { get; set; }
        public string BACKUPCOLUMN26 { get; set; }
        public string BACKUPCOLUMN27 { get; set; }
        public string BACKUPCOLUMN28 { get; set; }
        public string BACKUPCOLUMN29 { get; set; }
        public string BACKUPCOLUMN30 { get; set; }
        public string WITNESSUNIT { get; set; }
        public string WITNESSMAN { get; set; }
        public string WITNESSMANNUM { get; set; }
        public string WITNESSMANTEL { get; set; }
        public string SUPERMAN { get; set; }
        public string SUPERUNIT { get; set; }
        public string TAKESAMPLEUNIT { get; set; }
        public string TAKESAMPLEMANNUM { get; set; }
        public string TAKESAMPLEMAN { get; set; }
        public string TAKESAMPLEMANTEL { get; set; }
        public string INSPECTMAN { get; set; }
        public string CONSTRACTIONUNIT { get; set; }
        public DateTime? FACTCHECKDATE { get; set; }
        public string PROJECTNAME { get; set; }
        public string REPORTSTYLESTYPES { get; set; }
        public string SENDTOWEB { get; set; }
        public string CONSTRACTUNIT { get; set; }
        public int? YQLQ { get; set; }
        public string ENTRUSTUNIT { get; set; }
        public string QIANGDUGUOCHENGZHI { get; set; }
        public string SJYBH { get; set; }
        public string KYGRADE { get; set; }
        public string LYTBAZH { get; set; }
        public string TASKNUM { get; set; }
        public string CUSTOMID { get; set; }
        public string PK { get; set; }
        /// <summary>
        /// 是否已处理 1已处理
        /// </summary>
        public int? ISDEAL { get; set; }
        /// <summary>
        /// 处理过程
        /// </summary>
        public string DEALPROCESS { get; set; }
    }


    [Serializable]
    public class es_hppd_cur
    {
        public string CUSTOMID { get; set; }
        public string SYSPRIMARYKEY { get; set; }
        public string PROJECTNUM { get; set; }
        public string SENDSAMPLEMAN { get; set; }
        /// <summary>
        /// 合格证号
        /// </summary>
        public string HGZH { get; set; }
        public string SENDSAMPLEMANTEL { get; set; }
        public string SAMPLENUM { get; set; }
        public string ENTRUSTNUM { get; set; }
        public string REPORTNUM { get; set; }
        public string SAMPLENAME { get; set; }
        public string STANDARDNAME { get; set; }
        public string CHECKTYPE { get; set; }
        public string STRUCTPART { get; set; }
        public string DEPUTYBATCH { get; set; }
        public string SAMPLEAMOUNT { get; set; }
        public string SAMPLESTATUS { get; set; }
        public string SAMPLEDISPOSEMODE { get; set; }
        public string SAMPLEDISPOSEPHASE { get; set; }
        public string SAMPLECHARGETYPE { get; set; }
        public string PRODUCEFACTORY { get; set; }
        public DateTime? PRODUCEDATE { get; set; }
        public DateTime? REPORTCONSENTDATE { get; set; }
        public DateTime? ENTRUSTDATE { get; set; }
        public DateTime? CHECKDATE { get; set; }
        public DateTime? AUDITINGDATE { get; set; }
        public DateTime? APPROVEDATE { get; set; }
        public DateTime? PRINTDATE { get; set; }
        public string CHECKCONCLUSION { get; set; }
        public string CONCLUSIONCODE { get; set; }
        public string ACCEPTSAMPLEMAN { get; set; }
        public string FIRSTCHECKMAN { get; set; }
        public string SECONDCHECKMAN { get; set; }
        public string VERIFYMAN { get; set; }
        public string AUDITINGMAN { get; set; }
        public string APPROVEMAN { get; set; }
        public string PRINTMAN { get; set; }
        public string EXTENDMAN { get; set; }
        public string INSTRUMENTNUM { get; set; }
        public string INSTRUMENTNAME { get; set; }
        public string CHECKENVIRONMENT { get; set; }
        public string EXPLAIN { get; set; }
        public int? REPORTCONSENTDAYS { get; set; }
        public string WORKSTATION { get; set; }
        public string AUTODEFINENOTE { get; set; }
        public float? NEEDCHARGEMONEY { get; set; }
        public string COLLATEMAN { get; set; }
        public DateTime? COLLATEDATE { get; set; }
        public DateTime? VERIFYDATE { get; set; }
        public DateTime? EXTENDDATE { get; set; }
        public string BEFORESTATUS { get; set; }
        public string AFTERSTATUS { get; set; }
        public string TEMPERATURE { get; set; }
        public string HUMIDITY { get; set; }
        public string PDSTANDARDNAME { get; set; }
        public string BACKUPCOLUMN1 { get; set; }
        public string BACKUPCOLUMN2 { get; set; }
        public string BACKUPCOLUMN3 { get; set; }
        public string BACKUPCOLUMN4 { get; set; }
        public string BACKUPCOLUMN5 { get; set; }
        public string BACKUPCOLUMN6 { get; set; }
        public string BACKUPCOLUMN7 { get; set; }
        public string BACKUPCOLUMN8 { get; set; }
        public string BACKUPCOLUMN9 { get; set; }
        public string BACKUPCOLUMN10 { get; set; }
        public string BACKUPCOLUMN11 { get; set; }
        public string BACKUPCOLUMN12 { get; set; }
        public string BACKUPCOLUMN13 { get; set; }
        public string BACKUPCOLUMN14 { get; set; }
        public string BACKUPCOLUMN15 { get; set; }
        public string BACKUPCOLUMN16 { get; set; }
        public string BACKUPCOLUMN17 { get; set; }
        public string BACKUPCOLUMN18 { get; set; }
        public string BACKUPCOLUMN19 { get; set; }
        public string BACKUPCOLUMN20 { get; set; }
        public string BACKUPCOLUMN21 { get; set; }
        public string BACKUPCOLUMN22 { get; set; }
        public string BACKUPCOLUMN23 { get; set; }
        public string BACKUPCOLUMN24 { get; set; }
        public string BACKUPCOLUMN25 { get; set; }
        public string BACKUPCOLUMN26 { get; set; }
        public string BACKUPCOLUMN27 { get; set; }
        public string BACKUPCOLUMN28 { get; set; }
        public string BACKUPCOLUMN29 { get; set; }
        public string BACKUPCOLUMN30 { get; set; }
        public string WITNESSUNIT { get; set; }
        public string WITNESSMAN { get; set; }
        public string WITNESSMANNUM { get; set; }
        public string WITNESSMANTEL { get; set; }
        public string SUPERMAN { get; set; }
        public string SUPERUNIT { get; set; }
        public string TAKESAMPLEUNIT { get; set; }
        public string TAKESAMPLEMANNUM { get; set; }
        public string TAKESAMPLEMAN { get; set; }
        public string TAKESAMPLEMANTEL { get; set; }
        public string INSPECTMAN { get; set; }
        public string CONSTRACTIONUNIT { get; set; }
        public DateTime? FACTCHECKDATE { get; set; }
        public string PROJECTNAME { get; set; }
        public string REPORTSTYLESTYPES { get; set; }
        public string SENDTOWEB { get; set; }
        public string CONSTRACTUNIT { get; set; }
        public string ENTRUSTUNIT { get; set; }
        public string CHECKITEM1 { get; set; }
        public string CONTRACTNUM { get; set; }
        public string PURCHASERUNIT { get; set; }
        /// <summary>
        /// 供货日期
        /// </summary>
        public DateTime? PURCHASERDATE { get; set; }
        public string PHBNUM { get; set; }
        public string GRADE { get; set; }
        public string OTHERSTANDARD { get; set; }
        public string CEMTPRODUCE { get; set; }
        public string CEMTVARIETY { get; set; }
        public string CEMTSPEC { get; set; }
        public string CEMTGRDAE { get; set; }
        public string CEMTREPNUM { get; set; }
        public string CEMTSAMNUM { get; set; }
        public string SZIGPRODUCE { get; set; }
        public string SZIGVARIETY { get; set; }
        public string SZIGSPEC { get; set; }
        public string SZIGGRDAE { get; set; }
        public string SZIGREPNUM { get; set; }
        public string SZIGSAMNUM { get; set; }
        public string SIIGPRODUCE { get; set; }
        public string SIIGVARIETY { get; set; }
        public string SIIGSPEC { get; set; }
        public string SIIGGRDAE { get; set; }
        public string SIIGREPNUM { get; set; }
        public string SIIGSAMNUM { get; set; }
        public string WJJANAME { get; set; }
        public string WJJAPRODUCE { get; set; }
        public string WJJAVARIETY { get; set; }
        public string WJJASPEC { get; set; }
        public string WJJAGRDAE { get; set; }
        public string WJJAREPNUM { get; set; }
        public string WJJASAMNUM { get; set; }
        public string WJJBNAME { get; set; }
        public string WJJBPRODUCE { get; set; }
        public string WJJBVARIETY { get; set; }
        public string WJJBSPEC { get; set; }
        public string WJJBGRDAE { get; set; }
        public string WJJBREPNUM { get; set; }
        public string WJJBSAMNUM { get; set; }
        public string MTTANAME { get; set; }
        public string MTTAPRODUCE { get; set; }
        public string MTTAVARIETY { get; set; }
        public string MTTASPEC { get; set; }
        public string MTTAGRDAE { get; set; }
        public string MTTAREPNUM { get; set; }
        public string MTTASAMNUM { get; set; }
        public string MTTBNAME { get; set; }
        public string MTTBPRODUCE { get; set; }
        public string MTTBVARIETY { get; set; }
        public string MTTBSPEC { get; set; }
        public string MTTBGRDAE { get; set; }
        public string MTTBREPNUM { get; set; }
        public string MTTBSAMNUM { get; set; }
        public string PDTYPE { get; set; }
        public float? COUNT { get; set; }
        public float? BZZ { get; set; }
        public float? AVG { get; set; }
        public float? BZC { get; set; }
        public float? MIN { get; set; }
        public float? XSU { get; set; }
        public float? XSD { get; set; }
        public string NOTEPD { get; set; }
        public string TLDNUM { get; set; }
        public string TLDNOTE { get; set; }
        public string KSXGRADE { get; set; }
        public string KSXNUM { get; set; }
        public string KSXNOTE { get; set; }
        public string OTHERNOTE { get; set; }
        public string YCLNUM { get; set; }
        public string SZIHPRODUCE { get; set; }
        public string SZIHVARIETY { get; set; }
        public string SZIHSPEC { get; set; }
        public string SZIHGRDAE { get; set; }
        public string SZIHREPNUM { get; set; }
        public string SZIHSAMNUM { get; set; }
        public string SIIHPRODUCE { get; set; }
        public string SIIHVARIETY { get; set; }
        public string SIIHSPEC { get; set; }
        public string SIIHGRDAE { get; set; }
        public string SIIHREPNUM { get; set; }
        public string SIIHSAMNUM { get; set; }
        public string WATEPRODUCE { get; set; }
        public string WATESAMNUM { get; set; }
        public string NOTEPD1 { get; set; }
        public string NOTEPD2 { get; set; }
        public string PK { get; set; }
    }

    [Serializable]
    public class es_hntqdtj
    {
        public string PK { get; set; }
        public string CUSTOMID { get; set; }
        public string UNITCODE { get; set; }
        public string SYSPRIMARYKEY { get; set; }
        public DateTime? TJQSSJ { get; set; }
        public DateTime? UPDATETIME { get; set; }
        public DateTime? TJJSSJ { get; set; }
        public string QDDJ { get; set; }
        public int? TJZS { get; set; }
        public float? BZZ { get; set; }
        public float? AVG { get; set; }
        public float? BZC { get; set; }
        public float? MIN { get; set; }
        public float? XSU { get; set; }
        public float? XSD { get; set; }
        public string NOTEPD { get; set; }
        public string PDR { get; set; }
        public string PDFS { get; set; }
        public string TITLE { get; set; }
        public string SAMPLEDISPOSEMODE { get; set; }
        /// <summary>
        /// 是否已处理 1已处理
        /// </summary>
        public int? ISDEAL { get; set; }
        /// <summary>
        /// 处理过程
        /// </summary>
        public string DEALPROCESS { get; set; }
    }
    [Serializable]
    public class es_rmcr
    {
        public string CUSTOMID { get; set; }
        public string JYPC { get; set; }
        public string CONCLUSIONCODE { get; set; }
        public string PK { get; set; }
        public string REPORTNUM { get; set; }
        public string SAMPLENUM { get; set; }
        public string SYSPRIMARYKEY { get; set; }
        public DateTime? UPDATETIME { get; set; }
        public DateTime? UPLOADTIME { get; set; }
    }
}
