using Dhtmlx.Model.Grid;
using Dhtmlx.Model.TreeView;
using Newtonsoft.Json;
using Pkpm.Core.CheckUnitCore;
using Pkpm.Core.UserRoleCore;
using Pkpm.Framework.PkpmConfigService;
using PkpmGX.Architecture;
using PkpmGX.Models;
using QZWebService.ServiceModel;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace PkpmGX.Controllers
{
    //现场检测数据
    [Authorize]
    public class ZJSceneCheckController : PkpmController
    {
        IPkpmConfigService pkpmconfigService;
        ICheckUnitService checkUnitService;
        string GetSceneDataUrl;
        public ZJSceneCheckController(IPkpmConfigService pkpmconfigService, ICheckUnitService checkUnitService, IUserService userService) : base(userService)
        {
            this.pkpmconfigService = pkpmconfigService;
            this.checkUnitService = checkUnitService;
            GetSceneDataUrl = pkpmconfigService.GetSceneDataUrl;
        }

        // GET: ZJSceneCheck
        public ActionResult Index()
        {
            return View();
        }

        // GET: ZJSceneCheck/Details/5
        public ActionResult Details(int id, string checkNum, string pileNo)
        {
            ZJSceneCheckDetailsModel model = new ZJSceneCheckDetailsModel()
            {
                CurrentParam = string.Empty,
                SourceParam = string.Empty,
                GpsInfo = new ZJSceneCheckGpsModel(),
                PhotoInfos = new List<view_pilephoto>(),
                programme = new view_programmePileList(),
                TestLog = string.Empty,
            };

            var client = new JsonServiceClient(GetSceneDataUrl);

            #region  方案信息
            var programmeResponse = client.Get(new TestSiteDetailsDetailsProgrammeInfo()
            {
                CheckNum = checkNum
            });
            if (programmeResponse.IsSucc)
            {
                model.programme = programmeResponse.data;
            }
            #endregion

            #region  获取GPS信息和当前数据，仪器参数信息
            var gpsAndOtherInfoResponse = client.Get(new TestSiteDetailsDetailsDatas()
            {
                BasicInfoId = id
            });
            if (gpsAndOtherInfoResponse.IsSucc)
            {
                model.GpsInfo.IsVaild = gpsAndOtherInfoResponse.gpsisvalid == 1 ? true : false;
                model.GpsInfo.GpsLatitude = gpsAndOtherInfoResponse.gpslatitude;
                model.GpsInfo.GpsLongitude = gpsAndOtherInfoResponse.gpslongitude;

                if (!gpsAndOtherInfoResponse.currentparam.IsNullOrEmpty())
                {
                    DTreeResponse currentParamTree = new DTreeResponse()
                    {
                        data = new List<DTree>()
                    };

                    List<DTree> dtree = BuildDtree("当前参数设定", gpsAndOtherInfoResponse.currentparam, 0);

                    currentParamTree.data = dtree;

                    model.CurrentParam = currentParamTree.data.ToJson();
                }

                if (!gpsAndOtherInfoResponse.sourceparam.IsNullOrEmpty())
                {
                    DTreeResponse sourceParamTree = new DTreeResponse()
                    {
                        status = new Status() { code = 200, message = string.Empty },
                        data = new List<DTree>()
                    };

                    List<DTree> dtree = BuildDtree("原始参数", gpsAndOtherInfoResponse.sourceparam, 1);

                    sourceParamTree.data = dtree;

                    model.SourceParam = sourceParamTree.data.ToJson();
                }
            }
            #endregion

            #region 获取三图五表信息
            var imageAndTableResponse = client.Get(new TestSiteDetailsDetails()
            {
                BasicInfoId = id
            });

            if (imageAndTableResponse.IsSucc)
            {
                model.hzb = imageAndTableResponse.hzb;
                model.ysjlb = imageAndTableResponse.ysjlb;
                model.jzjlb = imageAndTableResponse.jzjlb;
                model.xzjlb = imageAndTableResponse.xzjlb;
                model.xgjlb = imageAndTableResponse.xgjlb;
                model.QsImageBytes = imageAndTableResponse.QsImageBytes;
                model.SlgtImageBytes = imageAndTableResponse.SlgtImageBytes;
                model.SlgQImageBytes = imageAndTableResponse.SlgQImageBytes;
            }
            #endregion

            #region 获取最后的六章图片信息
            var imageResponse = client.Get(new TestSiteDetailsDetailsPhotoInfo()
            {
                BasicInfoId = checkNum,
                PileNo = pileNo
            });

            if (imageResponse.IsSucc)
            {
                model.PhotoInfos = imageResponse.datas;
            }

            #endregion

            #region  获取测试日志信息
            var testLogResponse = client.Get(new TestSiteDetailsDetailsTestLog()
            {
                BasicInfoId = id,
            });

            if (testLogResponse.IsSucc)
            {
                if (testLogResponse.datas.Count() > 0)
                {
                    DTreeResponse TestLogTree = new DTreeResponse()
                    {
                        status = new Status() { code = 200, message = string.Empty },
                        data = new List<DTree>()
                    };

                    var dtree = new List<DTree>();

                    foreach (var item in testLogResponse.datas)
                    {
                        var xmlStr = item.Remark;
                        var rootIndex = 1;

                        var currentData = XElement.Parse(xmlStr);
                        dtree.Add(new DTree()
                        {
                            id = "0" + rootIndex,
                            title = "静载试验日志",
                            parentId = "0"
                        });

                        var timeElement = currentData.Element("时间");

                        dtree.Add(new DTree()
                        {
                            id = "02",
                            title = timeElement.Name + ":" + timeElement.Value,
                            parentId = "0" + rootIndex,
                            last = true
                        });

                        var syElement = currentData.Element("试验进程");
                        if (syElement != null)
                        {
                            dtree.Add(new DTree()
                            {
                                id = "03",
                                title = syElement.Name + ":" + syElement.Value,
                                parentId = "0" + rootIndex,
                                last = true
                            });
                        }

                        var index = 4;
                        foreach (var currentDataitem in currentData.Elements())
                        {
                            if (currentDataitem.Name == "时间" || currentDataitem.Name == "试验进程")
                            {
                                continue;
                            }
                            dtree.Add(new DTree()
                            {
                                id = "0" + index,
                                title = currentDataitem.Name + "",
                                parentId = "0" + rootIndex
                            });//构建除了时间和试验进程以外的同级节点

                            var childIndex = 1;
                            foreach (var childItem in currentDataitem.Elements())
                            {
                                if (childItem.HasElements)
                                {
                                    dtree.Add(new DTree()
                                    {
                                        id = "0" + index + childIndex,
                                        title = childItem.Name + ":",
                                        parentId = "0" + index,
                                    });

                                    var childchildIndex = 1;
                                    foreach (var childchildItem in childItem.Elements())
                                    {
                                        dtree.Add(new DTree()
                                        {
                                            id = "0" + index + childIndex + childchildIndex,
                                            title = childchildItem.Name + ":" + childchildItem.Value,
                                            parentId = "0" + index + childIndex,
                                            last = true
                                        });
                                        childchildIndex = childchildIndex + 1;
                                    }
                                }
                                else
                                {
                                    dtree.Add(new DTree()
                                    {
                                        id = "0" + index + childIndex,
                                        title = childItem.Name + ":" + childItem.Value,
                                        parentId = "0" + index,
                                        last = true
                                    });
                                }
                                childIndex = childIndex + 1;
                            }
                            index = index + 1;
                        }

                        rootIndex = rootIndex + 1;
                    }

                    TestLogTree.data = dtree;

                    model.TestLog = TestLogTree.data.ToJson();

                }
            }
            #endregion

            return View(model);
        }

        /// <summary>
        /// 从XML文件中构建DTree
        /// </summary>
        /// <param name="rootName">根节点名称</param>
        /// <param name="currentParam">当前XML文件</param>
        /// <param name="type">当前类型，暂时只有3种 0 当前数据 1 仪器参数  </param>
        /// <returns></returns>
        private static List<DTree> BuildDtree(string rootName, string currentParam, int type)
        {
            var currentData = XElement.Parse(currentParam);

            var dtree = new List<DTree>();

            dtree.Add(new DTree()
            {
                id = "01",
                title = rootName,
                parentId = "0"
            });

            if (type == 0)//当前数据和测试日志下面都有时间
            {
                var timeElement = currentData.Element("时间");

                dtree.Add(new DTree()
                {
                    id = "02",
                    title = timeElement.Name + ":" + timeElement.Value,
                    parentId = "01",
                    last = true
                });
            }
            else if (type == 1)//仪器参数里面没有时间这个选项，应该要去掉
            {

            }


            var index = 4;
            foreach (XElement item in currentData.Elements())
            {
                if (item.Name == "时间")
                {
                    continue;
                }
                dtree.Add(new DTree()
                {
                    id = "0" + index.ToString(),
                    title = item.Name + "",
                    parentId = "01"
                });
                var childIndex = 1;
                foreach (var childItem in item.Elements())
                {
                    dtree.Add(new DTree()
                    {
                        id = "0" + index.ToString() + childIndex++.ToString(),
                        title = childItem.Name + ":" + childItem.Value,
                        parentId = "0" + index.ToString(),
                        last = true
                    });
                }
                index = index + 1;
            }

            return dtree;
        }

        // GET: ZJSceneCheck/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ZJSceneCheck/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ZJSceneCheck/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ZJSceneCheck/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ZJSceneCheck/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ZJSceneCheck/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Search(ZJSceneCheckSearchModel model)
        {
            DhtmlxGrid grid = new DhtmlxGrid();

            var client = new JsonServiceClient(GetSceneDataUrl);

            var response = client.Get(new SenceSiteData()
            {
                customId = model.customId,
                projectName = model.projectname,
                testingpeople = model.testpeople,
                testtype = model.testtype,
                testingequip = model.testpeople,
                areainfo = model.areainfo,
                piletype = model.piletype,
                posStart = model.posStart,
                count = model.count
            });

            if (response.IsSucc)
            {
                var index = 1;
                int pos = model.posStart.HasValue ? model.posStart.Value : 0;
                grid.AddPaging((int)response.totalCount, pos);
                var allCustom = checkUnitService.GetAllCheckUnit();
                foreach (var item in response.datas)
                {
                    List<string> peopleName = new List<string>();
                    if (!item.testingpeople.IsNullOrEmpty())
                    {
                        var Peoples = JsonConvert.DeserializeObject<List<LiftingEquipmentPeopleModel>>(item.testingpeople);
                        foreach (var people in Peoples)
                        {
                            peopleName.Add(people.name);
                        }
                    }

                    DhtmlxGridRow row = new DhtmlxGridRow(item.pid.ToString());
                    row.AddCell(index++);
                    row.AddCell(checkUnitService.GetCheckUnitByIdFromAll(allCustom, item.customid));
                    row.AddCell(item.projectname);
                    row.AddCell(item.areainfo);
                    row.AddCell(item.projectaddress);
                    row.AddCell(peopleName.Join(","));
                    row.AddCell(item.testtype);
                    row.AddCell(item.piletype);
                    if (item.num > 0)
                    {
                        row.AddLinkJsCell(item.num.ToString(), "details(\"{0}\",\"{1}\")".Fmt(item.SerialNo, item.customid));
                    }
                    else
                    {
                        row.AddLinkJsCell(string.Empty, string.Empty);
                    }
                    grid.AddGridRow(row);
                }
            }


            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);
            return Content(str, "text/xml");
        }

        public ActionResult GetPileDataList(string checknum, string customid)
        {
            DhtmlxGrid grid = new DhtmlxGrid();

            var client = new JsonServiceClient(GetSceneDataUrl);

            var response = client.Get(new TestSiteDetails()
            {
                CheckNum = checknum,
                CustomId = customid,
                IsTesting = 1
            });

            if (response.IsSucc)
            {
                foreach (var item in response.datas)
                {
                    DhtmlxGridRow row = new DhtmlxGridRow(item.Id);
                    row.AddCell(item.projectname);
                    row.AddCell(item.TestType);
                    row.AddCell(item.PileNo);
                    row.AddCell(GetUIDtString(item.StartTime));
                    row.AddCell(GetUIDtString(item.CreateTime));
                    row.AddCell(string.Empty);
                    row.AddCell(item.MachineId);
                    row.AddCell(item.GpsIsValid == 1 ? new DhtmlxGridCell("已定位", false).AddCellAttribute("style", "color:green") : new DhtmlxGridCell("未定位", false).AddCellAttribute("style", "color:red"));
                    row.AddCell(string.Empty);//修改记录找不到字段和判断方法，先设置为空
                    if (item.nn > 0)
                    {
                        if (item.nn >= 6)
                        {
                            row.AddCell(new DhtmlxGridCell("有照片", false).AddCellAttribute("style", "color:green"));
                        }
                        else
                        {
                            row.AddCell(new DhtmlxGridCell("不完整({0})".Fmt(item.nn), false).AddCellAttribute("style", "color:red"));
                        }
                    }
                    else
                    {
                        row.AddCell(new DhtmlxGridCell("无照片", false).AddCellAttribute("style", "color:red"));
                    }
                    row.AddLinkJsCell("查看", "detailsUpload(\"{0}\",\"{1}\",\"{2}\")".Fmt(item.Id, item.checknum, item.PileNo));

                    grid.AddGridRow(row);
                }
            }

            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);
            return Content(str, "text/xml");
        }
    }
}
