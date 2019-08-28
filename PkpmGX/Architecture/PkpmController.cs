using Dhtmlx.Model.Grid;
using Microsoft.AspNet.Identity;
using Nest;
using Pkpm.Core.CheckUnitCore;
using Pkpm.Core.ItemNameCore;
using Pkpm.Core.ReportCore;
using Pkpm.Core.UserRoleCore;
using Pkpm.Entity;
using Pkpm.Entity.ElasticSearch;
using Pkpm.Framework.Logging;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;

namespace PkpmGX.Architecture
{
    public class PkpmController : Controller
    {

        protected IUserService userService;

        protected string printedPhrase;
        protected string testedPhrase;
        protected string collectedPhrase;
        protected string filterCustom;

        protected string checkedPhrase;
        protected string proofreadPhrase;
        protected string verifyedPhrase;
        protected string approvaledPhrase;
        protected string giveOutPhrase;
        protected string fileedPhrase;
        protected string canceledPhrase;

        protected static string canCheckAllCustomRoles = System.Configuration.ConfigurationManager.AppSettings["CanCheckAllCustomRoles"];//角色是否可以查看全部机构权限


        protected static string NeedRefreshUrl = System.Configuration.ConfigurationManager.AppSettings["NeedRefreshUrl"];

        public PkpmController(IUserService userService)
        {
            this.userService = userService;
            printedPhrase = System.Configuration.ConfigurationManager.AppSettings["PrintedSamplePhrase"];//已打印
            testedPhrase = System.Configuration.ConfigurationManager.AppSettings["TestedSamplePhrase"];//已检测
            collectedPhrase = System.Configuration.ConfigurationManager.AppSettings["CollectedSamplePhrase"];//已收样
            checkedPhrase = System.Configuration.ConfigurationManager.AppSettings["checkedSamplePhrase"];//已复核
            proofreadPhrase = System.Configuration.ConfigurationManager.AppSettings["proofreadSamplePhrase"];//已校核
            verifyedPhrase = System.Configuration.ConfigurationManager.AppSettings["verifyedSamplePhrase"];//已审核
            approvaledPhrase = System.Configuration.ConfigurationManager.AppSettings["approvaledSamplePhrase"];//已批准
            giveOutPhrase = System.Configuration.ConfigurationManager.AppSettings["giveOutSamplePhrase"];//已发放
            fileedPhrase = System.Configuration.ConfigurationManager.AppSettings["fileedSamplePhrase"];//已归档
            canceledPhrase = System.Configuration.ConfigurationManager.AppSettings["canceledSamplePhrase"];//已作废
            filterCustom = System.Configuration.ConfigurationManager.AppSettings["searchCustomBlackList"];//已收样
        }


        /// <summary>
        /// 判断当前用户是否为管理员
        /// </summary>
        /// <param name="roleService"></param>
        /// <returns></returns>
        protected bool IsCurrentAdmin()
        {
            return userService.IsAdmin(GetCurrentUserId());
        }

        /// <summary>
        /// 判断当前用户是否为监督
        /// </summary>
        /// <returns></returns>
        protected bool IsCurrentSuperVisor()
        {
            return userService.IsInspector(GetCurrentUserId());
        }

        protected string InstUserCustomId()
        {
            return userService.InstUserCustomId(GetCurrentUserId());
        }

        /// <summary>
        /// 判断当前用户是否为见证人
        /// </summary>
        /// <returns></returns>
        protected bool IsCurrentWitNess()
        {
            return userService.IsWitnes(GetCurrentUserId());
        }


        /// <summary>
        /// 判断当前用户是否为取样人
        /// </summary>
        /// <returns></returns>
        protected bool IsCurrentSample()
        {
            return userService.IsSample(GetCurrentUserId());
        }

        protected bool IsCurrentCheckPeople()
        {
            return userService.IsCheckPeople(GetCurrentUserId());
        }

        protected bool CurrentAccountIsCardNo()
        {
            int userId = GetCurrentUserId();
            return userService.CurrentAccountIsCardNo(userId);
        }

        protected bool IsCurrentAllQYYAndJYY()
        {
            int userId = GetCurrentUserId();
            return userService.IsAllQYYAndJYY(userId);
        }

        protected Role GetCurrentUserRole()
        {
            int userId = GetCurrentUserId();
            return userService.GetUserRole(userId);
        }

        protected List<User> GetSysExpertRoleUsers()
        {
            return userService.GetSysExpertRoleUsers();
        }


        //protected List<Entity.Role> GetCurrentUserRoles()
        //{
        //    int userId = GetCurrentUserId();
        //    return userService.GetUserRoles(userId);
        //}

        protected List<Pkpm.Entity.Path> GetCurentUserPaths()
        {
            int userId = GetCurrentUserId();
            return userService.GetUserPaths(userId);
        }

        protected List<Pkpm.Entity.Action> GetCurrentUserPathActions(string pathUrl = "")
        {
            if (pathUrl.IsNullOrEmpty())
            {
                pathUrl = '/' + this.ControllerContext.RouteData.Values["controller"].ToString();
            }

            int userId = GetCurrentUserId();
            var paths = GetCurentUserPaths();
            var foundPath = paths.Where(p => p.Url == pathUrl).FirstOrDefault();
            if (foundPath != null)
            {
                return userService.GetUserActions(userId, foundPath.Id);
            }
            else
            {
                return new List<Pkpm.Entity.Action>();
            }

        }

        protected bool HaveButtonFromAll(List<Pkpm.Entity.Action> buttons, string buttonUrl)
        {
            return buttons.Exists(b => b.Url == buttonUrl);
        }

        protected List<bool> GetButtonResults(List<string> buttons, string pathUrl = "")
        {
            if (pathUrl.IsNullOrEmpty())
            {
                pathUrl = this.ControllerContext.RouteData.Values["controller"].ToString();
            }

            var pageActions = GetCurrentUserPathActions(pathUrl);
            if (pageActions == null || pageActions.Count() == 0)
            {
                return buttons.Select(s => false).ToList();
            }
            else
            {
                return buttons.Select(s => pageActions.Exists(pa => pa.Url == s)).ToList();
            }
        }

        protected bool HaveButton(string button, string pathUrl = "")
        {
            if (pathUrl.IsNullOrEmpty())
            {
                pathUrl = this.ControllerContext.RouteData.Values["controller"].ToString();
            }

            if (GetCurrentUserPathActions(pathUrl).Exists(a => a.Url == button))
            {
                return true;
            }

            return false;
        }

        protected void LogUserAction(string logEvent)
        {
            SysLog sysLog = new SysLog()
            {
                IpAddress = Request.UserHostAddress,
                UserId = GetCurrentUserId(),
                UerName = User.Identity.GetUserName(),
                LogEvent = logEvent,
                LogType = "系统日志",
                LogTime = DateTime.Now
            };

            userService.AddUserEvent(sysLog);
        }

        protected void LogUserAction(string logEvent, ILogger logger, string logType = "系统日志")
        {
            SysLog sysLog = new SysLog()
            {
                IpAddress = Request.UserHostAddress,
                UserId = GetCurrentUserId(),
                UerName = User.Identity.GetUserName(),
                LogEvent = logEvent,
                LogType = logType,
                LogTime = DateTime.Now
            };

            logger.Information(sysLog.ToString());


        }

        protected int GetCurrentUserId()
        {
            return HttpContext.User.Identity.GetUserId<Int32>();
        }

        protected List<UserInArea> GetCurrentAreas()
        {
            int userId = GetCurrentUserId();
            return userService.GetUserAreas(userId);
        }

        protected InstShortInfos GetCurrentInsts()
        {
            int userId = GetCurrentUserId();
            return userService.GetUserInsts(userId, canCheckAllCustomRoles);
        }

        protected InstShortInfos GetCurrentInstsST()
        {
            int userId = GetCurrentUserId();
            return userService.GetUserInstsST(userId);
        }


        protected InstShortInfos GetCurrentFormalInsts()
        {
            int userId = GetCurrentUserId();
            return userService.GetUserFormalInsts(userId);
        }

        public ActionResult InstCombo()
        {
            XElement element = BuildInstCombo();
            string str = element.ToString(SaveOptions.DisableFormatting);
            return Content(str, "text/xml");
        }

        public ActionResult YangHuTiaoJianCombo()
        {
            var yanghutiaojianDicts = userService.GetDictByKey("yanghutiaojian");

            XElement element = new XElement("complete",
                new XElement("option",
                    new XAttribute("value", ""),
                    new XText("全部")),
                from a in yanghutiaojianDicts
                select new XElement("option",
                    new XAttribute("value", a.Key),
                    new XText(a.Value)));

            string str = element.ToString(SaveOptions.DisableFormatting);
            return Content(str, "text/xml");
        }

        public ActionResult QiangDuDengJiCombo()
        {
            var yanghutiaojianDicts = userService.GetDictByKey("SHEJIDENGJI");

            XElement element = new XElement("complete",
                new XElement("option",
                    new XAttribute("value", ""),
                    new XText("全部")),
                from a in yanghutiaojianDicts
                select new XElement("option",
                    new XAttribute("value", a.Key),
                    new XText(a.Value)));

            string str = element.ToString(SaveOptions.DisableFormatting);
            return Content(str, "text/xml");
        }

        public ActionResult InstSTCombo()
        {
            XElement element = BuildInstSTCombo();
            string str = element.ToString(SaveOptions.DisableFormatting);
            return Content(str, "text/xml");
        }

        public ActionResult CompanyQualificationCombo()
        {
            XElement element = BuildCompanyQualificationCombo();
            string str = element.ToString(SaveOptions.DisableFormatting);
            return Content(str, "text/xml");
        }

        public ActionResult InstFormalCombo()
        {
            XElement element = BuildInstFormalCombo();
            string str = element.ToString(SaveOptions.DisableFormatting);
            return Content(str, "text/xml");
        }

        public ActionResult AreaCombo()
        {
            XElement element = BuildAreaCombo();
            string str = element.ToString(SaveOptions.DisableFormatting);
            return Content(str, "text/xml");
        }

        /// <summary>
        /// 获取当前用户需要过滤的机构Id
        /// </summary>
        /// <returns></returns>
        protected InstFilter GetCurrentInstFilter()
        {
            int userId = HttpContext.User.Identity.GetUserId<Int32>();
            return userService.GetFilterInsts(userId, canCheckAllCustomRoles);
        }

        /// <summary>
        /// 获取当前用户需要过滤的监督机构id
        /// </summary>
        /// <returns></returns>
        protected InspectFilter GetCurrentInspectFilter()
        {
            int userId = HttpContext.User.Identity.GetUserId<Int32>();
            return userService.GetFilterInspect(userId);
        }

        /// <summary>
        /// 获取当前用户的机构信息
        /// </summary>
        /// <returns></returns>
        protected t_bp_custom GetCurrentCustomInfo()
        {
            int userId = HttpContext.User.Identity.GetUserId<Int32>();
            return userService.GetCustomInsts(userId);
        }

        protected XElement BuildAreaCombo()
        {
            var areas = userService.GetAllArea();

            //XElement element = new XElement("complete",
            //   new XElement("option",
            //       new XAttribute("value", ""),
            //       new XText("全部")),
            //   from a in yanghutiaojianDicts
            //   select new XElement("option",
            //       new XAttribute("value", a.Key),
            //       new XText(a.Value)));

            XElement element = new XElement("complete",
                   new XElement("option",
                   new XAttribute("value", ""),
                   new XText("全部")),
                from a in areas
                select new XElement("option",
                    new XAttribute("value", a.regionname),
                    new XText(a.regionname)));

            return element;
        }

        protected XElement BuildCompanyQualificationCombo()
        {
            var qualification = userService.GetAllCompanyQualification();//  userService.GetAllArea();

            XElement element = new XElement("complete",
                from a in qualification
                select new XElement("option",
                    new XAttribute("value", a.FTestTypeName),
                    new XText(a.FTestTypeName)));

            return element;
        }

        protected XElement BuildInstCombo()
        {
            var insts = GetCurrentInsts();

            XElement element = new XElement("complete",
                 new XElement("template",
                    new XElement("input", new XCData("#name#")),
                    new XElement("header", true),
                    new XElement("columns",
                        new XElement("column",
                            new XAttribute("width", 320),
                            new XAttribute("header", "机构名称"),
                            new XAttribute("option", "#name#")))),
                 new XElement("option",
                           new XAttribute("value", string.Empty),
                           new XElement("text",
                                new XElement("name", "全部"))),
                 from kv in insts
                 select new XElement("option",
                            new XAttribute("value", kv.Key),
                            new XElement("text",
                                new XElement("name", kv.Value.IsNullOrEmpty() ? "无机构名称" : kv.Value))));

            return element;
        }

        protected XElement BuildInstSTCombo()
        {
            var insts = GetCurrentInstsST();// GetCurrentInstsST();

            XElement element = new XElement("complete",
                 new XElement("template",
                    new XElement("input", new XCData("#name#")),
                    new XElement("header", true),
                    new XElement("columns",
                        new XElement("column",
                            new XAttribute("width", 320),
                            new XAttribute("header", "机构名称"),
                            new XAttribute("option", "#name#")))),
                 new XElement("option",
                           new XAttribute("value", string.Empty),
                           new XElement("text",
                                new XElement("name", "全部"))),
                 from kv in insts
                 select new XElement("option",
                            new XAttribute("value", kv.Key),
                            new XElement("text",
                                new XElement("name", kv.Value.IsNullOrEmpty() ? "无机构名称" : kv.Value))));

            return element;
        }


        protected XElement BuildInstFormalCombo()
        {
            var insts = GetCurrentFormalInsts();

            XElement element = new XElement("complete",
                 new XElement("template",
                    new XElement("input", new XCData("#name#")),
                    new XElement("header", true),
                    new XElement("columns",
                        new XElement("column",
                            new XAttribute("width", 320),
                            new XAttribute("header", "机构名称"),
                            new XAttribute("option", "#name#")))),
                 new XElement("option",
                           new XAttribute("value", string.Empty),
                           new XElement("text",
                                new XElement("name", "全部"))),
                 from kv in insts
                 select new XElement("option",
                            new XAttribute("value", kv.Key),
                            new XElement("text",
                                new XElement("name", kv.Value.IsNullOrEmpty() ? "无机构名称" : kv.Value))));

            return element;
        }

        protected XElement BuildItemCombo(Dictionary<string, string> allItems)
        {
            XElement element = new XElement("complete",
                 new XElement("template",
                    new XElement("input", new XCData("#name#")),
                    new XElement("header", true),
                    new XElement("columns",
                        new XElement("column",
                            new XAttribute("width", 320),
                            new XAttribute("header", "检测项目"),
                            new XAttribute("option", "#name#")))),
                  new XElement("option",
                           new XAttribute("value", string.Empty),
                           new XElement("text",
                                new XElement("name", "全部"))),
                 from kv in allItems
                 select new XElement("option",
                            new XAttribute("value", kv.Key),
                            new XElement("text",
                                new XElement("name", kv.Value))));

            return element;
        }

        protected XElement BuildInspectCombo(List<WxInspectUnit> allInspects)
        {
            var currentInspectFilter = GetCurrentInspectFilter();
            if (currentInspectFilter.NeedFilter
                && currentInspectFilter.UserInspect != null
                && !currentInspectFilter.UserInspect.InspectId.IsNullOrEmpty())
            {
                var afterFilterInspects = allInspects.Where(ai => ai.unitcode == currentInspectFilter.UserInspect.InspectId).ToList();

                XElement element = new XElement("complete",
                                         new XElement("option",
                                              new XAttribute("value", "ALL"),
                                              new XText("全部")),
                                         from inspect in afterFilterInspects
                                         select new XElement("option",
                                             new XAttribute("value", inspect.unitcode),
                                             new XText(inspect.unitName)));

                return element;
            }
            else
            {
                XElement element = new XElement("complete",
                                             new XElement("option",
                                                  new XAttribute("value", "ALL"),
                                                  new XText("全部")),
                                             from inspect in allInspects
                                             select new XElement("option",
                                                 new XAttribute("value", inspect.unitcode),
                                                 new XText(inspect.unitName)));

                return element;
            }
        }



        protected XElement BuildInstAreaTreeByName(ICheckUnitService checkUnitService, string name, List<string> selectedInstIds)
        {
            var instAreas = checkUnitService.GetInstAreasByName(name);
            XElement rootElem = new XElement("tree", new XAttribute("id", 0));

            var areas = instAreas.Where(ia => !string.IsNullOrWhiteSpace(ia.area)).Select(a => a.area).Distinct().ToList();

            foreach (var area in areas)
            {
                XElement areaElem = new XElement("item",
                    new XAttribute("text", area),
                    new XAttribute("id", area),
                    new XAttribute("nocheckbox", 1));

                foreach (var areaInst in instAreas.Where(ia => ia.area == area))
                {
                    XElement insElem = new XElement("item",
                        new XAttribute("text", areaInst.NAME),
                        new XAttribute("id", areaInst.ID));

                    if (selectedInstIds.Exists(si => si == areaInst.ID))
                    {
                        insElem.Add(new XAttribute("checked", 1));
                    }

                    areaElem.Add(insElem);
                }
                rootElem.Add(areaElem);
            }

            var noAreas = instAreas.Where(ia => string.IsNullOrWhiteSpace(ia.area)).Select(a => a.area).ToList();
            if (noAreas.Count > 0)
            {
                XElement areaElem = new XElement("item",
                   new XAttribute("text", "无地区"),
                   new XAttribute("id", "NoArea"),
                   new XAttribute("nocheckbox", 1));

                foreach (var areaInst in instAreas.Where(ia => string.IsNullOrWhiteSpace(ia.area)))
                {
                    XElement insElem = new XElement("item",
                        new XAttribute("text", areaInst.NAME),
                        new XAttribute("id", areaInst.ID));

                    if (selectedInstIds.Exists(si => si == areaInst.ID))
                    {
                        insElem.Add(new XAttribute("checked", 1));
                    }
                    areaElem.Add(insElem);
                }
                rootElem.Add(areaElem);
            }
            return rootElem;
        }

        protected XElement BuildInstAreaTree(ICheckUnitService checkUnitService, List<string> selectedInstIds)
        {
            var instAreas = checkUnitService.GetAllInstAreas();
            XElement rootElem = new XElement("tree", new XAttribute("id", 0));

            var areas = instAreas.Where(ia => !string.IsNullOrWhiteSpace(ia.area)).Select(a => a.area).Distinct().ToList();

            foreach (var area in areas)
            {
                XElement areaElem = new XElement("item",
                    new XAttribute("text", area),
                    new XAttribute("id", area),
                    new XAttribute("nocheckbox", 1));

                foreach (var areaInst in instAreas.Where(ia => ia.area == area))
                {
                    XElement insElem = new XElement("item",
                        new XAttribute("text", areaInst.NAME),
                        new XAttribute("id", areaInst.ID));

                    if (selectedInstIds.Exists(si => si == areaInst.ID))
                    {
                        insElem.Add(new XAttribute("checked", 1));
                    }

                    areaElem.Add(insElem);
                }
                rootElem.Add(areaElem);
            }

            var noAreas = instAreas.Where(ia => string.IsNullOrWhiteSpace(ia.area)).Select(a => a.area).ToList();
            if (noAreas.Count > 0)
            {
                XElement areaElem = new XElement("item",
                   new XAttribute("text", "无地区"),
                   new XAttribute("id", "NoArea"),
                   new XAttribute("nocheckbox", 1));

                foreach (var areaInst in instAreas.Where(ia => string.IsNullOrWhiteSpace(ia.area)))
                {
                    XElement insElem = new XElement("item",
                        new XAttribute("text", areaInst.NAME),
                        new XAttribute("id", areaInst.ID));

                    if (selectedInstIds.Exists(si => si == areaInst.ID))
                    {
                        insElem.Add(new XAttribute("checked", 1));
                    }
                    areaElem.Add(insElem);
                }
                rootElem.Add(areaElem);
            }
            return rootElem;
        }


        protected void BuildItemNameRow(IItemNameService itemNameService, ItemShortInfos allItems, es_t_bp_item item, DhtmlxGridRow row)
        {
            string uiItemName = string.Empty;

            if (uiItemName.IsNullOrEmpty())
            {
                uiItemName = item.ITEMCHNAME.IsNullOrEmpty() ?
                        itemNameService.GetItemCNNameFromAll(allItems, item.REPORTJXLB, item.ITEMNAME) : item.ITEMCHNAME;
            }

            row.AddCell(uiItemName);
        }

        protected void BuildReportNumRow(IReportService reportService, Dictionary<string, int> pkrReports, es_t_bp_item item, DhtmlxGridRow row)
        {
            if (pkrReports != null
                && pkrReports.Count > 0)
            {

                var pkrReportNum = reportService.GetCryptPkrReportNumFromDict(pkrReports, item);

                if (pkrReportNum.IsNullOrEmpty())
                {
                    row.AddLinkJsCell(item.REPORTNUM, string.Empty);
                }
                else
                {
                    row.AddLinkJsCell(item.REPORTNUM, "getPKRReport(\"{0}\")".Fmt(pkrReportNum));
                }

            }
            else
            {
                row.AddLinkJsCell(item.REPORTNUM, string.Empty);
            }
        }



        protected string GetUIDtString(DateTime dt, string format = "yyyy-MM-dd HH:mm")
        {
            return dt.ToString(format);
        }

        protected string GetUIDtString(DateTime? dt, string format = "yyyy-MM-dd HH:mm")
        {
            if (dt.HasValue)
            {
                return dt.Value.ToString(format);
            }
            else
            {
                return string.Empty;
            }

        }

        protected InstShortInfos GetCurrentEliminateIsUerInsts()
        {
            int userId = GetCurrentUserId();
            return userService.GetUserEliminateIsUerInsts(userId);
        }



        /// <summary>
        /// 获取过滤条件
        /// </summary>
        /// <param name="checkUnitService">检测机构</param>
        /// <param name="itemNameService">检测项目</param>
        /// <param name="model"></param>
        /// <param name="userCustoms">用户自定义检测机构</param>
        /// <param name="userItems">用户自定义检测项目</param>
        /// <returns></returns>
        protected Func<QueryContainerDescriptor<es_t_bp_item>, QueryContainer> GetFilterQuery(ICheckUnitService checkUnitService,
            IItemNameService itemNameService,
            SysSearchModel model,
            Dictionary<string, string> userCustoms,
            Dictionary<string, string> userItems)
        {
            #region 过滤条件

            Func<QueryContainerDescriptor<es_t_bp_item>, QueryContainer> filterQuery = q =>
            {
                string dtFormatStr = "yyyy-MM-dd'T'HH:mm:ss";


                QueryContainer initQuery = q.Exists(qe => qe.Field(qef => qef.SYSPRIMARYKEY));

                #region 对时间的处理
                //model.SearchType 只有从综合统计过来才有值，兼容综合统计部分
                if (model.SearchType.IsNullOrEmpty())
                {
                    if (model.StartDt.HasValue || model.EndDt.HasValue)
                    {
                        string startDtStr = model.StartDt.HasValue ? model.StartDt.Value.ToString(dtFormatStr) : string.Empty;
                        string endDtStr = model.EndDt.HasValue ? model.EndDt.Value.AddDays(1).ToString(dtFormatStr) : string.Empty;
                        if (model.DtType == "EntrustDt")
                        {
                            if (!startDtStr.IsNullOrEmpty())
                            {
                                initQuery = initQuery && +q.DateRange(d => d.Field(f => f.ENTRUSTDATE).GreaterThanOrEquals(DateMath.FromString(startDtStr)));
                            }
                            if (!endDtStr.IsNullOrEmpty())
                            {
                                initQuery = initQuery && +q.DateRange(d => d.Field(f => f.ENTRUSTDATE).LessThan(DateMath.FromString(endDtStr)));
                            }
                        }
                        else if (model.DtType == "CheckDt")
                        {
                            if (!startDtStr.IsNullOrEmpty())
                            {
                                initQuery = initQuery && +q.DateRange(d => d.Field(f => f.CHECKDATE).GreaterThanOrEquals(DateMath.FromString(startDtStr)));
                            }
                            if (!endDtStr.IsNullOrEmpty())
                            {
                                initQuery = initQuery && +q.DateRange(d => d.Field(f => f.CHECKDATE).LessThan(DateMath.FromString(endDtStr)));
                            }
                        }
                        else if (model.DtType == "ReportDt")
                        {
                            if (!startDtStr.IsNullOrEmpty())
                            {
                                initQuery = initQuery && +q.DateRange(d => d.Field(f => f.PRINTDATE).GreaterThanOrEquals(DateMath.FromString(startDtStr)));
                            }
                            if (!endDtStr.IsNullOrEmpty())
                            {
                                initQuery = initQuery && +q.DateRange(d => d.Field(f => f.PRINTDATE).LessThan(DateMath.FromString(endDtStr)));
                            }
                        }
                        else if (model.DtType == "APPROVEDATE")
                        {
                            if (!startDtStr.IsNullOrEmpty())
                            {
                                initQuery = initQuery && +q.DateRange(d => d.Field(f => f.APPROVEDATE).GreaterThanOrEquals(DateMath.FromString(startDtStr)));
                            }
                            if (!endDtStr.IsNullOrEmpty())
                            {
                                initQuery = initQuery && +q.DateRange(d => d.Field(f => f.APPROVEDATE).LessThan(DateMath.FromString(endDtStr)));
                            }
                        }
                        else if (model.DtType == "UploadDt")
                        {
                            if (!startDtStr.IsNullOrEmpty())
                            {
                                initQuery = initQuery && +q.DateRange(d => d.Field(f => f.UPLOADTIME).GreaterThanOrEquals(DateMath.FromString(startDtStr)));
                            }
                            if (!endDtStr.IsNullOrEmpty())
                            {
                                initQuery = initQuery && +q.DateRange(d => d.Field(f => f.UPLOADTIME).LessThan(DateMath.FromString(endDtStr)));
                            }
                        }
                        else if (model.DtType == "ENTRUSTDATE")
                        {
                            if (!startDtStr.IsNullOrEmpty())
                            {
                                initQuery = initQuery && +q.DateRange(d => d.Field(f => f.ENTRUSTDATE).GreaterThanOrEquals(DateMath.FromString(startDtStr)));
                            }
                            if (!endDtStr.IsNullOrEmpty())
                            {
                                initQuery = initQuery && +q.DateRange(d => d.Field(f => f.ENTRUSTDATE).LessThan(DateMath.FromString(endDtStr)));
                            }
                        }
                        else  //如果dtType为空值或者其他值，则按照报告时间进行查询
                        {
                            if (!startDtStr.IsNullOrEmpty())
                            {
                                initQuery = initQuery && +q.DateRange(d => d.Field(f => f.PRINTDATE).GreaterThanOrEquals(DateMath.FromString(startDtStr)));
                            }
                            if (!endDtStr.IsNullOrEmpty())
                            {
                                initQuery = initQuery && +q.DateRange(d => d.Field(f => f.PRINTDATE).LessThan(DateMath.FromString(endDtStr)));
                            }
                        }
                    }
                }
                else
                {

                    string startDtStr = string.Empty;
                    string endDtStr = string.Empty;
                    var date = DateTime.Today;
                    switch (model.SearchType)
                    {
                        case "Search":
                            if (model.StartDt.HasValue || model.EndDt.HasValue)
                            {
                                startDtStr = model.StartDt.HasValue ? model.StartDt.Value.ToString(dtFormatStr) : string.Empty;
                                endDtStr = model.EndDt.HasValue ? model.EndDt.Value.AddDays(1).ToString(dtFormatStr) : string.Empty;
                            }
                            break;
                        case "day":
                            startDtStr = DateTime.Today.ToString(dtFormatStr);
                            endDtStr = DateTime.Today.AddDays(1).ToString(dtFormatStr);
                            break;
                        case "week":
                            DateTime startWeek = date.AddDays(1 - Convert.ToInt32(date.DayOfWeek.ToString("d"))); //本周周一
                            DateTime endWeek = startWeek.AddDays(7);
                            startDtStr = startWeek.ToString(dtFormatStr);
                            endDtStr = endWeek.ToString(dtFormatStr);
                            break;
                        case "month":
                            DateTime startMonth = date.AddDays(1 - date.Day);  //本月月初  
                            DateTime endMonth = startMonth.AddMonths(1);  //本月月末  
                            // DateTime startMonth = new DateTime(2016, 09, 01);  //本月月初  
                            //DateTime endMonth = startMonth.AddMonths(1);  //本月月末  
                            startDtStr = startMonth.ToString(dtFormatStr);
                            endDtStr = endMonth.ToString(dtFormatStr);
                            break;
                        case "quarter":
                            DateTime startQuarter = date.AddMonths(0 - (date.Month - 1) % 3).AddDays(1 - date.Day);  //本季度初  
                            DateTime endQuarter = startQuarter.AddMonths(3);
                            startDtStr = startQuarter.ToString(dtFormatStr);
                            endDtStr = endQuarter.ToString(dtFormatStr);
                            break;
                        case "year":
                            DateTime startYear = new DateTime(date.Year, 1, 1);  //本年年初  
                            DateTime endYear = new DateTime(date.Year, 12, 31).AddDays(1);  //本年年末  
                            startDtStr = startYear.ToString(dtFormatStr);
                            endDtStr = endYear.ToString(dtFormatStr);
                            break;
                    }
                    if (!startDtStr.IsNullOrEmpty())
                    {
                        initQuery = initQuery && +q.DateRange(d => d.Field(f => f.ENTRUSTDATE).GreaterThanOrEquals(DateMath.FromString(startDtStr)));
                    }
                    if (!endDtStr.IsNullOrEmpty())
                    {
                        initQuery = initQuery && +q.DateRange(d => d.Field(f => f.ENTRUSTDATE).LessThanOrEquals(DateMath.FromString(endDtStr)));
                    }
                }
                #endregion

                if (!model.ReportStatus.IsNullOrEmpty())
                {
                    if (model.ReportStatus == "0")
                    {
                        initQuery = initQuery && q.Match(m => m.Field(f => f.SAMPLEDISPOSEPHASE).Query(printedPhrase));
                    }
                    else if (model.ReportStatus == "1")
                    {
                        initQuery = initQuery && q.Match(m => m.Field(f => f.SAMPLEDISPOSEPHASE).Query(collectedPhrase));//收样
                    }
                    else if (model.ReportStatus == "2")
                    {
                        initQuery = initQuery && q.Match(m => m.Field(f => f.SAMPLEDISPOSEPHASE).Query(testedPhrase));//检测
                    }
                }

                #region 对综合查询数据状态的处理
                if (!model.DataState.IsNullOrEmpty())
                {
                    if (model.DataState == "0")
                    {
                        initQuery = initQuery && q.Match(m => m.Field(f => f.SAMPLEDISPOSEPHASE).Query(collectedPhrase));//收样
                    }
                    else if (model.DataState == "1")
                    {
                        initQuery = initQuery && q.Match(m => m.Field(f => f.SAMPLEDISPOSEPHASE).Query(checkedPhrase));//复核
                    }
                    else if (model.DataState == "2")
                    {
                        initQuery = initQuery && q.Match(m => m.Field(f => f.SAMPLEDISPOSEPHASE).Query(testedPhrase));//检测
                    }
                    else if (model.DataState == "3")
                    {
                        initQuery = initQuery && q.Match(m => m.Field(f => f.SAMPLEDISPOSEPHASE).Query(proofreadPhrase));//校核
                    }
                    else if (model.DataState == "4")
                    {
                        initQuery = initQuery && q.Match(m => m.Field(f => f.SAMPLEDISPOSEPHASE).Query(verifyedPhrase));//审核
                    }
                    else if (model.DataState == "5")
                    {
                        initQuery = initQuery && q.Match(m => m.Field(f => f.SAMPLEDISPOSEPHASE).Query(approvaledPhrase));//批准
                    }
                    else if (model.DataState == "6")
                    {
                        initQuery = initQuery && q.Match(m => m.Field(f => f.SAMPLEDISPOSEPHASE).Query(printedPhrase));//打印
                    }
                    else if (model.DataState == "7")
                    {
                        initQuery = initQuery && q.Match(m => m.Field(f => f.SAMPLEDISPOSEPHASE).Query(fileedPhrase));//归档
                    }
                    else if (model.DataState == "8")
                    {
                        initQuery = initQuery && q.Match(m => m.Field(f => f.SAMPLEDISPOSEPHASE).Query(giveOutPhrase));//发放
                    }
                    else if (model.DataState == "9")
                    {
                        initQuery = initQuery && q.Match(m => m.Field(f => f.SAMPLEDISPOSEPHASE).Query(canceledPhrase));//作废
                    }

                }
                #endregion

                if (model.GetStartDt.HasValue)
                {
                    var getStartDtStr = model.GetStartDt.Value.ToString(dtFormatStr);
                    initQuery = initQuery && q.DateRange(d => d.Field(f => f.ACSTIME).GreaterThanOrEquals(DateMath.FromString(getStartDtStr)));
                }

                if (model.GetEndDt.HasValue)
                {
                    var gteEndDtStr = model.GetEndDt.Value.ToString(dtFormatStr);
                    initQuery = initQuery && q.DateRange(d => d.Field(f => f.ACSTIME).LessThan(DateMath.FromString(gteEndDtStr)));
                }



                if (model.ReportStartDt.HasValue)
                {
                    var reportStartDt = model.ReportStartDt.Value.ToString(dtFormatStr);
                    initQuery = initQuery && q.DateRange(d => d.Field(f => f.PRINTDATE).GreaterThanOrEquals(DateMath.FromString(reportStartDt)));
                }
                if (model.ReportEndDt.HasValue)
                {
                    var reportEndDt = model.ReportEndDt.Value.ToString(dtFormatStr);
                    initQuery = initQuery && q.DateRange(d => d.Field(f => f.PRINTDATE).LessThan(DateMath.FromString(reportEndDt)));
                }

                if (!string.IsNullOrWhiteSpace(model.Area))
                {
                    var areas = model.Area.Split(',').ToList();

                    Dictionary<string, string> CheckUnits = checkUnitService.GetUnitByArea(areas);
                    if (CheckUnits.Count > 0)
                    {
                        initQuery = initQuery && +q.Terms(t => t.Field(f => f.CUSTOMID).Terms(CheckUnits.Keys.ToList()));
                    }
                    else
                    {
                        initQuery = initQuery && +q.Terms(t => t.Field(f => f.CUSTOMID).Terms("asdfghj"));//by毛冰梅———要求该地区如果没有检测单位时，不查询所有地区数据
                    }
                }

                if (!model.CustomId.IsNullOrEmpty())
                {
                    initQuery = initQuery && +q.Term(t => t.Field(f => f.CUSTOMID).Value(model.CustomId));
                }

                if (model.modelType != SysSearchModelModelType.TotalSearchView)
                {
                    var instFilter = GetCurrentInstFilter();
                    if (instFilter.NeedFilter && instFilter.FilterInstIds.Count() > 0)
                    {
                        initQuery = initQuery && +q.Terms(qtm => qtm.Field(qtmf => qtmf.CUSTOMID).Terms(instFilter.FilterInstIds));
                    }
                }

                var inspectFilter = GetCurrentInspectFilter();


                if (!string.IsNullOrWhiteSpace(model.CheckInstID))
                {
                    initQuery = initQuery && +q.Term(t => t.Field(f => f.CUSTOMID).Value(model.CheckInstID));
                }
                if(!string.IsNullOrWhiteSpace(model.IsUse) )
                {
                    
                    initQuery = initQuery && +q.Term(t => t.Field(f => f.CUSTOMID).Value(model.IsUse));
                }


                if (!string.IsNullOrWhiteSpace(model.ProjectName))
                {
                    //initQuery = initQuery && q.QueryString(m => m.DefaultField(f => f.PROJECTNAME[0].Suffix("PROJECTNAMERAW")).Query("{0}{1}{0}".Fmt("*", model.ProjectName)));
                    initQuery = initQuery && q.Wildcard(w => w.Field(wf => wf.PROJECTNAME).Value("{0}{1}{0}".Fmt("*", model.ProjectName)));
                }

                if (!model.TestCategories.IsNullOrEmpty())
                {
                    initQuery = initQuery && q.Term(qt => qt.Field(qtf => qtf.REPORTJXLB).Value(model.TestCategories));
                }

                if (!string.IsNullOrWhiteSpace(model.EntrustUnit))
                {
                    initQuery = initQuery && q.Match(m => m.Field(f => f.ENTRUSTUNIT).Query(model.EntrustUnit));
                }

                if (!model.JDType.IsNullOrEmpty())
                {
                    initQuery = initQuery && q.Terms(m => m.Field(f => f.EXPLAIN).Terms(model.JDType));
                }

                if (!string.IsNullOrWhiteSpace(model.Num))
                {
                    //编号查询，支持模糊
                    initQuery = initQuery && (q.Term(qm => qm.Field(qmf => qmf.SAMPLENUM).Value(model.Num))
                                          || q.QueryString(qrm => qrm.DefaultField(qrmf => qrmf.SAMPLENUM).Query("{0}{1}{0}".Fmt("*", model.Num)))
                                          || q.Term(qe => qe.Field(qef => qef.REPORTNUM).Value(model.Num))
                                          || q.QueryString(qrm => qrm.DefaultField(qrmf => qrmf.REPORTNUM).Query("{0}{1}{0}".Fmt("*", model.Num)))
                                          || q.Term(qr => qr.Field(qrf => qrf.ENTRUSTNUM).Value(model.Num))
                                          || q.QueryString(qrm => qrm.DefaultField(qrmf => qrmf.ENTRUSTNUM).Query("{0}{1}{0}".Fmt("*", model.Num))));


                }

                if (!string.IsNullOrWhiteSpace(model.ReportNum))
                {
                    //initQuery = initQuery && q.QueryString(m => m.DefaultField(f => f.REPORTNUM).Query("{0}{1}{0}".Fmt("*", model.ReportNum)));
                    initQuery = initQuery && q.Wildcard(w => w.Field(f => f.REPORTNUM).Value("{0}{1}{0}".Fmt("*", model.ReportNum.Trim())));
                }

                if (model.IsCType.HasValue && model.IsCType.Value == 1)
                {
                    initQuery = initQuery && +q.Range(qtm => qtm.Field(qtmf => qtmf.ISCREPORT).GreaterThanOrEquals(1));
                }

                if (model.IsChanged.HasValue && model.IsChanged.Value != -1)
                {
                    if (model.IsChanged.Value == 1)
                    {
                        initQuery = initQuery && +q.Range(r => r.Field(f => f.HAVELOG).GreaterThanOrEquals(1));
                    }
                    else if (model.IsChanged.Value == 0)
                    {
                        initQuery = initQuery && (!q.Exists(e => e.Field(f => f.HAVELOG)) || q.Term(t => t.Field(f => f.HAVELOG).Value(0)));
                    }
                }

                if (model.HasArc.HasValue && model.HasArc.Value != -1)
                {
                    if (model.HasArc.Value == 1)
                    {
                        initQuery = initQuery && +q.Range(r => r.Field(f => f.HAVEACS).GreaterThanOrEquals(1)) && q.Exists(r => r.Field(f => f.ACSTIME));
                    }
                    else if (model.HasArc.Value == 0)
                    {
                        initQuery = initQuery && (!q.Exists(e => e.Field(f => f.HAVEACS)) || q.Term(t => t.Field(f => f.HAVEACS).Value(0)));
                    }
                }

                if (!string.IsNullOrWhiteSpace(model.CheckStatus) && model.CheckStatus != "A")
                {
                    initQuery = initQuery && +q.Term(t => t.Field(f => f.CONCLUSIONCODE).Value(model.CheckStatus));
                }
                if (!string.IsNullOrEmpty(model.SampleNum))
                {
                    initQuery = initQuery && q.QueryString(qrm => qrm.DefaultField(qrmf => qrmf.SAMPLENUM).Query("{0}{1}{0}".Fmt("*", model.SampleNum)));
                    //+ q.Term(t => t.Field(f => f.SAMPLENUM).Value(model.SampleNum));
                }

                if (model.IsReport.HasValue)
                {
                    if (model.IsReport.Value == 1)
                    {
                        initQuery = initQuery && q.Term(qt => qt.Field(qtf => qtf.HAVREPORT).Value(1));
                    }
                    else if (model.IsReport.Value == 0)
                    {
                        initQuery = initQuery && (!q.Exists(t => t.Field(tf => tf.HAVREPORT)) || q.Term(tt => tt.Field(ttt => ttt.HAVREPORT).Value(0)));
                    }
                }

                //委托编号
                if (!model.EntrustNum.IsNullOrEmpty())
                {
                    initQuery = initQuery && q.QueryString(qrm => qrm.DefaultField(qrmf => qrmf.ENTRUSTNUM).Query("{0}{1}{0}".Fmt("*", model.EntrustNum)));

                }

                ////样品编号
                //if (!model.SAMPLENUM.IsNullOrEmpty())
                //{
                //    initQuery = initQuery && q.QueryString(qrm => qrm.DefaultField(qrmf => qrmf.SAMPLENUM).Query("{0}{1}{0}".Fmt("*", model.SAMPLENUM)));
                //}

                if (!model.ReportNumPrefix.IsNullOrEmpty())
                {
                    initQuery = initQuery && q.Term(m => m.Field(f => f.REPORMNUMWITHOUTSEQ).Value(model.ReportNumPrefix));
                }



                #region 兼容不同控制器
                //不是综合查询就只显示已打印,统计以及不需要登录的综合查询也查询所有的数据
                if (model.modelType != SysSearchModelModelType.TotalSearch && model.modelType != SysSearchModelModelType.CheckStatis && model.modelType != SysSearchModelModelType.TotalSearchView)
                {
                    if ("1" == System.Configuration.ConfigurationManager.AppSettings["ShowPrinted"])
                    {
                        initQuery = initQuery && q.Match(m => m.Field(f => f.SAMPLEDISPOSEPHASE).Query(printedPhrase));
                    }
                }

                if (!model.SiderbarType.IsNullOrEmpty())
                {
                    if (model.Group == "1")
                    {
                        initQuery = initQuery && q.Term(tt => tt.Field(ttt => ttt.PROJECTNAME).Value(model.SiderbarType));

                    }
                    else
                    {
                        //initQuery = initQuery && q.Term(tt => tt.Field(ttt => ttt.ITEMNAME).Value(model.SiderbarType));
                        if (model.SiderbarType.Contains('|'))
                        {
                            var keys = model.SiderbarType.Split('|');
                            var typeCode = keys[0];
                            var itemCode = keys[1];
                            initQuery = initQuery && q.Term(tt => tt.Field(ttt => ttt.ITEMNAME).Value(itemCode)) && q.Term(tt => tt.Field(ttt => ttt.REPORTJXLB).Value(typeCode));
                        }
                    }
                }


                if (model.modelType == SysSearchModelModelType.NoAcsSearch)
                {
                    List<string> needAcsItems = itemNameService.GetAcsItemNames();
                    initQuery = initQuery && +q.Terms(qts => qts.Field(qtsf => qtsf.ITEMNAME).Terms(needAcsItems))
                                         && (q.Range(qt => qt.Field(qtf => qtf.HAVEACS).LessThanOrEquals(0)) //字段小于等于0
                                              || !q.Exists(qe => qe.Field(qef => qef.HAVEACS))); //无此字段  
                }

                if (model.modelType == SysSearchModelModelType.UnQualified)
                {
                    //不合格报告的条件
                    initQuery = initQuery && +q.Term(t => t.Field(f => f.CONCLUSIONCODE).Value("N"));
                }

                if (!model.itemkey.IsNullOrEmpty())
                {
                    //不合格报告中（材料动态分析中）点击查看某一项的动态分析表中使用

                    if (model.itemkey.Length == 9)
                    {
                        var keys = model.itemkey.Split('|');
                        var typeCode = keys[0];
                        var itemCode = keys[1];
                        initQuery = initQuery && q.Term(tt => tt.Field(ttt => ttt.REPORTJXLB).Value(typeCode));
                        initQuery = initQuery && q.Term(tt => tt.Field(ttt => ttt.ITEMNAME).Value(itemCode));
                    }
                    else
                    {
                        initQuery = initQuery && +q.Term(t => t.Field(f => f.ITEMNAME).Value(model.itemkey.Trim()));
                    }
                }

                if (model.modelType == SysSearchModelModelType.AcsTimeStatisc)
                {
                    initQuery = initQuery && +q.Term(t => t.Field(f => f.HAVEACS).Value(1)) && +q.Exists(e => e.Field(f => f.ACSTIME)) && +q.Exists(e => e.Field(f => f.CHECKDATE));

                    initQuery = initQuery && (q.Script(qs => qs.Inline("doc['CHECKDATE'].date.year > doc['ACSTIME'].date.year "))
                                           || q.Script(qs => qs.Inline("doc['CHECKDATE'].date.year ==  doc['ACSTIME'].date.year && doc['CHECKDATE'].date.monthOfYear > doc['ACSTIME'].date.monthOfYear  "))
                                           || q.Script(qs => qs.Inline("doc['CHECKDATE'].date.year == doc['ACSTIME'].date.year && doc['CHECKDATE'].date.monthOfYear == doc['ACSTIME'].date.monthOfYear && doc['CHECKDATE'].date.dayOfYear > doc['ACSTIME'].date.dayOfYear")));

                }

                if (model.modelType == SysSearchModelModelType.ReportdataAnalysis)
                {
                    string startDtStr = model.StartDt.HasValue ? model.StartDt.Value.ToString(dtFormatStr) : "";
                    string endDtStr = model.EndDt.HasValue ? model.EndDt.Value.AddDays(1).ToString(dtFormatStr) : "";
                    if (!(startDtStr.IsNullOrEmpty() && endDtStr.IsNullOrEmpty()))
                    {
                        initQuery = q.DateRange(d => d.Field(f => f.CHECKDATE).GreaterThanOrEquals(DateMath.FromString(startDtStr)).LessThanOrEquals(DateMath.FromString(endDtStr)));
                    }
                }

                if (model.modelType == SysSearchModelModelType.NoDataUploadAllDetails)
                {
                    initQuery = initQuery && (!q.Term(qtf => qtf.Field(t => t.HAVREPORT).Value(1)));
                }

                //if (model.modelType == SysSearchModelModelType.ReportCategory)
                //{
                //    initQuery = initQuery && q.Term(qt => qt.Field(qtf => qtf.SUBITEMCODE).Value(model.SubItemCodeRaw));
                //    initQuery = initQuery && q.Term(qt => qt.Field(qtf => qtf.PARMCODE).Value(model.ParamCodeRaw));
                //}

                //兼容某些需要查询被修改报告
                if (model.modelType == SysSearchModelModelType.ModifyReport)
                {
                    initQuery = initQuery && +(q.Term(qtf => qtf.Field(t => t.HAVELOG).Value(1)));
                }

                //用户自定义机构
                if (userCustoms != null && userCustoms.Count > 0)
                {
                    initQuery = initQuery && +q.Terms(qtm => qtm.Field(qtmf => qtmf.CUSTOMID).Terms(userCustoms.Values.ToList()));
                }
                //用户自定义项目
                if (userItems != null && userItems.Count > 0)
                {
                    initQuery = initQuery && +q.Terms(qtm => qtm.Field(qtmf => qtmf.ITEMNAME).Terms(userItems.Values.ToList()));
                }

                #endregion

                if (!filterCustom.IsNullOrEmpty())
                {
                    //var customs = filterCustom.Split(',').ToList();
                    initQuery = initQuery && +!q.Terms(qtm => qtm.Field(qtmf => qtmf.CUSTOMID).Terms(filterCustom.Split(',').ToList()));
                }


                if (model.IsMonthAgg)
                {
                    DateTime DtNow = DateTime.Now;
                    string endDtStr = DtNow.ToString(dtFormatStr);
                    DateTime DtStart = DtNow.AddMonths(-12);
                    string startDtStr = new DateTime(DtStart.Year, DtStart.Month, 1).ToString(dtFormatStr);
                    initQuery = initQuery && +q.DateRange(qdr => qdr.Field(qdrf => qdrf.ENTRUSTDATE).GreaterThanOrEquals(DateMath.FromString(startDtStr)))
                    && q.DateRange(qddr => qddr.Field(qddrf => qddrf.ENTRUSTDATE).LessThanOrEquals(DateMath.FromString(endDtStr)));
                }

                #region 兼容不合格报告
                if (!model.ItemName.IsNullOrEmpty())
                {
                    if (model.ItemName.Length == 9)
                    {
                        var keys = model.ItemName.Split('|');
                        var typeCode = keys[0];
                        var itemCode = keys[1];
                        initQuery = initQuery && q.Term(tt => tt.Field(ttt => ttt.REPORTJXLB).Value(typeCode));
                        initQuery = initQuery && q.Term(tt => tt.Field(ttt => ttt.ITEMNAME).Value(itemCode));
                    }
                    else
                    {
                        initQuery = initQuery && +q.Term(t => t.Field(f => f.ITEMNAME).Value(model.ItemName));
                    }

                }

                if (!string.IsNullOrWhiteSpace(model.ProjectNameRaw))
                {
                    initQuery = initQuery && +q.Term(t => t.Field(f => f.PROJECTNAME[0].Suffix("PROJECTNAMERAW")).Value(model.ProjectNameRaw));
                }
                #endregion
                if (!model.CheckItem.IsNullOrEmpty())
                {
                    if (model.CheckItem.Length == 8)
                    {
                        var typeCode = model.CheckItem.Substring(0, 4);
                        var itemCode = model.CheckItem.Substring(4, 4);
                        initQuery = initQuery && q.Term(tt => tt.Field(ttt => ttt.REPORTJXLB).Value(typeCode));
                        initQuery = initQuery && q.Term(tt => tt.Field(ttt => ttt.ITEMNAME).Value(itemCode));
                    }
                    else
                    {
                        initQuery = initQuery && +q.Term(t => t.Field(f => f.ITEMNAME).Value(model.CheckItem));
                    }
                }
                //if (!model.TestCategories.IsNullOrEmpty())
                //{
                //    var data = itemNameService.GetItemNameByCategory(model.TestCategories);
                //    if (data.Count > 0)
                //    {
                //        initQuery = initQuery && q.Terms(qt => qt.Field(qtf => qtf.ITEMNAME).Terms(data.Keys.ToList()));
                //    }
                //}


                return initQuery;
            };
            #endregion


            return filterQuery;
        }

        public Func<FieldsDescriptor<es_t_bp_item>, IPromise<Fields>> GetDefaultFields()
        {
            return t => t.Fields(
                      f => f.SYSPRIMARYKEY,
                              f => f.CUSTOMID,
                              f => f.PROJECTNAME,
                              f => f.STRUCTPART,
                              f => f.ITEMNAME,
                              f => f.ITEMCHNAME,
                              f => f.ACSTIME,
                              f => f.ENTRUSTDATE,
                              f => f.CHECKDATE,
                              f => f.PRINTDATE,
                              f => f.SAMPLENUM,
                              f => f.REPORTNUM,
                              f => f.HAVREPORT,
                              f => f.ISCREPORT
                              //f => f.SUBITEMCODE
                             );
        }

        protected XElement BuildItemCodeTree(IItemNameService itemNameService)
        {
            XElement rootElem = new XElement("tree", new XAttribute("id", 0));
            var allTypes = itemNameService.GetAllTypeCodes();

            foreach (var type in allTypes)
            {
                XElement typeElem = new XElement("item",
                    new XAttribute("text", "{0}({1})".Fmt(type.CheckItemName, type.CheckItemCode)),
                    new XAttribute("id", type.CheckItemCode),
                    //new XAttribute("nocheckbox", 1),
                    from textItem in itemNameService.GetItemCodesByTypeCode(type.CheckItemCode)
                    select new XElement("item",
                        new XAttribute("text", "{0}({1})".Fmt(textItem.ItemName, textItem.itemcode)),
                        new XAttribute("id", textItem.itemcode)

                        //from parm in itemNameService.GetParmCodesByItemCode(textItem.itemcode)
                        //select new XElement("item",
                        //    new XAttribute("text", "{0}({1})".Fmt(parm.parmname, parm.parmcode)),
                        //    new XAttribute("id", parm.parmcode))
                        ));

                rootElem.Add(typeElem);
            }
            return rootElem;
        }
    }
}
