using QZWebService.ServiceInterface.Repsitory;
using QZWebService.ServiceModel;
using ServiceStack;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QZWebService.ServiceInterface
{
    public  class XCService :Service
    {
        IRepsitory<view_projectinfo> projectinfoRep;
        IRepsitory<tab_custominfo> custominfoRep;
        IRepsitory<view_programmeSecneList> programmeSecneListRep;
        IRepsitory<tab_xc_report> reportRep;
        IRepsitory<t_bp_item> itemRep;
        public XCService(IRepsitory<view_projectinfo> projectinfoRep,
        IRepsitory<tab_custominfo> custominfoRep,
               IRepsitory<t_bp_item> itemRep,
        IRepsitory<view_programmeSecneList> programmeSecneListRep, 
        IRepsitory<tab_xc_report> reportRep)
        {
            this.projectinfoRep = projectinfoRep;
            this.custominfoRep = custominfoRep;
            this.programmeSecneListRep = programmeSecneListRep;
            this.itemRep = itemRep;
            this.reportRep = reportRep;
        }

        public GetXCProjectInfosResponse Get(GetXCProjectInfos request)
        {
            GetXCProjectInfosResponse response = new GetXCProjectInfosResponse()
            {
                IsSucc = true,
                Msg = string.Empty,
                datas = new List<ServiceModel.view_projectinfo>()
            };
            try
            {
                var predicate = PredicateBuilder.True<view_projectinfo>();
                if (!request.customname.IsNullOrEmpty())
                {
                    predicate = predicate.And(ttt => ttt.customid == request.customname);
                }
                if (!request.projectname.IsNullOrEmpty())
                {
                    predicate = predicate.And(ttt => ttt.projectname.Contains(request.projectname));
                }
                if (!request.areainfo.IsNullOrEmpty())
                {
                    predicate = predicate.And(ttt => ttt.areainfo == request.areainfo);
                }
                if (request.StartDate.HasValue)
                {
                    predicate = predicate.And(t => t.addtime.HasValue && t.addtime.Value >= request.StartDate.Value);
                }
                if (request.EndDate.HasValue)
                {
                    predicate = predicate.And(t => t.addtime.HasValue && t.addtime.Value <= request.EndDate.Value);
                }
                var pos = request.posStart.HasValue ? request.posStart.Value : 0;
                var count = request.count.HasValue ? request.count.Value : 30;
                var pagineOption = new PagingOptions<view_projectinfo>(pos, count, t => t.addtime, true);

                response.datas = projectinfoRep.GetByConditonPage(predicate, pagineOption);
                
                response.totalCount = pagineOption.TotalItems;
            }
            catch (Exception ex)
            {
                response.IsSucc = false;
                response.Msg = ex.Message;
            }

            return response;
        }

        public GetProjectInfosResponse Get(GetProjectInfos request)
        {
            GetProjectInfosResponse response = new GetProjectInfosResponse()
            {
                IsSucc = true,
                Msg = string.Empty,
                project = new view_projectinfo()
            };
            try
            {
                response.project = projectinfoRep.GetById(request.id);
            }
            catch (Exception ex)
            {
                response.IsSucc = false;
                response.Msg = ex.Message;
            }
            return response;
        }

        public GetprogrammeSecneListsResponse Get(GetprogrammeSecneLists request)
        {
            GetprogrammeSecneListsResponse response = new GetprogrammeSecneListsResponse()
            {
                IsSucc = true,
                Msg = string.Empty,
                datas = new List<view_programmeSecneList>()
            };
            try
            {
                var predicate = PredicateBuilder.True<view_programmeSecneList>();
                if (!request.customname.IsNullOrEmpty())
                {
                    predicate = predicate.And(ttt => ttt.customid == request.customname);
                }
                if (!request.projectname.IsNullOrEmpty())
                {
                    predicate = predicate.And(ttt => ttt.projectname.Contains(request.projectname));
                }
                if (!request.areainfo.IsNullOrEmpty())
                {
                    predicate = predicate.And(ttt => ttt.areainfo == request.areainfo);
                }
                if(!request.testingpeople.IsNullOrEmpty())
                {
                    predicate = predicate.And(ttt => ttt.testingpeople.Contains(request.testingpeople));
                }
                if(!request.IsReport.IsNullOrEmpty())
                {
                    if (request.IsReport == "true")
                    {
                        predicate = predicate.And(ttt => ttt.reportcount > 0);
                    }
                    else
                    {
                        predicate = predicate.And(ttt => ttt.reportcount == 0);
                    }
                    
                }
                if(!request.IsPhoto.IsNullOrEmpty())
                {
                    if (request.IsPhoto == "true")
                    {
                        predicate = predicate.And(ttt => ttt.photoid.HasValue && ttt.photoid > 0);
                    }
                    else
                    {
                        predicate = predicate.And(ttt => ttt.photoid >= 0);
                    }
                }
                if (request.StartDate.HasValue)
                {
                    predicate = predicate.And(t => t.addtime.HasValue && t.addtime.Value >= request.StartDate.Value);
                }
                if (request.EndDate.HasValue)
                {
                    predicate = predicate.And(t => t.addtime.HasValue && t.addtime.Value <= request.EndDate.Value);
                }


                var pos = request.posStart.HasValue ? request.posStart.Value : 0;
                var count = request.count.HasValue ? request.count.Value : 30;
                var pagineOption = new PagingOptions<view_programmeSecneList>(pos, count, t => t.addtime, true);

                response.datas = programmeSecneListRep.GetByConditonPage(predicate, pagineOption);

                response.totalCount = pagineOption.TotalItems;
            }
            catch (Exception ex)
            {
                response.IsSucc = false;
                response.Msg = ex.Message;
            }
            return response;

        }

        public GetPSLsResponse Get(GetPSLs request)
        {
            GetPSLsResponse response = new GetPSLsResponse()
            {
                IsSucc = true,
                Msg = string.Empty,
                project = new view_programmeSecneList()
            };
            try
            {
                response.project = programmeSecneListRep.GetById(request.id);
            }
            catch (Exception ex)
            {
                response.IsSucc = false;
                response.Msg = ex.Message;
            }
            return response;
        }


        public GetReportsResponse Get(GetReports request)
        {
            GetReportsResponse response = new GetReportsResponse()
            {
                IsSucc = true,
                Msg = string.Empty,
                Reports = new List<tab_xc_report>()
            };
            try
            {
                response.Reports = reportRep.GetByCondition(t => t.checknum == request.checknum);
            }
            catch (Exception ex)
            {
                response.IsSucc = false;
                response.Msg = ex.Message;
            }
            return response;
        }

        public XCGetSysprimaryByReportNumResponse GET(XCGetSysprimaryByReportNum request)
        {
            XCGetSysprimaryByReportNumResponse response = new XCGetSysprimaryByReportNumResponse()
            {
                IsSucc = true,
                Msg = string.Empty,
                SysPrimaryKey = string.Empty
            };

            try
            {
                var sysPrimaryKey = itemRep.GetByConditon<string>(ttt => ttt.unitCode == request.customId.Trim() && ttt.REPORTNUM == request.reportNum.Trim(), t => new { t.Id });
                if (sysPrimaryKey != null && sysPrimaryKey.Count() > 0)
                {
                    response.SysPrimaryKey = sysPrimaryKey.First();
                }
            }
            catch (Exception ex)
            {
                response.Msg = ex.Message;
                response.IsSucc = false;
            }

            return response;
        }
    }
}
