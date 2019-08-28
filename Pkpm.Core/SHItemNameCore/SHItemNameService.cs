using Pkpm.Entity;
using Pkpm.Framework.Cache;
using Pkpm.Framework.Repsitory;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Core.SHItemNameCore
{
   public  class SHItemNameService:ISHItemNameService
    {

        IRepsitory<TotalItem> totalItemRep;
        ICache<ItemShortInfos> cacheItem;
        IDbConnectionFactory dbFactory;
        public SHItemNameService(IRepsitory<TotalItem> totalItemRep, ICache<ItemShortInfos> cacheItem, IDbConnectionFactory dbFactory)
        {
            this.totalItemRep = totalItemRep;
            this.cacheItem = cacheItem;
            this.dbFactory = dbFactory;
        }


        public ItemShortInfos GetAllSHItemName()
        {
            var data = cacheItem.Get(PkPmCacheKeys.AllSHItemName);
            if (data == null)
            {
                using (var db = dbFactory.Open())
                {
                    //var dicData = db.Dictionary<string, string>("   select itemTableName,itemChName from uv_t_bp_itemList_view");
                    var allItems = totalItemRep.GetByCondition(r => true);

                    Dictionary<string, string> dicData = new Dictionary<string, string>();
                    foreach (var item in allItems)
                    {
                        string itemKey = item.ITEMTABLENAME;
                        dicData[itemKey] = item.ITEMCHNAME;
                    }

                    data = ItemShortInfos.FromDictonary(dicData);
                }
                cacheItem.Put(PkPmCacheKeys.AllSHItemName, data);
            }
            return data;
        }


        public string GetItemSHNameFromAll(Dictionary<string, string> data, string itemCode)
        {
            string itemName = string.Empty;
            if (data.IsNullOrEmpty() || itemCode.IsNullOrEmpty())
            {
                return string.Empty;
            }

            if (!data.TryGetValue(itemCode, out itemName))
            {
                itemName = string.Empty;
            }

            return itemName;
        }


    }
}
