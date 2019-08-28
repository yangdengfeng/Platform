using QZWebService.ServiceInterface.Repsitory;
using QZWebService.ServiceModel;
using RsLib;
using ServiceStack;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace QZWebService.ServiceInterface
{
    public class ZJService : Service
    {
        IRepsitory<view_testingSite> testSiterep;
        IRepsitory<view_pileDataList> pileDataListRep;
        IRepsitory<view_programmePileList> programmePileListRep;
        IRepsitory<tab_zj_updatelog> updatelogRep;
        IRepsitory<Jy_BasicInfo> basicInfoRep;
        IRepsitory<Jy_TestingLogInfo> testLogInfoRep;
        IRepsitory<view_pilephoto> viewPilePhotoRep;
        IRepsitory<view_GpsPileInfo> gpsPileInfo;
        IRepsitory<view_testingHis>testingHisRep;
        

        public ZJService(IRepsitory<view_testingSite> testSiterep,
            IRepsitory<tab_zj_updatelog> updatelogRep,
            IRepsitory<view_GpsPileInfo> gpsPileInfo,
             IRepsitory<view_pileDataList> pileDataListRep,
             IRepsitory<Jy_BasicInfo> basicInfoRep,
              IRepsitory<Jy_TestingLogInfo> testLogInfoRep,
               IRepsitory<view_pilephoto> viewPilePhotoRep,
               IRepsitory<view_testingHis> testingHisRep,
            IRepsitory<view_programmePileList> programmePileListRep)
        {
            this.gpsPileInfo = gpsPileInfo;
            this.updatelogRep = updatelogRep;
            this.pileDataListRep = pileDataListRep;
            this.programmePileListRep = programmePileListRep;
            this.testSiterep = testSiterep;
            this.basicInfoRep = basicInfoRep;
            this.viewPilePhotoRep = viewPilePhotoRep;
            this.testingHisRep = testingHisRep;
            this.testLogInfoRep = testLogInfoRep;
        }



        public GetZjCheckGPSByAreaResponse GET(GetZjCheckGPSByArea request)
        {
            GetZjCheckGPSByAreaResponse response = new GetZjCheckGPSByAreaResponse()
            {
                IsSucc = true,
                Msg = string.Empty,
                gpsPileInfo = new List<view_GpsPileInfo>()
            };

            try
            {
                var predicate = PredicateBuilder.True<view_GpsPileInfo>();
                if (!request.Area.IsNullOrEmpty())
                {
                    predicate = predicate.And(t => t.areainfo == request.Area);

                }


                response.gpsPileInfo = gpsPileInfo.GetByCondition(predicate);//.GetByConditon(predicate);

            }
            catch (Exception ex)
            {
                response.IsSucc = false;
                response.Msg = ex.Message;
            }
            return response;


        }


        /// <summary>
        /// 桩基现场异常处理记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public PileUpdataInfoResponse GET(PileUpdataInfo request)
        {
            PileUpdataInfoResponse response = new PileUpdataInfoResponse()
            {
                IsSucc = true,
                Msg = string.Empty,
                datas = new List<tab_zj_updatelog>()
            };

            string dtFormatStr = "yyyy-MM-dd'T'HH:mm:ss";
            try
            {
                var predicate = PredicateBuilder.True<tab_zj_updatelog>();
                if (!request.UnitName.IsNullOrEmpty())
                {
                    predicate = predicate.And(tt => tt.customid == request.UnitName);
                }
                if (request.UpdataStartDate.HasValue || request.UpdataEndDate.HasValue)
                {
                    string startDtStr = request.UpdataStartDate.HasValue ? request.UpdataStartDate.Value.ToString(dtFormatStr) : string.Empty;
                    string endDtStr = request.UpdataEndDate.HasValue ? request.UpdataEndDate.Value.ToString(dtFormatStr) : string.Empty;
                    if (!startDtStr.IsNullOrEmpty())
                    {
                        predicate = predicate.And(tt => tt.addtime >= DateTime.Parse(startDtStr));
                    }
                    if (!endDtStr.IsNullOrEmpty())
                    {
                        predicate = predicate.And(tt => tt.addtime <= DateTime.Parse(endDtStr));
                    }
                }
                var pos = request.posStart.HasValue ? request.posStart.Value : 0;
                var count = request.count.HasValue ? request.count.Value : 30;
                var pagineOption = new PagingOptions<tab_zj_updatelog>(pos, count, t => t.id);
                response.datas = updatelogRep.GetByConditonPage(predicate, pagineOption);

            }
            catch (Exception ex)
            {
                response.IsSucc = false;
                response.Msg = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// 根据桩基id获取桩基检测方案
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public GetZJCheckByIdResponse GET(GetZJCheckById request)
        {
            GetZJCheckByIdResponse response = new GetZJCheckByIdResponse()
            {
                IsSucc = true,
                Msg = string.Empty,
                data = new view_programmePileList()
            };
            try
            {
                response.data = programmePileListRep.GetById(request.id);

            }
            catch (Exception ex)
            {
                response.IsSucc = false;
                response.Msg = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// 获取桩基检测方案列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public GetZJCheckListResponse GET(GetZJCheckList request)
        {
            GetZJCheckListResponse response = new GetZJCheckListResponse()
            {
                IsSucc = true,
                Msg = string.Empty,
                datas = new List<view_programmePileList>()
            };

            try
            {
                var predicate = PredicateBuilder.True<view_programmePileList>();
                #region  动态查询
                if (!request.CheckUnitName.IsNullOrEmpty())
                {
                    predicate = predicate.And(tt => tt.customid == request.CheckUnitName);
                }
                if (!request.CheckPeople.IsNullOrEmpty())
                {
                    predicate = predicate.And(tt => tt.testingpeople.Contains(request.CheckPeople));
                }
                if (!request.Area.IsNullOrEmpty())
                {
                    predicate = predicate.And(tt => tt.areainfo == request.Area);
                }
                if (!request.CheckEquip.IsNullOrEmpty())
                {
                    predicate = predicate.And(tt => tt.testingequipment.Contains(request.CheckEquip));
                }
                if (!request.Report.IsNullOrEmpty())
                {
                    if (request.Report == "0")
                    {

                    }
                    else
                    {
                        predicate = predicate.And(tt => tt.reportcount != 0);
                    }

                }
                if (!request.ProjectName.IsNullOrEmpty())
                {
                    predicate = predicate.And(tt => tt.projectname.Contains(request.ProjectName));
                }
                if (!request.ZX.IsNullOrEmpty())
                {
                    predicate = predicate.And(tt => tt.piletype == request.ZX);
                }

                if (request.StartDate.HasValue)
                {
                    predicate = predicate.And(t => t.addtime.HasValue && t.addtime.Value >= request.StartDate.Value);
                }
                if (request.EndDate.HasValue)
                {
                    predicate = predicate.And(t => t.addtime.HasValue && t.addtime.Value <= request.EndDate.Value);
                }

                #endregion
                var pos = request.posStart.HasValue ? request.posStart.Value : 0;
                var count = request.count.HasValue ? request.count.Value : 30;
                var pagineOption = new PagingOptions<view_programmePileList>(pos, count, t => t.addtime ,true);

                var data = programmePileListRep.GetByConditonPage(predicate, pagineOption);

                response.datas = data;
                response.totalCount = pagineOption.TotalItems;
            }
            catch (Exception ex)
            {
                response.IsSucc = false;
                response.Msg = ex.Message;
            }

            return response;
        }


        public SenceSiteDataResponse GET(SenceSiteData request)
        {
            SenceSiteDataResponse response = new SenceSiteDataResponse()
            {
                IsSucc = true,
                Msg = string.Empty,
                datas = new List<ServiceModel.view_testingSite>()
            };

            try
            {
                var predicate = PredicateBuilder.True<view_testingSite>();
                #region  动态查询
                if (!request.customId.IsNullOrEmpty())
                {
                    predicate = predicate.And(ttt => ttt.customid == request.customId);
                }
                if (!request.projectName.IsNullOrEmpty())
                {
                    predicate = predicate.And(ttt => ttt.projectname.Contains(request.projectName));
                }
                if (!request.piletype.IsNullOrEmpty())
                {
                    predicate = predicate.And(ttt => ttt.piletype == request.piletype.Trim());
                }
                if (!request.areainfo.IsNullOrEmpty())
                {
                    predicate = predicate.And(ttt => ttt.areainfo == request.areainfo);
                }
                if (!request.testingequip.IsNullOrEmpty())
                {
                    predicate = predicate.And(ttt => ttt.testingequipment.Contains(request.testingequip));
                }
                if (!request.testingpeople.IsNullOrEmpty())
                {
                    predicate = predicate.And(ttt => ttt.testingpeople.Contains(request.testingpeople));
                }
                if (!request.testtype.IsNullOrEmpty())
                {
                    predicate = predicate.And(ttt => ttt.testtype.Contains(request.testtype));
                }
                #endregion

                var pos = request.posStart.HasValue ? request.posStart.Value : 0;
                var count = request.count.HasValue ? request.count.Value : 30;
                var pagineOption = new PagingOptions<view_testingSite>(pos, count, t => t.pid);

                var data = testSiterep.GetByConditonPage(predicate, pagineOption);

                response.datas = data;
                response.totalCount = pagineOption.TotalItems;
            }
            catch (Exception ex)
            {
                response.IsSucc = false;
                response.Msg = ex.Message;
            }

            return response;
        }

        public TestSiteDetailsResponse GET(TestSiteDetails request)
        {
            TestSiteDetailsResponse response = new TestSiteDetailsResponse()
            {
                IsSucc = true,
                Msg = string.Empty,
                datas = new List<TestSiteDetailsModel>()
            };

            try
            {
                //0 istesting
                //1 checknum
                //2 customid
                //3 checknum
                string str = "select a.*,b.checknum,b.projectname,b.testingequipment,b.reportcount,c.nn,isnull(d.num,0) as num from (select *,DATEDIFF(mi,LastSampleTime,UpdateTime) as difftime from Jy_BasicInfo where istesting='{0}' and SerialNo='{1}') a left join (select checknum,projectname,testingequipment,reportcount from view_programmePileList where 1=1 and customid='{2}' and checknum='{3}') b on a.SerialNo=b.checknum left join (select count(*) as nn,pileno,serialno from tab_zj_photoinfo where photopath!='' group by pileno,serialno) c on  a.pileno=c.pileno and a.SerialNo=c.serialno left join (SELECT * FROM view_updatenum) d on a.BasicInfoId=d.BasicInfoId order by a.starttime desc".Fmt(
                    request.IsTesting, request.CheckNum, request.CustomId, request.CheckNum);
                var datas = Db.SqlList<TestSiteDetailsModel>(str);

                response.datas = datas;
            }
            catch (Exception ex)
            {
                response.IsSucc = false;
                response.Msg = ex.Message;
            }


            return response;
        }


        public TestSiteDetailsDetailsResponse GET(TestSiteDetailsDetails request)
        {
            TestSiteDetailsDetailsResponse response = new TestSiteDetailsDetailsResponse()
            {
                IsSucc = true,
                Msg = string.Empty,
                hzb = string.Empty,
                jzjlb = string.Empty,
                xgjlb = string.Empty,
                xzjlb = string.Empty,
                ysjlb = string.Empty
            };

            try
            {
                if (request.BasicInfoId.HasValue)
                {
                    var hzbTable = new ZJTable();
                    var ysjlbTable = new ZJTable();
                    var jzjlbTable = new ZJTable();
                    var xzjlbTable = new ZJTable();
                    var xgjlbTable = new ZJTable();
                    var sql = "select * from Jy_DetailsData where BasicInfoId={0}".Fmt(request.BasicInfoId);
                    DataTable dtBase = SqlHelper.SqlHelper.SearchDataTable(sql);

                    JyDoc doc = new JyDoc(dtBase);

                    var hzbDs = doc.MakeGatherTable();
                    hzbTable.TrOneClass = "jcr2";
                    hzbTable.TrTwoClass = "jcr1";
                    hzbTable.ColorMouse = "#F9F9F9";
                    response.hzb = hzbTable.CreateJyTable(hzbDs, 0);

                    var ysjlbDs = doc.MakeSourceTable();
                    ysjlbTable.TrOneClass = "jcr2";
                    ysjlbTable.TrTwoClass = "jcr1";
                    ysjlbTable.ColorMouse = "#F9F9F9";
                    response.ysjlb = ysjlbTable.CreateJyTable(ysjlbDs, 0);

                    var jzjlbDs = doc.MakeLoadTable();
                    jzjlbTable.TrOneClass = "jcr2";
                    jzjlbTable.TrTwoClass = "jcr1";
                    jzjlbTable.ColorMouse = "#F9F9F9";
                    response.jzjlb = jzjlbTable.CreateJyTable(jzjlbDs, 0);


                    var xzjlbDs = doc.MakeUnloadTable();
                    xzjlbTable.TrOneClass = "jcr2";
                    xzjlbTable.TrTwoClass = "jcr1";
                    xzjlbTable.ColorMouse = "#F9F9F9";
                    response.xzjlb = xzjlbTable.CreateJyTable(xzjlbDs, 0);


                    var xgjlbDs = doc.MakeChangeTable();
                    xgjlbTable.TrOneClass = "jcr2";
                    xgjlbTable.TrTwoClass = "jcr1";
                    xgjlbTable.ColorMouse = "#F9F9F9";
                    response.xgjlb = xgjlbTable.CreateJyTable(xgjlbDs, 0);

                    response.QsImageBytes = doc.MakeQsImage();
                    response.SlgtImageBytes = doc.MakeSlgtImage();
                    response.SlgQImageBytes = doc.MakeSlgQImage();
                }
            }
            catch (Exception ex)
            {
                response.IsSucc = false;
                response.Msg = ex.Message;
            }

            return response;
        }

        public TestSiteDetailsDetailsDatasResponse GET(TestSiteDetailsDetailsDatas request)
        {
            TestSiteDetailsDetailsDatasResponse response = new TestSiteDetailsDetailsDatasResponse()
            {
                IsSucc = true,
                Msg = string.Empty
            };
            try
            {
                if (request.BasicInfoId.HasValue)
                {
                    var basicInfos = basicInfoRep.GetByCondition(t => t.Id == request.BasicInfoId);
                    if (basicInfos != null && basicInfos.Count > 0)
                    {
                        var basicInfo = basicInfos.First();
                        response.gpsisvalid = basicInfo.GpsIsValid;
                        response.gpslatitude = basicInfo.GpsLatitude;
                        response.gpslongitude = basicInfo.GpsLongitude;
                        response.currentparam = basicInfo.CurrentParam;
                        response.sourceparam = basicInfo.SourceParam;
                    }

                }
            }
            catch (Exception ex)
            {
                response.IsSucc = false;
                response.Msg = ex.Message;
            }

            return response;
        }

        public TestSiteDetailsDetailsTestLogResponse GET(TestSiteDetailsDetailsTestLog request)
        {
            TestSiteDetailsDetailsTestLogResponse response = new TestSiteDetailsDetailsTestLogResponse()
            {
                IsSucc = true,
                Msg = string.Empty,
                datas = new List<Jy_TestingLogInfo>()
            };

            try
            {
                if (request.BasicInfoId.HasValue)
                {
                    response.datas = testLogInfoRep.GetByCondition(t => t.BasicInfoId == request.BasicInfoId);
                }
            }
            catch (Exception ex)
            {
                response.IsSucc = false;
                response.Msg = ex.Message;
            }

            return response;
        }

        public TestSiteDetailsDetailsPhotoInfoResponse GET(TestSiteDetailsDetailsPhotoInfo request)
        {
            TestSiteDetailsDetailsPhotoInfoResponse response = new TestSiteDetailsDetailsPhotoInfoResponse()
            {
                IsSucc = true,
                Msg = string.Empty,
                datas = new List<view_pilephoto>()
            };

            try
            {
                if (!request.BasicInfoId.IsNullOrEmpty() && !request.PileNo.IsNullOrEmpty())
                {
                    response.datas = viewPilePhotoRep.GetByCondition(t => t.PileNo == request.PileNo && t.SerialNo == request.BasicInfoId);
                }
            }
            catch (Exception ex)
            {
                response.IsSucc = false;
                response.Msg = ex.Message;
            }

            return response;
        }

        public TestSiteDetailsDetailsProgrammeInfoResponse GET(TestSiteDetailsDetailsProgrammeInfo request)
        {
            TestSiteDetailsDetailsProgrammeInfoResponse response = new TestSiteDetailsDetailsProgrammeInfoResponse()
            {
                IsSucc = true,
                Msg = string.Empty,
                data = new view_programmePileList()
            };

            try
            {
                var datas = programmePileListRep.GetByCondition(t => t.checknum == request.CheckNum);
                if (datas != null && datas.Count > 0)
                {
                    response.data = datas.First();
                }
            }
            catch (Exception ex)
            {
                response.IsSucc = false;
                response.Msg = ex.Message;
            }

            return response;
        }




        public HistorySiteResponse GET(HistorySite request)
        {
            HistorySiteResponse response = new HistorySiteResponse()
            {
                IsSucc = true,
                Msg = string.Empty,
                datas = new List<view_testingHis>()
            };


            try
            {
                var predicate = PredicateBuilder.True<view_testingHis>();
                #region  动态查询
                if (!request.customId.IsNullOrEmpty())
                {
                    predicate = predicate.And(ttt => ttt.customid == request.customId);
                }
                if (!request.projectName.IsNullOrEmpty())
                {
                    predicate = predicate.And(ttt => ttt.projectname.Contains(request.projectName));
                }
                if (!request.piletype.IsNullOrEmpty())
                {
                    predicate = predicate.And(ttt => ttt.piletype == request.piletype.Trim());
                }
                if (!request.areainfo.IsNullOrEmpty())
                {
                    predicate = predicate.And(ttt => ttt.areainfo == request.areainfo);
                }
                if (!request.testingequip.IsNullOrEmpty())
                {
                    predicate = predicate.And(ttt => ttt.testingequipment.Contains(request.testingequip));
                }
                if (!request.testingpeople.IsNullOrEmpty())
                {
                    predicate = predicate.And(ttt => ttt.testingpeople.Contains(request.testingpeople));
                }
                if (!request.testtype.IsNullOrEmpty())
                {
                    predicate = predicate.And(ttt => ttt.testtype.Contains(request.testtype));
                }
                if (request.hasReport.HasValue)
                {
                    if (request.hasReport.Value) //有报告
                    {
                        predicate = predicate.And(ttt => ttt.reportcount > 0);
                    }

                    if (!request.hasReport.Value)
                    {
                        predicate = predicate.And(ttt => ttt.reportcount == 0);//无报告
                    }
                }
                #endregion
                var pos = request.posStart.HasValue ? request.posStart.Value : 0;
                var count = request.count.HasValue ? request.count.Value : 30;
                var pagineOption = new PagingOptions<view_testingHis>(pos, count, t => t.pid);


                var data = testingHisRep.GetByConditonPage(predicate, pagineOption);

                response.datas = data;
                response.totalCount = pagineOption.TotalItems;
            }
            catch (Exception ex)
            {
                response.IsSucc = false;
                response.Msg = ex.Message;
            }

            return response;
        }
    }


}
