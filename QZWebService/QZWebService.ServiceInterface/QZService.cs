using QZWebService.ServiceInterface.Repsitory;
using QZWebService.ServiceModel;
using ServiceStack;
using ServiceStack.Caching;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QZWebService.ServiceInterface
{
    public class QZService : Service
    {
        //IDbConnectionFactory dbFactory;
        //ICacheClient cacheClient;
        IRepsitory<tab_qrinfo> rep;
        IRepsitory<view_programmeLiftList> programRep;
        IRepsitory<tab_qz_report> reportRep;
        IRepsitory<t_bp_item> itemRep;


        public QZService(IRepsitory<tab_qrinfo> rep, IRepsitory<view_programmeLiftList> programRep, IRepsitory<tab_qz_report> reportRep, IRepsitory<t_bp_item> itemRep)
        {
            this.rep = rep;
            this.programRep = programRep;
            this.reportRep = reportRep;
            this.itemRep = itemRep;
        }

        public QzQrcodeBarsResponse Get(QzQrcodeBars request)
        {
            QzQrcodeBarsResponse response = new QzQrcodeBarsResponse();
            response.IsSucc = true;
            try
            {
                Dictionary<string, string> qrinfoAndEntrs = new Dictionary<string, string>();

                qrinfoAndEntrs = rep.GetDictByCondition<string, string>(t => request.entrNums.Contains(t.liftno), r => new { r.liftno, r.qrinfo });
                response.Datas = qrinfoAndEntrs;
            }
            catch (Exception ex)
            {
                response.IsSucc = false;
                response.Msg = ex.Message;
            }
            return response;
        }

        public QzQrcodeBarResponse Get(QzQrcodeBar request)
        {
            QzQrcodeBarResponse response = new QzQrcodeBarResponse();
            response.IsSucc = true;
            try
            {

                var qrinfo = rep.GetByCondition(t => t.liftno == request.entrNum);
                if (qrinfo != null && qrinfo.Count > 0)
                {
                    response.qrinfo = qrinfo.First().qrinfo;
                }
            }
            catch (Exception ex)
            {
                response.IsSucc = false;
                response.Msg = ex.Message;
            }
            return response;
        }

        public GetProgrammeResponse Get(GetProgramme request)
        {
            GetProgrammeResponse response = new GetProgrammeResponse()
            {
                IsSucc = true,
                Msg = string.Empty,
                programe = new view_programmeLiftList()
            };

            try
            {
                var programme = programRep.GetByCondition(t => t.qrinfo == request.qrinfo);
                if (programme != null && programme.Count > 0)
                {
                    response.programe = programme.First();
                }
            }
            catch (Exception ex)
            {
                response.IsSucc = false;
                response.Msg = ex.Message;
            }

            return response;
        }

        public GetListProgrammeResponse GET(GetListProgramme request)
        {
            GetListProgrammeResponse response = new GetListProgrammeResponse()
            {
                IsSucc = true,
                Msg = string.Empty,
                programes = new List<view_programmeLiftList>()
            };

            try
            {
                var predicate = PredicateBuilder.True<view_programmeLiftList>();

                #region 动态查询
                if (request.IsPhoto)
                {
                    //predicate = predicate.And(ttt => !ttt.qrpath.IsNullOrEmpty());
                    //predicate = predicate.And(ttt => !ttt.peoplepath.IsNullOrEmpty());
                    //predicate = predicate.And(ttt => !ttt.photopath.IsNullOrEmpty());
                    predicate = predicate.And(ttt => ttt.photoid.HasValue && ttt.photoid > 0);
                }
                if (request.IsReport)
                {
                    predicate = predicate.And(ttt => ttt.reportcount > 0);
                }
                if (!request.projectname.IsNullOrEmpty())
                {
                    predicate = predicate.And(ttt => ttt.projectname.Contains(request.projectname.Trim()));
                }
                if (!request.testpeople.IsNullOrEmpty())
                {
                    predicate = predicate.And(ttt => ttt.testingpeople.Contains(request.testpeople));
                }
                if (!request.customId.IsNullOrEmpty())
                {
                    predicate = predicate.And(ttt => ttt.customid == request.customId);
                }
                if (!request.areainfo.IsNullOrEmpty())
                {
                    predicate = predicate.And(ttt => ttt.areainfo == request.areainfo);
                }
                if(request.StartDate.HasValue)
                {
                    predicate = predicate.And(t => t.addtime.HasValue && t.addtime.Value >= request.StartDate.Value);
                }
                if (request.EndDate.HasValue)
                {
                    predicate = predicate.And(t => t.addtime.HasValue && t.addtime.Value <= request.EndDate.Value);
                }
                #endregion

                int pos = request.posStart.HasValue ? request.posStart.Value : 0;
                int count = request.count.HasValue ? request.count.Value : 30;

                var pageOption = new PagingOptions<view_programmeLiftList>(pos, count, t=>t.addtime,true);

                var datas = programRep.GetByConditonPage(predicate, pageOption);
                response.programes = datas;
                response.totalCount = pageOption.TotalItems;

            }
            catch (Exception ex)
            {
                response.IsSucc = false;
                response.Msg = ex.Message;
            }

            return response;
        }

        public GetProgrammeReportsResponse GET(GetProgrammeReports request)
        {
            GetProgrammeReportsResponse response = new GetProgrammeReportsResponse()
            {
                IsSucc = true,
                Msg = string.Empty,
                reports = new List<tab_qz_report>()
            };

            try
            {
                var predicate = PredicateBuilder.True<tab_qz_report>();
                if (!request.projectnum.IsNullOrEmpty())
                {
                    predicate = predicate.And(ttt => ttt.projectnum == request.projectnum.Trim());
                }
                if (!request.checknum.IsNullOrEmpty())
                {
                    predicate = predicate.And(ttt => ttt.checknum == request.checknum.Trim());
                }
                var datas = reportRep.GetByCondition(predicate);
                response.reports = datas;
            }
            catch (Exception ex)
            {
                response.IsSucc = false;
                response.Msg = ex.Message;
            }

            return response;
        }

        public GetSysprimaryByReportNumResponse GET(GetSysprimaryByReportNum request)
        {
            GetSysprimaryByReportNumResponse response = new GetSysprimaryByReportNumResponse()
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
