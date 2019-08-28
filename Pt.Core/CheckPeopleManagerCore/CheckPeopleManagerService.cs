using Pkpm.Core.CheckUnitCore;
using Pkpm.Entity;
using Pkpm.Entity.Models;
using Pkpm.Framework.Cache;
using Pkpm.Framework.Common;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Core.CheckPeopleManagerCore
{
    public class CheckPeopleManagerService:ICheckPeopleManagerService
    {
        IDbConnectionFactory dbFactory;
        ICheckUnitService checkUnitService;
        ICache<InstShortInfos> cacheInsts;
        public CheckPeopleManagerService(
            IDbConnectionFactory dbFactory,
            ICheckUnitService checkUnitService,
            ICache<InstShortInfos> cacheInsts)
        {
            this.dbFactory = dbFactory;
            this.checkUnitService = checkUnitService;
            this.cacheInsts = cacheInsts;
        }

        public bool ApplyChangePeople(SupvisorJob job, int peopleId, out string errorMsg)
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
                        db.UpdateOnly(() => new t_bp_People() { Approvalstatus = "5" }, t => t.id == peopleId);

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

        public bool EditPeople(CheckPeopleSaveModel editModel, out string erroMsg)
        {
            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        erroMsg = string.Empty;

                        //审核状态不能更新
                        db.UpdateOnly(editModel.people, r => new
                        {
                            r.Customid,
                            r.Name,
                            r.SelfNum,
                            r.ishaspostnum,
                            r.PostDate,
                            r.Education,
                            r.Professional,
                            r.zw,
                            r.iszcgccs,
                            r.isreghere,
                            r.zcgccszh,
                            r.Sex,
                            r.Birthday,
                            r.PostNum,
                            r.postnumstartdate,
                            r.postnumenddate,
                            r.School,
                            r.Title,
                            r.Tel,
                            r.Email,
                            r.iscb,
                            r.SBNum,
                            r.zcgccszhstartdate,
                            r.zcgccszhenddate,
                            r.PostType,
                            r.postTypeCode,
                            r.postDelayReg,
                            r.PhotoPath,
                            r.selfnumPath,
                            r.educationpath,
                            r.zcgccszhpath,
                            r.PostPath,
                            r.titlepath
                        }, r => r.id == editModel.people.id);

                        //申请修改已批准 6
                        //此编辑还需要再次审核
                        //if (editModel.people.Approvalstatus == "6")
                        //{
                        //    SupvisorJob superJob = new SupvisorJob
                        //    {
                        //        ApproveType = ApproveType.ApprovePeople,
                        //        CreateBy = editModel.OpUserId,
                        //        CreateTime = DateTime.Now,
                        //        CustomId = editModel.people.Customid,
                        //        NeedApproveId = editModel.people.id.ToString(),
                        //        NeedApproveStatus = NeedApproveStatus.ApproveChange,
                        //        SubmitName = "检测人员信息",
                        //        SubmitText = editModel.CustomName
                        //    };

                        //    db.Insert(superJob);
                        //}

                        db.Delete<t_bp_PeoChange>(r => r.PeopleId == editModel.people.id);
                        for (int i = 0; i < editModel.PeoChanges.Count; i++)
                        {
                            db.Insert(new t_bp_PeoChange()
                            {
                                PeopleId = editModel.PeoChanges[i].PeopleId,
                                ChaContent = editModel.PeoChanges[i].ChaContent,
                                ChaDate = editModel.PeoChanges[i].ChaDate
                            });

                            //editModel.PeoChanges[i], true);
                        }

                        //db.Delete<t_bp_PeopleN>(r => r.PeopleID == editModel.people.id.ToString());
                        //for (int i = 0; i < editModel.PeopleNs.Count; i++)
                        //{
                        //    db.InsertOnly(new t_bp_PeopleN()
                        //    {
                        //        PeopleID = editModel.PeopleNs[i].PeopleID,
                        //        Pcontext = editModel.PeopleNs[i].Pcontext,
                        //        addtime = editModel.PeopleNs[i].addtime
                        //    }, p => new { p.PeopleID, p.Pcontext, p.addtime });

                        //    //editModel.PeopleNs[i].id =-1;
                        //    //db.Insert(editModel.PeopleNs[i], true);
                        //}

                        db.Delete<t_bp_PeoEducation>(r => r.PeopleId == editModel.people.id);
                        for (int i = 0; i < editModel.PeoEducations.Count; i++)
                        {

                            db.Insert(new t_bp_PeoEducation()
                            {
                                PeopleId = editModel.PeoEducations[i].PeopleId,
                                TestDate = editModel.PeoEducations[i].TestDate,
                                TestResult = editModel.PeoEducations[i].TestResult,
                                TrainContent = editModel.PeoEducations[i].TrainContent,
                                TrainDate = editModel.PeoEducations[i].TrainDate,
                                TrainUnit = editModel.PeoEducations[i].TrainUnit
                            });

                            //editModel.PeoEducations[i].id = -1;
                            //db.Insert(editModel.PeoEducations[i], true);
                        }

                        db.Delete<t_bp_PeoAward>(r => r.PeopleId == editModel.people.id);
                        for (int i = 0; i < editModel.PeoAwards.Count; i++)
                        {
                            db.Insert(new t_bp_PeoAward()
                            {
                                PeopleId = editModel.PeoAwards[i].PeopleId,
                                AwaContent = editModel.PeoAwards[i].AwaContent,
                                AwaDate = editModel.PeoAwards[i].AwaDate,
                                AwaUnit = editModel.PeoAwards[i].AwaUnit
                            });

                            //editModel.PeoAwards[i].id = -1;
                            //db.Insert(editModel.PeoAwards[i], true);
                        }

                        db.Delete<t_bp_PeoPunish>(r => r.PeopleId == editModel.people.id);
                        for (int i = 0; i < editModel.PeoPunishs.Count; i++)
                        {
                            db.Insert(new t_bp_PeoPunish()
                            {
                                PeopleId = editModel.PeoPunishs[i].PeopleId,
                                PunContent = editModel.PeoPunishs[i].PunContent,
                                PunDate = editModel.PeoPunishs[i].PunDate,
                                PunName = editModel.PeoPunishs[i].PunName,
                                PunUnit = editModel.PeoPunishs[i].PunUnit
                            });

                            // editModel.PeoPunishs[i].id = -1;
                            // db.Insert(editModel.PeoPunishs[i], true);
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
        /// 修改人员信息,先保存到临时表，待审核 add by ydf 2019-04-09
        /// </summary>
        /// <param name="editModel"></param>
        /// <param name="erroMsg"></param>
        /// <returns></returns>
        public bool EditPeople(CheckPeopleTmpSaveModel editModel, out string erroMsg)
        {
            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        erroMsg = string.Empty;

                        //先删掉之前的记录，再保存到临时表
                        db.Delete<t_bp_People_tmp>(p => p.id == editModel.people.id);

                        //已递交待审核
                        editModel.people.Approvalstatus = "1";
                        db.Insert(editModel.people, true);

                        //更新原始人员信息状态
                        var peopleOrig = new t_bp_People();
                        peopleOrig.id = editModel.people.id;
                        peopleOrig.Approvalstatus = "1";
                        db.UpdateOnly(peopleOrig, p => p.Approvalstatus, p => p.id == peopleOrig.id);

                        #region 关联表更新

                        db.Delete<t_bp_PeoChange>(r => r.PeopleId == editModel.people.id);
                        for (int i = 0; i < editModel.PeoChanges.Count; i++)
                        {
                            db.Insert(new t_bp_PeoChange()
                            {
                                PeopleId = editModel.PeoChanges[i].PeopleId,
                                ChaContent = editModel.PeoChanges[i].ChaContent,
                                ChaDate = editModel.PeoChanges[i].ChaDate
                            });
                        }

                        db.Delete<t_bp_PeoEducation>(r => r.PeopleId == editModel.people.id);
                        for (int i = 0; i < editModel.PeoEducations.Count; i++)
                        {

                            db.Insert(new t_bp_PeoEducation()
                            {
                                PeopleId = editModel.PeoEducations[i].PeopleId,
                                TestDate = editModel.PeoEducations[i].TestDate,
                                TestResult = editModel.PeoEducations[i].TestResult,
                                TrainContent = editModel.PeoEducations[i].TrainContent,
                                TrainDate = editModel.PeoEducations[i].TrainDate,
                                TrainUnit = editModel.PeoEducations[i].TrainUnit
                            });

                            //editModel.PeoEducations[i].id = -1;
                            //db.Insert(editModel.PeoEducations[i], true);
                        }

                        db.Delete<t_bp_PeoAward>(r => r.PeopleId == editModel.people.id);
                        for (int i = 0; i < editModel.PeoAwards.Count; i++)
                        {
                            db.Insert(new t_bp_PeoAward()
                            {
                                PeopleId = editModel.PeoAwards[i].PeopleId,
                                AwaContent = editModel.PeoAwards[i].AwaContent,
                                AwaDate = editModel.PeoAwards[i].AwaDate,
                                AwaUnit = editModel.PeoAwards[i].AwaUnit
                            });

                            //editModel.PeoAwards[i].id = -1;
                            //db.Insert(editModel.PeoAwards[i], true);
                        }

                        db.Delete<t_bp_PeoPunish>(r => r.PeopleId == editModel.people.id);
                        for (int i = 0; i < editModel.PeoPunishs.Count; i++)
                        {
                            db.Insert(new t_bp_PeoPunish()
                            {
                                PeopleId = editModel.PeoPunishs[i].PeopleId,
                                PunContent = editModel.PeoPunishs[i].PunContent,
                                PunDate = editModel.PeoPunishs[i].PunDate,
                                PunName = editModel.PeoPunishs[i].PunName,
                                PunUnit = editModel.PeoPunishs[i].PunUnit
                            });

                            // editModel.PeoPunishs[i].id = -1;
                            // db.Insert(editModel.PeoPunishs[i], true);
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
        /// 对修改的人员信息进行审核
        /// </summary>
        /// <param name="formCol"></param>
        /// <param name="dic"></param>
        /// <param name="erroMsg"></param>
        /// <returns></returns>
        public bool AuditPeople(NameValueCollection formCol, Dictionary<string, List<SysDict>> dic, out string errorMsg)
        {
            errorMsg = string.Empty;

            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        var checkUnits = checkUnitService.GetAllCheckUnit();
                        int id = Convert.ToInt32(formCol["Id"]);
                        string op = formCol["Operate"];
                        var result = 0;
                        if (op == "Y") //审核通过
                        {
                            //删除人员信息临时表数据
                            db.Delete<t_bp_People_tmp>(r => r.id == id);

                            //更新机构信息表
                            t_bp_People people = new t_bp_People();
                            people.id = id;
                            people.Approvalstatus = "0"; //审核通过重置为未递交
                            int index = 0;
                            formCol.Remove("Operate"); //移除掉
                            formCol.Add("Approvalstatus", people.Approvalstatus);
                            string[] columns = new string[formCol.Count - 1];

                            //通过反射给实体字段赋值
                            PropertyInfo[] mPi = typeof(t_bp_People).GetProperties();
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
                                                pi.SetValue(people, Convert.ToDateTime(formCol[item.ToString()]));
                                            }
                                            else
                                            {
                                                if (pi.Name == "Customid")
                                                {
                                                    pi.SetValue(people, checkUnits.Where(p => p.Value == formCol[item.ToString()]).Select(p => p.Key).FirstOrDefault());
                                                }
                                                else if (pi.Name == "isreghere")
                                                {
                                                    pi.SetValue(people, dic["yesNo"].Where(x => x.Name == formCol[item.ToString()]).ToList().FirstOrDefault().KeyValue);
                                                }
                                                else if (pi.Name == "ishaspostnum")
                                                {
                                                    pi.SetValue(people, dic["yesNo"].Where(x => x.Name == formCol[item.ToString()]).ToList().FirstOrDefault().KeyValue);
                                                }
                                                else if (pi.Name == "Title")
                                                {
                                                    pi.SetValue(people, dic["personnelTitles"].Where(x => x.Name == formCol[item.ToString()]).ToList().FirstOrDefault().KeyValue);
                                                }
                                                else if (pi.Name == "iscb")
                                                {
                                                    pi.SetValue(people, dic["workStatus"].Where(x => x.Name == formCol[item.ToString()]).ToList().FirstOrDefault().KeyValue);
                                                }
                                                else if (pi.Name == "iszcgccs")
                                                {
                                                    pi.SetValue(people, dic["engineersTypes"].Where(x => x.Name == formCol[item.ToString()]).ToList().FirstOrDefault().KeyValue);
                                                }
                                                else if (pi.Name == "PostType")
                                                {
                                                    pi.SetValue(people, formCol[item.ToString()].Replace(" ", ";<br>"));
                                                }
                                                else
                                                {
                                                    pi.SetValue(people, formCol[item.ToString()]);
                                                }
                                            }
                                        }
                                    }

                                    columns[index] = item.ToString();
                                    index++;
                                }
                            }

                            //更新，根据给定字段
                            result = db.UpdateOnly(people, columns, r => r.id == people.id);
                        }
                        else //审核不通过
                        {
                            //删除人员信息临时表数据
                            db.Delete<t_bp_People_tmp>(r => r.id == id);

                            //更新人员信息表 t_bp_custom
                            t_bp_People people = new t_bp_People();
                            people.id = id;
                            people.Approvalstatus = "0";

                            //更新
                            result = db.UpdateOnly(people, r => r.Approvalstatus, r => r.id == people.id);
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



        public bool EditPeopleField(CheckPeopleSaveModel editModel, out string erroMsg)
        {
            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        erroMsg = string.Empty;

                        //审核状态不能更新
                        db.UpdateOnly(editModel.people, r => new
                        {
                            r.Customid,
                            r.Name,
                            r.SelfNum,
                            r.Education,
                            r.Professional,
                            r.zw,
                            r.iszcgccs,
                            r.isreghere,
                            r.zcgccszh,
                            r.Sex,
                            r.Birthday,
                            r.School,
                            r.Title,
                            r.Tel,
                            r.Email,
                            r.iscb,
                            r.SBNum,
                            r.zcgccszhstartdate,
                            r.zcgccszhenddate,
                            r.PostDate,
                            r.PostType,
                            r.postTypeCode,
                            r.postDelayReg,
                            r.PhotoPath,
                            r.selfnumPath,
                            r.educationpath,
                            r.zcgccszhpath,
                            r.PostPath,
                            r.titlepath,
                            r.ishaspostnum
                        }, r => r.id == editModel.people.id);


                        db.Delete<t_bp_PeoChange>(r => r.PeopleId == editModel.people.id);
                        for (int i = 0; i < editModel.PeoChanges.Count; i++)
                        {
                            db.Insert(new t_bp_PeoChange()
                            {
                                PeopleId = editModel.PeoChanges[i].PeopleId,
                                ChaContent = editModel.PeoChanges[i].ChaContent,
                                ChaDate = editModel.PeoChanges[i].ChaDate
                            });

                            //editModel.PeoChanges[i], true);
                        }

                        //db.Delete<t_bp_PeopleN>(r => r.PeopleID == editModel.people.id.ToString());
                        //for (int i = 0; i < editModel.PeopleNs.Count; i++)
                        //{
                        //    db.InsertOnly(new t_bp_PeopleN()
                        //    {
                        //        PeopleID = editModel.PeopleNs[i].PeopleID,
                        //        Pcontext = editModel.PeopleNs[i].Pcontext,
                        //        addtime = editModel.PeopleNs[i].addtime
                        //    }, p => new { p.PeopleID, p.Pcontext, p.addtime });

                        //    //editModel.PeopleNs[i].id =-1;
                        //    //db.Insert(editModel.PeopleNs[i], true);
                        //}

                        db.Delete<t_bp_PeoEducation>(r => r.PeopleId == editModel.people.id);
                        for (int i = 0; i < editModel.PeoEducations.Count; i++)
                        {

                            db.Insert(new t_bp_PeoEducation()
                            {
                                PeopleId = editModel.PeoEducations[i].PeopleId,
                                TestDate = editModel.PeoEducations[i].TestDate,
                                TestResult = editModel.PeoEducations[i].TestResult,
                                TrainContent = editModel.PeoEducations[i].TrainContent,
                                TrainDate = editModel.PeoEducations[i].TrainDate,
                                TrainUnit = editModel.PeoEducations[i].TrainUnit
                            });

                            //editModel.PeoEducations[i].id = -1;
                            //db.Insert(editModel.PeoEducations[i], true);
                        }

                        db.Delete<t_bp_PeoAward>(r => r.PeopleId == editModel.people.id);
                        for (int i = 0; i < editModel.PeoAwards.Count; i++)
                        {
                            db.Insert(new t_bp_PeoAward()
                            {
                                PeopleId = editModel.PeoAwards[i].PeopleId,
                                AwaContent = editModel.PeoAwards[i].AwaContent,
                                AwaDate = editModel.PeoAwards[i].AwaDate,
                                AwaUnit = editModel.PeoAwards[i].AwaUnit
                            });

                            //editModel.PeoAwards[i].id = -1;
                            //db.Insert(editModel.PeoAwards[i], true);
                        }

                        db.Delete<t_bp_PeoPunish>(r => r.PeopleId == editModel.people.id);
                        for (int i = 0; i < editModel.PeoPunishs.Count; i++)
                        {
                            db.Insert(new t_bp_PeoPunish()
                            {
                                PeopleId = editModel.PeoPunishs[i].PeopleId,
                                PunContent = editModel.PeoPunishs[i].PunContent,
                                PunDate = editModel.PeoPunishs[i].PunDate,
                                PunName = editModel.PeoPunishs[i].PunName,
                                PunUnit = editModel.PeoPunishs[i].PunUnit
                            });

                            // editModel.PeoPunishs[i].id = -1;
                            // db.Insert(editModel.PeoPunishs[i], true);
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


        public bool CreatPeople(CheckPeopleSaveModel createModel, out string erroMsg)
        {
            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        erroMsg = string.Empty;
                        var peopleId = db.Insert(createModel.people, true);
                        var userId = db.Insert(createModel.user, true);
                        if (userId > 0)
                        {
                            UserInRole UserRole = new UserInRole()
                            {
                                UserId = (int)userId,
                                RoleId = 9
                            };
                            var UserRoleId = db.Insert(UserRole, true);
                        }
                        for (int i = 0; i < createModel.PeoChanges.Count; i++)
                        {
                            createModel.PeoChanges[i].PeopleId = (int)peopleId;
                            db.Insert(createModel.PeoChanges[i]);
                        }

                        //for (int i = 0; i < createModel.PeopleNs.Count; i++)
                        //{
                        //    createModel.PeopleNs[i].PeopleID = peopleId.ToString();
                        //    db.InsertOnly(createModel.PeopleNs[i], r => new { r.addtime, r.PeopleID, r.Pcontext });
                        //}

                        for (int i = 0; i < createModel.PeoEducations.Count; i++)
                        {
                            createModel.PeoEducations[i].PeopleId = (int)peopleId;
                            db.Insert(createModel.PeoEducations[i]);
                        }

                        for (int i = 0; i < createModel.PeoAwards.Count; i++)
                        {
                            createModel.PeoAwards[i].PeopleId = (int)peopleId;
                            db.Insert(createModel.PeoAwards[i]);
                        }

                        for (int i = 0; i < createModel.PeoPunishs.Count; i++)
                        {
                            createModel.PeoPunishs[i].PeopleId = (int)peopleId;
                            db.Insert(createModel.PeoPunishs[i]);
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

        public bool SetPosttype(string id, string mv, out string erroMsg)
        {
            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        erroMsg = string.Empty;
                        string postTypeTime = db.Single<t_bp_postType>(p => p.Id == id).postTypeTime;
                        db.UpdateOnly(() => new t_bp_postType() { postTypeTime = postTypeTime + "|" + mv }, t => t.Id == id);
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

        public bool DeletePeople(string selectedId, out string erroMsg)
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
                            //修改删除的时候只是将状态修改为-1
                            string selfNum = db.Single<t_bp_People>(r => r.id == Convert.ToInt32(siArray[i])).SelfNum;
                            db.Delete<User>(r => r.UserName == selfNum);

                            db.UpdateOnly<t_bp_People>(new t_bp_People() { data_status = "-1", update_time = DateTime.Now },
                                    p => new { p.data_status, p.update_time },
                                    p => p.id == Convert.ToInt32(siArray[i]));

                            //db.Delete<t_bp_People>(r => r.id == Convert.ToInt32(siArray[i]));
                            //db.Delete<t_bp_PeoChange>(r => r.PeopleId == Convert.ToInt32(siArray[i]));
                            //db.Delete<t_bp_PeopleN>(r => r.PeopleID == siArray[i]);
                            //db.Delete<t_bp_PeoEducation>(r => r.PeopleId == Convert.ToInt32(siArray[i]));
                            //db.Delete<t_bp_PeoAward>(r => r.PeopleId == Convert.ToInt32(siArray[i]));
                            //db.Delete<t_bp_PeoPunish>(r => r.PeopleId == Convert.ToInt32(siArray[i]));
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

        public bool SendPeople(string selectedId, out string erroMsg)
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
                            db.UpdateOnly(new t_bp_People { Approvalstatus = "1" }, p => p.Approvalstatus, p => p.id == Convert.ToInt32(siArray[i]));
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

        public bool ReturnStatePeople(string selectedId, out string erroMsg)
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
                            db.UpdateOnly(new t_bp_People { Approvalstatus = "0" }, p => p.Approvalstatus, p => p.id == Convert.ToInt32(siArray[i]));
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

        public bool AnnualTestPeople(string selectedId, out string erroMsg)
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
                            //db.InsertOnly(new t_bp_PeopleN { PeopleID = siArray[i], Pcontext = DateTime.Now.Year.ToString() + "年通过年审", addtime = DateTime.Now }, p => new { p.PeopleID, p.Pcontext, p.addtime });
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

        public bool FirePeople(string selectedId, out string erroMsg)
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
                            db.UpdateOnly(new t_bp_People { iscb = "0" }, p => p.Approvalstatus, p => p.id == Convert.ToInt32(siArray[i]));
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

        public bool ChangePeople(string peopleId, string Customid, out string erroMsg)
        {
            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        erroMsg = string.Empty;
                        var people = db.Single<t_bp_People>(p => p.id == Convert.ToInt32(peopleId));
                        string OldCustomid = people.Customid;
                        string OldCustomName = checkUnitService.GetCheckUnitById(OldCustomid);

                        string NewCustomName = checkUnitService.GetCheckUnitById(Customid);

                        if (NewCustomName == Customid)
                        {
                            erroMsg = "机构信息不存在，请确认选择后重新提交";
                            return false;
                        }
                        string ChaContent = "工作单位由" + OldCustomName + "变更为" + NewCustomName;
                        DateTime ChaDate = DateTime.Now.Date;
                        db.Insert(new t_bp_PeoChange { PeopleId = Convert.ToInt32(peopleId), ChaContent = ChaContent, ChaDate = ChaDate });
                        db.UpdateOnly(new t_bp_People { Customid = Customid }, p => p.Customid, p => p.id == Convert.ToInt32(peopleId));
                        var user = db.Single<User>(p => p.UserName == people.SelfNum);
                        var userId = 0;
                        if (user == null)
                        {
                            //db.Insert(new User { UserName = people.SelfNum });
                            User us = new User
                            {
                                UserName = people.SelfNum,
                                UserDisplayName = people.Name,
                                Sex = people.Sex,
                                CustomId = Customid,
                                StationId = "2700001",
                                CheckStatus = "1",
                                PasswordHash = HashUtility.MD5HashHexStringFromUTF8String("123") + "|" + people.SelfNum + "|",
                                SecurityStamp = Guid.NewGuid().ToString(),
                                Status = "00",
                                Grade = "02",
                            };
                            userId = (int)db.Insert(us, true);
                            if (userId > 0)
                            {
                                UserInRole userInRole = new UserInRole()
                                {
                                    UserId = userId,
                                    RoleId = 2
                                };
                                db.Insert(userInRole);
                            }
                        }
                        else
                        {
                            db.UpdateOnly(new User { CustomId = Customid }, p => p.CustomId, p => p.Id == user.Id);

                        }
                        cacheInsts.Remove(PkPmCacheKeys.CustomsByUserIdFmt.Fmt(userId));
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
        public bool SetPeopleScreeningState(string selectedId, out string erroMsg)
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
                            db.UpdateOnly(() => new t_bp_People { IsUse = 0 }, p => p.id == int.Parse(siArray[i]));

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

        public bool SetPeopleRelieveScreeningSate(string selectId, out string erroMsg)
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
                            db.UpdateOnly(() => new t_bp_People { IsUse = 1 }, p => p.id == int.Parse(siArray[i]));
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

        public List<CheckPeopleUIModel> GetRegisterOnJobPeople(Expression<Func<t_bp_People, bool>> predicate, List<string> visibleInstIds, int? pot, int? count, out int dbCount)
        {
            //var predicate = PredicateBuilder.True<t_bp_custom>();

            using (var db = dbFactory.Open())
            {
                //predicate = predicate.And(t => t.DETECTNUM.Contains("建检字第") && t.ID.Length>0);

                var peo = db.From<t_bp_People>();
                var cus = db.From<t_bp_custom>();

                //根据条件查出所有CustomId
                if (visibleInstIds != null && visibleInstIds.Count() > 0)
                {
                    cus = cus.Where(c => visibleInstIds.Contains(c.ID));
                }
                cus = cus
                    .Where(t => t.DETECTNUM.Contains("建检字第") && t.ID != null)
                    .Select(t => t.ID);

                //查所有CustomId所对应的id
                peo = peo
                    .Where(t => Sql.In(t.Customid, cus) && Sql.In(t.iscb, "1", "2", "3"))
                    .Where(predicate)
                    .Select(t => t.id);

                dbCount = (int)db.Count(peo);
                //分页查询对应的id集合
                var ids = db.Select<int>(peo.Limit(pot, count));

                //根据id集合多表关联查出所有要查询的字段

                var q2 = db.From<t_bp_People>()
                    .Where(t => ids.Contains(t.id))
                    .Select(p => new { p.id, p.Name, p.Customid, p.SelfNum, p.PostNum, p.zw, p.Title, p.iscb, p.Approvalstatus });

                return db.Select<CheckPeopleUIModel>(q2);
            }
        }

        public List<CheckPeopleUIModel> GetNotRegisterPeople(Expression<Func<t_bp_People, bool>> predicate, List<string> visibleInstIds, int? pot, int? count, out int dbCount)
        {
            //var predicate = PredicateBuilder.True<t_bp_custom>();

            using (var db = dbFactory.Open())
            {
                //predicate = predicate.And(t => t.DETECTNUM ==null && t.ID.Length > 0 && t.ID.Substring(t.ID.Length - 1, t.ID.Length )=="1");

                var peo = db.From<t_bp_People>();
                var cus = db.From<t_bp_custom>();

                //根据条件查出所有CustomId
                if (visibleInstIds != null && visibleInstIds.Count() > 0)
                {
                    cus = cus.Where(c => visibleInstIds.Contains(c.ID));
                }

                cus = cus
                    .Where(t => t.DETECTNUM == null && (t.ID != null || t.ID.EndsWith("1")))
                    .Select(t => t.ID);

                //查所有CustomId所对应的id
                peo = peo
                    .Where(t => Sql.In(t.Customid, cus))
                    .Where(predicate)
                    .Select(t => t.id);

                dbCount = (int)db.Count(peo);
                //分页查询对应的id集合
                var ids = db.Select<int>(peo.Limit(pot, count));

                //根据id集合多表关联查出所有要查询的字段
                var q2 = db.From<t_bp_People>()
                    .Where(t => ids.Contains(t.id))
                    .Select(p => new { p.id, p.Name, p.Customid, p.SelfNum, p.PostNum, p.zw, p.Title, p.iscb, p.Approvalstatus });

                return db.Select<CheckPeopleUIModel>(q2);
            }
        }

        public List<CheckPeopleUIModel> GetRegisterLeaveJobPeople(Expression<Func<t_bp_People, bool>> predicate, List<string> visibleInstIds, int? pot, int? count, out int dbCount)
        {
            //var predicate = PredicateBuilder.True<t_bp_custom>();

            using (var db = dbFactory.Open())
            {
                // predicate = predicate.And(t => t.DETECTNUM.Contains("建检字第") && t.ID.Length > 0);

                var peo = db.From<t_bp_People>();
                var cus = db.From<t_bp_custom>();

                //根据条件查出所有CustomId
                if (visibleInstIds != null && visibleInstIds.Count() > 0)
                {
                    cus = cus.Where(c => visibleInstIds.Contains(c.ID));
                }

                cus = cus
                    .Where(t => t.DETECTNUM.Contains("建检字第") && t.ID != null)
                    .Select(t => t.ID);

                //查所有CustomId所对应的id
                peo = peo
                    .Where(t => Sql.In(t.Customid, cus) && t.iscb == "0")
                    .Where(predicate)
                    .Select(t => t.id);

                dbCount = (int)db.Count(peo);
                //分页查询对应的id集合
                var ids = db.Select<int>(peo.Limit(pot, count));

                //根据id集合多表关联查出所有要查询的字段
                var q2 = db.From<t_bp_People>()
                    .Where(t => ids.Contains(t.id))
                    .Select(p => new { p.id, p.Name, p.Customid, p.SelfNum, p.PostNum, p.zw, p.Title, p.iscb, p.Approvalstatus });

                return db.Select<CheckPeopleUIModel>(q2);
            }
        }

        public List<CheckPeopleUIModel> GetLogoutPeople(Expression<Func<t_bp_People, bool>> predicate, List<string> visibleInstIds, int? pot, int? count, out int dbCount)
        {
            //var predicate = PredicateBuilder.True<t_bp_custom>();

            using (var db = dbFactory.Open())
            {
                //predicate = predicate.And(t => t.DETECTNUM.Contains("建检字第") && t.ID.Length > 0);

                var peo = db.From<t_bp_People>();
                var cus = db.From<t_bp_custom>();

                //根据条件查出所有CustomId
                if (visibleInstIds != null && visibleInstIds.Count() > 0)
                {
                    cus = cus.Where(c => visibleInstIds.Contains(c.ID));
                }

                cus = cus
                    .Where(t => t.DETECTNUM.Contains("建检字第") && t.ID != null)
                    .Select(t => t.ID);

                //查所有CustomId所对应的id
                peo = peo
                    .Where(t => Sql.In(t.Customid, cus) && t.iscb == "4")
                    .Where(predicate)
                    .Select(t => t.id);

                dbCount = (int)db.Count(peo);
                //分页查询对应的id集合
                var ids = db.Select<int>(peo.Limit(pot, count));

                //根据id集合多表关联查出所有要查询的字段
                var q2 = db.From<t_bp_People>()
                    .Where(t => ids.Contains(t.id))
                    .Select(p => new { p.id, p.Name, p.Customid, p.SelfNum, p.PostNum, p.zw, p.Title, p.iscb, p.Approvalstatus });

                return db.Select<CheckPeopleUIModel>(q2);
            }
        }

        public bool UpdateAttachPathsIntoPeople(int id, string fieldname, string pathname, out string erroMsg)
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
                            case "PhotoPath":
                                db.UpdateOnly(new t_bp_People { PhotoPath = pathname }, p => p.PhotoPath, p => p.id == id);
                                break;
                            case "selfnumPath":
                                db.UpdateOnly(new t_bp_People { selfnumPath = pathname }, p => p.selfnumPath, p => p.id == id);
                                break;
                            case "educationpath":
                                db.UpdateOnly(new t_bp_People { educationpath = pathname }, p => p.educationpath, p => p.id == id);
                                break;
                            case "zcgccszhpath":
                                db.UpdateOnly(new t_bp_People { zcgccszhpath = pathname }, p => p.zcgccszhpath, p => p.id == id);
                                break;
                            case "PostPath":
                                db.UpdateOnly(new t_bp_People { PostPath = pathname }, p => p.PostPath, p => p.id == id);
                                break;
                            case "titlepath":
                                db.UpdateOnly(new t_bp_People { titlepath = pathname }, p => p.titlepath, p => p.id == id);
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

        public bool UpdatecellModelParams(string id, string value, out string erroMsg)
        {
            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        erroMsg = string.Empty;
                        db.UpdateOnly(new t_sys_cellModelParam { modelParams = value }, p => p.modelParams, p => p.id == id);
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

        public bool UpdatePostNumDate(int id, DateTime PostDate, DateTime PostNumStartDate, DateTime PostNumEndDate, out string erroMsg)
        {
            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        erroMsg = string.Empty;
                        db.UpdateOnly(new t_bp_People { PostDate = PostDate, postnumstartdate = PostNumStartDate, postnumenddate = PostNumEndDate }, p => new { p.PostDate, p.postnumstartdate, p.postnumenddate }, p => p.id == id);
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


        public bool UpdatePostType(string PostType, string postTypeCode, string selfnum, out string erroMsg)
        {
            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        erroMsg = string.Empty;
                        db.UpdateOnly(new t_bp_People { PostType = PostType, postTypeCode = postTypeCode }, p => new { p.PostType, p.postTypeCode }, p => p.SelfNum == selfnum);
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

        //public List<AutoType> GetAutoType(string UserId)
        //{
        //    using (var db = dbFactory.Open())
        //    {
        //        string SqlGetT_Ks_Score = "select id,listnum,typeid,(select FCardNo from curTestRegister where ID IN (curtestregisterid) ) as CardNo " +
        //                    " ,(select posttype from t_bp_postType where KTestTypeId =typeid ) as ff ,(select code from t_bp_postType where KTestTypeId =typeid ) as code " +
        //                    " from T_Ks_Score where curtestregisterid IN (SELECT TOP 1 ID FROM dbo.curTestRegister WHERE FCARDNO IN (SELECT SELFNUM FROM t_bp_people WHERE ID=" + UserId + ")) and stateN =1" +
        //                    " and id in (select max(id) from T_Ks_Score group by typeid )";

        //        var list = ServiceStack.OrmLite.Dapper.SqlMapper.Query<AutoType>(db, SqlGetT_Ks_Score).ToList();
        //        return list;
        //    }
        //}

        public bool CanEditPeople(string approvalstatus)
        {
            if (approvalstatus == "0")
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool CanApplyChangePeople(string approvalstatus)
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
    }
}
