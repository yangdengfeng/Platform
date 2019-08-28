using Pkpm.Entity;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Core.HtyService
{
    public interface IHtyService
    {
        bool UploadZNHTY(tab_hty_programme model, out string ErrorMsg);
        bool UploadZNHTYGJ(ZNHTYGjDataModel model, out string ErrorMsg);
        List<HTYGjViewModel> GetHTYGjData(HTYGjSearchModel model);
    }
    public class HtyService: IHtyService
    {
        IDbConnectionFactory dbFactory;
        public HtyService(IDbConnectionFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }
        public bool UploadZNHTY(tab_hty_programme model, out string ErrorMsg)
        {
            ErrorMsg = string.Empty;
            bool result = true;

            tab_hty_programme prog = model;

            try
            {
                using (var db = dbFactory.Open())
                {
                    int ProgId = 0;
                    var ExistsQuery = db.From<tab_hty_programme>()
                                        .Where(w => w.checknum == model.checknum)
                                        .Select(s => s.id);
                    var Exists = db.Select(ExistsQuery);
                    if (Exists != null && Exists.Count > 0)
                    {
                        #region 更新
                        ProgId = Exists.FirstOrDefault().id;
                        prog.id = ProgId;
                        db.Update<tab_hty_programme>(prog);
                        #endregion
                    }
                    else
                    {
                        db.Insert<tab_hty_programme>(prog);
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                ErrorMsg = ex.Message;
            }
            return result;
        }

        public bool UploadZNHTYGJ(ZNHTYGjDataModel model, out string ErrorMsg)
        {
            tab_hty_gj gj = model.ConvertTo<tab_hty_gj>();
            List<tab_hty_gjcq> listgjcq = model.gjcqDatas;
            ErrorMsg = string.Empty;
            bool result = true;

            try
            {
                using (var db = dbFactory.Open())
                {
                    using (var trans = db.OpenTransaction())
                    {
                        try
                        {
                            int gjId = 0;
                            var ExistsQuery = db.From<tab_hty_gj>()
                                                .Where(w => w.checknum == model.checknum && w.gjNo == model.gjNo)
                                                .Select(s => s.id);
                            var Exists = db.Select(ExistsQuery);
                            if (Exists != null && Exists.Count > 0)
                            {
                                #region 更新
                                gjId = Exists.FirstOrDefault().id;
                                gj.id = gjId;
                                db.Update<tab_hty_gj>(gj);
                                db.Delete<tab_hty_gjcq>(w => w.progid == gjId);
                                #endregion
                            }
                            else
                            {
                                gjId = (int)db.Insert<tab_hty_gj>(gj, true);
                            }
                            if (listgjcq != null && listgjcq.Count > 0)
                            {
                                foreach (var gjcq in listgjcq)
                                {
                                    gjcq.gjid = gjId;
                                    gjcq.progid = model.progid;
                                }
                                db.InsertAll<tab_hty_gjcq>(listgjcq);
                            }
                            trans.Commit();
                        }
                        catch (Exception)
                        {
                            trans.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                ErrorMsg = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 取回弹仪构件数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<HTYGjViewModel> GetHTYGjData(HTYGjSearchModel model)
        {
            string SqlWhere = string.Empty;

            List<HTYGjViewModel> Datas = new List<HTYGjViewModel>();

            using (var db = dbFactory.Open())
            {
                List<IDbDataParameter> Params = new List<IDbDataParameter>();
                if (model.gjid.HasValue)
                {
                    SqlWhere += " AND gj.id=@id ";
                    Params.Add(db.CreateParam("id", model.gjid, dbType: DbType.Int32));
                }
                if (!string.IsNullOrEmpty(model.CustomId))
                {
                    SqlWhere += " AND prog.unitcode=@unitcode ";
                    Params.Add(db.CreateParam("unitcode", model.CustomId, dbType: DbType.String));
                }
                if (!string.IsNullOrEmpty(model.AreaName))
                {
                    SqlWhere += " AND prog.area=@area ";
                    Params.Add(db.CreateParam("area", model.AreaName, dbType: DbType.String));
                }
                if (!string.IsNullOrEmpty(model.ProjectName))
                {
                    SqlWhere += " AND prog.projectname=@ProjectName ";
                    Params.Add(db.CreateParam("ProjectName", model.ProjectName, dbType: DbType.String));
                }


                string strSql = @"SELECT prog.checknum,prog.unitcode,proj.PROJECTNAME,prog.area,proj.PROJECTADDRESS,prog.testingpeople,gj.id,gj.gjcqNum,gj.gjNo,gj.gjName,gj.tjjd,gj.jzm,gj.bsfs,gj.cqqxNo,gj.thms,gj.maxTh,gj.checkTime,gj.htyType,gj.htyNo,gj.minTd,gj.gjqdTd,gj.Floor,gj.BuildingNum,gj.hnttdz,gj.hntbh,gj.hg,gj.ShiGongDuiName,gj.avgth,COUNT(img.Id) AS imgcount
FROM tab_hty_gj gj
JOIN tab_hty_programme prog ON gj.checknum=prog.checknum 
LEFT JOIN dbo.t_bp_project proj ON prog.projectnum=proj.PROJECTNUM AND prog.unitcode=proj.UNITCODE
LEFT JOIN dbo.t_hty_Image img ON  prog.checknum=img.CheckNum AND gj.gjNo=img.gjNo
WHERE 1=1 {0} 
GROUP BY prog.id,prog.checknum,prog.unitcode,proj.PROJECTNAME,prog.area,proj.PROJECTADDRESS,prog.testingpeople,gj.id,gj.gjcqNum,gj.gjNo,gj.gjName,gj.tjjd,gj.jzm,gj.bsfs,gj.cqqxNo,gj.thms,gj.maxTh,gj.checkTime,gj.htyType,gj.htyNo,gj.minTd,gj.gjqdTd,gj.Floor,gj.BuildingNum,gj.hnttdz,gj.hntbh,gj.hg,gj.ShiGongDuiName,gj.avgth";
                strSql = string.Format(strSql, SqlWhere);
                Datas = db.Select<HTYGjViewModel>(strSql, Params).Skip(0).Take(30).ToList();
            }
            return Datas;
        }
    }
}
