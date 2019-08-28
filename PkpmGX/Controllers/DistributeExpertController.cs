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
    public class DistributeExpertController : PkpmController
    {

        ISysDictService sysDictServcie;
        IPkpmConfigService pkpmConfigService;
        ICheckUnitService checkUnitService;
        ICheckQualifyService checkQualifyService;
        IRepsitory<t_D_UserTableTen> rep;
        IRepsitory<t_D_UserTableOne> repOne;

        public DistributeExpertController(ISysDictService sysDictServcie,
            IPkpmConfigService pkpmConfigService,
            ICheckUnitService checkUnitService,
            ICheckQualifyService checkQualifyService,
        IRepsitory<t_D_UserTableTen> rep,
        IRepsitory<t_D_UserTableOne> repOne,
        IUserService userService) : base(userService)
        {
            this.rep = rep;
            this.repOne = repOne;
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


        public ActionResult Search(DistributeExpertSearchModel searchModel)
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
                DhtmlxGridRow row = new DhtmlxGridRow(rowData.id);
                row.AddCell(string.Empty);
                row.AddCell((pos + i + 1).ToString());//序号

                var tbOne = repOne.GetById(rowData.pid);
                if (tbOne != null)
                {
                    row.AddCell(tbOne.unitname);
                }
                else
                {
                    row.AddCell("");
                }

                var zj = string.Empty;
                if (!string.IsNullOrEmpty(rowData.zjsp1))
                {
                    zj += rowData.zjsp1;
                }
                if (!string.IsNullOrEmpty(rowData.zjsp2))
                {
                    zj += "," + rowData.zjsp2;
                }
                if (!string.IsNullOrEmpty(zj))
                {
                    var userDic = userService.ApiGetUserNameByUserIds(zj.Split(',').ToList());
                    row.AddCell(userDic.Values.Join(","));
                }
                else
                {
                    row.AddCell("");
                }

                if (HaveButtonFromAll(buttons, "Distribute") && (rowData.@static == 1 || rowData.@static == 2))
                {
                    row.AddLinkJsCell("分配专家", "DistributeExpert(\"{0}\",\"{1}\")".Fmt(rowData.id, rowData.pid));
                }
                else
                {
                    row.AddCell(string.Empty);
                }

                grid.AddGridRow(row);
            }
            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }

        private SearchResult<DistributeExpertUIModel> GetSearchResult(DistributeExpertSearchModel searchModel)
        {

            var predicate = PredicateBuilder.True<t_D_UserTableTen>();

            #region 动态查询

            //var instFilter = GetCurrentInstFilter();
            //if (instFilter.NeedFilter && instFilter.FilterInstIds.Count() > 0)
            //{
            //    predicate = predicate.And(t => instFilter.FilterInstIds.Contains(t.unitcode));
            //}

            //if (IsCurrentSuperVisor())
            //{
            //    var area = GetCurrentAreas();
            //    var userInArea = checkUnitService.GetUnitByArea(area);
            //    var insts = userInArea.Select(t => t.Key).ToList();
            //    if (insts != null && insts.Count >= 0)
            //    {
            //        predicate = predicate.And(t => insts.Contains(t.unitcode));
            //    }
            //}

            #endregion

            int pos = searchModel.posStart.HasValue ? searchModel.posStart.Value : 0;
            int count = searchModel.count.HasValue ? searchModel.count.Value : 30;

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

            var customs = rep.GetByConditionSort<DistributeExpertUIModel>(predicate, r => new
            {
                r.id,
                r.pid,
                r.zjsp1,
                r.zjsp2,
                r.@static
            },
               pagingOption);

            return new SearchResult<Models.DistributeExpertUIModel>(pagingOption.TotalItems, customs);
        }



        public ActionResult Distribute(int id, int pid)
        {
            var model = new DistributeExpertViewModels();
            var users = GetSysExpertRoleUsers();
            if (users != null && users.Count > 0)
            {
                var dic = new Dictionary<int, string>();
                foreach (var item in users)
                {
                    dic.Add(item.Id, item.UserDisplayName);
                }

                model.Experts = dic;
            }

            model.Id = id;
            model.PID = pid;

            return View(model);
        }

        [HttpPost]
        public ActionResult Distribute(DistributeExpertViewModels model)
        {
            ControllerResult result = ControllerResult.SuccResult;

            try
            {
                var zjsp1 = string.Empty;
                var zjsp2 = string.Empty;
                var experts = model.DistributedExpert.Split(',');
                if(experts.Count() == 1)
                {
                    zjsp1 = experts[0];
                }
                else if (experts.Count() == 2)
                {
                    zjsp1 = experts[0];
                    zjsp2 = experts[1];
                }

                var ude = new t_D_UserTableTen()
                {
                    id = model.Id,
                    pid = model.PID,
                    zjsp1 = zjsp1,
                    zjsp2 = zjsp2,
                    @static = 2
                };

                string errorMsg = string.Empty;
                if (!checkQualifyService.SaveDistributeExpert(ude, out errorMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = errorMsg;
                }
                else
                {
                    LogUserAction("对检测机构资质Id：{0} 分配专家：[{1}]".Fmt(model.Id, model.DistributedExpert));
                }
            }
            catch(Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
            }

            return Content(result.ToJson());
        }




    }
}