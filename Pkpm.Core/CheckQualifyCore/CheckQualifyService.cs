using Pkpm.Entity;
using Pkpm.Framework.Repsitory;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Core.CheckQualifyCore
{
    public class CheckQualifyService : ICheckQualifyService
    {
        IRepsitory<t_D_UserTableOne> rep;
        IRepsitory<t_D_UserTableTwo> repTwo;
        IRepsitory<t_D_UserDistributeExpert> repDis;
        IRepsitory<t_D_UserExpertUnit> repExp;
        IRepsitory<t_D_UserTableThree> repThree;
        IRepsitory<t_D_UserTableFour> repFour;
        IDbConnectionFactory dbFactory;

        public CheckQualifyService(IRepsitory<t_D_UserTableOne> rep,
            IRepsitory<t_D_UserTableTwo> repTwo,
            IRepsitory<t_D_UserTableThree> repThree,
            IRepsitory<t_D_UserTableFour> repFour,
            IRepsitory<t_D_UserExpertUnit> repExp,
            IRepsitory<t_D_UserDistributeExpert> repDis,
            IDbConnectionFactory dbFactory)
        {
            this.dbFactory = dbFactory;
            this.rep = rep;
            this.repTwo = repTwo;
            this.repDis = repDis;
            this.repExp = repExp;
            this.repThree = repThree;
            this.repFour = repFour;
        }





        /// <summary>
        /// t_D_UserTableOne 新增
        /// </summary>
        /// <param name="saveModel"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool SaveApplyQualifyOne(ApplyQualifySaveModel saveModel, out string errorMsg)
        {
            using (var db = dbFactory.Open())
            {
                try
                {
                    errorMsg = string.Empty;

                    //更新原始机构信息状态
                    var model = new t_D_UserTableOne()
                    {
                        name = saveModel.Name,
                        addtime = saveModel.AddTime.HasValue ? saveModel.AddTime : DateTime.Now,
                        time = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")),
                        selfnum = saveModel.Selfnum,
                        unitname = saveModel.UnitName,
                        unitCode = saveModel.UnitCode,
                        @static = 0,
                        onepath_zl = "1",   //表示已添加
                        type = saveModel.Type,
                        area = saveModel.Area
                    };

                    db.Insert(model, true);

                    return true;
                }
                catch (Exception ex)
                {
                    errorMsg = ex.Message;
                    return false;
                }
            }
        }


        /// <summary>
        /// t_D_UserTableOne 修改
        /// </summary>
        /// <param name="saveModel"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool EditApplyQualifyOne(ApplyQualifySaveModel saveModel, out string errorMsg)
        {
            using (var db = dbFactory.Open())
            {
                try
                {
                    errorMsg = string.Empty;

                    var model = new t_D_UserTableOne()
                    {
                        name = saveModel.Name,
                        selfnum = saveModel.Selfnum,
                        unitname = saveModel.UnitName,
                        area = saveModel.Area
                    };

                    db.UpdateOnly(model, r => new
                    {
                        r.name,
                        r.selfnum,
                        r.unitname,
                        r.area
                    }, r => r.id == saveModel.Id);

                    return true;
                }
                catch (Exception ex)
                {
                    errorMsg = ex.Message;
                    return false;
                }
            }
        }

        /// <summary>
        /// t_D_UserTableTwo
        /// </summary>
        /// <param name="saveModel"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool SaveUserTableTwo(t_D_UserTableTwo saveModel, out string errorMsg)
        {
            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        errorMsg = string.Empty;
                        if (db.Select<t_D_UserTableTwo>(r => r.pid == saveModel.pid).Count > 0)
                        {
                            db.UpdateOnly(saveModel, r => new
                            {
                                r.name,
                                r.address,
                                r.tel,
                                r.postcode,
                                r.Fax,
                                r.Email,
                                r.businessnum,
                                r.businessnumpath,
                                r.businessnumunit,
                                r.regaprice,
                                r.economicnature,
                                r.measnum,
                                r.measunit,
                                r.fr,
                                r.frzw,
                                r.frzc,
                                r.jsfzr,
                                r.jsfzrzw,
                                r.jsfzrzc,
                                r.zbryzs,
                                r.zyjsrys,
                                r.zjzcrs,
                                r.gjzcrs,
                                r.yqsbzs,
                                r.yqsbgtzc,
                                r.gzmj,
                                r.fwjzmj,
                                r.sqjcyw,
                                r.measnumpath,
                                r.zzjgdm,
                                r.YZZPath,
                                r.SQname,
                                r.SQTel,
                                r.bgcszmPath,
                                r.zxzmPath,
                                r.gqzmPath,
                                r.zzscwjPath,
                                r.glscwjPath
                            }, r => r.pid == saveModel.pid);
                        }
                        else
                        {
                            db.Insert<t_D_UserTableTwo>(saveModel);
                        }

                        //更新t_D_UserTableOne上的相应标识
                        db.UpdateOnly(new t_D_UserTableOne
                        {
                            twopath_zl = "1"
                        }, r => new
                        {
                            r.twopath_zl
                        }, r => r.id == saveModel.pid);


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

        /// <summary>
        /// t_D_UserTableThree
        /// </summary>
        /// <param name="model"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool EditQualifyThree(t_D_UserTableThree model, out string errorMsg)
        {
            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        errorMsg = string.Empty;
                        if (db.Select<t_D_UserTableThree>(r => r.pid == model.pid).Count > 0)
                        {
                            db.UpdateOnly(new t_D_UserTableThree
                            {
                                name = model.name,
                                sex = model.sex,
                                time = model.time,
                                hshxhzy = model.hshxhzy,
                                postNum = model.postNum,
                                zw = model.zw,
                                zc = model.zc,
                                xl = model.xl,
                                zcpath = model.zcpath,
                                xlpath = model.xlpath,
                                photopath = model.photopath,
                                postNumpath = model.postNumpath,
                                gzjl = model.gzjl,
                                jcgzlx = model.jcgzlx,
                                yddh = model.yddh,
                                bgdh = model.bgdh
                            }, r => new
                            {
                                r.name,
                                r.sex,
                                r.time,
                                r.zw,
                                r.zc,
                                r.xl,
                                r.xlpath,
                                r.hshxhzy,
                                r.gzjl,
                                r.jcgzlx,
                                r.photopath,
                                r.yddh,
                                r.bgdh,
                                r.zcpath,
                                r.postNumpath,
                            }, r => r.pid == model.pid);
                        }
                        else
                        {
                            db.Insert<t_D_UserTableThree>(model);
                        }

                        //更新t_D_UserTableOne上的相应标识
                        db.UpdateOnly(new t_D_UserTableOne
                        {
                            threepath_zl = "1"
                        }, r => new
                        {
                            r.threepath_zl
                        }, r => r.id == Convert.ToInt32(model.pid));

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

        /// <summary>
        /// t_D_UserTableFour
        /// </summary>
        /// <param name="model"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool EditQualifyFours(t_D_UserTableFour model, out string errorMsg)
        {
            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        errorMsg = string.Empty;
                        if (db.Select<t_D_UserTableFour>(r => r.pid == model.pid).Count > 0)
                        {
                            db.UpdateOnly(new t_D_UserTableFour
                            {
                                name = model.name,
                                sex = model.sex,
                                time = model.time,
                                hshxhzy = model.hshxhzy,
                                postNum = model.postNum,
                                zw = model.zw,
                                zc = model.zc,
                                xl = model.xl,
                                zcpath = model.zcpath,
                                xlpath = model.xlpath,
                                photopath = model.photopath,
                                postNumpath = model.postNumpath,
                                gzjl = model.gzjl,
                                jcgzlx = model.jcgzlx,
                                yddh = model.yddh,
                                bgdh = model.bgdh
                            }, r => new
                            {
                                r.name,
                                r.sex,
                                r.time,
                                r.zw,
                                r.zc,
                                r.xl,
                                r.xlpath,
                                r.hshxhzy,
                                r.gzjl,
                                r.jcgzlx,
                                r.photopath,
                                r.yddh,
                                r.bgdh,
                                r.zcpath,
                                r.postNumpath,
                            }, r => r.pid == model.pid);
                        }
                        else
                        {
                            db.Insert<t_D_UserTableFour>(model);
                        }

                        // 更新t_D_UserTableOne上的相应标识
                        db.UpdateOnly(new t_D_UserTableOne
                        {
                            Fourpath_zl = "1"
                        }, r => new
                        {
                            r.Fourpath_zl
                        }, r => r.id == Convert.ToInt32(model.pid));

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

        /// <summary>
        /// 递交操作
        /// </summary>
        /// <param name="selectedId"></param>
        /// <param name="state"></param>
        /// <param name="erroMsg"></param>
        /// <returns></returns>
        public bool SetInstSendState(string selectedId, string state, string table, out string erroMsg)
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
                            if (table == "t_D_UserTableOne")
                            {
                                db.UpdateOnly(new t_D_UserTableOne { @static = Convert.ToInt32(state) }, p => p.@static, p => p.id == Convert.ToInt32(siArray[i]));
                            }
                            else if (table == "t_D_UserChange")
                            {
                                db.UpdateOnly(new t_D_UserChange { @static = Convert.ToInt32(state) }, p => p.@static, p => p.id == Convert.ToInt32(siArray[i]));
                            }
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
        /// 受理操作
        /// </summary>
        /// <param name="model"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool Audit(t_D_UserTableTen model, out string errorMsg)
        {
            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        errorMsg = string.Empty;

                        db.Insert(model, true);

                        db.UpdateOnly(new t_D_UserTableOne { @static = 2 }, p => p.@static, p => p.id == model.pid);

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


        /// <summary>
        /// 分配专家
        /// </summary>
        /// <param name="model"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool SaveDistributeExpert(t_D_UserTableTen model, out string errorMsg)
        {
            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        errorMsg = string.Empty;

                        int needUnitBuildingQualify = 0; //是否需要填写 建设工程质量检测资质机构审核表
                        int needSpecialQualify = 0; //是否需要填写 专项检测备案审核表
                        var tbTwo = repTwo.GetByCondition(r => r.pid == model.pid).FirstOrDefault();
                        if(tbTwo != null)
                        {
                            if (tbTwo.sqjcyw.IndexOf("地基基础工程检测") >= 0 ||
                           tbTwo.sqjcyw.IndexOf("主体结构工程现场检测") >= 0 ||
                           tbTwo.sqjcyw.IndexOf("建筑幕墙工程检测") >= 0 ||
                           tbTwo.sqjcyw.IndexOf("钢结构工程检测") >= 0 ||
                           tbTwo.sqjcyw.IndexOf("见证取样检测") >= 0)
                            {
                                needUnitBuildingQualify = 1;
                            }

                            if (tbTwo.sqjcyw.IndexOf("室内环境质量检测") >= 0 ||
                               tbTwo.sqjcyw.IndexOf("建筑附属设备安装工程检测") >= 0)
                            {
                                needSpecialQualify = 1;
                            }
                        }

                        //先删除之前分配的专家
                        repExp.DeleteByCondition(r => r.pid == model.pid);

                        if (!string.IsNullOrEmpty(model.zjsp1))
                        {
                            var ueu = new t_D_UserExpertUnit();
                            ueu.userid = Convert.ToInt32(model.zjsp1);
                            ueu.pid = Convert.ToInt32(model.pid);
                            ueu.addtime = DateTime.Now;
                            ueu.status = 0;
                            ueu.needUnitBuildingQualify = needUnitBuildingQualify;
                            ueu.needSpecialQualify = needSpecialQualify;

                            db.Insert(ueu, true);
                        }

                        if (!string.IsNullOrEmpty(model.zjsp2))
                        {
                            var ueu = new t_D_UserExpertUnit();
                            ueu.userid = Convert.ToInt32(model.zjsp2);
                            ueu.pid = Convert.ToInt32(model.pid);
                            ueu.addtime = DateTime.Now;
                            ueu.status = 0;
                            ueu.needUnitBuildingQualify = needUnitBuildingQualify;
                            ueu.needSpecialQualify = needSpecialQualify;

                            db.Insert(ueu, true);
                        }

                        //更新 
                        db.UpdateOnly(new t_D_UserTableOne { @static = 3 }, p => p.@static, p => p.id == model.pid);
                        db.UpdateOnly(model, r => new
                        {
                            r.zjsp1,
                            r.zjsp2,
                            r.@static
                        }, r => r.id == model.id);

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


        /// <summary>
        /// 专家 建设工程质量检测机构审核表
        /// </summary>
        /// <param name="saveModel"></param>
        /// <param name="userid"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool SaveExpertUnitQualify(t_D_userTableSH saveModel, int userid, out string errorMsg)
        {
            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        errorMsg = string.Empty;

                        //保存 t_D_userTableSH 建设工程质量检测机构审核表 记录
                        var id = db.Insert(saveModel, true);

                        //更新状态
                        db.UpdateOnly(new t_D_UserExpertUnit
                        {
                            status = 1,
                            qualifystatus = 1,
                            qualifychecktime = DateTime.Now,
                            shid = id
                        }, r => new
                        {
                            r.status,
                            r.qualifystatus,
                            r.qualifychecktime,
                            r.shid
                        }, r => r.pid == saveModel.pid && r.userid == userid);

                        //先提交
                        dbTrans.Commit();

                        //再查询判断所有专家的审批是否已经完成
                        int index = 0;
                        int ApprovaledCnt = 0;
                        var ueus = repExp.GetByCondition(r => r.pid == saveModel.pid);
                        foreach(var ueu in ueus)
                        {
                            index++;
                            if (ueu.needUnitBuildingQualify == 1 && ueu.needSpecialQualify == 1)
                            {
                                if (ueu.qualifystatus == 1 && ueu.speicalstatus == 1)
                                {
                                    ApprovaledCnt += 1;
                                }
                            }
                            else if (ueu.needUnitBuildingQualify == 1 && ueu.needSpecialQualify == 0)
                            {
                                if (ueu.qualifystatus == 1)
                                {
                                    ApprovaledCnt += 1;
                                }
                            }
                            else if (ueu.needUnitBuildingQualify == 0 && ueu.needSpecialQualify == 1)
                            {
                                if (ueu.speicalstatus == 1)
                                {
                                    ApprovaledCnt += 1;
                                }
                            }
                        }

                        //审批都已经经完成
                        if (index == ApprovaledCnt)
                        {
                            db.UpdateOnly(new t_D_UserTableTen
                            {
                                @static = 3
                            }, r => new
                            {
                                r.@static
                            }, r => r.pid == saveModel.pid);
                        }
                        
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

        /// <summary>
        /// 专家 保存专项检测备案审核表
        /// </summary>
        /// <param name="saveModel"></param>
        /// <param name="userid"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool SaveSpeicalQualify(t_D_userTableSC saveModel, int userid, out string errorMsg)
        {
            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        errorMsg = string.Empty;

                        //保存 t_D_userTableSC 专项检测备案审核表 记录
                        var id = db.Insert(saveModel, true);

                        //更新状态
                        db.UpdateOnly(new t_D_UserExpertUnit
                        {
                            status = 1,
                            speicalstatus = 1,
                            speicalchecktime = DateTime.Now,
                            scid = id
                        }, r => new
                        {
                            r.status,
                            r.speicalstatus,
                            r.speicalchecktime,
                            r.scid
                        }, r => r.pid == saveModel.pid && r.userid == userid);

                        //先提交
                        dbTrans.Commit();

                        //再查询判断所有专家的审批是否已经完成
                        int index = 0;
                        int ApprovaledCnt = 0;
                        var ueus = repExp.GetByCondition(r => r.pid == saveModel.pid);
                        foreach (var ueu in ueus)
                        {
                            index++;
                            if (ueu.needUnitBuildingQualify == 1 && ueu.needSpecialQualify == 1)
                            {
                                if (ueu.qualifystatus == 1 && ueu.speicalstatus == 1)
                                {
                                    ApprovaledCnt += 1;
                                }
                            }
                            else if (ueu.needUnitBuildingQualify == 1 && ueu.needSpecialQualify == 0)
                            {
                                if (ueu.qualifystatus == 1)
                                {
                                    ApprovaledCnt += 1;
                                }
                            }
                            else if (ueu.needUnitBuildingQualify == 0 && ueu.needSpecialQualify == 1)
                            {
                                if (ueu.speicalstatus == 1)
                                {
                                    ApprovaledCnt += 1;
                                }
                            }
                        }
                        
                        //审批都已经经完成
                        if (index == ApprovaledCnt)
                        {
                            db.UpdateOnly(new t_D_UserTableTen
                            {
                                @static = 3
                            }, r => new
                            {
                                r.@static
                            }, r => r.pid == saveModel.pid);
                        }

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

        

        /// <summary>
        /// 填写初审表
        /// </summary>
        /// <param name="saveModel"></param>
        /// <param name="userid"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool SaveCBRUnit(t_D_UserTableCS saveModel, int userid, out string errorMsg)
        {
            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        errorMsg = string.Empty;

                        db.Insert(saveModel, true);

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

        /// <summary>
        /// 承办人审批
        /// </summary>
        /// <param name="saveModel"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool SaveCBRUnitApproval(t_D_UserTableTen saveModel, out string errorMsg)
        {
            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        errorMsg = string.Empty;
                        if (db.Select<t_D_UserTableTen>(r => r.id == saveModel.id).Count > 0)
                        {
                            db.UpdateOnly(saveModel, r => new
                            {
                                r.ThreeYJ,
                                r.ThreeFZr,
                                r.ThreeZZZSBH,
                                r.ThreeTime,
                                r.ThreeYXQBegin,
                                r.ThreeYXQEnd,
                                r.cbr,
                                r.@static,
                                r.outstaticinfo
                            }, r => r.id == saveModel.id);
                        }
                        else
                        {
                            db.Insert<t_D_UserTableTen>(saveModel);
                        }

                        db.UpdateOnly(new t_D_UserTableOne { @static = saveModel.@static }, p => p.@static, p => p.id == saveModel.pid);

                        //更新t_bp_custom表相关数据 证书、资质、有效期 同步到主表
                        var tbTwo = db.Select<t_D_UserTableTwo>(r => r.pid == saveModel.pid).FirstOrDefault();
                        if(tbTwo != null)
                        {
                            db.UpdateOnly(new t_bp_custom
                            {
                                BUSINESSNUM = tbTwo.businessnum,
                                businessnumPath = tbTwo.businessnumpath,
                                BUSINESSNUMUNIT = tbTwo.businessnumunit,
                                ECONOMICNATURE = tbTwo.economicnature,
                                MEASNUM = tbTwo.measnum,
                                MEASNUMPATH = tbTwo.measnumpath,
                                MEASUNIT = tbTwo.measunit,
                                zzlbmc = tbTwo.sqjcyw,
                                detectappldate = saveModel.ThreeYXQBegin,
                                measnumEndDate = saveModel.ThreeYXQEnd
                            }, r => new
                            {
                                r.BUSINESSNUM,
                                r.businessnumPath,
                                r.BUSINESSNUMUNIT,
                                r.ECONOMICNATURE,
                                r.MEASNUM,
                                r.MEASNUMPATH,
                                r.MEASUNIT,
                                r.zzlbmc,
                                r.detectappldate,
                                r.measnumEndDate
                            }, r => r.ID == tbTwo.unitcode);
                        }
                        

                        dbTrans.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        dbTrans.Rollback();
                        errorMsg = ex.Message + ex.StackTrace;
                        return false;
                    }
                }

            }
        }



        /// <summary>
        /// 新增 资质证书变更申请审核表
        /// </summary>
        /// <param name="saveModel"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool SaveUserChange(t_D_UserChange saveModel, out string errorMsg)
        {
            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        errorMsg = string.Empty;

                        db.Insert(saveModel, true);

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

        /// <summary>
        /// 修改 资质证书变更申请审核表
        /// </summary>
        /// <param name="saveModel"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool EditUserChange(t_D_UserChange saveModel, out string errorMsg)
        {
            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        errorMsg = string.Empty;

                        db.UpdateOnly(saveModel, r => new
                        {
                            r.unitname,
                            r.area,
                            r.bgnr,
                            r.bgq,
                            r.bgh,
                            r.bgclpath,
                            r.SQname,
                            r.SQTel,
                            r.YZZPath
                        }, r => r.id == saveModel.id);

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


        /// <summary>
        /// 审批 资质证书变更申请审核表
        /// </summary>
        /// <param name="saveModel"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool Approval(t_D_UserChange saveModel, out string errorMsg)
        {
            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        errorMsg = string.Empty;

                        db.UpdateOnly(saveModel, r => new
                        {
                            r.@static,
                            r.EndTime
                        }, r => r.id == saveModel.id);

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



    }
}
