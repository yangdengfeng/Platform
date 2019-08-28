using PkpmGX.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pkpm.Core.UserRoleCore;
using Pkpm.Core.SysDictCore;
using Pkpm.Core.CheckUnitCore;
using Pkpm.Framework.PkpmConfigService;
using Pkpm.Framework.Repsitory;
using Pkpm.Entity;
using Pkpm.Framework.FileHandler;
using Dhtmlx.Model.Toolbar;
using PkpmGX.Models;
using Dhtmlx.Model.Grid;
using ServiceStack;
using Pkpm.Framework.Common;
using ServiceStack.OrmLite;
using Dhtmlx.Model.Form;
using Pkpm.Entity.DTO;
using Pkpm.Entity.Models;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using System.Reflection;
using System.ComponentModel;
using Pkpm.Core.CheckQualifyCore;

namespace PkpmGX.Controllers
{
    [Authorize]
    public class CbrApprovalController : PkpmController
    {

        ISysDictService sysDictServcie;
        IPkpmConfigService pkpmConfigService;
        ICheckUnitService checkUnitService;
        ICheckQualifyService checkQualifyService;
        IRepsitory<User> repUser;
        IRepsitory<t_D_UserExpertUnit> repEU;
        IRepsitory<t_D_UserTableCS> repCS;
        IRepsitory<t_D_UserTableOne> repOne;
        IRepsitory<t_D_UserTableTen> repTen;

        public CbrApprovalController(ISysDictService sysDictServcie,
            IPkpmConfigService pkpmConfigService,
            ICheckUnitService checkUnitService,
            ICheckQualifyService checkQualifyService,
        IRepsitory<User> repUser,
        IRepsitory<t_D_UserExpertUnit> repEU,
        IRepsitory<t_D_UserTableCS> repCS,
        IRepsitory<t_D_UserTableOne> repOne,
        IRepsitory<t_D_UserTableTen> repTen,
        IUserService userService) : base(userService)
        {
            this.repUser = repUser;
            this.repEU = repEU;
            this.repCS = repCS;
            this.repOne = repOne;
            this.repTen = repTen;
            this.sysDictServcie = sysDictServcie;
            this.pkpmConfigService = pkpmConfigService;
            this.checkUnitService = checkUnitService;
            this.checkQualifyService = checkQualifyService;
            this.userService = userService;
        }


        // GET: DistributeExpert
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Search(CBRUnitSearchModel searchModel)
        {
            var data = GetSearchResult(searchModel);

            DhtmlxGrid grid = new DhtmlxGrid();
            int pos = searchModel.posStart.HasValue ? searchModel.posStart.Value : 0;
            grid.AddPaging(data.TotalCount, pos);
            var buttons = GetCurrentUserPathActions();
            var currentUserRole = GetCurrentUserRole();

            for (int i = 0; i < data.Results.Count; i++)
            {
                var rowData = data.Results[i];
                DhtmlxGridRow row = new DhtmlxGridRow(rowData.id.ToString());
                row.AddCell(string.Empty);
                row.AddCell((pos + i + 1).ToString());//序号

                var tbOne =  repOne.GetById(rowData.pid);
                if(tbOne != null)
                {
                    row.AddCell(tbOne.unitname);
                }
                else
                {
                    row.AddCell("");
                }
                
                if (rowData.zjsp1.HasValue)
                {
                    var user = repUser.GetById(rowData.zjsp1);
                    var ueu = repEU.GetByCondition(r => r.userid == user.Id && r.pid == rowData.pid).FirstOrDefault();
                    if (ueu != null)
                    {
                        if (ueu.qualifystatus == 1)
                        {
                            row.AddLinkJsCell(user.UserDisplayName + "/[建设工程质量检测资质机构审核表]", "DetailUnitBuildingQualify(\"{0}\",\"{1}\")".Fmt(ueu.shid, ueu.pid));
                        }
                        else
                        {
                            row.AddCell("");
                        }

                        if (ueu.speicalstatus == 1)
                        {
                            row.AddLinkJsCell(user.UserDisplayName + "/[专项检测备案审核表]", "DetailSpecialQualify(\"{0}\",\"{1}\")".Fmt(ueu.scid, ueu.pid));
                        }
                        else
                        {
                            row.AddCell("");
                        }
                    }
                    else
                    {
                        row.AddCell("");
                        row.AddCell("");
                    }
                }
                else
                {
                    row.AddCell("");
                    row.AddCell("");
                }

                if (rowData.zjsp2.HasValue)
                {
                    var user = repUser.GetById(rowData.zjsp2);
                    var ueu = repEU.GetByCondition(r => r.userid == user.Id && r.pid == rowData.pid).FirstOrDefault();
                    if(ueu != null)
                    {
                        if (ueu.qualifystatus == 1)
                        {
                            row.AddLinkJsCell(user.UserDisplayName + "/[建设工程质量检测资质机构审核表]", "DetailUnitBuildingQualify(\"{0}\",\"{1}\")".Fmt(ueu.shid, ueu.pid));
                        }
                        else
                        {
                            row.AddCell("");
                        }

                        if (ueu.speicalstatus == 1)
                        {
                            row.AddLinkJsCell(user.UserDisplayName + "/[专项检测备案审核表]", "DetailSpecialQualify(\"{0}\",\"{1}\")".Fmt(ueu.scid, ueu.pid));
                        }
                        else
                        {
                            row.AddCell("");
                        }
                    }
                    else
                    {
                        row.AddCell("");
                        row.AddCell("");
                    }
                }
                else
                {
                    row.AddCell("");
                    row.AddCell("");
                }

                row.AddCell(rowData.slr);
                row.AddCell(rowData.cbr);

                if(rowData.@static == 1)
                {
                    row.AddCell("受理人受理");
                }
                else if (rowData.@static == 2)
                {
                    row.AddCell("已分配专家");
                }
                else if (rowData.@static == 3)
                {
                    row.AddCell("专家审批完成");
                }
                else if (rowData.@static == 4)
                {
                    row.AddCell("承办人审批通过");
                }
                else if (rowData.@static == 5)
                {
                    row.AddCell("承办人审批不通过");
                }
                else if (rowData.@static == 6)
                {
                    row.AddCell("申诉复核通过");
                }
                else if (rowData.@static == 7)
                {
                    row.AddCell("申诉复核不通过");
                }
                else
                {
                    row.AddCell("");
                }

                var tbCS = repCS.GetByCondition(r => r.pid == rowData.pid).FirstOrDefault();
                if (rowData.@static >= 3)
                {
                    if (tbCS != null)
                    {
                        row.AddLinkJsCell("查看初审表", "Detail(\"{0}\")".Fmt(tbCS.id));
                    }
                    else
                    {
                        row.AddLinkJsCell("填写初审表", "FillIn(\"{0}\")".Fmt(rowData.pid));
                    }
                }
                else
                {
                    row.AddCell(string.Empty);
                }
                
                if(rowData.@static == 3)
                {
                    if (HaveButtonFromAll(buttons, "Approval") && tbCS != null)
                    {
                        row.AddLinkJsCell("审批", "Approval(\"{0}\",\"{1}\")".Fmt(rowData.id, rowData.pid));
                    }
                    else
                    {
                        row.AddCell(string.Empty);
                    }
                }
                else if (rowData.@static == 5)
                {
                    row.AddLinkJsCell("查看原因", "Reason(\"{0}\")".Fmt(rowData.outstaticinfo));
                }
                

                grid.AddGridRow(row);
            }
            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }

        private SearchResult<CBRUnitUIModel> GetSearchResult(CBRUnitSearchModel searchModel)
        {

            var predicate = PredicateBuilder.True<t_D_UserTableTen>();

            int pos = searchModel.posStart.HasValue ? searchModel.posStart.Value : 0;
            int count = searchModel.count.HasValue ? searchModel.count.Value : 30;

            //if (!string.IsNullOrWhiteSpace(searchModel.CheckUnitName))
            //{
            //    predicate = predicate.And(t => t.unitCode == searchModel.CheckUnitName);
            //}

            string orderby = string.Empty;
            string orderByAcs = string.Empty;
            bool isDes = true;
            string sortProperty = "id";

            //string[] columns = { "CheckBox", "SeqNo", "NAME", "UNITNAME" };
            //if (searchModel.orderColInd.HasValue
            //    && !string.IsNullOrWhiteSpace(searchModel.direct)
            //    && searchModel.orderColInd.Value <= columns.Length)
            //{
            //    if (searchModel.direct == "asc")
            //    {
            //        sortProperty = orderby = columns[searchModel.orderColInd.Value];
            //        isDes = false;
            //    }
            //    if (searchModel.direct == "des")
            //    {
            //        sortProperty = orderByAcs = columns[searchModel.orderColInd.Value];
            //        isDes = true;
            //    }
            //}

            PagingOptions<t_D_UserTableTen> pagingOption = new PagingOptions<t_D_UserTableTen>(pos, count, sortProperty, isDes);

            var customs = repTen.GetByConditionSort<CBRUnitUIModel>(predicate, r => new
            {
                r.id,
                r.pid,
                r.zjsp1,
                r.zjsp2,
                r.slr,
                r.slbh,
                r.cbr,
                r.@static,
                r.outstaticinfo
            },
               pagingOption);

            return new SearchResult<Models.CBRUnitUIModel>(pagingOption.TotalItems, customs);
        }


        public ActionResult FillIn(int? id)
        {
            var model = new CBRUnitUIModel()
            {
                pid = id
            };

            return View(model);
        }

        public ActionResult Detail(int? id)
        {
            var tbCS = repCS.GetById(id);
            var model = new CBRUnitSaveModel()
            {
                csbh = tbCS.csbh,
                content = tbCS.content
            };

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult FillIn(CBRUnitSaveModel model)
        {
            ControllerResult result = ControllerResult.SuccResult;

            try
            {
                var ude = new t_D_UserTableCS()
                {
                    csbh = model.csbh,
                    pid = model.pid,
                    addtime = DateTime.Now,
                    content = model.content
                };

                string errorMsg = string.Empty;
                if (!checkQualifyService.SaveCBRUnit(ude, GetCurrentUserId(), out errorMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = errorMsg;
                }
                else
                {
                    LogUserAction("对检测机构资质Id：{0} 填写初审表完成".Fmt(model.pid));
                }
            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
            }

            return Content(result.ToJson());
        }


        public ActionResult Approval(int id, int pid)
        {
            var model = new CBRUnitApprovalUIModel()
            {
                id = id,
                pid = pid,
                ThreeFZr = repUser.GetById(GetCurrentUserId()).UserDisplayName
            };

            return View(model);
        }

        [HttpPost]
        //[ValidateInput(false)]
        public ActionResult Approval(CBRUnitApprovalSaveModel model)
        {
            ControllerResult result = ControllerResult.SuccResult;

            try
            {
                var utt = new t_D_UserTableTen()
                {
                    id = model.id,
                    ThreeYJ = model.ThreeYJ,
                    ThreeFZr = model.ThreeFZr,
                    ThreeZZZSBH = model.ThreeZZZSBH,
                    ThreeTime = DateTime.Now,
                    ThreeYXQBegin = model.ThreeYXQBegin,
                    ThreeYXQEnd = model.ThreeYXQEnd,
                    pid = model.pid,
                    cbr = repUser.GetById(GetCurrentUserId()).UserDisplayName,
                    @static = 4 //审核通过
                };

                string errorMsg = string.Empty;
                if (!checkQualifyService.SaveCBRUnitApproval(utt, out errorMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = errorMsg;
                }
                else
                {
                    LogUserAction("对检测机构资质Id：{0} 审批完成".Fmt(model.pid));
                }
            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message + ex.StackTrace;

            }

            return Content(result.ToJson());
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult NoApproval(CBRUnitApprovalSaveModel model)
        {
            ControllerResult result = ControllerResult.SuccResult;

            try
            {
                var utt = new t_D_UserTableTen()
                {
                    id = model.id,
                    ThreeYJ = model.ThreeYJ,
                    ThreeFZr = model.ThreeFZr,
                    ThreeZZZSBH = model.ThreeZZZSBH,
                    ThreeTime = DateTime.Now,
                    ThreeYXQBegin = model.ThreeYXQBegin,
                    ThreeYXQEnd = model.ThreeYXQEnd,
                    pid = model.pid,
                    cbr = repUser.GetById(GetCurrentUserId()).UserDisplayName,
                    outstaticinfo = model.outstaticinfo,
                    @static = 5 //审核不通过
                };

                string errorMsg = string.Empty;
                if (!checkQualifyService.SaveCBRUnitApproval(utt, out errorMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = errorMsg;
                }
                else
                {
                    LogUserAction("对检测机构资质Id：{0} 审批完成".Fmt(model.pid));
                }
            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
            }

            return Content(result.ToJson());
        }







    }
}