using Pkpm.Entity;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Core.STCheckPeopleCore
{
    public class STCheckPeopleService: ISTCheckPeopleService
    {
        IDbConnectionFactory dbFactory;

        public STCheckPeopleService(IDbConnectionFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public bool DeletePeople(string ids, out string erroMsg)
        {

            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        erroMsg = string.Empty;
                        string[] siArray = ids.Split(',');
                        for (int i = 0; i < siArray.Length; i++)
                        {
                            db.Delete<t_bp_People_ST>(r => r.Id == Convert.ToInt32(siArray[i]));
                            //db.Delete<t_bp_equItemSubItemList_ST>(r => r.equId == Convert.ToInt32(siArray[i]));
                        }
                        dbTrans.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        dbTrans.Rollback();
                        erroMsg = ex.Message;
                        return false;
                    }
                }
            }
        }

        public bool SetInstSendState(string selectedId, string state, out string erroMsg)
        {
            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        erroMsg = string.Empty;
                        string[] siArray = selectedId.Split(',');
                        for (int i = 0; i < siArray.Length; i++)
                        {
                            db.UpdateOnly(new t_bp_People_ST { Approvalstatus = state }, p => p.Approvalstatus, p => p.Id == int.Parse(siArray[i]));
                        }
                        dbTrans.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        dbTrans.Rollback();
                        erroMsg = ex.Message;
                        return false;
                    }
                }
            }
        }

        public bool CanApplyChangeCustom(string approvalstatus)
        {
            if (approvalstatus.IsNullOrEmpty())
            {
                return false;
            }

            if (approvalstatus == "1" || approvalstatus == "7")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ApplyChangeForCustom(SupvisorJob job, string Id, out string errorMsg)
        {
            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        errorMsg = string.Empty;

                        db.Insert<SupvisorJob>(job);

                        //5 申请修改
                        db.UpdateOnly(() => new t_bp_People_ST() { Approvalstatus = "5" }, t => t.Id == int.Parse(Id));

                        dbTrans.Commit();
                        return true;

                    }
                    catch (Exception ex)
                    {
                        dbTrans.Rollback();
                        errorMsg = ex.Message;
                        return false;
                    }
                }
            }
        }

        public bool UpdateCustomStatus(string Id, string STEuqipStatus, string Reason, out string errorMsg)
        {
            using (var db = dbFactory.Open())
            {

                try
                {
                    errorMsg = string.Empty;
                    db.UpdateOnly(() => new t_bp_People_ST() { Approvalstatus = STEuqipStatus }, t => t.Id == int.Parse(Id));
                    return true;

                }
                catch (Exception ex)
                {
                    errorMsg = ex.Message;
                    return false;
                }
            }
        }

        public bool EditPeople(STCheckPeopleSaveModel model, out string erroMsg)
        {
            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        erroMsg = string.Empty;
                        db.UpdateOnly(model.People, r => new
                        {
                            r.Name,
                            r.Customid,
                            r.Sex,
                            r.Birthday,
                            r.SelfNum,
                            r.PostNum,
                            r.ishaspostnum,
                            r.postnumstartdate,
                            r.postnumenddate,
                            r.School,
                            r.Education,
                            r.Title,
                            r.Professional,
                            r.Tel,
                            r.zw,
                            r.Email,
                            r.SBNum,
                            r.isjs,
                            r.ismanager,
                            r.issy,
                            r.PostType,
                            r.postDelayReg,
                            r.selfnumPath,
                            r.educationpath,
                            r.PhotoPath,
                            r.PostPath,
                            r.sbnumpath
                        },r=>r.Id==model.People.Id);
                        db.Delete<t_bp_PeoAwards_ST>(r => r.PeopleId == model.People.Id);
                        for (int i = 0; i < model.PeoAward.Count; i++)
                        {
                            model.PeoAward[i].Id = 0;
                            db.Insert(model.PeoAward[i], true);
                        }

                        db.Delete<t_bp_PeoChange_ST>(r => r.PeopleId == model.People.Id);

                        for (int i = 0; i < model.PeoChange.Count; i++)
                        {
                            model.PeoChange[i].Id = 0;
                            db.Insert(model.PeoChange[i], true);
                        }
                        db.Delete<t_bp_PeoPunish_ST>(r => r.PeopleId == model.People.Id);
                        for (int i = 0; i < model.PeoPunish.Count; i++)
                        {
                            model.PeoPunish[i].Id = 0;
                            db.Insert(model.PeoPunish[i], true);
                        }

                        db.Delete<t_bp_PeoEducation_ST>(r => r.PeopleId == model.People.Id);
                        for (int i = 0; i < model.PeoEducation.Count; i++)
                        {
                            model.PeoEducation[i].Id = 0;
                            db.Insert(model.PeoEducation[i], true);
                        }
                        dbTrans.Commit();
                        return true;
                    }
                    catch(Exception ex)
                    {
                        dbTrans.Rollback();
                        erroMsg = ex.Message;
                        return false;
                    }
                }
            }
        }

        public bool CreatePeople(STCheckPeopleSaveModel model, out string erroMsg)
        {
            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        erroMsg = string.Empty;
                        var peopleId = db.Insert(model.People, true);
                        for (int i = 0; i < model.PeoAward.Count; i++)
                        {
                            model.PeoAward[i].PeopleId = (int)peopleId;
                            db.Insert(model.PeoAward[i], true);
                        }
                        for (int i = 0; i < model.PeoChange.Count; i++)
                        {
                            model.PeoChange[i].PeopleId = (int)peopleId;
                            db.Insert(model.PeoChange[i], true);
                        }
                        for (int i = 0; i < model.PeoPunish.Count; i++)
                        {
                            model.PeoPunish[i].PeopleId = (int)peopleId;
                            db.Insert(model.PeoPunish[i], true);
                        }
                        for (int i = 0; i < model.PeoEducation.Count; i++)
                        {
                            model.PeoEducation[i].PeopleId = (int)peopleId;
                            db.Insert(model.PeoEducation[i], true);
                        }
                        dbTrans.Commit();
                        return true;
                    }
                    catch(Exception ex)
                    {
                        dbTrans.Rollback();
                        erroMsg = ex.Message;
                        return false;
                    }
                }
            }
        }
    }
}
