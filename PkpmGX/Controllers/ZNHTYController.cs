using Dhtmlx.Model.Grid;
using Pkpm.Core.CheckUnitCore;
using Pkpm.Core.UserRoleCore;
using Pkpm.Entity;
using Pkpm.Framework.Repsitory;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceStack;
using Pkpm.Framework.Common;
using Pkpm.Core;
using PkpmGX.Models;
using Pkpm.Core.SysDictCore;
using PkpmGX.Architecture;
using Pkpm.Core.HtyService;

namespace Pkpm.Web.Controllers
{
    public class ZNHTYController : PkpmController
    {
        IRepsitory<t_hty_Image> imgRep;
        ICheckUnitService checkUnitServce;
        IRepsitory<tab_hty_programme> HTYRep;
        IRepsitory<tab_hty_gjcq> gjcqRep;
        IRepsitory<t_bp_People> peopleRep;
        IRepsitory<t_bp_Equipment> equipRep;
        IRepsitory<t_bp_project> projectRep;
        ISysDictService sysDictService;
        IHtyService zjSvc;
        public ZNHTYController(IHtyService zjSvc, ICheckUnitService checkUnitServce, IRepsitory<tab_hty_programme> HTYRep, IRepsitory<t_bp_People> peopleRep, IRepsitory<t_bp_Equipment> equipRep, IRepsitory<t_bp_project> projectRep, IRepsitory<tab_hty_gjcq> gjcqRep, ISysDictService sysDictService, IRepsitory<t_hty_Image> imgRep, IUserService userSvc) : base(userSvc)
        {
            this.HTYRep = HTYRep;
            this.peopleRep = peopleRep;
            this.checkUnitServce = checkUnitServce;
            this.equipRep = equipRep;
            this.projectRep = projectRep;
            this.zjSvc = zjSvc;
            this.gjcqRep = gjcqRep;
            this.sysDictService = sysDictService;
            this.imgRep = imgRep;
        }
        // GET: ZNHTY
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(int id)
        {
            ZNHTYFangAnViewModel model = new ZNHTYFangAnViewModel();
            var progData = HTYRep.GetById(id);
            if (!string.IsNullOrEmpty(progData.filepath))
            {
                progData.filepath = SymCryptoUtility.Encrypt(progData.filepath);
            }
            if (!string.IsNullOrEmpty(progData.hqfilepath))
            {
                progData.hqfilepath = SymCryptoUtility.Encrypt(progData.hqfilepath);
            }
            
            model.programme = progData;
            var projects = projectRep.GetByCondition(r => r.PROJECTNUM == model.programme.projectnum);//.GetById(id);
            if (projects != null && projects.Count > 0)
            {
                model.project = projects.First();
                model.PLANSTARTDATE = model.project.PLANSTARTDATE.HasValue ? model.project.PLANSTARTDATE.Value.ToString("yyyy-MM-dd") : string.Empty;
            }
            
            model.date = (model.programme.planstartdate.HasValue ? model.programme.planstartdate.Value.ToString("yyyy-MM-dd") : string.Empty) + " - " + (model.programme.planenddate.HasValue ? model.programme.planenddate.Value.ToString("yyyy-MM-dd") : string.Empty);
            var allCustoms = checkUnitServce.GetAllCheckUnit();
            model.programme.unitcode = checkUnitServce.GetCheckUnitByIdFromAll(allCustoms, model.programme.unitcode);

            var peopleIds = progData.testingpeople;
            if (!peopleIds.IsNullOrEmpty())
            {
                var peoIds = peopleIds.Split(',').ToList();
                var peoples = peopleRep.GetByCondition(w => peoIds.Contains(w.id.ToString()));
                model.people = peoples;
            }
            var equipIds = progData.testingequipment;
            if (!equipIds.IsNullOrEmpty())
            {
                var equIds = equipIds.Split(',').ToList();
                var equips = equipRep.GetByCondition(w => equIds.Contains(w.id.ToString()));
                model.EquipNums = equips.Select(s => s.equnum).Join(",");
            }
            return View(model);
        }

        public ActionResult Search(HTYSerchViewModel searchModel)
        {

            var predicate = PredicateBuilder.True<tab_hty_programme>();
            if (!string.IsNullOrEmpty(searchModel.CheckUnitName))
            {
                predicate = predicate.And(t => t.unitcode == searchModel.CheckUnitName);
            }
            if (!string.IsNullOrEmpty(searchModel.CheckProject))
            {
                predicate = predicate.And(t => t.projectname.Contains(searchModel.CheckProject));
            }


            int pos = searchModel.posStart.HasValue ? searchModel.posStart.Value : 0;
            int count = searchModel.count.HasValue ? searchModel.count.Value : 30;
            PagingOptions<tab_hty_programme> pagingOption = new PagingOptions<tab_hty_programme>(pos, count, u => new { u.id });
            var piles = HTYRep.GetByConditonPage(predicate, pagingOption);

            DhtmlxGrid grid = new DhtmlxGrid();
            string customname = string.Empty;
            grid.AddPaging(pagingOption.TotalItems, pos);
            var allCustoms = checkUnitServce.GetAllCheckUnit();
            // var allunit=
            for (int i = 0; i < piles.Count; i++)
            {
                var pile = piles[i];

                DhtmlxGridRow row = new DhtmlxGridRow(pile.id.ToString());
                row.AddCell((pos + i + 1).ToString());
                string customName = checkUnitServce.GetCheckUnitByIdFromAll(allCustoms, pile.unitcode);// string.Empty;
                row.AddCell(customName);
                row.AddCell(pile.projectname);
                var peopleNames = string.Empty;
                var peopleIds = pile.testingpeople;
                if (!peopleIds.IsNullOrEmpty())
                {
                    var peoIds = peopleIds.Split(',').ToList();
                    var peoples = peopleRep.GetByCondition(w => peoIds.Contains(w.id.ToString()));
                    peopleNames = peoples.Select(s => s.Name).Join("、");
                }
                row.AddCell(peopleNames);
                var equipNames = string.Empty;
                var equipIds = pile.testingequipment;
                if (!equipIds.IsNullOrEmpty())
                {
                    var equIds = equipIds.Split(',').ToList();
                    var equips = equipRep.GetByCondition(w => equIds.Contains(w.id.ToString()));
                    equipNames = equips.Select(s => s.EquName).Join("、");
                }
                row.AddCell(equipNames);
                row.AddCell(pile.checknum);
                row.AddCell(new DhtmlxGridCell("查看", false).AddCellAttribute("title", "查看"));
                grid.AddGridRow(row);
            }

            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }
        public ActionResult download(string filepath, string fileName)
        {
            var path=SymCryptoUtility.Decrypt(filepath);
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            string mimeType = MimeMapping.GetMimeMapping(fileName);

            return File(bytes, mimeType);

        }

        public ActionResult gjIndex()
        {
            return View();
        }

        public ActionResult gjcqDetails(int gjid)
        {
            HTYGjSearchModel model = new HTYGjSearchModel { gjid = gjid };
            var Datas = zjSvc.GetHTYGjData(model);
            var SubDatas = gjcqRep.GetByCondition(w => w.gjid == gjid);
            HTYGjcqDetailViewModel viewModel = new HTYGjcqDetailViewModel();
            if (Datas != null && Datas.Count > 0)
            {
                var Data = Datas.First();
                var tjjdSysDict = sysDictService.GetDictsByKey("TJJD");
                var jzmSysDict = sysDictService.GetDictsByKey("JZM");
                var bsfsSysDict = sysDictService.GetDictsByKey("BSFS");
                HTYGjDTOViewModel Dto = Data.ConvertTo<HTYGjDTOViewModel>();
                if (Data.tjjd.HasValue)
                {
                    Dto.tjjd = SysDictUtility.GetKeyFromDic(tjjdSysDict, Data.tjjd.ToString());
                }
                if (Data.jzm.HasValue)
                {
                    Dto.jzm = SysDictUtility.GetKeyFromDic(jzmSysDict, Data.jzm.ToString());
                }
                if (Data.bsfs.HasValue)
                {
                    Dto.bsfs = SysDictUtility.GetKeyFromDic(bsfsSysDict, Data.bsfs.ToString());
                }
                viewModel.Data = Dto;
                viewModel.SubDatas = SubDatas;
            }

            return View(viewModel);
        }

        public ActionResult Searchgj(HTYGjSearchModel model)
        {
            int pos = model.posStart.HasValue ? model.posStart.Value : 0;
            int count = model.count.HasValue ? model.count.Value : 30;
            int pageSize = pos * count;

            DhtmlxGrid grid = new DhtmlxGrid();
            var Datas = zjSvc.GetHTYGjData(model);

            if (Datas.Count > 0)
            {
                var tjjdSysDict = sysDictService.GetDictsByKey("TJJD");
                var jzmSysDict = sysDictService.GetDictsByKey("JZM");
                var bsfsSysDict = sysDictService.GetDictsByKey("BSFS");

                var allInsts = checkUnitServce.GetAllCheckUnit();
                int index = 0;
                foreach (var item in Datas)
                {
                    DhtmlxGridRow row = new DhtmlxGridRow(item.id.ToString());
                    //row.AddCell("/ZNHTY/SearchGjcq?gjId={0}".Fmt(item.id));
                    row.AddCell((pageSize + (++index)).ToString());
                    row.AddCell(checkUnitServce.GetCheckUnitByIdFromAll(allInsts, item.unitcode));
                    row.AddCell(item.PROJECTNAME);
                    row.AddCell(item.checknum);
                    row.AddCell(item.gjNo);
                    row.AddCell(item.gjName);
                    row.AddCell(item.gjcqNum);
                    if (item.tjjd.HasValue)
                    {
                        row.AddCell(SysDictUtility.GetKeyFromDic(tjjdSysDict, item.tjjd.ToString()));
                    }
                    else
                    {
                        row.AddCell("");
                    }
                    if (item.jzm.HasValue)
                    {
                        row.AddCell(SysDictUtility.GetKeyFromDic(jzmSysDict, item.jzm.ToString()));
                    }
                    else
                    {
                        row.AddCell("");
                    }
                    if (item.bsfs.HasValue)
                    {
                        row.AddCell(SysDictUtility.GetKeyFromDic(bsfsSysDict, item.bsfs.ToString()));
                    }
                    else
                    {
                        row.AddCell("");
                    }

                    row.AddCell(item.cqqxNo);
                    row.AddCell(item.thms.HasValue ? item.thms.Value.ToString() : "");
                    row.AddCell(GetUIDtString(item.checkTime));
                    row.AddCell(item.htyNo);
                    var imgcount = item.imgcount.HasValue ? item.imgcount.Value : 0;
                    if (imgcount > 0)
                    {
                        row.AddLinkJsCell(imgcount.ToString(), "showimg(\"{0}\",\"{1}\")".Fmt(item.checknum, item.gjNo));
                    }
                    else
                    {
                        row.AddCell("0");
                    }

                    row.AddCell(new DhtmlxGridCell("编辑", false).AddCellAttribute("title", "编辑"));
                    grid.AddGridRow(row);
                }
            }
            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }

        public ActionResult SearchGjcq(int gjId)
        {
            var gjcqDatas = gjcqRep.GetByCondition(w => w.gjid == gjId);
            DhtmlxGrid grid = new DhtmlxGrid();
            if (gjcqDatas.Count > 0)
            {
                int index = 0;
                foreach (var item in gjcqDatas)
                {
                    DhtmlxGridRow row = new DhtmlxGridRow(Guid.NewGuid().ToString());

                    row.AddCell((++index).ToString());
                    row.AddCell(item.cqno);
                    row.AddLinkJsCell("详情", "gjcqView(\"{0}\",\"{1}\")".Fmt(item.gjid, item.id));
                    grid.AddGridRow(row);
                }
                grid.Header = new DhtmlxGridHeader();

                grid.Header.AddColumn(new DhtmlxGridColumn("测区") { ColumnType = "ro", Width = "*", ColumnSort = "str" });
                grid.Header.AddColumn(new DhtmlxGridColumn("") { ColumnType = "jslink", Width = "150", ColumnSort = "str" });
            }
            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }

        [HttpPost]
        public ActionResult GetHTYImages(string checknum, string gjno)
        {
            var data = imgRep.GetByCondition(w => w.CheckNum == checknum && w.gjNo == gjno);
            return Content(data.ToJson());
        }

        public ActionResult BuildImage(string ImagePath)
        {
            byte[] ImgData = System.IO.File.ReadAllBytes(ImagePath); 
            string imageFileName = "image.jpg";
            string contentType = MimeMapping.GetMimeMapping(imageFileName);
            return new FileContentResult(ImgData, contentType);
        }
    }
}