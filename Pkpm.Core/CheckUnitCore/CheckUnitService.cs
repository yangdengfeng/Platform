using Pkpm.Entity;
using Pkpm.Framework.Cache;
using Pkpm.Framework.Repsitory;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using ServiceStack;
using Pkpm.Framework.Common;
using Pkpm.Framework.PkpmConfigService;
using System.Collections.Specialized;
using System.Reflection;
using System.Text;

namespace Pkpm.Core.CheckUnitCore
{
    public class CheckUnitService : ICheckUnitService
    {
        ICache<InstShortInfos> cacheInst;
        ICache<Dictionary<string,string>> cacheInstInArea;
        ICache<ItemShortInfos> cacheItem;
        ICache<DateTime> cacheDatetime;
        IRepsitory<t_bp_custom> rep;
        IRepsitory<t_bp_custom_ST> repST;

        IRepsitory<Entity.User> userRep;
        IDbConnectionFactory dbFactory;
        IRepsitory<WxUser> wxuserRep;
        IPkpmConfigService pkpmConfigService;

        public CheckUnitService(ICache<InstShortInfos> cacheInst,
             ICache<Dictionary<string,string>> cacheInstInArea,

            ICache<ItemShortInfos> cacheItem,
            IRepsitory<t_bp_custom> rep,
             IRepsitory<t_bp_custom_ST> repST,
            IRepsitory<Entity.User> userRep,
        IRepsitory<WxUser> wxuserRep,
            ICache<DateTime> cacheDatetime,
            IDbConnectionFactory dbFactory,
            IPkpmConfigService pkpmConfigService)
        {
            this.cacheInst = cacheInst;
            this.cacheInstInArea = cacheInstInArea;
            this.cacheItem = cacheItem;
            this.rep = rep;
            this.userRep = userRep;
            this.dbFactory = dbFactory;
            this.wxuserRep = wxuserRep;
            this.cacheDatetime = cacheDatetime;
            this.pkpmConfigService= pkpmConfigService;
            this.repST = repST;
        }


        public void ClearCheckUnitCache()
        {
            cacheInst.Clear();
        }

        public InstShortInfos GetAllCheckUnit()
        {
            var data = cacheInst.Get(PkPmCacheKeys.AllCheckUnit);

            if (data == null)
            {
                var dicData = rep.GetDictByCondition<string, string>(r => r.ID != null && (r.data_status == null || r.data_status != "-1" )&& (r.IsUse == null || r.IsUse ==1 || r.IsUse == 2 ), r => new { r.ID, r.NAME });
                data = InstShortInfos.FromDictonary(dicData);
                cacheInst.Put(PkPmCacheKeys.AllCheckUnit, data);
            }
            return data;
        }
        public InstShortInfos GetAllSTCheckUnit()
        {
            var data = cacheInst.Get(PkPmCacheKeys.AllSTCheckUnit);

            if (data == null)
            {
                var dicData = repST.GetDictByCondition<string, string>(r => r.Id != null && (r.data_status == null || r.data_status != "-1"), r => new { r.Id, r.NAME });
                data = InstShortInfos.FromDictonary(dicData);
                cacheInst.Put(PkPmCacheKeys.AllSTCheckUnit, data);
            }
            return data;
        }

        /// <summary>
        /// 获取所有机构所在地区
        /// </summary>
        /// <returns></returns>
        public Dictionary<string,string> GetAllCustomInArea()
        {
            Dictionary<string, string> allCustomInArea = new Dictionary<string, string>();
            var data = cacheInstInArea.Get(PkPmCacheKeys.AllCheckUnitInArea);
            if (data == null)
            {
                data = rep.GetDictByCondition<string, string>(t => t.area != null, t => new { t.ID, t.area });
                cacheInstInArea.Put(PkPmCacheKeys.AllCheckUnitInArea, data);

            }

            //var data = rep.GetDictByCondition<string, string>(t => t.area != null, t => new { t.ID, t.area });
            return data;
        }

        public bool UpdateCustomStatus(string customId, string CustomStatus, string Reason, out string errorMsg)
        {
            using (var db = dbFactory.Open())
            {

                try
                {
                    errorMsg = string.Empty;
                    db.UpdateOnly(() => new t_bp_custom() { APPROVALSTATUS = CustomStatus }, t => t.ID == customId);

                    return true;

                }
                catch (Exception ex)
                {
                    errorMsg = ex.Message;
                    return false;
                }
            }
        }


        public InstShortInfos GetAllFormalCheckUnit()
        {
            var data = cacheInst.Get(PkPmCacheKeys.AllCheckUnit);
            if (data == null)
            {
                var dicData = rep.GetDictByCondition<string, string>(r => r.ID.StartsWith(pkpmConfigService.UploadInstStartWith), r => new { r.ID, r.NAME });
                data = InstShortInfos.FromDictonary(dicData);
                cacheInst.Put(PkPmCacheKeys.AllCheckUnit, data);
            } 

            return data;
        }

        public InstShortInfos GetCustomDetectNum()
        {
            var data = cacheInst.Get(PkPmCacheKeys.CustomDetectNum);

            if (data == null)
            {
                //var dicData = rep.GetDictByCondition<string, string>(r => r.DETECTNUM != null, r => new { r.DETECTNUM, r.NAME });
                Dictionary<string, string> dicData = new Dictionary<string, string>();
                var Customs = rep.GetByConditionSort(r => r.DETECTNUM != null, new SortingOptions<t_bp_custom>(r => new { r.ID })); // r => new { r.ID, r.DETECTNUM, r.NAME }, null);
                int ID = 0;
                string Key = string.Empty;
                foreach (var item in Customs)
                {
                    int.TryParse(item.ID, out ID);
                    if (ID > 0)
                    {
                        Key = item.DETECTNUM;
                    }
                    else
                    {
                        Key = item.DETECTNUM + item.ID.Substring(6, 1);
                    }
                    if (!dicData.ContainsKey(Key))
                    {
                        dicData[Key] = item.NAME;
                    }
                }
                data = InstShortInfos.FromDictonary(dicData);
                cacheInst.Put(PkPmCacheKeys.CustomDetectNum, data);

            }
            return data;
        }

        public bool IsCanUploadUnit(string customId)
        {
            if (customId.IsNullOrEmpty())
            {
                return false;
            }
            else
            {
                return customId.StartsWith(pkpmConfigService.UploadInstStartWith);
            }
        }

        public Dictionary<string, string> GetCanUploadCheckUnit()
        {
            var data = GetAllCheckUnit();
            Dictionary<string, string> masterDict = new Dictionary<string, string>();
            foreach (var item in data)
            {
                if (item.Key.IsNullOrEmpty())
                {
                    continue;
                }

                if (item.Key.StartsWith(pkpmConfigService.UploadInstStartWith))
                {
                    string itemValue = item.Value.IsNullOrEmpty() ? string.Empty : item.Value;
                    masterDict[item.Key] = itemValue;
                }
            }

            return masterDict;
        }

        public Dictionary<string, string> GetAllMasterCheckUnit()
        {
            var data = GetAllCheckUnit();
            Dictionary<string, string> masterDict = new Dictionary<string, string>();
            foreach (var item in data)
            {
                if (item.Key.IsNullOrEmpty())
                {
                    continue;
                }

                if (item.Key.StartsWith(pkpmConfigService.UploadInstStartWith) && item.Key.EndsWith("1"))
                {
                    data[item.Key] = item.Value.IsNullOrEmpty() ? string.Empty : item.Value;
                }
            }

            return data;
        }

        public InstShortInfos GetPartCheckUnit(string customPart)
        {
            return InstShortInfos.FromDictonary(rep.GetDictByCondition<string, string>(r => r.ID != null && r.ID.StartsWith(customPart), r => new { r.ID, r.NAME }));
        }

        

        public InstShortInfos GetSubInsts(string customId)
        {
            if (IsMasterCustomId(customId))
            {
                return GetPartCheckUnit(customId.Substring(0, customId.Length - 1));
            }
            else
            {
                return GetUnitById(customId);
            }
        }

     

        public string GetSubInstFlag(string customId)
        {
            if (customId.IsNullOrEmpty())
            {
                return string.Empty;
            }

            if (IsMasterCustomId(customId))
            {
                return string.Empty;
            }
            else
            {
                return customId.SafeSubstring(customId.Length - 1, 1);
            }
        }

        public InstShortInfos GetUnitById(string customId)
        {
            return InstShortInfos.FromDictonary(rep.GetDictByCondition<string, string>(r => r.ID != null && r.ID == customId, r => new { r.ID, r.NAME }));
        }

       

        public InstShortInfos GetDefaultInsts()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add(pkpmConfigService.DefaultInst, pkpmConfigService.DefaultInst);
            return InstShortInfos.FromDictonary(dic);
        }

      

        public bool IsMasterCustomId(string customId)
        {
            if (customId.IsNullOrEmpty())
            {
                return false;
            }
            return char.IsNumber(customId[0]) && customId.EndsWith("1");
        }

        public string GetMasterCustomId(string customId)
        {
            if (customId.IsNullOrEmpty())
            {
                return string.Empty;
            }

            return "{0}{1}".Fmt(customId.Substring(0, customId.Length - 1), "1");
        }

        public bool DeletePathsIntoUnit(string selectedId, out string erroMsg)
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

                            //TODO: 删除机构
                            db.UpdateOnly<t_bp_custom>(new t_bp_custom() { data_status = "-1", update_time = DateTime.Now },
                                    p => new { p.data_status, p.update_time },
                                    p => p.ID == siArray[i]);


                            //db.Delete<t_bp_CusAchievement>(r => r.CustomId == siArray[i]);
                            //db.Delete<t_bp_CusAward>(r => r.CustomId == siArray[i]);
                            //db.Delete<t_bp_CusPunish>(r => r.CustomId == siArray[i]);
                            //db.Delete<t_bp_CheckCustom>(r => r.CustomId == siArray[i]);
                            //db.Delete<t_bp_CusChange>(r => r.CustomId == siArray[i]);
                            //db.Delete<t_bp_custom>(r => r.ID == siArray[i]);
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
                            db.UpdateOnly(new t_bp_custom { APPROVALSTATUS = state }, p => p.APPROVALSTATUS, p => p.ID == siArray[i]);
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

        public bool SetInstScreeningState(string selectedId, out string erroMsg)
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
                            //TODO:  机构是否使用
                            db.UpdateOnly(() => new t_bp_custom { IsUse = 0 }, p => p.ID == siArray[i]);

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

        public bool SetInstRelieveScreeningSate(string selectId, out string erroMsg)
        {
            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        erroMsg = string.Empty;
                        string[] siArray = selectId.Split(',');
                        for (int i = 0; i < siArray.Length; i++)
                        {
                            //TODO:
                            db.UpdateOnly(() => new t_bp_custom { IsUse = 1 }, p => p.ID == siArray[i]);
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
        public InstShortInfos GetUnitByArea(List<string> areas)
        {
            //List<string> areaNames = areas.Select(a => a.Area).ToList();
            return InstShortInfos.FromDictonary(rep.GetDictByCondition<string, string>(r => r.area != null && areas.Contains(r.area), r => new { r.ID, r.NAME }));
        }
        public InstShortInfos GetUnitByArea(List<UserInArea> areas)
        {
            List<string> areaNames = areas.Select(a => a.Area).ToList();
            return InstShortInfos.FromDictonary(rep.GetDictByCondition<string, string>(r => r.area != null && areaNames.Contains(r.area), r => new { r.ID, r.NAME }));
        }

        public string GetCheckUnitById(string Customid)
        {
            if(Customid.IsNullOrEmpty())
            {
                return string.Empty;
            }
            var allCheckUnit = GetAllCheckUnit();
            string unitName = string.Empty;

            if (allCheckUnit.TryGetValue(Customid, out unitName))
            {
                return unitName;
            }
            else
            {
                return Customid;
            }
        }
        public string GetSTCheckUnitById(string Customid)
        {
            if (Customid.IsNullOrEmpty())
            {
                return string.Empty;
            }
            var allCheckUnit = GetAllSTCheckUnit();
            string unitName = string.Empty;

            if (allCheckUnit.TryGetValue(Customid, out unitName))
            {
                return unitName;
            }
            else
            {
                return Customid;
            }
        }

        public string GetCheckUnitByIdFromAll(InstShortInfos allInsts, string customId)
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

        public string GetCheckUnitByName(string CustomName)
        {
            var allCheckUnit = GetAllCheckUnit();
            var found = allCheckUnit.Where(kv => kv.Value == CustomName).Select(kv => kv.Key).ToList();

            if (found != null && found.Count > 0)
            {
                return found.First();
            }
            else
            {
                return string.Empty;
            }
        }

        public int CountCardChanges(string name, string customIDs)
        {
            using (var db = dbFactory.Open())
            {
                string sql = String.Format(@"SELECT COUNT(n.ID)
                        from t_bp_people a inner join t_bp_postType b on charIndex('|' + b.code + '|', '|' + a.postTypeCode + '|') > 0
                        inner join t_bp_postType c on c.Id = b.parentId
                        inner join t_bp_custom n on a.customid = n.id
                        WHERE a.Name like '%{0}%'", name);
                if (!string.IsNullOrEmpty(customIDs))
                {
                    sql += " AND a.Customid IN (" + customIDs + ")";
                }
                var r = ServiceStack.OrmLite.Dapper.SqlMapper.ExecuteScalar<int>(db, sql);
                return r;
            }
        }

        public List<UICardChange> GetCardChanges(string name, string customIDs, int skip, int count)
        {
            using (var db = dbFactory.Open())
            {
                string sql = @"SELECT TOP {1} * FROM
	                (SELECT row_number() OVER(ORDER BY n.ID) AS R#, n.NAME CustomName, a.Name PeopleName, b.PostType, b.ParentID, c.PostType PostType2
                        from t_bp_people a inner join t_bp_postType b on charIndex('|' + b.code + '|', '|' + a.postTypeCode + '|') > 0
                        inner join t_bp_postType c on c.Id = b.parentId
                        inner join t_bp_custom n on a.customid = n.id ";
                if (!string.IsNullOrEmpty(customIDs))
                {
                    sql += " AND a.Customid IN (" + customIDs + ")";
                }
                if (!string.IsNullOrEmpty(name))
                    sql += " WHERE a.Name like '%" + name + "%'";
                sql += ") t WHERE R#>{0} AND R#<{0}+{1}+1";
                sql = string.Format(sql, skip, count);
                var list = ServiceStack.OrmLite.Dapper.SqlMapper.Query<UICardChange>(db, sql).ToList();
                return list;
            }
        }

        /// <summary>
        /// 修改机构信息
        /// </summary>
        /// <param name="editModel"></param>
        /// <param name="erroMsg"></param>
        /// <returns></returns>
        public bool EditCustom(CheckCustomSaveModel editModel, out string erroMsg)
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
                            r.ID,
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
                            r.MEASNUMPATH,
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
                            r.DETECTPATH,
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
                            r.businessnumPath,
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
                            r.shebaopeoplelistpath,
                            r.workercount,
                            r.zgcount,
                            r.instrumentpath,
                            r.datatype,
                            r.ispile,
                            r.NETADDRESS,
                            r.REGMONEYS,
                            r.PERP,
                            r.CMANUM,
                            r.CMAUNIT,
                            r.CMANUMCERF,
                            r.AVAILABILITYTIME,
                            r.GMANAGER,
                            r.GFA,
                            r.GFB,
                            r.TMANAGER,
                            r.TFA,
                            r.TFB,
                            r.ALLMANS,
                            r.TMANS,
                            r.MLEVELS,
                            r.HLEVELS,
                            r.EQUIPMENTS,
                            r.EQMONEYS,
                            r.WORKAREA,
                            r.CMANUMENDDATE,
                            r.CMAENDDATE,
                            r.USEENDDATE,
                            r.SELECTTEL,
                            r.APPEALTEL,
                            r.APPEALEMAIL,
                            r.zzlbgs,
                            r.zzxmgs,
                            r.zzcsgs,
                            r.certCode, 
                        }, r => r.ID == editModel.custom.ID);

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

                        db.Delete<t_bp_CusAchievement>(r => r.CustomId == editModel.custom.ID);
                        for (int i = 0; i < editModel.CusAchievement.Count; i++)
                        {
                            editModel.CusAchievement[i].id = 0;
                            db.Insert(editModel.CusAchievement[i], true);
                        }

                        db.Delete<t_bp_CusAward>(r => r.CustomId == editModel.custom.ID.ToString());
                        for (int i = 0; i < editModel.CusAward.Count; i++)
                        {
                            editModel.CusAward[i].id = 0;
                            db.Insert(editModel.CusAward[i], true);
                        }

                        db.Delete<t_bp_CusPunish>(r => r.CustomId == editModel.custom.ID);
                        for (int i = 0; i < editModel.CusPunish.Count; i++)
                        {
                            editModel.CusPunish[i].id = 0;
                            db.Insert(editModel.CusPunish[i], true);
                        }

                        db.Delete<t_bp_CheckCustom>(r => r.CustomId == editModel.custom.ID);
                        for (int i = 0; i < editModel.CheckCustom.Count; i++)
                        {
                            editModel.CheckCustom[i].id = 0;
                            db.Insert(editModel.CheckCustom[i], true);
                        }

                        db.Delete<t_bp_CusChange>(r => r.CustomId == editModel.custom.ID);
                        for (int i = 0; i < editModel.CusChange.Count; i++)
                        {
                            editModel.CusChange[i].id = 0;
                            db.Insert(editModel.CusChange[i], true);
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

        /// <summary>
        /// 修改机构信息,先保存到临时表，待审核 add by ydf 2019-04-03
        /// </summary>
        /// <param name="editModel"></param>
        /// <param name="erroMsg"></param>
        /// <returns></returns>
        public bool EditCustom(CheckCustomTmpSaveModel editModel, out string erroMsg)
        {
            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        erroMsg = string.Empty;

                        //先删掉之前的记录，再保存到临时表
                        db.Delete<t_bp_custom_tmp>(r => r.ID == editModel.custom.ID);

                        //已递交待审核
                        editModel.custom.APPROVALSTATUS = "1";
                        db.Insert(editModel.custom, true);

                        //更新原始机构信息状态
                        var customOrig = new t_bp_custom();
                        customOrig.ID = editModel.custom.ID;
                        customOrig.APPROVALSTATUS = "1";
                        db.UpdateOnly(customOrig, r => r.APPROVALSTATUS, r => r.ID == customOrig.ID);

                        #region 关联表更新

                        db.Delete<t_bp_CusAchievement>(r => r.CustomId == editModel.custom.ID);
                        for (int i = 0; i < editModel.CusAchievement.Count; i++)
                        {
                            editModel.CusAchievement[i].id = 0;
                            db.Insert(editModel.CusAchievement[i], true);
                        }

                        db.Delete<t_bp_CusAward>(r => r.CustomId == editModel.custom.ID.ToString());
                        for (int i = 0; i < editModel.CusAward.Count; i++)
                        {
                            editModel.CusAward[i].id = 0;
                            db.Insert(editModel.CusAward[i], true);
                        }

                        db.Delete<t_bp_CusPunish>(r => r.CustomId == editModel.custom.ID);
                        for (int i = 0; i < editModel.CusPunish.Count; i++)
                        {
                            editModel.CusPunish[i].id = 0;
                            db.Insert(editModel.CusPunish[i], true);
                        }

                        db.Delete<t_bp_CheckCustom>(r => r.CustomId == editModel.custom.ID);
                        for (int i = 0; i < editModel.CheckCustom.Count; i++)
                        {
                            editModel.CheckCustom[i].id = 0;
                            db.Insert(editModel.CheckCustom[i], true);
                        }

                        db.Delete<t_bp_CusChange>(r => r.CustomId == editModel.custom.ID);
                        for (int i = 0; i < editModel.CusChange.Count; i++)
                        {
                            editModel.CusChange[i].id = 0;
                            db.Insert(editModel.CusChange[i], true);
                        }

                        #endregion

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

        /// <summary>
        /// 机构申请
        /// </summary>
        /// <param name="editModel"></param>
        /// <param name="erroMsg"></param>
        /// <returns></returns>
        public bool EditCustom(ApplyCustomSaveModel editModel, out string erroMsg)
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
                            r.ID,
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
                            r.MEASNUMPATH,
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
                            r.DETECTPATH,
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
                            r.businessnumPath,
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
                            r.shebaopeoplelistpath,
                            r.workercount,
                            r.zgcount,
                            r.instrumentpath,
                            r.datatype,
                            r.ispile,
                            r.NETADDRESS,
                            r.REGMONEYS,
                            r.PERP,
                            r.CMANUM,
                            r.CMAUNIT,
                            r.CMANUMCERF,
                            r.AVAILABILITYTIME,
                            r.GMANAGER,
                            r.GFA,
                            r.GFB,
                            r.TMANAGER,
                            r.TFA,
                            r.TFB,
                            r.ALLMANS,
                            r.TMANS,
                            r.MLEVELS,
                            r.HLEVELS,
                            r.EQUIPMENTS,
                            r.EQMONEYS,
                            r.WORKAREA,
                            r.CMANUMENDDATE,
                            r.CMAENDDATE,
                            r.USEENDDATE,
                            r.SELECTTEL,
                            r.APPEALTEL,
                            r.APPEALEMAIL,
                            r.zzlbgs,
                            r.zzxmgs,
                            r.zzcsgs,
                            r.certCode,
                            r.SQname,
                            r.SQTel,
                            r.sqjcyw,
                            r.FRSex,
                            r.FRBirth,
                            r.FRTITLE,
                            r.FRYEAR,
                            r.FRGraduationTime,
                            r.FRCollege,
                            r.FREducation,
                            r.FRSubject,
                            r.FRTel,
                            r.FRMobile,
                            r.JSSex,
                            r.JSBirth,
                            r.JSGraduationTime,
                            r.JSCollege,
                            r.JSEducation,
                            r.JSSubject,
                            r.JSTel,
                            r.JSMobile,
                            r.frgzjl,
                            r.jsgzjl,
                            r.bgcszmPath,
                            r.zxzmPath,
                            r.gqzmPath,
                            r.zzscwjPath,
                            r.glscwjPath,
                            r.frzcpath,
                            r.frxlpath,
                            r.jszcpath,
                            r.jsxlpath
                        }, r => r.ID == editModel.custom.ID);


                        #region 关联表更新

                        db.Delete<t_bp_CusAchievement>(r => r.CustomId == editModel.custom.ID);
                        for (int i = 0; i < editModel.CusAchievement.Count; i++)
                        {
                            editModel.CusAchievement[i].id = 0;
                            db.Insert(editModel.CusAchievement[i], true);
                        }

                        db.Delete<t_bp_CusAward>(r => r.CustomId == editModel.custom.ID.ToString());
                        for (int i = 0; i < editModel.CusAward.Count; i++)
                        {
                            editModel.CusAward[i].id = 0;
                            db.Insert(editModel.CusAward[i], true);
                        }

                        db.Delete<t_bp_CusPunish>(r => r.CustomId == editModel.custom.ID);
                        for (int i = 0; i < editModel.CusPunish.Count; i++)
                        {
                            editModel.CusPunish[i].id = 0;
                            db.Insert(editModel.CusPunish[i], true);
                        }

                        db.Delete<t_bp_CheckCustom>(r => r.CustomId == editModel.custom.ID);
                        for (int i = 0; i < editModel.CheckCustom.Count; i++)
                        {
                            editModel.CheckCustom[i].id = 0;
                            db.Insert(editModel.CheckCustom[i], true);
                        }

                        db.Delete<t_bp_CusChange>(r => r.CustomId == editModel.custom.ID);
                        for (int i = 0; i < editModel.CusChange.Count; i++)
                        {
                            editModel.CusChange[i].id = 0;
                            db.Insert(editModel.CusChange[i], true);
                        }

                        #endregion

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


        /// <summary>
        /// 审核机构修改信息 add by ydf 2019-04-04
        /// </summary>
        /// <param name="formCol"></param>
        /// <param name="dic"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool AuditCustom(NameValueCollection formCol, Dictionary<string, List<SysDict>> dic, out string errorMsg)
        {
            errorMsg = string.Empty;

            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        string id = formCol["Id"];
                        string op = formCol["Operate"];
                        var result = 0;
                        if (op == "Y") //审核通过
                        {
                            //删除机构信息临时表数据
                            db.Delete<t_bp_custom_tmp>(r => r.ID == id);

                            //更新机构信息表 t_bp_custom
                            t_bp_custom custom = new t_bp_custom();
                            custom.ID = id;
                            custom.APPROVALSTATUS = "0"; //审核通过重置为未递交
                            int index = 0;
                            formCol.Remove("Operate"); //移除掉
                            formCol.Add("APPROVALSTATUS", custom.APPROVALSTATUS);
                            string[] columns = new string[formCol.Count - 1];

                            //通过反射给实体字段赋值
                            PropertyInfo[] mPi = typeof(t_bp_custom).GetProperties();
                            foreach (var item in formCol)
                            {
                                if (item.ToString() != "Id")
                                {
                                    foreach (PropertyInfo pi in mPi)
                                    {
                                        if (pi.Name == item.ToString())
                                        {
                                            if (pi.PropertyType == typeof(DateTime?))
                                            {
                                                pi.SetValue(custom, Convert.ToDateTime(formCol[item.ToString()]));
                                            }
                                            else
                                            {
                                                if (pi.Name == "companytype")
                                                {
                                                    pi.SetValue(custom, dic["companyTypes"].Where(x => x.Name == formCol[item.ToString()]).ToList().FirstOrDefault().KeyValue);
                                                }
                                                else if (pi.Name == "ispile")
                                                {
                                                    pi.SetValue(custom, dic["yesNo"].Where(x => x.Name == formCol[item.ToString()]).ToList().FirstOrDefault().KeyValue);
                                                }
                                                else if (pi.Name == "JSTIILE")
                                                {
                                                    pi.SetValue(custom, dic["personnelTitles"].Where(x => x.Name == formCol[item.ToString()]).ToList().FirstOrDefault().KeyValue);
                                                }
                                                else if (pi.Name == "ZLTITLE")
                                                {
                                                    pi.SetValue(custom, dic["personnelTitles"].Where(x => x.Name == formCol[item.ToString()]).ToList().FirstOrDefault().KeyValue);
                                                }
                                                else
                                                {
                                                    pi.SetValue(custom, formCol[item.ToString()]);
                                                }
                                            }
                                        }
                                    }

                                    columns[index] = item.ToString();
                                    index++;
                                }
                            }

                            //更新，根据给定字段
                            result = db.UpdateOnly(custom, columns, r => r.ID == custom.ID);
                        }
                        else //审核不通过
                        {
                            //删除机构信息临时表数据
                            db.Delete<t_bp_custom_tmp>(r => r.ID == id);

                            //更新机构信息表 t_bp_custom
                            t_bp_custom custom = new t_bp_custom();
                            custom.ID = id;
                            custom.APPROVALSTATUS = "0";

                            //更新
                            result = db.UpdateOnly(custom, r => r.APPROVALSTATUS, r => r.ID == custom.ID);
                        }
                        
                        dbTrans.Commit();
                        return result > 0 ? true : false;
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


        public bool EditCustomCheckQualify(CheckCustomSaveModel editModel, out string erroMsg)
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
                            r.ID,
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
                            r.MEASNUMPATH,
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
                            //r.DETECTNUM,
                            r.APPLDATE,
                            r.DETECTPATH,
                            r.QUAINFO,
                            r.APPROVALSTATUS,
                            r.ADDDATE,
                            r.phone,
                            r.detectnumStartDate,
                            r.detectnumEndDate,
                            //r.measnumStartDate,
                            //r.measnumEndDate,
                            r.hasNumPerCount,
                            r.instrumentNum,
                            r.businessnumPath,
                            r.approveadvice,
                            r.subunitnum,
                            r.issubunit,
                            r.supunitcode,
                            r.subunitdutyman,
                            r.area,
                            //r.detectunit,
                            //r.detectappldate,
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
                            r.shebaopeoplelistpath,
                            r.workercount,
                            r.zgcount,
                            r.instrumentpath,
                            r.datatype,
                            r.ispile,
                            r.NETADDRESS,
                            r.REGMONEYS,
                            r.PERP,
                            r.CMANUM,
                            r.CMAUNIT,
                            r.CMANUMCERF,
                            r.AVAILABILITYTIME,
                            r.GMANAGER,
                            r.GFA,
                            r.GFB,
                            r.TMANAGER,
                            r.TFA,
                            r.TFB,
                            r.ALLMANS,
                            r.TMANS,
                            r.MLEVELS,
                            r.HLEVELS,
                            r.EQUIPMENTS,
                            r.EQMONEYS,
                            r.WORKAREA,
                            r.CMANUMENDDATE,
                            r.CMAENDDATE,
                            r.USEENDDATE,
                            r.SELECTTEL,
                            r.APPEALTEL,
                            r.APPEALEMAIL,
                            r.zzlbgs,
                            r.zzxmgs,
                            r.zzcsgs,
                            r.certCode
                        }, r => r.ID == editModel.custom.ID);

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

                        db.Delete<t_bp_CusAchievement>(r => r.CustomId == editModel.custom.ID);
                        for (int i = 0; i < editModel.CusAchievement.Count; i++)
                        {
                            editModel.CusAchievement[i].id = 0;
                            db.Insert(editModel.CusAchievement[i], true);
                        }

                        db.Delete<t_bp_CusAward>(r => r.CustomId == editModel.custom.ID.ToString());
                        for (int i = 0; i < editModel.CusAward.Count; i++)
                        {
                            editModel.CusAward[i].id = 0;
                            db.Insert(editModel.CusAward[i], true);
                        }

                        db.Delete<t_bp_CusPunish>(r => r.CustomId == editModel.custom.ID);
                        for (int i = 0; i < editModel.CusPunish.Count; i++)
                        {
                            editModel.CusPunish[i].id = 0;
                            db.Insert(editModel.CusPunish[i], true);
                        }

                        db.Delete<t_bp_CheckCustom>(r => r.CustomId == editModel.custom.ID);
                        for (int i = 0; i < editModel.CheckCustom.Count; i++)
                        {
                            editModel.CheckCustom[i].id = 0;
                            db.Insert(editModel.CheckCustom[i], true);
                        }

                        db.Delete<t_bp_CusChange>(r => r.CustomId == editModel.custom.ID);
                        for (int i = 0; i < editModel.CusChange.Count; i++)
                        {
                            editModel.CusChange[i].id = 0;
                            db.Insert(editModel.CusChange[i], true);
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
                        db.UpdateOnly(() => new t_bp_custom() { APPROVALSTATUS = "5" }, t => t.ID == customId);

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

        public bool EditEquipment(CheckEquipSaveModel editModel, out string erroMsg)
        {
            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        erroMsg = string.Empty;
                        db.UpdateOnly(editModel.equipment, r => new
                        {
                            r.customid,
                            r.equclass,
                            r.EquName,
                            r.equspec,
                            r.degree,
                            r.equnum,
                            r.equtype,
                            r.testrange,
                            r.uncertainty,
                            r.checkunit,
                            r.checkcerfnum,
                            r.checkstartdate,
                            r.checkenddate,
                            r.repairunit,
                            r.repaircerfnum,
                            r.repairstartdate,
                            r.repairenddate,
                            r.selfcheckitem,
                            r.selfcheckstandardname,
                            r.selfchecknum,
                            r.selfrepairitem,
                            r.selfrepairstandardname,
                            r.selfrepairnum,
                            r.explain,
                            r.isautoacs,
                            r.autoacsprovider,
                            r.checkcerfnumpath,
                            r.repaircerfnumpath,
                            r.equclassId
                            //r.approveadvice,
                            //r.approvalstatus
                        }, r => r.id == editModel.equipment.id);

                        //申请修改已批准	6
                        //此编辑还需要再次审核
                        //if (editModel.equipment.approvalstatus == "6")
                        //{
                        //    SupvisorJob superJob = new SupvisorJob
                        //    {
                        //        ApproveType = ApproveType.ApproveEquip,
                        //        CreateBy = editModel.OpUserId,
                        //        CreateTime = DateTime.Now,
                        //        CustomId = editModel.equipment.customid,
                        //        NeedApproveId = editModel.equipment.id.ToString(),
                        //        NeedApproveStatus = NeedApproveStatus.CreateForChange,
                        //        SubmitName = "检测设备信息",
                        //        SubmitText = string.Empty
                        //    };

                        //    db.Insert(superJob);
                        //}

                        db.Delete<t_bp_equItemSubItemList>(r => r.equId == editModel.equipment.id);
                        for (int i = 0; i < editModel.Projects.Count; i++)
                        {
                            editModel.Projects[i].id = 0;
                            db.Insert(editModel.Projects[i], true);
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

        /// <summary>
        /// 修改设备信息，保存到临时表，待审核 add by ydf 2019-04-11
        /// </summary>
        /// <param name="editModel"></param>
        /// <param name="erroMsg"></param>
        /// <returns></returns>
        public bool EditEquipment(CheckEquipTmpSaveModel editModel, out string erroMsg)
        {
            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        erroMsg = string.Empty;

                        //先删掉之前的记录，再保存到临时表
                        db.Delete<t_bp_Equipment_tmp>(e => e.id == editModel.equipment.id);

                        //已递交待审核
                        editModel.equipment.approvalstatus = "1";
                        db.Insert(editModel.equipment, true);

                        //更新原始设备信息状态
                        var equipmentOrig = new t_bp_Equipment();
                        equipmentOrig.id = editModel.equipment.id;
                        equipmentOrig.approvalstatus = "1";
                        db.UpdateOnly(equipmentOrig, e => e.approvalstatus, e => e.id == equipmentOrig.id);

                        //关联表更新
                        db.Delete<t_bp_equItemSubItemList>(r => r.equId == editModel.equipment.id);
                        for (int i = 0; i < editModel.Projects.Count; i++)
                        {
                            editModel.Projects[i].id = 0;
                            db.Insert(editModel.Projects[i], true);
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


        /// <summary>
        /// 对修改的设备信息进行审核
        /// </summary>
        /// <param name="formCol"></param>
        /// <param name="dic"></param>
        /// <param name="erroMsg"></param>
        /// <returns></returns>
        public bool AuditEquipment(NameValueCollection formCol, Dictionary<string, List<SysDict>> dic, out string errorMsg)
        {
            errorMsg = string.Empty;

            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        var checkUnits = this.GetAllCheckUnit();
                        int id = Convert.ToInt32(formCol["Id"]);
                        string op = formCol["Operate"];
                        var result = 0;
                        if (op == "Y") //审核通过
                        {
                            //删除设备信息临时表数据
                            db.Delete<t_bp_Equipment_tmp>(e => e.id == id);

                            //更新设备信息表
                            t_bp_Equipment equip = new t_bp_Equipment();
                            equip.id = id;
                            equip.approvalstatus = "3"; //审核通过重置为未递交
                            int index = 0;
                            formCol.Remove("Operate"); //移除掉
                            formCol.Add("Approvalstatus", equip.approvalstatus);
                            string[] columns = new string[formCol.Count - 1];

                            //通过反射给实体字段赋值
                            PropertyInfo[] mPi = typeof(t_bp_Equipment).GetProperties();
                            foreach (var item in formCol)
                            {
                                if (item.ToString() != "Id")
                                {
                                    foreach (PropertyInfo pi in mPi)
                                    {
                                        if (pi.Name == item.ToString())
                                        {
                                            if (pi.PropertyType == typeof(DateTime?))
                                            {
                                                pi.SetValue(equip, Convert.ToDateTime(formCol[item.ToString()]));
                                            }
                                            else
                                            {
                                                if (pi.Name == "customid")
                                                {
                                                    pi.SetValue(equip, checkUnits.Where(p => p.Value == formCol[item.ToString()]).Select(p => p.Key).FirstOrDefault());
                                                }
                                                else
                                                {
                                                    pi.SetValue(equip, formCol[item.ToString()]);
                                                }
                                            }
                                        }
                                    }

                                    columns[index] = item.ToString();
                                    index++;
                                }
                            }

                            //更新，根据给定字段
                            result = db.UpdateOnly(equip, columns, r => r.id == equip.id);
                        }
                        else //审核不通过
                        {
                            //删除设备信息临时表数据
                            db.Delete<t_bp_Equipment_tmp>(r => r.id == id);

                            //更新设备信息表 t_bp_custom
                            t_bp_Equipment equip = new t_bp_Equipment();
                            equip.id = id;
                            equip.approvalstatus = "0";

                            //更新
                            result = db.UpdateOnly(equip, r => r.approvalstatus, r => r.id == equip.id);
                        }

                        dbTrans.Commit();
                        return result > 0 ? true : false;
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


        public bool ApplyChangeForEquip(SupvisorJob job, int equipId, out string errorMsg)
        {
            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        errorMsg = string.Empty;

                        db.Insert<SupvisorJob>(job);

                        //5  申请修改
                        db.UpdateOnly(() => new t_bp_Equipment() { approvalstatus = "5" }, t => t.id == equipId);

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

        public bool CreatEquipment(CheckEquipSaveModel createModel, out string erroMsg)
        {
            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        erroMsg = string.Empty;
                        createModel.equipment.approvalstatus = "0"; //未递交
                        var equipmentId = db.Insert(createModel.equipment, true);

                        for (int i = 0; i < createModel.Projects.Count; i++)
                        {
                            createModel.Projects[i].equId = (int)equipmentId;
                            db.Insert(createModel.Projects[i], true);
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

        public bool DeleteEquipments(string selectedId, out string erroMsg)
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
                            db.Delete<t_bp_Equipment>(r => r.id == Convert.ToInt32(siArray[i]));
                            db.Delete<t_bp_equItemSubItemList>(r => r.equId == Convert.ToInt32(siArray[i]));
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

        public bool SendStatueForEquips(string selectedId, out string erroMsg)
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
                            db.UpdateOnly(new t_bp_Equipment { approvalstatus = "1" }, p => p.approvalstatus, p => p.id == Convert.ToInt32(siArray[i]));
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

        public bool ReturnStateFroEquips(string selectedId, out string erroMsg)
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
                            db.UpdateOnly(new t_bp_Equipment { approvalstatus = "0" }, p => p.approvalstatus, p => p.id == Convert.ToInt32(siArray[i]));
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

        public bool UpdateAttachPathsIntoCustom(string customid, string fieldname, string pathname, out string erroMsg)
        {

            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        erroMsg = string.Empty;
                        switch (fieldname)
                        {
                            case "businessnumPath":
                                db.UpdateOnly(new t_bp_custom { businessnumPath = pathname }, p => p.businessnumPath, p => p.ID == customid);
                                break;
                            case "DETECTPATH":
                                db.UpdateOnly(new t_bp_custom { DETECTPATH = pathname }, p => p.DETECTPATH, p => p.ID == customid);
                                break;
                            case "MEASNUMPATH":
                                db.UpdateOnly(new t_bp_custom { MEASNUMPATH = pathname }, p => p.MEASNUMPATH, p => p.ID == customid);
                                break;
                            case "instrumentpath":
                                db.UpdateOnly(new t_bp_custom { instrumentpath = pathname }, p => p.instrumentpath, p => p.ID == customid);
                                break;
                            case "shebaopeoplelistpath":
                                db.UpdateOnly(new t_bp_custom { shebaopeoplelistpath = pathname }, p => p.shebaopeoplelistpath, p => p.ID == customid);
                                break;
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

        public bool UpdateAttachPathsIntoEquip(int id, string fieldname, string pathname, out string erroMsg)
        {

            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        erroMsg = string.Empty;
                        switch (fieldname)
                        {
                            case "checkcerfnumpath":
                                db.UpdateOnly(new t_bp_Equipment { checkcerfnumpath = pathname }, p => p.checkcerfnumpath, p => p.id == id);
                                break;
                            case "DETECTPATH":
                                db.UpdateOnly(new t_bp_Equipment { repaircerfnumpath = pathname }, p => p.repaircerfnumpath, p => p.id == id);
                                break;
                            case "repaircerfnumpath":
                                db.UpdateOnly(new t_bp_Equipment { repaircerfnumpath = pathname }, p => p.repaircerfnumpath, p => p.id == id);
                                break;
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

        public List<InstWithArea> GetAllInstAreas()
        {
            var data = rep.GetByConditionSort<InstWithArea>(tpm => tpm.ID.StartsWith("270") && tpm.NAME != null && tpm.NAME != "",
                p => new { p.ID, p.NAME, p.area }, 
                new SortingOptions<t_bp_custom>(s => new { s.ID }));
            return data;
        }

        public List<InstWithArea> GetInstAreasByName(string name)
        {
            var data = rep.GetByConditionSort<InstWithArea>(tpm => tpm.ID.StartsWith("270") && tpm.NAME != null && tpm.NAME != "" && tpm.NAME.Contains(name),
                p => new { p.ID, p.NAME, p.area },
                 new SortingOptions<t_bp_custom>(s => new { s.ID }));
            return data;
        }

        public string GetCustomCert(string customId)
        {
            var certs = rep.GetColumnByCondition<string>(c => c.ID == customId, r => new { r.DETECTNUM });
            if (certs != null && certs.Count > 0)
            {
                return certs.First();
            }
            else
            {
                return string.Empty;
            }
        }

        public bool NotifyCustomOffline(string customId, out string erroMsg)
        {
            erroMsg = string.Empty;

            int roleId = 0;

            using (var db = dbFactory.Open())
            {
                var instRole = db.Select<Role>().Where(r => r.Code == pkpmConfigService.InstRoleCode).FirstOrDefault();
                if (instRole != null)
                {
                    roleId = instRole.Id;
                }
            }

            var users = userRep.GetMasterDetailConditionSort<UserInRole>(us => us.CustomId == customId, ur => ur.RoleId == roleId, new SortingOptions<User>(u => new { u.Id }));
            if (users.Count == 0)
            {
                erroMsg = "此检测机构在系统中无账号！";
                return false;
            }

            string openId = string.Empty;
            //从wxuser取openid
            List<int> listId = users.Select(r => r.Id).ToList();
            var wxuser = wxuserRep.GetByCondition(p => listId.Contains(p.UserId));
            if (wxuser.Count <= 0)
            {
                erroMsg = "此检测机构账号暂没关注公众号!";
                return false;
            }

            string response = string.Empty;
            string customName = GetCheckUnitById(customId);
            foreach (var item in wxuser)
            {
                if (!string.IsNullOrEmpty(item.OpenId))
                {
                    Dictionary<string, string> postDic = new Dictionary<string, string>();
                    postDic.Add("openId", item.OpenId);
                    postDic.Add("first", "{0}离线半天通知".Fmt(customName));
                    postDic.Add("keyword1", "{0}".Fmt(customName));
                    postDic.Add("keyword2", "{0}".Fmt(DateTime.Now.ToLongTimeString()));
                    postDic.Add("remark", "请查看此检测机构详情");

                    string url = pkpmConfigService.WxWebUrl + "/Message/Offline";
                    response = url.PostToUrl(postDic);
                }
            }


            if (response != null && response.ToUpper().Contains("TRUE"))
            {
                return true;
            }
            else
            {
                erroMsg = response;
                return false;
            }
        }

        public bool NewCustom(string customId, string customName, out string erroMsg)
        {
            erroMsg = string.Empty;
            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {

                    try
                    {
                        var hashData = System.Security.Cryptography.MD5.Create().ComputeHash(Encoding.UTF8.GetBytes("123456"));
                        User user = new User()
                        {
                            CustomId = customId,
                            UserDisplayName = customName,
                            UserName = customName,
                            PasswordHash = HashUtility.ConvertToHex(hashData) + "|" + customName + "|",
                            SecurityStamp = Guid.NewGuid().ToString(),
                            Sex = "",
                            PhoneNumber = "",
                            Email = "",
                            Status = "00",
                            Grade =  "05",
                            CheckStatus = "1",
                            UnitName = customName 
                        };

                       var  UserId = (int)db.Insert(user, true);
                        if (UserId > 0)
                        {
                            UserInRole UserRole = new UserInRole()
                            {
                                UserId = UserId,
                                RoleId = 2   //角色为检测机构
                            };
                            var UserRoleId = db.Insert(UserRole, true);
                        }

                        t_bp_custom custom = new t_bp_custom
                        {
                            ID = customId,
                            NAME = customName,
                            CREATETIME = DateTime.Now,
                            issubunit = "0",
                            data_status = "1",
                            APPROVALSTATUS = "0"
                        };
                        db.Insert(custom);

                        dbTrans.Commit();
                    }
                    catch (Exception ex)
                    {
                        dbTrans.Rollback();
                        erroMsg = ex.Message;
                        return false;
                    }


                }
                cacheInst.Clear();
                return true; 
            }
        }

        public bool SetPrintConfig(string customId, string cbgfs, out string errorMsg)
        {
            errorMsg = string.Empty;

            try
            {
                using (var db = dbFactory.Open())
                {
                    //TODO:
                    db.UpdateOnly(new t_bp_custom { bgfs = cbgfs }, p => p.bgfs, p => p.ID == customId);
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }

            return true;
        }

        public bool IsCustomIdExist(string customid)
        {
            if (string.IsNullOrWhiteSpace(customid))
            {
                return false;
            }
            var allCheckUnits = GetAllCheckUnit();
            return allCheckUnits.ContainsKey(customid);
        }

        /// <summary>
        /// 机构从临时转为正式
        /// </summary>
        /// <param name="customid">临时的机构编号</param>
        /// <param name="FormalCustomId">正式机构编号</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool SetInstFormal(string customid, string FormalCustomId, out string errMsg)
        {
            errMsg = string.Empty;
            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        db.UpdateOnly(() => new t_bp_custom { ID = FormalCustomId }, t => t.ID == customid);
                        db.UpdateOnly(() => new t_bp_People { Customid = FormalCustomId }, t => t.Customid == customid);
                        db.UpdateOnly(() => new t_bp_Equipment { customid = FormalCustomId }, t => t.customid == customid);
                        db.UpdateOnly(() => new t_bp_CusAchievement { CustomId = FormalCustomId }, t => t.CustomId == customid);
                        db.UpdateOnly(() => new t_bp_CusAward { CustomId = FormalCustomId }, t => t.CustomId == customid);
                        db.UpdateOnly(() => new t_bp_CusPunish { CustomId = FormalCustomId }, t => t.CustomId == customid);
                        db.UpdateOnly(() => new t_bp_CusChange { CustomId = FormalCustomId }, t => t.CustomId == customid);
                        var user = userRep.GetByCondition(t => t.UserName == FormalCustomId).FirstOrDefault();
                        var custom = db.Select<t_bp_custom>(db.From<t_bp_custom>().Where(t => t.ID == FormalCustomId)).FirstOrDefault();
                        //= rep.GetByCondition(t => t.ID == customid).FirstOrDefault();
                        if (user == null)
                        {
                            var userEntity = new User()
                            {
                                CustomId = FormalCustomId,
                                UserDisplayName = custom.NAME,
                                UserName = FormalCustomId,
                                PasswordHash = HashUtility.MD5HashHexStringFromUTF8String("123") + "|" + FormalCustomId + "|",
                                SecurityStamp = Guid.NewGuid().ToString(),
                                // Id = viewModel.UserId,
                                //Sex = viewModel.Sex,
                                //PhoneNumber = viewModel.Tel,
                                //Email = viewModel.Email,
                                Status = "00",
                                Grade = "02",
                                CheckStatus = "1"
                            };
                        }
                        else
                        {
                            db.UpdateOnly(() => new User { UserName = FormalCustomId }, t => t.UserName == customid);
                        }
                        var cusChange = new t_bp_CusChange()
                        {
                            CustomId = FormalCustomId,
                            ChaContent = "机构编号由{0}变更为{1},机构由临时机构变更为正式机构".Fmt(customid, FormalCustomId),
                            ChaDate = DateTime.Now,
                            //ChaAppUnit =
                            ChaRem = "机构所属人员，设备等信息一同变更"
                        };
                        var userId = db.Insert<t_bp_CusChange>(cusChange);
                        if (userId > 0)
                        {
                            UserInRole userInRole = new UserInRole()
                            {
                                UserId = (int)userId,
                                RoleId = 2
                            };
                            db.Insert(userInRole);
                        }
                        dbTrans.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        dbTrans.Rollback();
                        errMsg = ex.Message;
                        return false;
                    }
                }
            }
        }

        public bool UpdateWjlrAndZzlbmc(string wjlr, string zzlbmc, string customid, out string ErrMsg)
        {
            try
            {
                using (var db = dbFactory.Open())
                {
                    ErrMsg = string.Empty;
                    db.UpdateOnly(() => new t_bp_custom { wjlr = wjlr, zzlbmc = zzlbmc }, t => t.ID == customid);
                    return true;
                }
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
        }

        public bool NoReportNumCanSearch()
        {
            int TimeInterval = 1;
            int.TryParse(pkpmConfigService.NoReportNumCanSearchTimeInterval, out TimeInterval);
            DateTime dtNow = DateTime.Now;
            bool result = false;
            var dtLastTime = cacheDatetime.Get(PkPmCacheKeys.NoReportNumCanSearchDateTime);
            if (dtLastTime == null || dtLastTime == DateTime.MinValue)
            {
                result = true;
                cacheDatetime.Put(PkPmCacheKeys.NoReportNumCanSearchDateTime, dtNow.AddMinutes(TimeInterval));
            }
            else
            {
                if (dtLastTime <= dtNow)
                {
                    result = true;
                    cacheDatetime.Put(PkPmCacheKeys.NoReportNumCanSearchDateTime, dtNow.AddMinutes(TimeInterval));
                }
                else
                {
                    result = false;
                }
            }
            return result;
        }

        public bool CanEditEquip(string approvalstatus)
        {
            //(equip.approvalstatus == "6" || equip.approvalstatus == "2" || equip.approvalstatus == "0"))
            if (approvalstatus.IsNullOrEmpty())
            {
                return false;
            }

            if (approvalstatus == "6" || approvalstatus == "2" || approvalstatus == "0")
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool CanApplyChangeEquip(string approvalstatus)
        {
            if (approvalstatus.IsNullOrEmpty())
            {
                return false;
            }

            if (approvalstatus == "1")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CanEditCustom(string approvalstatus)
        {
            if (approvalstatus.IsNullOrEmpty())
            {
                return false;
            }

            if (approvalstatus == "6" || approvalstatus == "4" || approvalstatus == "2" || approvalstatus == "0" || approvalstatus =="5")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CanApplyChangeCustom(string approvalstatus)
        {
            if (approvalstatus.IsNullOrEmpty())
            {
                return false;
            }

            if (approvalstatus == "1"|| approvalstatus=="7")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public InstShortInfos GetCheckUnitByListCustomId(List<string> CustomIds)
        {
            var dicData = rep.GetDictByCondition<string, string>(r => CustomIds.Contains(r.ID) , r => new { r.ID, r.NAME });
            var data = InstShortInfos.FromDictonary(dicData);
            return data;
        }
 

        /// <summary>
        /// 获取机构信息，但是排除已经屏蔽的机构
        /// </summary>
        /// <returns></returns>
        public InstShortInfos GetAllCheckUnitEliminateIsUer()
        {
            var data = cacheInst.Get(PkPmCacheKeys.AllCheckUnitEliminateIsUser);

            if (data == null)
            {
                var dicData = rep.GetDictByCondition<string, string>(r => r.ID != null && r.data_status != "-1" && r.IsUse != 0, r => new { r.ID, r.NAME });
                data = InstShortInfos.FromDictonary(dicData);
                cacheInst.Put(PkPmCacheKeys.AllCheckUnitEliminateIsUser, data);

            }
            return data;
        }

        public bool SetInstStoppingState(string selectedId, out string erroMsg)// IsUse = 2 时为停业状态————by毛冰梅 2019.6.24
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
                            db.UpdateOnly(() => new t_bp_custom { IsUse = 2 }, p => p.ID == siArray[i]);
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
    }
}
