using Nest;
using Pkpm.Entity.ElasticSearch;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PkpmGx.ESIndexCreate
{
    class Program
    {
        static void Main(string[] args)
        {
            string esNodeUrl = ConfigurationManager.AppSettings["ESUrl"];
            var settings = new ConnectionSettings(new Uri(esNodeUrl)).DefaultIndex("gx-tbpitem").DefaultTypeNameInferrer(ft => ft.Name.ToString()).DefaultFieldNameInferrer(f => f);
            ElasticClient client = new ElasticClient(settings);


            //CreateESItemIndex(client);

            //CreatePKRIndex(client);

            //CreateAcsIndex(client);

            //CreateModifyLogIndex(client);

            //CreateQuestionIndex(client);

            //CreateESProjectIndex(client);

            CreateESTbpItemListIndex(client);

            //CreateESWordReportIndex(client);

            //CreateESExtReporttIndex(client);

            //CreateESHNKYIndex(client);

            //CreateESSJKYIndex(client);

            System.Console.WriteLine("ElasticSearch index is setup ...");

            System.Console.ReadLine();
        }


        private static void CreateESItemIndex(ElasticClient client)
        {


            var newIndex = client.CreateIndex("gx-tbpitem", index => index
                                .Settings(s => s.NumberOfShards(3).NumberOfReplicas(1))
                                 .Mappings(m => m.Map<es_t_bp_item>(mm => mm.
                                      Properties(p => p
                                          .Keyword(t => t.Name(n => n.CUSTOMID))
                                          .Keyword(t => t.Name(n => n.SYSPRIMARYKEY))
                                          .Keyword(t => t.Name(n => n.PROJECTNUM))
                                          .Keyword(t => t.Name(n => n.SENDSAMPLEMAN))
                                          .Object<string>(d => d.Name(n => n.SENDSAMPLEMANTEL).Enabled(false))
                                          .Keyword(t => t.Name(n => n.SAMPLENUM))
                                          .Keyword(t => t.Name(n => n.ENTRUSTNUM))
                                          .Keyword(t => t.Name(n => n.REPORTNUM))
                                          .Keyword(t => t.Name(n => n.REPORTJXLB))
                                          .Keyword(t => t.Name(n => n.SAMPLENAME))
                                          .Keyword(t => t.Name(n => n.STANDARDNAME))
                                          .Keyword(t => t.Name(n => n.CHECKTYPE))
                                          .Keyword(t => t.Name(n => n.STRUCTPART))
                                          .Text(t => t.Name(n => n.SAMPLEDISPOSEPHASE))
                                          .Keyword(t => t.Name(n => n.SAMPLEDISPOSEPHASEORIGIN))
                                          .Keyword(t => t.Name(n => n.SAMPLECHARGETYPE))
                                          .Keyword(t => t.Name(n => n.PRODUCEFACTORY))
                                          .Date(d => d.Name(n => n.REPORTCONSENTDATE))
                                          .Date(d => d.Name(n => n.ENTRUSTDATE))
                                          .Date(d => d.Name(n => n.CHECKDATE))
                                          .Date(d => d.Name(n => n.AUDITINGDATE))
                                          .Date(d => d.Name(n => n.APPROVEDATE))
                                          .Date(d => d.Name(n => n.PRINTDATE))
                                          .Keyword(t => t.Name(n => n.CHECKCONCLUSION))
                                          .Keyword(t => t.Name(n => n.CONCLUSIONCODE))
                                          .Keyword(t => t.Name(n => n.ACCEPTSAMPLEMAN))
                                          .Keyword(t => t.Name(n => n.FIRSTCHECKMAN))
                                          .Keyword(t => t.Name(n => n.SECONDCHECKMAN))
                                          .Keyword(t => t.Name(n => n.VERIFYMAN))
                                          .Keyword(t => t.Name(n => n.AUDITINGMAN))
                                          .Keyword(t => t.Name(n => n.APPROVEMAN))
                                          .Keyword(t => t.Name(n => n.EXTENDMAN))
                                          .Keyword(t => t.Name(n => n.INSTRUMENTNUM))
                                          .Keyword(t => t.Name(n => n.INSTRUMENTNAME))
                                          .Keyword(t => t.Name(n => n.ITEMCHNAME))
                                          .Keyword(t => t.Name(n => n.SUBITEMLIST))
                                          .Keyword(t => t.Name(n => n.UNITCODE))
                                          .Keyword(t => t.Name(n => n.EXPLAIN))
                                          .Number(t => t.Name(n => n.REPORTCONSENTDAYS).Type(NumberType.Integer))
                                          .Keyword(t => t.Name(n => n.WORKSTATION))
                                          .Number(t => t.Name(n => n.NEEDCHARGEMONEY).Type(NumberType.Float))
                                          .Number(t => t.Name(n => n.POINTCOUNT).Type(NumberType.Float))
                                          .Keyword(t => t.Name(n => n.COLLATEMAN))
                                          .Date(d => d.Name(n => n.COLLATEDATE))
                                          .Date(d => d.Name(n => n.VERIFYDATE))
                                          .Date(d => d.Name(n => n.EXTENDDATE))
                                          .Object<string>(d => d.Name(n => n.BEFORESTATUS).Enabled(false))
                                          .Object<string>(d => d.Name(n => n.AFTERSTATUS).Enabled(false))
                                          .Object<string>(d => d.Name(n => n.TEMPERATURE).Enabled(false))
                                          .Object<string>(d => d.Name(n => n.HUMIDITY).Enabled(false))
                                          .Keyword(t => t.Name(n => n.REPORTFILE))
                                          .Keyword(t => t.Name(n => n.INVALIDATE))
                                          .Keyword(t => t.Name(n => n.INVALIDATETEXT))
                                          .Keyword(t => t.Name(n => n.PDSTANDARDNAME))
                                          .Keyword(t => t.Name(n => n.TAKESAMPLEMAN))
                                          .Date(d => d.Name(n => n.FACTCHECKDATE))
                                          .Keyword(t => t.Name(n => n.CERTCHECK))
                                          .Keyword(t => t.Name(n => n.PROJECTNAME))
                                          .Keyword(t => t.Name(n => n.REPORTSTYLESTYPES))
                                          .Keyword(t => t.Name(n => n.SENDTOWEB))
                                          .Keyword(t => t.Name(n => n.CONSTRACTUNIT))
                                          .Keyword(t => t.Name(n => n.ENTRUSTUNIT))
                                          .Keyword(t => t.Name(n => n.SUPERUNIT))
                                          .Keyword(t => t.Name(n => n.ITEMNAME))
                                          .Keyword(t => t.Name(n => n.CODEBAR))
                                          .Keyword(t => t.Name(n => n.QRCODEBAR))
                                          .Keyword(t => t.Name(n => n.SUPERCODE))
                                          .Number(n => n.Name(nn => nn.HAVEACS).Type(NumberType.Integer))
                                          .Number(n => n.Name(nn => nn.HAVELOG).Type(NumberType.Integer))
                                          .Number(n => n.Name(nn => nn.ISCREPORT).Type(NumberType.Integer))
                                          .Date(t => t.Name(n => n.ADDTIME))
                                          .Date(t => t.Name(n => n.UPLOADTIME))
                                          .Date(d => d.Name(n => n.UPLOADTIME))
                                          .Date(d => d.Name(n => n.ACSTIME))
                                          .Keyword(t => t.Name(n => n.REPORMNUMWITHOUTSEQ))
                                          .Keyword(t => t.Name(n => n.CSIZE))
                                          .Keyword(t => t.Name(n => n.CUSTOMCODE))
                                          .Keyword(t => t.Name(n => n.COLUMN1))
                                          .Keyword(t => t.Name(n => n.COLUMN2))
                                          .Keyword(t => t.Name(n => n.CONTRACTNUM))
                                          .Keyword(t => t.Name(n => n.TASKNUM))
                                          .Keyword(t => t.Name(n => n.RECORDNUM))
                                          .Number(t => t.Name(n => n.REPORTTYPES).Type(NumberType.Integer))
                                          .Number(t => t.Name(n => n.JDSTATIC).Type(NumberType.Integer))
                                          .Number(n => n.Name(nn => nn.REPSEQNO).Type(NumberType.Integer))
                                          ))));


            if (newIndex.IsValid)
            {
                System.Console.WriteLine("t-bp-item done");
            }
            else
            {
                System.Console.WriteLine("error");
            }

        }

        private static void CreatePKRIndex(ElasticClient client)
        {
            var newIndex = client.CreateIndex("gx-t-pkpm-pkr", index => index
                                      .Settings(s => s.NumberOfShards(3).NumberOfReplicas(1))
                                       .Mappings(m => m
                                        .Map<es_t_pkpm_binaryReport>(mm => mm.
                                           Properties(p => p

                                           #region t_pkpm_binaryReport mappings
                                                 .Keyword(t => t.Name(n => n.CUSTOMID))
                                                 .Keyword(t => t.Name(n => n.REPORTNUM))
                                               .Keyword(t => t.Name(n => n.REPORTTYPE))
                                               .Keyword(t => t.Name(n => n.REPORTPATH))
                                               .Keyword(t => t.Name(n => n.PKRCONTENTIDPATH))
                                               .Keyword(t => t.Name(n => n.SYSPRIMARYKEYS))
                                               .Date(t => t.Name(n => n.UPLOADTIME))
                                               .Date(d => d.Name(n => n.UPDATETIME))
                                               .Number(n => n.Name(nn => nn.STATUS).Type(NumberType.Integer))
                                               .Number(n => n.Name(nn => nn.ORDERID).Type(NumberType.Integer))
                                               )
                                        #endregion

                                               )));


            if (newIndex.IsValid)
            {
                System.Console.WriteLine("pkr done");
            }
            else
            {
                System.Console.WriteLine("error");
            }
        }

        private static void CreateAcsIndex(ElasticClient client)
        {
            var newIndex = client.CreateIndex("gx-t-bp-acs", index => index
                                      .Settings(s => s.NumberOfShards(3).NumberOfReplicas(1))
                                       .Mappings(m => m
                                        .Map<es_t_bp_acs>(mm => mm.
                                           Properties(p => p

                                           #region t_bp_acs mapping
                                                .Keyword(t => t.Name(n => n.CUSTOMID))
                                               .Keyword(t => t.Name(n => n.UNITCODE))
                                               .Keyword(t => t.Name(n => n.SYSPRIMARYKEY))
                                               .Keyword(t => t.Name(n => n.SAMPLENUM))
                                               .Number(n => n.Name(nn => nn.MAXLC).Type(NumberType.Integer))
                                               .Keyword(t => t.Name(n => n.TIMES))
                                               .Object<string>(t => t.Name(n => n.MAXVALUE).Enabled(false))
                                               .Object<string>(t => t.Name(n => n.QFVALUE).Enabled(false))
                                               .Date(d => d.Name(n => n.ACSTIME))
                                               .Keyword(t => t.Name(n => n.DATATYPES))
                                               .Object<string>(t => t.Name(n => n.A).Enabled(false))
                                               .Object<string>(t => t.Name(n => n.B).Enabled(false))
                                               .Object<string>(t => t.Name(n => n.A1).Enabled(false))
                                               .Object<string>(t => t.Name(n => n.B1).Enabled(false))
                                               .Object<string>(t => t.Name(n => n.A2).Enabled(false))
                                               .Object<string>(t => t.Name(n => n.B2).Enabled(false))
                                               .Object<string>(t => t.Name(n => n.YSN).Enabled(false))
                                               .Object<string>(t => t.Name(n => n.CJSJ).Enabled(false))
                                               .Object<string>(t => t.Name(n => n.BZC).Enabled(false))
                                               .Object<string>(t => t.Name(n => n.BYXS).Enabled(false))
                                               .Object<string>(t => t.Name(n => n.PJZ).Enabled(false))
                                               .Object<string>(t => t.Name(n => n.CJSJ).Enabled(false))
                                               .Keyword(t => t.Name(n => n.CHECKMAN))
                                               .Keyword(t => t.Name(n => n.INSTRUMENTNAME))
                                               .Keyword(t => t.Name(n => n.INSTRUMENTNUM))
                                               .Date(t => t.Name(n => n.UPLOADTIME))
                                               .Date(d => d.Name(n => n.UPDATETIME))
                                               .Object<string>(t => t.Name(n => n.SPEED).Enabled(false))
                                               .Object<string>(t => t.Name(n => n.ACSDATAPATH).Enabled(false))
                                           #endregion

                                               ))
                                           ));


            if (newIndex.IsValid)
            {
                System.Console.WriteLine(" acs done");
            }
            else
            {
                System.Console.WriteLine("error");
            }
        }

        private static void CreateModifyLogIndex(ElasticClient client)
        {

            var newIndex = client.CreateIndex("gx-t-bp-modifylog", index => index
                                       .Settings(s => s.NumberOfShards(3).NumberOfReplicas(1))
                                        .Mappings(m => m.Map<es_t_bp_modify_log>(mm => mm
                                             .Properties(p => p

                                             #region t_bp_modifylog mapping
                                                .Keyword(t => t.Name(n => n.CUSTOMID))
                                                 .Keyword(t => t.Name(n => n.MODIFYPRIMARYKEY))
                                                 .Keyword(t => t.Name(n => n.SYSPRIMARYKEY))
                                                 .Number(n => n.Name(nn => nn.MODIFYTIMES).Type(NumberType.Integer))
                                                  .Number(n => n.Name(nn => nn.ISUPLOADED).Type(NumberType.Integer))
                                                 .Keyword(t => t.Name(n => n.SAMPLENUM))
                                                 .Keyword(t => t.Name(n => n.REPORTNUM))
                                                 .Keyword(t => t.Name(n => n.ENTRUSTNUM))
                                                 .Keyword(t => t.Name(n => n.PROJECTNUM))
                                                 .Keyword(t => t.Name(n => n.ITEMTABLENAME))
                                                 .Object<string>(d => d.Name(n => n.MODIFYMAN).Enabled(false))
                                                 .Date(d => d.Name(n => n.MODIFYDATETIME))
                                                 .Object<string>(d => d.Name(n => n.BEFOREMODIFYVALUES).Enabled(false))
                                                 .Object<string>(d => d.Name(n => n.AFTERMODIFYVALUES).Enabled(false))
                                                 .Keyword(t => t.Name(n => n.SENDFLAGS))
                                                 .Date(d => d.Name(n => n.UPLOADTIME))
                                                 .Date(d => d.Name(n => n.UPLOADTIME))
                                                 .Object<string>(t => t.Name(n => n.COMPUTERNAME).Enabled(false))
                                                 .Object<string>(t => t.Name(n => n.MACADDRESS).Enabled(false))
                                             #endregion

                                                 ))

                                         ));


            if (newIndex.IsValid)
            {
                System.Console.WriteLine("modifylog done");
            }
            else
            {
                System.Console.WriteLine("error");
            }

        }

        private static void CreateQuestionIndex(ElasticClient client)
        {

            var newIndex = client.CreateIndex("gx-t-bp-question", index => index
                                       .Settings(s => s.NumberOfShards(3).NumberOfReplicas(1))
                                        .Mappings(m => m
                                          .Map<es_t_bp_question>(mm => mm.
                                            Properties(p => p

                                            #region t_bp_question mapping
                                             .Keyword(t => t.Name(n => n.CUSTOMID))
                                             .Keyword(t => t.Name(n => n.QUESTIONPRIMARYKEY))
                                             .Keyword(t => t.Name(n => n.SYSPRIMARYKEY))
                                             .Keyword(t => t.Name(n => n.ITEMTABLENAME))
                                             .Keyword(t => t.Name(n => n.SAMPLENUM))
                                             .Object<string>(t => t.Name(n => n.RECORDMAN).Enabled(false))
                                             .Date(d => d.Name(n => n.RECORDTIME))
                                             .Keyword(t => t.Name(n => n.RECORDINGPHASE))
                                             .Object<string>(t => t.Name(n => n.AUDITINGMAN).Enabled(false))
                                             .Date(d => d.Name(n => n.AUDITINGTIME))
                                             .Keyword(t => t.Name(n => n.ISAUDITING))
                                             .Object<string>(t => t.Name(n => n.APPROVEMAN).Enabled(false))
                                             .Date(d => d.Name(n => n.APPROVETIME))
                                             .Keyword(t => t.Name(n => n.ISAPPROVE))
                                             .Object<string>(t => t.Name(n => n.NEEDPROCMAN).Enabled(false))
                                             .Keyword(t => t.Name(n => n.ISPROCED))
                                             .Date(d => d.Name(n => n.NEEDPROCTIME))
                                             .Keyword(t => t.Name(n => n.QUESTIONTYPES))
                                             .Object<string>(t => t.Name(n => n.CONTEXT).Enabled(false))
                                             .Object<string>(t => t.Name(n => n.AUDITINGCONTEXT).Enabled(false))
                                             .Object<string>(t => t.Name(n => n.APPROVECONTEXT).Enabled(false))
                                             .Number(n => n.Name(nn => nn.MODIFYRANGE).Type(NumberType.Integer))
                                             .Keyword(t => t.Name(n => n.ISMODIFIED))
                                             .Date(d => d.Name(n => n.UPLOADTIME))
                                             .Date(d => d.Name(n => n.UPLOADTIME)).Date(d => d.Name(n => n.UPLOADTIME))
                                             .Keyword(t => t.Name(n => n.ISREFUNDMONEY))
                                             .Number(n => n.Name(nn => nn.ISCONSOLEAPPROVED).Type(NumberType.Integer))
                                            #endregion

                                             ))

                                         ));


            if (newIndex.IsValid)
            {
                System.Console.WriteLine("question done");
            }
            else
            {
                System.Console.WriteLine("error");
            }

        }

        private static void CreateESProjectIndex(ElasticClient client)
        {


            var newIndex = client.CreateIndex("gx-t-bp-project", index => index
                                       .Settings(s => s.NumberOfShards(3).NumberOfReplicas(1))
                                       .Mappings(m => m.Map<es_t_bp_project>(mm => mm
                                              .Properties(p => p

                                              #region t_bp_project mapping
                                                  .Keyword(t => t.Name(n => n.CUSTOMID))
                                                  .Keyword(t => t.Name(n => n.PROJECTNUM))
                                                  .Keyword(t => t.Name(n => n.PROJECTNAME))
                                                  .Keyword(t => t.Name(n => n.ENTRUSTUNIT))
                                                  .Object<string>(t => t.Name(n => n.SENDSAMPLEMAN).Enabled(false))
                                                  .Object<string>(t => t.Name(n => n.SENDSAMPLEMANTEL).Enabled(false))
                                                  .Object<string>(t => t.Name(n => n.ENTRUSTUNITLINKMAN).Enabled(false))
                                                  .Object<string>(t => t.Name(n => n.ENTRUSTUNITLINKMANTEL).Enabled(false))
                                                  .Keyword(t => t.Name(n => n.CONSTRACTUNIT))
                                                  .Keyword(t => t.Name(n => n.CONSTRACTIONUNIT))
                                                  .Keyword(t => t.Name(n => n.WITNESSUNIT))
                                                  .Object<string>(t => t.Name(n => n.WITNESSMAN).Enabled(false))
                                                  .Object<string>(t => t.Name(n => n.WITNESSMANNUM).Enabled(false))
                                                  .Object<string>(t => t.Name(n => n.WITNESSMANTEL).Enabled(false))
                                                  .Keyword(t => t.Name(n => n.SUPERUNIT))
                                                  .Object<string>(t => t.Name(n => n.SUPERMAN).Enabled(false))
                                                  .Keyword(t => t.Name(n => n.DESIGNUNIT))
                                                  .Keyword(t => t.Name(n => n.INSPECTUNIT))
                                                  .Object<string>(t => t.Name(n => n.INSPECTMAN).Enabled(false))
                                                  .Text(t => t.Name(n => n.INVESTIGATEUNIT))
                                                  .Object<string>(t => t.Name(n => n.TAKESAMPLEMAN).Enabled(false))
                                                  .Object<string>(t => t.Name(n => n.TAKESAMPLEMANNUM).Enabled(false))
                                                  .Object<string>(t => t.Name(n => n.TAKESAMPLEMANTEL).Enabled(false))
                                                  .Keyword(t => t.Name(n => n.TAKESAMPLEUNIT))
                                                  .Keyword(t => t.Name(n => n.PROJECTADDRESS))
                                                  .Keyword(t => t.Name(n => n.PROJECTAREA))
                                                  .Keyword(t => t.Name(n => n.PROJECTSTATUS))
                                                  .Keyword(t => t.Name(n => n.PROJECTCHARGETYPE))
                                                  .Number(n => n.Name(nn => nn.ACCOUNTBALANCE).Type(NumberType.Double))
                                                  .Number(n => n.Name(nn => nn.TOTALCONSUMEDMONEY).Type(NumberType.Double))
                                                  .Number(n => n.Name(nn => nn.TOTALFAVOURABLEMONEY).Type(NumberType.Double))
                                                  .Keyword(t => t.Name(n => n.UNITCREDITLEVEL))
                                                  .Keyword(t => t.Name(n => n.DEFAULTCONSUMETYPE))
                                                  .Date(d => d.Name(n => n.CREATEDATE))
                                                  .Date(d => d.Name(n => n.DESTROYDATE))
                                                  .Keyword(t => t.Name(n => n.STRUTTYPE))
                                                  .Keyword(t => t.Name(n => n.ISUSERICCARD))
                                                  .Keyword(t => t.Name(n => n.PROJECTID))
                                                  .Keyword(t => t.Name(n => n.CONTRACTNUM))
                                                  .Object<string>(t => t.Name(n => n.EMAIL).Enabled(false))
                                                  .Keyword(t => t.Name(n => n.AUTOSENDREPORT))
                                                  .Object<string>(t => t.Name(n => n.CREATEMAN).Enabled(false))
                                                  .Object<string>(t => t.Name(n => n.ENTRUSTUNITADDRESS).Enabled(false))
                                                  .Keyword(t => t.Name(n => n.ENTRUSTUNITNUM))
                                                  .Keyword(t => t.Name(n => n.CONSTRACTUNITID))
                                                  .Keyword(t => t.Name(n => n.DESIGNUNITID))
                                                  .Keyword(t => t.Name(n => n.INVESTIGATEUNITID))
                                                  .Keyword(t => t.Name(n => n.SUPERUNITID))
                                                  .Keyword(t => t.Name(n => n.CONSTRACTIONUNITID))
                                                  .Keyword(t => t.Name(n => n.INSPECTUNITID))
                                                  .Keyword(t => t.Name(n => n.AREAID))
                                                  .Keyword(t => t.Name(n => n.ENTRUSTUNITID))
                                                  .Keyword(t => t.Name(n => n.TAKESAMPLEUNITID))
                                                  .Keyword(t => t.Name(n => n.WITNESSUNITID))
                                                  .Keyword(t => t.Name(n => n.SUPERMANID))
                                                  .Keyword(t => t.Name(n => n.INSPECTMANID))
                                                  .Object<string>(t => t.Name(n => n.SUPERMANTEL).Enabled(false))
                                                  .Object<string>(t => t.Name(n => n.INSPECTMANTEL).Enabled(false))
                                                  .Object<string>(t => t.Name(n => n.GCBH).Enabled(false))
                                                  .Keyword(t => t.Name(n => n.CONSTRACTUNITMANID))
                                                  .Object<string>(t => t.Name(n => n.CONSTRACTUNITMAN).Enabled(false))
                                                  .Object<string>(t => t.Name(n => n.CONSTRACTUNITMANTEL).Enabled(false))
                                                  .Keyword(t => t.Name(n => n.CONSTRACTIONUNITMANID))
                                                  .Object<string>(t => t.Name(n => n.CONSTRACTIONUNITMAN).Enabled(false))
                                                  .Object<string>(t => t.Name(n => n.CONSTRACTIONUNITMANTEL).Enabled(false))
                                                  .Keyword(t => t.Name(n => n.DESIGNUNITMANID))
                                                  .Object<string>(t => t.Name(n => n.DESIGNUNITMANTEL).Enabled(false))
                                                  .Keyword(t => t.Name(n => n.INVESTIGATEUNITMANID))
                                                  .Object<string>(t => t.Name(n => n.INVESTIGATEUNITMAN).Enabled(false))
                                                  .Object<string>(t => t.Name(n => n.INVESTIGATEUNITMANTEL).Enabled(false))
                                                  .Keyword(t => t.Name(n => n.ANQUANREGID))
                                                  .Date(d => d.Name(n => n.PLANENDDATE))
                                                  .Date(d => d.Name(n => n.PLANSTARTDATE))
                                                  .Number(n => n.Name(nn => nn.PROTECTSQUARE).Type(NumberType.Double))
                                                  .Number(n => n.Name(nn => nn.SQUARE).Type(NumberType.Double))
                                                  .Object<string>(t => t.Name(n => n.STRUCTLEVLES).Enabled(false))
                                                  .Number(n => n.Name(nn => nn.TOTALPRICE).Type(NumberType.Double))
                                                  .Object<string>(t => t.Name(n => n.SENDSAMPLEMANNUM).Enabled(false))
                                                  .Keyword(t => t.Name(n => n.JGLX))
                                                  .Date(d => d.Name(n => n.CHECKTIME))
                                                  .Object<string>(t => t.Name(n => n.SUPERMANTEL1).Enabled(false))
                                                  .Keyword(t => t.Name(n => n.FPROJECTNAME))
                                                  .Keyword(t => t.Name(n => n.JX_PROJECTID))
                                                  .Keyword(t => t.Name(n => n.JX_ITEMID))
                                                  .Keyword(t => t.Name(n => n.INSPECTUNITNUM))
                                                  .Keyword(t => t.Name(n => n.CONTRACTIONPERMITNUM))
                                                  .Keyword(t => t.Name(n => n.INSPECTPROJECTNUM))
                                                  .Object<string>(t => t.Name(n => n.HAINANXIANGMUBIAOSHI).Enabled(false))
                                                  .Object<string>(t => t.Name(n => n.HAINANXIANGMUBIANHAO).Enabled(false))
                                                  .Object<string>(t => t.Name(n => n.HAINANXIANGMUMINGCHEN).Enabled(false))
                                                  .Object<string>(t => t.Name(n => n.HAINANGONGCHENGBIAOSHI).Enabled(false))
                                                  .Object<string>(t => t.Name(n => n.HAINANGONGCHENGBIANHAO).Enabled(false))
                                                  .Object<string>(t => t.Name(n => n.HAINANSHIGONGDANWEIBIAOSHI).Enabled(false))
                                                  .Number(n => n.Name(nn => nn.HAINANISVARIFYINFO).Type(NumberType.Integer))
                                                  .Object<string>(t => t.Name(n => n.HAINANDANWEIGONGCHENGMING).Enabled(false))
                                                  .Object<string>(t => t.Name(n => n.RECKONER).Enabled(false))
                                                  .Object<string>(t => t.Name(n => n.DEPOSITRATE).Enabled(false))
                                                  .Object<string>(t => t.Name(n => n.USERDEFINEBAR).Enabled(false))
                                                  .Number(n => n.Name(nn => nn.ICARD).Type(NumberType.Long))
                                                  .Object<string>(t => t.Name(n => n.SALEMAN).Enabled(false))
                                                  .Date(d => d.Name(n => n.UPLOADTIME))
                                                  .Date(d => d.Name(n => n.UPDATETIME))
                                                  .Keyword(t => t.Name(n => n.PROJECTKEY))
                                              #endregion

                                                  ))
                                                  ));

            if (newIndex.IsValid)
            {
                System.Console.WriteLine("bpproject done");
            }
            else
            {
                System.Console.WriteLine("error");
            }

        }

        private static void CreateESTbpItemListIndex(ElasticClient client)
        {

            var newIndex = client.CreateIndex("gx-t-bp-itemlist", index => index
                                       .Settings(s => s.NumberOfShards(3).NumberOfReplicas(1))
                                       .Mappings(m => m.Map<es_t_bp_item_list>(mm => mm
                                               .Properties(p => p

                                               #region t_bp_itemList mapping
                                                .Keyword(t => t.Name(n => n.CUSTOMID))
                                                  .Keyword(t => t.Name(n => n.UNITCODE))
                                                  .Keyword(t => t.Name(n => n.ITEMTABLENAME))
                                                  .Keyword(t => t.Name(n => n.ITEMCHNAME))
                                                  .Keyword(t => t.Name(n => n.ITEMTYPES))
                                                  .Keyword(t => t.Name(n => n.ITEMSTANDARDS))
                                                  .Number(n => n.Name(nn => nn.REPORTCONLINES).Type(NumberType.Integer))
                                                  .Number(n => n.Name(nn => nn.RECORDCONLINES).Type(NumberType.Integer))
                                                  .Number(n => n.Name(nn => nn.ENTRFORMCONLINES).Type(NumberType.Integer))
                                                  .Number(n => n.Name(nn => nn.REPORTORGI).Type(NumberType.Integer))
                                                  .Number(n => n.Name(nn => nn.RECORDORGI).Type(NumberType.Integer))
                                                  .Keyword(t => t.Name(n => n.TESTEQUIPMENTNUM))
                                                  .Keyword(t => t.Name(n => n.TESTEQUIPMENTNAME))
                                                  .Number(n => n.Name(nn => nn.REPORTCONSENTDAYS).Type(NumberType.Integer))
                                                  .Number(n => n.Name(nn => nn.REPORTCONSENTCWORKDAYS).Type(NumberType.Integer))
                                                  .Keyword(t => t.Name(n => n.KEYS))
                                                  .Number(n => n.Name(nn => nn.USEFREQ).Type(NumberType.Integer))
                                                  .Number(n => n.Name(nn => nn.CHECKMANCOUNT).Type(NumberType.Integer))
                                                  .Keyword(t => t.Name(n => n.PRINTDATEMODE))
                                                  .Keyword(t => t.Name(n => n.STANDARDNAMEMODE))
                                                   .Number(n => n.Name(nn => nn.ISHAVEGJ).Type(NumberType.Integer))
                                                  .Number(n => n.Name(nn => nn.ONLYMANAGEREPORT).Type(NumberType.Integer))
                                                  .Number(n => n.Name(nn => nn.CHECKDATESTYLE).Type(NumberType.Integer))
                                                  .Keyword(t => t.Name(n => n.ISUSE))
                                                   .Number(n => n.Name(nn => nn.CHECKDAYS).Type(NumberType.Integer))
                                                  .Number(n => n.Name(nn => nn.REPORTDAYS).Type(NumberType.Integer))
                                                  .Keyword(t => t.Name(n => n.JX_LBDM))
                                                  .Keyword(t => t.Name(n => n.JX_ITEMCODE))
                                                  .Keyword(t => t.Name(n => n.JX_LB))
                                                  .Keyword(t => t.Name(n => n.JX_ITEMCHNAME))
                                                  .Keyword(t => t.Name(n => n.HEBEI_ITEMCODE))
                                                  .Keyword(t => t.Name(n => n.HEBEI_TABLENAME))
                                                  .Keyword(t => t.Name(n => n.HEBEI_ITEMCHNAME))
                                                  .Date(d => d.Name(n => n.STARTTIME))
                                                  .Date(d => d.Name(n => n.UPLOADTIME))
                                                  .Date(d => d.Name(n => n.UPDATETIME))
                                                  .Date(d => d.Name(n => n.VALIDDATE))
                                                  .Keyword(t => t.Name(n => n.PDCOLUMNS))
                                               #endregion

                                                  ))));

            if (newIndex.IsValid)
            {
                System.Console.WriteLine("itemlist done");
            }
            else
            {
                System.Console.WriteLine("error");
            }

        }

        private static void CreateESWordReportIndex(ElasticClient client)
        {
            var newIndex = client.CreateIndex("gx-t-bp-wordreport", index => index
                                      .Settings(s => s.NumberOfShards(3).NumberOfReplicas(1))
                                       .Mappings(m => m
                                          .Map<es_t_bp_wordreport>(mm => mm.
                                           Properties(p => p

                                           #region t_bp_wordreport mappings
                                                 .Keyword(t => t.Name(n => n.CUSTOMID))
                                               .Keyword(t => t.Name(n => n.ITEMTABLENAME))
                                               .Keyword(t => t.Name(n => n.SYSPRIMARYKEY))
                                               .Keyword(t => t.Name(n => n.REPORTTYPES))
                                               .Keyword(t => t.Name(n => n.WORDREPORTPATH))
                                               .Date(t => t.Name(n => n.UPLOADTIME))
                                               .Date(d => d.Name(n => n.UPDATETIME))
                                               .Date(t => t.Name(n => n.PRINTDATE))
                                               .Number(t => t.Name(n => n.MODIFIED).Type(NumberType.Integer))
                                           #endregion

                                               ))

                                                ));


            if (newIndex.IsValid)
            {
                System.Console.WriteLine("word report done");
            }
            else
            {
                System.Console.WriteLine("error");
            }
        }

        private static void CreateESExtReporttIndex(ElasticClient client)
        {
            var newIndex = client.CreateIndex("gx-t-bp-extreport", index => index
                                      .Settings(s => s.NumberOfShards(3).NumberOfReplicas(1))
                                       .Mappings(m => m
                                           .Map<es_extReportMange>(mm => mm.
                                           Properties(p => p

                                           #region extReportmanage mapping
                                                .Keyword(t => t.Name(n => n.IDENTKEY))
                                               .Keyword(t => t.Name(n => n.ITEMTABLENAME))
                                               .Keyword(t => t.Name(n => n.SYSPRIMARYKEY))
                                               .Keyword(t => t.Name(n => n.REPORTNUM))
                                               .Keyword(t => t.Name(n => n.FILETYPE))
                                               .Keyword(t => t.Name(n => n.REPORTNAME))
                                               .Keyword(t => t.Name(n => n.WORDREPORTPATH))
                                               .Date(t => t.Name(n => n.UPLOADTIME))
                                               .Date(d => d.Name(n => n.UPDATETIME))
                                                .Date(t => t.Name(n => n.PRINTDATE))
                                           #endregion

                                               ))));


            if (newIndex.IsValid)
            {
                System.Console.WriteLine("ext report done");
            }
            else
            {
                System.Console.WriteLine("error");
            }
        }

        private static void CreateESHNKYIndex(ElasticClient client)
        {
            var newIndex = client.CreateIndex("gx-t-bp-hnky", index => index
                                      .Settings(s => s.NumberOfShards(3).NumberOfReplicas(1))
                                       .Mappings(m => m
                                           .Map<HNKY_CUR>(mm => mm.
                                           Properties(p => p

                                           #region extReportmanage mapping

                                               .Date(t => t.Name(n => n.PRODUCEDATE))
                                               .Date(d => d.Name(n => n.REPORTCONSENTDATE))
                                               .Date(t => t.Name(n => n.ENTRUSTDATE))
                                               .Date(t => t.Name(n => n.CHECKDATE))
                                               .Date(d => d.Name(n => n.AUDITINGDATE))
                                               .Date(t => t.Name(n => n.APPROVEDATE))
                                               .Date(t => t.Name(n => n.PRINTDATE))
                                               .Number(t => t.Name(n => n.REPORTCONSENTDAYS).Type(NumberType.Integer))
                                               .Number(t => t.Name(n => n.YANGHUXISHU).Type(NumberType.Float))
                                               .Date(t => t.Name(n => n.CHENGXINGRIQI))
                                               .Number(t => t.Name(n => n.LINQI).Type(NumberType.Integer))
                                               .Number(t => t.Name(n => n.CHANG1).Type(NumberType.Integer))
                                               .Number(t => t.Name(n => n.CHANG2).Type(NumberType.Integer))
                                               .Number(t => t.Name(n => n.CHANG3).Type(NumberType.Integer))
                                               .Number(t => t.Name(n => n.KUAN1).Type(NumberType.Integer))
                                               .Number(t => t.Name(n => n.KUAN2).Type(NumberType.Integer))
                                               .Number(t => t.Name(n => n.KUAN3).Type(NumberType.Integer))
                                                .Number(t => t.Name(n => n.GAO1).Type(NumberType.Integer))
                                               .Number(t => t.Name(n => n.GAO2).Type(NumberType.Integer))
                                               .Number(t => t.Name(n => n.GAO3).Type(NumberType.Integer))
                                               .Number(t => t.Name(n => n.CHICUNXISHU).Type(NumberType.Float))
                                               .Number(t => t.Name(n => n.JIXIANGHEZAI1).Type(NumberType.Float))
                                               .Number(t => t.Name(n => n.JIXIANGHEZAI2).Type(NumberType.Float))
                                               .Number(t => t.Name(n => n.JIXIANGHEZAI3).Type(NumberType.Float))
                                               .Number(t => t.Name(n => n.QIANGDU3).Type(NumberType.Float))
                                               .Number(t => t.Name(n => n.QIANGDU2).Type(NumberType.Float))
                                               .Number(t => t.Name(n => n.QIANGDU1).Type(NumberType.Float))
                                               .Number(t => t.Name(n => n.BAIFENGBI).Type(NumberType.Float))
                                               .Number(t => t.Name(n => n.NEEDCHARGEMONEY).Type(NumberType.Float))
                                               .Date(d => d.Name(n => n.COLLATEDATE))
                                               .Date(t => t.Name(n => n.VERIFYDATE))
                                               .Date(t => t.Name(n => n.EXTENDDATE))
                                               .Date(t => t.Name(n => n.FACTCHECKDATE))
                                                .Number(t => t.Name(n => n.YQLQ).Type(NumberType.Integer))
                                                .Number(t => t.Name(n => n.SHEJIQIANGDU).Type(NumberType.Float))
                                                .Number(t => t.Name(n => n.DATATYPE).Type(NumberType.Integer))
                                                 .Number(t => t.Name(n => n.QIANGDUAVG).Type(NumberType.Float))
                                                 .Date(t => t.Name(n => n.YANGHUBDATE))
                                               .Date(t => t.Name(n => n.YANGHUEDATE))
                                                .Number(t => t.Name(n => n.YANGHUDAYS).Type(NumberType.Integer))
                                            .Date(t => t.Name(n => n.REQUESTCHECKDATE))
                                             .Number(t => t.Name(n => n.SPEEDMAX).Type(NumberType.Float))
                                               .Number(t => t.Name(n => n.SPEEDMIN).Type(NumberType.Float))
                                               .Number(t => t.Name(n => n.SPEEDAVG).Type(NumberType.Float))
                                               .Number(t => t.Name(n => n.TTJDBZ).Type(NumberType.Float))
                                           #endregion

                                               ))));


            if (newIndex.IsValid)
            {
                System.Console.WriteLine("hnky done");
            }
            else
            {
                System.Console.WriteLine("error");
            }
        }

        private static void CreateESSJKYIndex(ElasticClient client)
        {
            var newIndex = client.CreateIndex("gx-t-bp-sjky", index => index
                                      .Settings(s => s.NumberOfShards(3).NumberOfReplicas(1))
                                       .Mappings(m => m
                                           .Map<SJKY_CUR>(mm => mm.
                                           Properties(p => p

                                           #region extReportmanage mapping

                                                .Date(t => t.Name(n => n.PRODUCEDATE))
                                                .Date(t => t.Name(n => n.REPORTCONSENTDATE))
                                                .Date(t => t.Name(n => n.ENTRUSTDATE))
                                                .Date(t => t.Name(n => n.CHECKDATE))
                                                .Date(t => t.Name(n => n.AUDITINGDATE))
                                                .Date(t => t.Name(n => n.APPROVEDATE))
                                                .Date(t => t.Name(n => n.PRINTDATE))
                                                 .Number(t => t.Name(n => n.REPORTCONSENTDAYS).Type(NumberType.Integer))
                                                 .Date(t => t.Name(n => n.CHENGXINGDATE))
                                                  .Number(t => t.Name(n => n.LINGQI).Type(NumberType.Integer))
                                               .Number(t => t.Name(n => n.CHANG1).Type(NumberType.Float))
                                               .Number(t => t.Name(n => n.CHANG2).Type(NumberType.Float))
                                               .Number(t => t.Name(n => n.CHANG3).Type(NumberType.Float))
                                               .Number(t => t.Name(n => n.CHANG4).Type(NumberType.Float))
                                               .Number(t => t.Name(n => n.CHANG5).Type(NumberType.Float))
                                               .Number(t => t.Name(n => n.CHANG6).Type(NumberType.Float))
                                               .Number(t => t.Name(n => n.KUAN1).Type(NumberType.Float))
                                               .Number(t => t.Name(n => n.KUAN2).Type(NumberType.Float))
                                               .Number(t => t.Name(n => n.KUAN3).Type(NumberType.Float))
                                               .Number(t => t.Name(n => n.KUAN4).Type(NumberType.Float))
                                               .Number(t => t.Name(n => n.KUAN5).Type(NumberType.Float))
                                               .Number(t => t.Name(n => n.KUAN6).Type(NumberType.Float))
                                                .Number(t => t.Name(n => n.GAO1).Type(NumberType.Float))
                                               .Number(t => t.Name(n => n.GAO2).Type(NumberType.Float))
                                               .Number(t => t.Name(n => n.GAO3).Type(NumberType.Float))
                                                .Number(t => t.Name(n => n.GAO4).Type(NumberType.Float))
                                               .Number(t => t.Name(n => n.GAO5).Type(NumberType.Float))
                                               .Number(t => t.Name(n => n.GAO6).Type(NumberType.Float))
                                                .Number(t => t.Name(n => n.JIXIANHEZAI1).Type(NumberType.Float))
                                               .Number(t => t.Name(n => n.JIXIANHEZAI2).Type(NumberType.Float))
                                               .Number(t => t.Name(n => n.JIXIANHEZAI3).Type(NumberType.Float))
                                                .Number(t => t.Name(n => n.JIXIANHEZAI4).Type(NumberType.Float))
                                               .Number(t => t.Name(n => n.JIXIANHEZAI5).Type(NumberType.Float))
                                               .Number(t => t.Name(n => n.JIXIANHEZAI6).Type(NumberType.Float))
                                               .Number(t => t.Name(n => n.QIANGDU1).Type(NumberType.Float))
                                               .Number(t => t.Name(n => n.QIANGDU2).Type(NumberType.Float))
                                               .Number(t => t.Name(n => n.QIANGDU3).Type(NumberType.Float))
                                                .Number(t => t.Name(n => n.QIANGDU4).Type(NumberType.Float))
                                               .Number(t => t.Name(n => n.QIANGDU5).Type(NumberType.Float))
                                               .Number(t => t.Name(n => n.QIANGDU6).Type(NumberType.Float))
                                               .Number(t => t.Name(n => n.QIANGDUBAIFENBI).Type(NumberType.Float))
                                                .Number(t => t.Name(n => n.QIANGDUMIN).Type(NumberType.Float))
                                                 .Number(t => t.Name(n => n.QIANGDUMAX).Type(NumberType.Float))
                                                  .Number(t => t.Name(n => n.NEEDCHARGEMONEY).Type(NumberType.Float))
                                                   .Date(t => t.Name(n => n.COLLATEDATE))
                                                .Date(t => t.Name(n => n.VERIFYDATE))
                                                .Date(t => t.Name(n => n.EXTENDDATE))
                                                .Date(t => t.Name(n => n.FACTCHECKDATE))
                                                  .Number(t => t.Name(n => n.YQLQ).Type(NumberType.Integer))
                                                    .Number(t => t.Name(n => n.DATATYPE).Type(NumberType.Integer))
                                                     .Number(t => t.Name(n => n.HSXS).Type(NumberType.Float))
                                                      .Number(t => t.Name(n => n.QIANGDUAVG).Type(NumberType.Float))
                                                   .Date(t => t.Name(n => n.REQUESTCHECKDATE))
                                           #endregion

                                               ))));


            if (newIndex.IsValid)
            {
                System.Console.WriteLine("sjky done");
            }
            else
            {
                System.Console.WriteLine("error");
            }
        }
    }
}
