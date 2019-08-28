using Pkpm.Core.CheckUnitCore;
using Pkpm.Core.UserRoleCore;
using Pkpm.Entity;
using Pkpm.Entity.DTO;
using Pkpm.Framework.Repsitory;
using PkpmGX.Models;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PkpmGX.Controllers
{
    public class InstBaseRecordController : Controller
    {
        IDbConnectionFactory dbFactory;
        ICheckUnitService checkUnitService;
        IRepsitory<t_bp_JZJNCheck> jzjnCheckRep;
        IRepsitory<t_bp_SZDLCheck> szdlCheckRep;
        IRepsitory<t_bp_SLKQCheck> slkqCheckRep;
        
            IRepsitory<t_instcheck> jbxxCheckRep;
        IRepsitory<t_jclcheck> jclCheckRep;
        IRepsitory<CheckUnitRecord_MajorStructure> majorRep;
        public InstBaseRecordController(IRepsitory<t_jclcheck> jclCheckRep,IRepsitory<t_instcheck> jbxxCheckRep, IRepsitory<t_bp_JZJNCheck> jzjnCheckRep, IRepsitory<t_bp_SZDLCheck> szdlCheckRep, IRepsitory<t_bp_SLKQCheck> slkqCheckRep,ICheckUnitService checkUnitService)
        {
            this.majorRep = majorRep;
            this.dbFactory = dbFactory;
            this.jzjnCheckRep = jzjnCheckRep;
            this.szdlCheckRep = szdlCheckRep;
            this.slkqCheckRep = slkqCheckRep;
            this.checkUnitService = checkUnitService;
            this.jbxxCheckRep = jbxxCheckRep;
            this.jclCheckRep = jclCheckRep;
        }
        // GET: InstBaseRecord
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            var allUnit = checkUnitService.GetAllCheckUnit();
            return View(allUnit);
        }
        [HttpPost]
        public ActionResult JbxxCreate(t_instcheck model)
        {
            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                jbxxCheckRep.Insert(model);
            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
            }
            return Content(result.ToJson());
        }
        public ActionResult JbxxEdit(int id)
        {
            JbxxDetailsModel model = new JbxxDetailsModel()
            {
                jbxxCheck = new t_instcheck(),
                allUnit = new InstShortInfos()
            };
            var data = jbxxCheckRep.GetById(id);
            model.allUnit = checkUnitService.GetAllCheckUnit();
            model.jbxxCheck = data;
            return View(model);
        }

        [HttpPost]
        public ActionResult JbxxEdit(t_instcheck model)
        {
            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                jbxxCheckRep.Update(model);
            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
            }

            return Content(result.ToJson());
        }
        public ActionResult JbxxDetails(int id)
        {
            JbxxDetailsModel model = new JbxxDetailsModel()
            {
                jbxxCheck = new t_instcheck(),
                allUnit = new InstShortInfos()
            };
            var data = jbxxCheckRep.GetById(id);
            model.allUnit = checkUnitService.GetAllCheckUnit();
            model.jbxxCheck = data;
            return View(model);
        }

        /// <summary>
        ///表二  建材类检测
        /// </summary>
        /// <returns></returns>
        public ActionResult JCLCheck()
        {
            return View();
        }

        /// <summary>
        /// 表三 桩基现场检测
        /// </summary>
        /// <returns></returns>
        public ActionResult ZJXCCheck()
        {
            return View();
        }

        /// <summary>
        ///表四 建筑节能检测
        /// </summary>
        /// <returns></returns>
        public ActionResult JZJNCheck()
        {
            return View();
        }

        public ActionResult JZJNCreate()
        {
            var allUnit = checkUnitService.GetAllCheckUnit();
            return View(allUnit);
        }

        [HttpPost]
        public ActionResult JZJNCreate(t_bp_JZJNCheck model)
        {
            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                jzjnCheckRep.Insert(model);
            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
            }
            return Content(result.ToJson());
        }

        public ActionResult JZJNEdit(int id)
        {
            JZJNDetailsModel model = new JZJNDetailsModel()
            {
                jzjnCheck = new t_bp_JZJNCheck(),
                allUnit = new InstShortInfos()
            };
            var data = jzjnCheckRep.GetById(id);
            model.allUnit = checkUnitService.GetAllCheckUnit();
            model.jzjnCheck = data;
            return View(model);
        }

        [HttpPost]
        public ActionResult JZJNEdit(t_bp_JZJNCheck model)
        {
            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                //jzjnCheckRep.UpdateOnly(model, t => t.Id == model.Id, t => new
                //{
                //});
                jzjnCheckRep.Update(model);
            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
            }

            return Content(result.ToJson());
        }

        public ActionResult JZJNDetails(int id)
        {
            JZJNDetailsModel model = new JZJNDetailsModel()
            {
                jzjnCheck = new t_bp_JZJNCheck(),
                allUnit = new InstShortInfos()
            };
            var data = jzjnCheckRep.GetById(id);
            model.allUnit = checkUnitService.GetAllCheckUnit();
            model.jzjnCheck = data;
            return View(model);
        }

        /// <summary>
        ///表五 市政道路(含桥梁)检测
        /// </summary>
        /// <returns></returns>
        public ActionResult SZDLCheck()
        {
            return View();
        }


        public ActionResult SZDLCreate()
        {
            var allUnit = checkUnitService.GetAllCheckUnit();
            return View(allUnit);
        }

        [HttpPost]
        public ActionResult SZDLCreate(t_bp_SZDLCheck model)
        {
            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                szdlCheckRep.Insert(model);
            }
            catch(Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
            }
            return Content(result.ToJson());
        }

        public ActionResult SZDLDetails(int id)
        {
            SZDLDetailsModel model = new SZDLDetailsModel()
            {
               szdlCheck = new t_bp_SZDLCheck(),
                allUnit = new InstShortInfos()
            };
            var data = szdlCheckRep.GetById(id);
            model.allUnit = checkUnitService.GetAllCheckUnit();
            model.szdlCheck = data;
            return View(model);
        }

        public ActionResult SZDLEdit(int id)
        {
            SZDLDetailsModel model = new SZDLDetailsModel()
            {
                szdlCheck = new t_bp_SZDLCheck(),
                allUnit = new InstShortInfos()
            };
            var data = szdlCheckRep.GetById(id);
            model.allUnit = checkUnitService.GetAllCheckUnit();
            model.szdlCheck = data;
            return View(model);
        }

        [HttpPost]
        public ActionResult SZDLEdit(t_bp_SZDLCheck model)
        {
            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                //jzjnCheckRep.UpdateOnly(model, t => t.Id == model.Id, t => new
                //{
                //});
                szdlCheckRep.Update(model);
            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
            }

            return Content(result.ToJson());
        }
        /// <summary>
        /// 表六 室内空气检测
        /// </summary>
        /// <returns></returns>
        public ActionResult SLKQCheck()
        {
          
            return View();
        }

        public ActionResult SLKQCreate()
        {
            var allUnit = checkUnitService.GetAllCheckUnit();
            return View(allUnit);
        }

        [HttpPost]
        public ActionResult SLKQCreate(t_bp_SLKQCheck model)
        {
            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                slkqCheckRep.Insert(model);
            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
            }
            return Content(result.ToJson());
        }

        public ActionResult SLKQDetails(int id)
        {
            SLKQDetailsModel model = new SLKQDetailsModel()
            {
                slkqCheck = new  t_bp_SLKQCheck(),
                allUnit = new InstShortInfos()
            };
            var data = slkqCheckRep.GetById(id);
            model.allUnit = checkUnitService.GetAllCheckUnit();
            model.slkqCheck = data;
            return View(model);
        }

        public ActionResult SLKQEdit(int id)
        {
            SLKQDetailsModel model = new SLKQDetailsModel()
            {
                slkqCheck = new t_bp_SLKQCheck(),
                allUnit = new InstShortInfos()
            };
            var data = slkqCheckRep.GetById(id);
            model.allUnit = checkUnitService.GetAllCheckUnit();
            model.slkqCheck = data;
            return View(model);
        }

        [HttpPost]
        public ActionResult SLKQEdit(t_bp_SLKQCheck model)
        {
            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                //jzjnCheckRep.UpdateOnly(model, t => t.Id == model.Id, t => new
                //{
                //});
                slkqCheckRep.Update(model);
            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
            }

            return Content(result.ToJson());
        }

        /// <summary>
        ///表七 主体结构(含钢结构）检测
        /// </summary>
        /// <returns></returns>
        public ActionResult ZTJGCheck()
        {
            return View();
        }

        /// <summary>
        ///表八 起重设备检测
        /// </summary>
        /// <returns></returns>
        public ActionResult QZSBCheck()
        {
            return View();
        }

        public ActionResult QZSBCreate()
        {
            Dictionary<string, string> allUnit = checkUnitService.GetAllCheckUnit();
            return View(allUnit);
        }

        public ActionResult QZSBDetails(int id)
        {
            return View();
        }

        public ActionResult QZSBEdit(int id)
        {
            return View();
        }

        //GET:InstBaseRecord/ZTJGCreate
        public ActionResult ZTJGCreate()
        {
            Dictionary<string,string> allUnit = checkUnitService.GetAllCheckUnit();
            return View(allUnit);
        }

        //GET:InstBaseRecord/ZTJGEdit
        public ActionResult ZTJGEdit(int id)
        {
            ZTJGEditViewModel model = new ZTJGEditViewModel()
            {
                Major = new CheckUnitRecord_MajorStructure(),
            };
            model.Major = majorRep.GetById(id);
            model.allUnit= checkUnitService.GetAllCheckUnit();
            return View(model);
        }

        // POST: InstBaseRecord/ZTJGCreate

        // POST: InstBaseRecord/ZTJGEdit
        [HttpPost]
        public ActionResult ZTJGEdit(ZTJGEditModel model)
        {
            ControllerResult result = ControllerResult.SuccResult;
            CheckUnitRecord_MajorStructure major = new CheckUnitRecord_MajorStructure()
            {
                Id=model.Id,
                CustomId = model.UnitName,
                JCBGSCQ = model.BGSCQ,
                JCBGSCS = model.BGSCS,
                BGWZZQQ = model.BGWZZQQ,
                BGWZZQS = model.BGWZZQS,
                CYSLMZGFYQq = model.CYSLMZGFYQQ,
                CYSLMZGFYQS = model.CYSLMZGFYQS,
                SYYQSBZYXQQ = model.SYYQSBZYXQQ,
                SYYQSBZYXQS = model.SYYQSBZYXQS,
                BGGGFHCXS = model.BGGGFHCXS,
                BGGGFHCXQ = model.BGGGFHCXQ,
                BHGTZQ = model.BHGTZQ,
                BHGTZS = model.BHGTZS,
                Remark = model.Remark,
                CheckDate = model.CheckDate,
                CheckPeople = model.CheckPeople
            };
            try
            {
                var db = dbFactory.Open();
                db.Update(major);
                result.IsSucc = true;
            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
                result.IsSucc = false;
            }

            return Content(result.ToJson());
        }

        [HttpPost]
        public ActionResult ZTJGCreate(ZTJGCreateModel model)
        {
            ControllerResult result = ControllerResult.SuccResult;

            CheckUnitRecord_MajorStructure major = new CheckUnitRecord_MajorStructure()
            {
                CustomId = model.UnitName,
                JCBGSCQ = model.BGSCQ,
                JCBGSCS = model.BGSCS,
                BGWZZQQ = model.BGWZZQQ,
                BGWZZQS = model.BGWZZQS,
                CYSLMZGFYQq = model.CYSLMZGFYQQ,
                CYSLMZGFYQS = model.CYSLMZGFYQS,
                SYYQSBZYXQQ = model.SYYQSBZYXQQ,
                SYYQSBZYXQS = model.SYYQSBZYXQS,
                BGGGFHCXS = model.BGGGFHCXS,
                BGGGFHCXQ = model.BGGGFHCXQ,
                BHGTZQ = model.BHGTZQ,
                BHGTZS = model.BHGTZS,
                Remark = model.Remark,
                CheckDate = model.CheckDate,
                CheckPeople = model.CheckPeople
            };

            try
            {
                var db = dbFactory.Open();
                db.Insert(major);
                result.IsSucc = true;
            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
                result.IsSucc = false;
            }

            return Content(result.ToJson());
        }

    }
}

