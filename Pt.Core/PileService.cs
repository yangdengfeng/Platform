using Pkpm.Entity;
using Pkpm.Entity.Models;
using Pkpm.Framework.Repsitory;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Pkpm.Core
{
    public interface IPileService
    {
        bool UploadZhuangji(pile_programme_model model, out string ErrorMsg);
        List<ZJSceneTestDetailModel> GetZJSceneTestDetailData(int INFO_STATUS, string checknum);
        bool CreateException(tab_pile_exception entity, out int id, out string errMsg);
        bool ApproveException(int Id, string handleContent, string handlePeople, out string errMsg);
        List<PeopleAndEquipInProgramme> GetZhuangji(string ProjectName, string checknum, string jzsynos, string area, int? posStart, int? count);
        bool UploadPileReportLink(string CheckNums, string SysPrimaryKey, out string ErrorMsg);
        ZJProjectGPSModel GetZJProjectGPS(string checknum, string stakeid, int infostatus);
        List<ZJExceptionModel> GetZJException(string checknum, string projectname, string stakeid, int? posStart, int? pageSize);
    }
    public class PileService : IPileService
    {
        IDbConnectionFactory dbFactory;
        IRepsitory<tab_pile_exception> exceptionRep;
        public PileService(IRepsitory<tab_pile_exception> exceptionRep, IDbConnectionFactory dbFactory)
        {
            this.dbFactory = dbFactory;
            this.exceptionRep = exceptionRep;
        }

        public bool UploadZhuangji(pile_programme_model model, out string ErrorMsg)
        {
            ErrorMsg = string.Empty;
            bool result = true;

            tab_pile_programme prog = model.ConvertTo<tab_pile_programme>();
            prog.testingequipment = model.testingequipment;
            prog.testingpeople = model.testingpeople;
            //prog.createtime = DateTime.Now;
            try
            {
                using (var db = dbFactory.Open())
                {
                    using (var trans = db.OpenTransaction())
                    {
                        try
                        {
                            List<string> equipids = model.testingequipment.Split(',').ToList();
                            var equipnums = db.Select<string>(db.From<t_bp_Equipment>().Where(w => equipids.Contains(w.id.ToString())).Select(t => t.equnum)); //db.Where<> //db.Select<t_bp_Equipment>(f => model.testingequipment.Contains(f.id.ToString()));
                            prog.testingequipment = equipnums.Join(",");
                            int ProgId = 0;
                            var ExistsQuery = db.From<tab_pile_programme>()
                                                .Where(w => w.checknum == model.checknum)
                                                .Select(s => s.Id);
                            var Exists = db.Select(ExistsQuery);
                            if (Exists != null && Exists.Count > 0)
                            {
                                #region 更新
                                ProgId = Exists.FirstOrDefault().Id;
                                prog.Id = ProgId;
                                db.Update<tab_pile_programme>(prog);
                                db.Delete<t_prog_people>(w => w.progid == ProgId);
                                db.Delete<t_prog_equip>(w => w.progid == ProgId);
                                #endregion
                            }
                            else
                            {
                                ProgId = (int)db.Insert<tab_pile_programme>(prog, true);
                            }
                            List<t_prog_people> peoples = null;
                            if (!string.IsNullOrEmpty(model.testingpeople))
                            {
                                peoples = new List<t_prog_people>();
                                var PeopleArr = model.testingpeople.Split(',');
                                foreach (var peoid in PeopleArr)
                                {
                                    t_prog_people people = new t_prog_people()
                                    {
                                        progid = ProgId,
                                        checknum = model.checknum,
                                        peopleid = int.Parse(peoid)
                                    };
                                    peoples.Add(people);
                                }
                            }
                            db.InsertAll<t_prog_people>(peoples);
                            List<t_prog_equip> equips = null;
                            if (!string.IsNullOrEmpty(model.testingpeople))
                            {
                                equips = new List<t_prog_equip>();
                                var EquipArr = model.testingequipment.Split(',');
                                foreach (var equipid in EquipArr)
                                {
                                    t_prog_equip equip = new t_prog_equip()
                                    {
                                        progid = ProgId,
                                        checknum = model.checknum,
                                        equipid = int.Parse(equipid)
                                    };
                                    equips.Add(equip);
                                }
                            }
                            db.InsertAll<t_prog_equip>(equips);
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

        public List<ZJSceneTestDetailModel> GetZJSceneTestDetailData(int INFO_STATUS, string checknum)
        {
            List<ZJSceneTestDetailModel> Datas = new List<ZJSceneTestDetailModel>();
            string strSql = @"SELECT prog.id, prog.checknum,proj.PROJECTNAME,start.TEST_TYPE,start.STAKE_ID,start.MACHINE_ID
FROM T_MSG_START start
JOIN tab_pile_programme prog ON start.FLOW_NO=prog.checknum 
LEFT JOIN dbo.t_bp_project proj ON prog.projectnum=proj.PROJECTNUM AND prog.unitcode=proj.UNITCODE
WHERE INFO_STATUS=@infostatus and prog.checknum=@checknum";
            using (var db = dbFactory.Open())
            {
                List<IDbDataParameter> Params = new List<IDbDataParameter>();
                Params.Add(db.CreateParam("infostatus", INFO_STATUS, dbType: DbType.Int16));
                Params.Add(db.CreateParam("checknum", checknum, dbType: DbType.String));
                Datas = db.Select<ZJSceneTestDetailModel>(strSql, Params).Skip(0).Take(30).ToList();
            }
            return Datas;
        }
        public bool CreateException(tab_pile_exception entity, out int id, out string errMsg)
        {
            errMsg = string.Empty;
            id = 0;

            try
            {
                id = (int)exceptionRep.Insert(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }
        }

        public bool ApproveException(int Id, string handleContent, string handlePeople, out string errMsg)
        {
            errMsg = string.Empty;
            try
            {

                exceptionRep.UpdateOnly(new tab_pile_exception()
                {
                    handleContent = handleContent,
                    handlePeople = handlePeople,
                    handleTime = DateTime.Now
                }, t => t.Id == Id, t => new { t.handleContent, t.handlePeople, t.handleTime });
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }
        }

        public List<PeopleAndEquipInProgramme> GetZhuangji(string ProjectName, string checknum, string jzsynos, string area, int? posStart, int? count)
        {
            var predicate = PredicateBuilder.True<tab_pile_programme>();
            if (!ProjectName.IsNullOrEmpty())
            {
                predicate = predicate.And(tt => tt.projectname.Contains(ProjectName));
            }
            if (!checknum.IsNullOrEmpty())
            {
                predicate = predicate.And(tt => tt.checknum == checknum);
            }
            if (!jzsynos.IsNullOrEmpty())
            {
                predicate = predicate.And(tt => tt.jzsynos == jzsynos);
            }
            if (!area.IsNullOrEmpty())
            {
                predicate = predicate.And(tt => tt.Area == area);
            }
            var ZhuangJis = new List<PeopleAndEquipInProgramme>();

            int pos = posStart.HasValue ? posStart.Value : 0;
            int count1 = count.HasValue ? count.Value : 30;
            using (var db = dbFactory.Open())
            {
                var zhuangJis = db.Select(db.From<tab_pile_programme>().Where(predicate).OrderBy(t => t.Id).Select(t => new
                {
                    t.Id,
                    t.unitcode,
                    t.projectnum,
                    t.checknum,
                    t.projectname,
                    t.planenddate,
                    t.planstartdate,
                    t.basictype,
                    t.structuretype,
                    t.piletype,
                    t.elevation,
                    t.pilenum,
                    t.eigenvalues,
                    t.pilelenght,
                    t.concretestrength,
                    t.jzsynum,
                    t.jzsynos,
                    t.Area
                }).Limit(pos, count1));

                var proIds = zhuangJis.Select(t => t.Id).ToList();

                //从人员表和设备表查出所有包括在查出来的方案中的设备和人员信息
                //不能用dictory，因为key有重复的
                var allPeople = db.Select(db.From<t_prog_people>().Where(t => proIds.Contains(t.progid)));
                var allEquip = db.Select(db.From<t_prog_equip>().Where(t => proIds.Contains(t.progid)));

                var allPoepleIds = allPeople.Select(t => t.peopleid).ToList();
                var allequipIds = allEquip.Select(t => t.equipid).ToList();

                var allPeoples = db.Dictionary<int, string>(db.From<t_bp_People>().Where(t => allPoepleIds.Contains(t.id)).Select(t => new { t.id, t.Name })); //所有的人员信息
                var allEquips = db.Dictionary<int, string>(db.From<t_bp_Equipment>().Where(t => allequipIds.Contains(t.id)).Select(t => new { t.id, t.EquName })); //所有的设备信息


                foreach (var item in zhuangJis)
                {
                    PeopleAndEquipInProgramme oneZhuangJi = new PeopleAndEquipInProgramme()
                    {
                        ZhuangJi = item,
                        Peoples = new Dictionary<int, string>(),
                        Equips = new Dictionary<int, string>()
                    };

                    var peopleInOneProIds = allPeople.Where(t => t.progid == item.Id).Select(t => t.peopleid).ToList();//一个方案中的所有人员ID
                    var equipInOneProIds = allEquip.Where(t => t.progid == item.Id).Select(t => t.equipid).ToList();//一个方案中的所有设备ID

                    foreach (var peopleInOneProId in peopleInOneProIds)
                    {
                        string peopleName = string.Empty;
                        allPeoples.TryGetValue(peopleInOneProId, out peopleName);
                        oneZhuangJi.Peoples.Add(peopleInOneProId, peopleName);
                    }

                    foreach (var equipInOneProId in equipInOneProIds)
                    {
                        string equipName = string.Empty;
                        allEquips.TryGetValue(equipInOneProId, out equipName);
                        oneZhuangJi.Equips.Add(equipInOneProId, equipName);
                    }
                    ZhuangJis.Add(oneZhuangJi);
                }
                //foreach (var item in zhuangJis)
                //{
                //    PeopleAndEquipInProgramme oneZhuangJi = new PeopleAndEquipInProgramme()
                //    {
                //        ZhuangJi = item,
                //        Peoples = new List<t_prog_people>(),
                //        Equips = new List<t_prog_equip>()
                //    };
                //    var peoples = allPeoples.Where(t => t.progid == item.Id).ToList();
                //    var equips = allEquips.Where(t => t.progid == item.Id).ToList();
                //    oneZhuangJi.Peoples = peoples;
                //    oneZhuangJi.Equips = equips;

                //    ZhuangJis.Add(oneZhuangJi);
                //}
            }

            return ZhuangJis;
        }

        public bool UploadPileReportLink(string CheckNums, string SysPrimaryKey, out string ErrorMsg)
        {
            ErrorMsg = string.Empty;
            bool result = false;
            try
            {
                using (var db = dbFactory.Open())
                {
                    t_PileReportLink model = new Entity.t_PileReportLink()
                    {
                        CheckNums = CheckNums,
                        SysPrimaryKey = SysPrimaryKey,
                        UploadDt = DateTime.Now
                    };
                    db.Insert(model);
                }
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.Message;
                result = true;
            }
            return result;
        }

        public ZJProjectGPSModel GetZJProjectGPS(string checknum, string stakeid, int infostatus)
        {

            ZJProjectGPSModel Data = new ZJProjectGPSModel();
            string strSql = @"SELECT  a.STAKE_ID, b.Area,a.LATITUDE,a.LONGITUDE, b.checknum,b.projectnum,b.projectname FROM T_MSG_START a 
INNER JOIN tab_pile_programme b ON a.FLOW_NO=b.checknum
WHERE a.INFO_STATUS=@infostatus AND b.checknum=@checknum AND a.STAKE_ID=@stakeid";
            List<IDbDataParameter> Params = new List<IDbDataParameter>();
            using (var db = dbFactory.Open())
            {
                Params.Add(db.CreateParam("infostatus", infostatus, dbType: DbType.Int32));
                Params.Add(db.CreateParam("checknum", checknum, dbType: DbType.String));
                Params.Add(db.CreateParam("stakeid", stakeid, dbType: DbType.String));

                var Datas = db.Select<ZJProjectGPSModel>(strSql, Params).ToList();
                if (Datas != null && Datas.Count > 0)
                {
                    Data = Datas.First();
                }
            }
            return Data;
        }

        public List<ZJExceptionModel> GetZJException(string checknum, string projectname, string stakeid, int? posStart, int? pageSize)
        {
            string SqlWhere = string.Empty;
            int pos = posStart.HasValue ? posStart.Value : 0;
            int count = pageSize.HasValue ? pageSize.Value > 100 ? 100 : pageSize.Value : 30;
            List<ZJExceptionModel> Datas = new List<ZJExceptionModel>();

            using (var db = dbFactory.Open())
            {
                List<IDbDataParameter> Params = new List<IDbDataParameter>();
                if (!string.IsNullOrEmpty(checknum))
                {
                    SqlWhere += " AND a.checknum=@checknum ";
                    Params.Add(db.CreateParam("checknum", checknum, dbType: DbType.String));
                }
                if (!string.IsNullOrEmpty(projectname))
                {
                    SqlWhere += " AND b.projectname like @projectname ";
                    Params.Add(db.CreateParam("projectname", "%" + projectname + "%", dbType: DbType.String));
                }
                if (!string.IsNullOrEmpty(stakeid))
                {
                    SqlWhere += " AND a.pileno=@stakeid ";
                    Params.Add(db.CreateParam("stakeid", stakeid, dbType: DbType.String));
                }


                string strSql = @"SELECT a.unitcode,a.projectnum,a.checknum,a.pileno,a.people,a.content,a.handleContent,a.handlePeople,b.PROJECTNAME,c.NAME AS customname FROM tab_pile_exception a
LEFT JOIN dbo.t_bp_project b ON a.projectnum=b.PROJECTNUM
LEFT JOIN dbo.t_bp_custom c ON a.unitcode=c.ID
WHERE 1=1 {0} ";
                strSql = string.Format(strSql, SqlWhere);
                Datas = db.Select<ZJExceptionModel>(strSql, Params).Skip(pos).Take(count).ToList();
            }
            return Datas;
        }
    }
}