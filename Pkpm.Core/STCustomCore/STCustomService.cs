using Pkpm.Entity;
using Pkpm.Framework.Cache;
using Pkpm.Framework.Repsitory;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Core.STCustomCore
{
    public class STCustomService:ISTCustomService
    {
        IDbConnectionFactory dbFactory;
        ICache<InstShortInfos> cacheInst;
        IRepsitory<t_bp_custom_ST> repST;
        public STCustomService(IDbConnectionFactory dbFactory, ICache<InstShortInfos> cacheInst, IRepsitory<t_bp_custom_ST> repST)
        {
            this.dbFactory = dbFactory;
            this.cacheInst = cacheInst;
            this.repST = repST;
        }

        public bool DeleteCustom(string selectedId, out string erroMsg)
        {
            using (var db = dbFactory.Open())
            {
                try
                {
                    erroMsg = string.Empty;
                    string[] siArray = selectedId.Split(',');
                    for (int i = 0; i < siArray.Length; i++)
                    {

                        //TODO: 删除机构
                        db.UpdateOnly<t_bp_custom_ST>(new t_bp_custom_ST() { data_status = "-1" },
                                p => new { p.data_status },
                                p => p.Id == siArray[i]);
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    erroMsg = ex.Message;
                    return false;
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
                            db.UpdateOnly(new t_bp_custom_ST { APPROVALSTATUS = state }, p => p.APPROVALSTATUS, p => p.Id == siArray[i]);
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

        public bool ApplyChangeForCustom(SupvisorJob job, string customId, out string errorMsg)
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
                        db.UpdateOnly(() => new t_bp_custom_ST() { APPROVALSTATUS = "5" }, t => t.Id == customId);

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

        public bool EditCustom(STCustomSaveModel editModel, out string erroMsg)
        {
            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        erroMsg = string.Empty;
                        db.UpdateOnly(editModel.custom, r => new
                        {
                            r.Id,
                            r.NAME,
                            r.STATIONID,
                            r.POSTCODE,
                            r.TEL,
                            r.FAX,
                            r.ADDRESS,
                            r.CREATETIME,
                            r.EMAIL,
                            r.BUSINESSNUM,
                            r.BUSINESSNUMUNIT,
                            r.REGAPRICE,
                            r.ECONOMICNATURE,
                            r.MEASNUM,
                            r.MEASUNIT,
                            r.FR,
                            r.JSNAME,
                            r.JSTIILE,
                            r.JSYEAR,
                            r.ZLNAME,
                            r.ZLTITLE,
                            r.ZLYEAR,
                            r.PERCOUNT,
                            r.MIDPERCOUNT,
                            r.HEIPERCOUNT,
                            r.REGYTSTA,
                            r.REGJGSTA,
                            r.INSTRUMENTPRICE,
                            r.HOUSEAREA,
                            r.DETECTAREA,
                            r.DETECTTYPE,
                            r.DETECTNUM,
                            r.APPLDATE,
                            r.QUAINFO,
                            r.APPROVALSTATUS,
                            r.ADDDATE,
                            r.phone,
                            r.detectnumStartDate,
                            r.detectnumEndDate,
                            r.measnumStartDate,
                            r.measnumEndDate,
                            r.hasNumPerCount,
                            r.instrumentNum,
                            r.approveadvice,
                            r.subunitnum,
                            r.issubunit,
                            r.supunitcode,
                            r.subunitdutyman,
                            r.area,
                            r.detectunit,
                            r.detectappldate,
                            r.shebaopeoplenum,
                            r.captial,
                            r.credit,
                            r.companytype,
                            r.floorarea,
                            r.yearplanproduce,
                            r.preyearproduce,
                            r.businesspermit,
                            r.businesspermitpath,
                            r.enterprisemanager,
                            r.financeman,
                            r.director,
                            r.cerfgrade,
                            r.cerfno,
                            r.cerfnopath,
                            r.sslcmj,
                            r.sslczk,
                            r.szssccnl,
                            r.fmhccnl,
                            r.chlccnl,
                            r.ytwjjccnl,
                            r.managercount,
                            r.jsglcount,
                            r.testcount,
                            r.sysarea,
                            r.yharea,
                            r.workercount,
                            r.zgcount,
                            r.datatype,
                            r.zzbgpath
                        }, r => r.Id == editModel.custom.Id);

                        //新增后4，申请修改已批准6，审核未通过2。这3个状态编辑后都需要再次审核。
                        ////申请修改已批准	6
                        ////此编辑还需要再次审核
                        //if (editModel.custom.APPROVALSTATUS == "6" || editModel.custom.APPROVALSTATUS == "4")
                        //{
                        //    SupvisorJob superJob = new SupvisorJob
                        //    {
                        //        ApproveType = ApproveType.ApproveCustom,
                        //        CreateBy = editModel.OpUserId,
                        //        CreateTime = DateTime.Now,
                        //        CustomId = editModel.custom.ID,
                        //        NeedApproveId = editModel.custom.ID,
                        //        NeedApproveStatus = NeedApproveStatus.CreateForChange,
                        //        SubmitName = editModel.custom.NAME,
                        //        SubmitText = string.Format("[{0}]已经填写完毕，请审核!", editModel.custom.NAME)
                        //    };

                        //    db.Insert(superJob);
                        //}

                        db.Delete<t_bp_CusAwards_ST>(r => r.CustomId == editModel.custom.Id.ToString());
                        for (int i = 0; i < editModel.stCusAwards.Count; i++)
                        {
                            editModel.stCusAwards[i].Id = 0;
                            db.Insert(editModel.stCusAwards[i], true);
                        }



                        db.Delete<t_bp_CheckCustom_ST>(r => r.CustomId == editModel.custom.Id);
                        for (int i = 0; i < editModel.stCustomReChecks.Count; i++)
                        {
                            editModel.stCustomReChecks[i].Id = 0;
                            db.Insert(editModel.stCustomReChecks[i], true);
                        }


                        db.Delete<t_bp_CusPunish_ST>(r => r.CustomId == editModel.custom.Id);
                        for (int i = 0; i < editModel.stCusPunishs.Count; i++)
                        {
                            editModel.stCusPunishs[i].Id = 0;
                            db.Insert(editModel.stCusPunishs[i], true);
                        }

                        db.Delete<t_bp_pumpsystem>(t => t.customid == editModel.custom.Id);
                        foreach (var item in editModel.PumpSystems)
                        {
                            item.Id = 0;
                            db.Insert(item);
                        }

                        db.Delete<t_bp_pumpvehicle>(t => t.customid == editModel.custom.Id);
                        foreach (var item in editModel.Pumpvehicles)
                        {
                            item.Id = 0;
                            db.Insert(item);
                        }

                        db.Delete<t_bp_carriervehicle>(t => t.customid == editModel.custom.Id);
                        foreach (var item in editModel.Carrievechicles)
                        {
                            item.id = 0;
                            db.Insert(item);
                        }


                        //db.Delete<t_bp_CusChange_ST>(r => r.CustomId == editModel.custom.Id);
                        //for (int i = 0; i < editModel.CusChange.Count; i++)
                        //{
                        //    editModel.CusChange[i].Id = 0;
                        //    db.Insert(editModel.CusChange[i], true);
                        //}

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

        public bool NewCustom(string customId, string customName, out string erroMsg)
        {
            erroMsg = string.Empty;
            try
            {
                t_bp_custom_ST custom = new t_bp_custom_ST
                {
                    Id = customId,
                    NAME = customName,
                    CREATETIME = DateTime.Now,
                    issubunit = "0",
                    APPROVALSTATUS = "0"
                };
                using (var db = dbFactory.Open())
                {
                    db.Insert(custom);
                }
                cacheInst.Clear();
                return true;
            }
            catch (Exception ex)
            {
                erroMsg = ex.Message;
                return false;
            }
        }

        public bool UpdateCustomStatus(string customId, bool rsult, string Reason, out string errorMsg)
        {
            using (var db = dbFactory.Open())
            {

                try
                {
                    errorMsg = string.Empty;
                    if (rsult)
                    {
                        db.UpdateOnly(() => new t_bp_custom_ST() { APPROVALSTATUS = "0" }, t => t.Id == customId);
                    }
                    else
                    {
                        db.UpdateOnly(() => new t_bp_custom_ST() { APPROVALSTATUS = "7" }, t => t.Id == customId);

                    }

                    return true;

                }
                catch (Exception ex)
                {
                    errorMsg = ex.Message;
                    return false;
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

        public string GetSTUnitByIdFromAll(InstShortInfos allInsts, string customId)
        {
            string unitName = string.Empty;

            if (customId.IsNullOrEmpty())
            {
                return unitName;
            }

            if (allInsts.TryGetValue(customId, out unitName))
            {
                return unitName;
            }
            else
            {
                return customId;
            }
        }

        public InstShortInfos GetAllCustomST()
        {
            var data = cacheInst.Get(PkPmCacheKeys.AllCustomST);

            if (data == null)
            {
                var dicData = repST.GetDictByCondition<string, string>(r => r.Id != null && r.data_status != "-1", r => new { r.Id, r.NAME });
                data = InstShortInfos.FromDictonary(dicData);
                cacheInst.Put(PkPmCacheKeys.AllCustomST, data);

            }
            return data;
        }

        public InstShortInfos GetSubInstSTs(string customId)
        {
            if (IsMasterCustomId(customId))
            {
                return GetPartCheckUnitST(customId.Substring(0, customId.Length - 1));
            }
            else
            {
                return GetUnitByIdST(customId);
            }
        } 

        public InstShortInfos GetPartCheckUnitST(string customPart)
        {
            return InstShortInfos.FromDictonary(repST.GetDictByCondition<string, string>(r => r.Id != null && r.Id.StartsWith(customPart), r => new { r.Id, r.NAME }));
        }

        public bool IsMasterCustomId(string customId)
        {
            if (customId.IsNullOrEmpty())
            {
                return false;
            }
            return char.IsNumber(customId[0]) && customId.EndsWith("1");
        } 

        public InstShortInfos GetUnitByIdST(string customId)
        {
            return InstShortInfos.FromDictonary(repST.GetDictByCondition<string, string>(r => r.Id != null && r.Id == customId, r => new { r.Id, r.NAME }));
        }

        public InstShortInfos GetDefaultInstSTs()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add(string.Empty, string.Empty);
            return InstShortInfos.FromDictonary(dic);
        }

    }
}
