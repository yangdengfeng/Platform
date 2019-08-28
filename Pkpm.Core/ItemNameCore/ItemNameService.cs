using System;
using System.Collections.Generic;
using System.Linq;
using ServiceStack.Data;
using Pkpm.Framework.Cache;
using ServiceStack.OrmLite;
using ServiceStack;
using Pkpm.Entity;

using Pkpm.Framework.Repsitory;
using System.Text.RegularExpressions;
using Pkpm.Framework.PkpmConfigService;
using Pkpm.Entity.ElasticSearch;
using Nest;

namespace Pkpm.Core.ItemNameCore
{
    public class ItemNameService : IItemNameService
    {
        ICache<ItemShortInfos> cacheItem;
        ICache<List<CheckItemType>> cacheTypeCodes;
        ICache<List<totalitems>> cacheItemCodes;
        IDbConnectionFactory dbFactory;
        IRepsitory<sysitemtype> itemTypeRep;
        IRepsitory<totalitems> totalItemRep;
        IRepsitory<CheckItemType> checkItemTypeRep;
        IPkpmConfigService pkpmConfigService;



        public ItemNameService(ICache<ItemShortInfos> cacheItem,
             ICache<List<CheckItemType>> cacheTypeCodes,
             ICache<List<totalitems>> cacheItemCodes,
             IDbConnectionFactory dbFactory,
             IRepsitory<totalitems> totalItemRep,
             IRepsitory<sysitemtype> itemTypeRep,
             IRepsitory<CheckItemType> checkItemTypeRep,
             IPkpmConfigService pkpmConfigService)
        {
            this.cacheItem = cacheItem;
            this.cacheTypeCodes = cacheTypeCodes;
            this.cacheItemCodes = cacheItemCodes;
            this.dbFactory = dbFactory;
            this.totalItemRep = totalItemRep;
            this.checkItemTypeRep = checkItemTypeRep;
            this.itemTypeRep = itemTypeRep;
            this.pkpmConfigService = pkpmConfigService;
        }

        public ItemShortInfos GetAllItemName()
        {
            var data = cacheItem.Get(PkPmCacheKeys.AllItemName);
            if (data == null)
            {
                using (var db = dbFactory.Open())
                {
                    //var dicData = db.Dictionary<string, string>("   select itemTableName,itemChName from uv_t_bp_itemList_view");
                    var allItems = checkItemTypeRep.GetByCondition(r => true);

                    Dictionary<string, string> dicData = new Dictionary<string, string>();
                    foreach (var item in allItems)
                    {
                        string itemKey = "{0}{1}".Fmt(item.CheckItemName, item.CheckItemCode);
                        if (item.CheckItemCode.Contains("MA"))
                        {
                            dicData[itemKey] = item.CheckItemName + "(房建)(" + item.CheckItemCode + ")";
                        }
                        else if (item.CheckItemCode.Contains("MB"))
                        {
                            dicData[itemKey] = item.CheckItemName + "(市政)(" + item.CheckItemCode + ")";
                        }
                        else if (item.CheckItemCode.Contains("MN"))
                        {
                            dicData[itemKey] = item.CheckItemName + "(现场)(" + item.CheckItemCode + ")";
                        }
                        else
                        {
                            dicData[itemKey] = item.CheckItemName + "(房建专项)(" + item.CheckItemCode + ")";
                        }
                    }

                    data = ItemShortInfos.FromDictonary(dicData);
                }
                cacheItem.Put(PkPmCacheKeys.AllItemName, data);
            }
            return data;
        }

        public bool GetCheckItemChange(string value , out Dictionary<string, string> dicData)
        {
            dicData = new Dictionary<string, string>();
            try
            {
                using (var db = dbFactory.Open())
                {
                    var allItems = totalItemRep.GetByCondition(r => r.typecode == value);

                    foreach (var item in allItems)
                    {
                        string itemKey = "{0}{1}".Fmt(item.typecode, item.itemcode);
                        if (item.typecode.Contains("MA"))
                        {
                            dicData[itemKey] = item.ItemName + "(房建)(" + item.typecode + ")";
                        }
                        else if (item.typecode.Contains("MB"))
                        {
                            dicData[itemKey] = item.ItemName + "(市政)(" + item.typecode + ")";
                        }
                        else if (item.typecode.Contains("MN"))
                        {
                            dicData[itemKey] = item.ItemName + "(现场)(" + item.typecode + ")";
                        }
                        else
                        {
                            dicData[itemKey] = item.ItemName + "(房建专项)(" + item.typecode + ")";
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
           
          
        }

        public string GetItemCNNameFromAll(Dictionary<string, string> data, string typecode, string itemCode)
        {
            string itemName = string.Empty;

            if (data.IsNullOrEmpty() || itemCode.IsNullOrEmpty() || typecode.IsNullOrEmpty())
            {
                return string.Empty;
            }

            string dicKey = "{0}{1}".Fmt(typecode, itemCode);

            if (!data.TryGetValue(dicKey, out itemName))
            {
                itemName = string.Empty;
            }

            return itemName;
        }

        public ItemShortInfos GetAllCategoryName()
        {
            var data = cacheItem.Get(PkPmCacheKeys.AllCategoryName);
            if (data == null)
            {
                using (var db = dbFactory.Open())
                {
                    var q = db.From<CheckItemType>()
                            .Select();
                    var AllItem = db.Select(q);
                    var dicData = new Dictionary<string, string>();
                    foreach (var item in AllItem)
                    {
                        dicData[item.CheckItemCode] = item.CheckItemName + "(" + item.CheckItemCode + ")";
                    }
                    data = ItemShortInfos.FromDictonary(dicData);
                }
                cacheItem.Put(PkPmCacheKeys.AllCategoryName, data);
            }
            return data;
        }


        public bool IsAcsReport(string itemName)
        {
            if (string.IsNullOrWhiteSpace(itemName))
            {
                return false;
            }

            if (pkpmConfigService.AcsTypeItemNames.Contains(itemName))
            {
                return true;
            }

            return false;
        }

        public List<string> GetAcsItemNames()
        {
            return pkpmConfigService.AcsTypeItemNames;// acsTypeItemNames;
        }

        public bool IsCReport(string typecode, string itemcode)
        {
            if (typecode.IsNullOrEmpty() || itemcode.IsNullOrEmpty())
            {
                return false;
            }
            else
            {
                var allReportType = GetAllItemCodes();
                var type = allReportType.Where(r => r.itemcode == itemcode && r.typecode == typecode).FirstOrDefault();
                if (type != null && type.itemtype == "C类")
                {
                    return true;
                }
            }

            return false;
        }

        public List<string> GetAcsPicItemType()
        {
            return pkpmConfigService.AcsPicDataTypes; //acsPicDataTypes;
        }

        public List<string> GetReCheckItemType()
        {
            return pkpmConfigService.RecheckTypeItemNames;// recheckTypeItemNames;
        }

        public bool IsAcsPicItemType(string dataType)
        {
            if (dataType.IsNullOrEmpty())
            {
                return false;
            }
            return pkpmConfigService.AcsPicDataTypes.Contains(dataType);
        }

        public string GetAcsAxisLabel(string itemName)
        {
            if (itemName.IsNullOrEmpty())
            {
                return "N";
            }

            if (pkpmConfigService.AcsAxisNLabel.Contains(itemName))
            {
                return "N";
            }
            else
            {
                return "kN";
            }
        }

        public List<string> GetFixDtItems()
        {
            return pkpmConfigService.FixDtTypes; //fixDtTypes;
        }

        public List<CheckItemType> GetAllTypeCodes()
        {
            var data = cacheTypeCodes.Get(PkPmCacheKeys.CheckItemType);
            if (data != null)
            {
                return data;
            }
            else
            {
                using (var db = dbFactory.Open())
                {
                    var result = db.Select<CheckItemType>(t => true);
                    cacheTypeCodes.Put(PkPmCacheKeys.CheckItemType, result);
                    return result;
                }
            }
        }

        public List<totalitems> GetAllItemCodes()
        {
            var data = cacheItemCodes.Get(PkPmCacheKeys.totalitems);
            if (data != null)
            {
                return data;
            }
            else
            {
                using (var db = dbFactory.Open())
                {
                    var result = db.Select<totalitems>(r => true);//查询部分列
                    cacheItemCodes.Put(PkPmCacheKeys.totalitems, result);
                    return result;
                }
            }
        }

        public List<totalitems> GetItemCodesByTypeCode(string typeCode)
        {
            var allData = GetAllItemCodes();
            return allData.Where(s => s.itemcode != null && s.itemcode.StartsWith(typeCode)).OrderBy(s => s.itemcode).ToList();
        }

        public bool NewCheckItemType(CheckItemType item, out string errorMsg)
        {
            errorMsg = string.Empty;
            bool Itemtypes = false;
            var itemtypes = GetAllTypeCodes();
            foreach (var itemtype in itemtypes)
            {
                if (item.CheckItemCode.Contains(itemtype.CheckItemCode))
                {
                    errorMsg = "已有该类别编号!";
                    return false;
                }
                else
                {
                    Itemtypes = true;
                }
            }
            if (Itemtypes)
            {
                using (var db = dbFactory.Open())
                {
                    try
                    {
                        db.Insert(new CheckItemType { CheckItemCode = item.CheckItemCode, CheckItemName = item.CheckItemName });
                        cacheTypeCodes.Clear();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        errorMsg = ex.Message;
                        return false;
                    }
                }
            }
            else
            {
                return false;

            }
        }

        public bool NewTotalItem(totalitems item, out string errorMsg)
        {
            errorMsg = string.Empty;
            bool ItemNames = false;
            var itemnames = GetAllItemCodes();
            foreach (var itemname in itemnames)
            {
                if (itemname.itemcode != null)
                {
                    if (item.itemcode.Contains(itemname.itemcode))
                    {
                        errorMsg = "已有该检查项目编号!";
                        return false;
                    }
                    else
                    {
                        ItemNames = true;
                    }
                }
            }
            if (ItemNames)
            {
                using (var db = dbFactory.Open())
                {
                    try
                    {
                        errorMsg = string.Empty;
                        db.Insert(new totalitems { typecode = item.typecode, ItemName = item.ItemName, itemtype = item.itemtype, itemcode = item.itemcode });
                        cacheItemCodes.Clear();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        errorMsg = ex.Message;
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }

        }

        public bool EditTotalItem(totalitems item, out string errorMsg)
        {
            errorMsg = string.Empty;
            try
            {
                int updateCount = totalItemRep.UpdateOnly(item, l => l.Id == item.Id, r => new
                {
                    r.ItemName,
                    r.itemcode,
                    r.itemtype
                });
                return true;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }

        }

        public bool DelTotalItemById(int id, out string errorMsg)
        {
            errorMsg = string.Empty;
            try
            {
                int delCount = totalItemRep.DeleteById(id);
                return true;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }

        }

    }
}
