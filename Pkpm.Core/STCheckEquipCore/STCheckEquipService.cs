using Pkpm.Entity;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Core.STCheckEquipCore
{
    public class STCheckEquipService: ISTCheckEquipService
    {
        IDbConnectionFactory dbFactory;

        public STCheckEquipService(IDbConnectionFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public bool EditCheckEquip(STCheckEquipEditServiceModel model, out string erroMsg)
        {
            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        erroMsg = string.Empty;
                        db.UpdateOnly(new t_bp_Equipment_ST
                        {
                            customid = model.Customid,
                            EquName = model.EquName,
                            equspec = model.equspec,
                            equtype = model.equtype,
                            checktime = model.checktime,
                            buytime = model.buyTime,
                            timeend = model.TimeEnd,
                            timestart = model.TimeStart
                        }, r => new
                        {
                            r.customid,
                            r.EquName,
                            r.equspec,
                            r.equtype,
                            r.checktime,
                            r.buytime,
                            r.timestart,
                            r.timeend
                        }, r => r.Id == int.Parse(model.id)
                            );
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

        public bool CreateCheckEquip(STCheckEquipEditServiceModel model, out string erroMsg)
        {

            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        erroMsg = string.Empty;
                        var STEquip = new t_bp_Equipment_ST()
                        {
                            customid = model.Customid,
                            EquName = model.EquName,
                            equspec = model.equspec,
                            equtype = model.equtype,
                            checkitem = model.checktime.ToString(),
                            buytime = model.buyTime,
                            timeend = model.TimeEnd,
                            timestart = model.TimeStart,
                            approvalstatus = "0",
                            Id=0
                        };
                        db.Insert(STEquip);
                       
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

        public bool DeleteEquip(string ids, out string erroMsg)
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
                            db.Delete<t_bp_Equipment_ST>(r => r.Id == Convert.ToInt32(siArray[i]));
                            db.Delete<t_bp_equItemSubItemList_ST>(r => r.equId == Convert.ToInt32(siArray[i]));
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
                            db.UpdateOnly(new t_bp_Equipment_ST { approvalstatus = state }, p => p.approvalstatus, p => p.Id ==int.Parse(siArray[i]));
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
                        db.UpdateOnly(() => new t_bp_Equipment_ST() { approvalstatus = "5" }, t => t.Id == int.Parse(Id));

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
                    db.UpdateOnly(() => new t_bp_Equipment_ST() { approvalstatus = STEuqipStatus }, t => t.Id ==int.Parse(Id));
                    return true;

                }
                catch (Exception ex)
                {
                    errorMsg = ex.Message;
                    return false;
                }
            }
        }
    }
}
