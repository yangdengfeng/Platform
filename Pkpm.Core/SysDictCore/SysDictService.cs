using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pkpm.Entity;
using Pkpm.Framework.Repsitory;
using Pkpm.Framework.Cache;
using ServiceStack; 

namespace Pkpm.Core.SysDictCore
{
    public class SysDictService : ISysDictService
    {
        IRepsitory<SysDict> rep;
        ICache<List<SysDict>> cache;

        public SysDictService(IRepsitory<SysDict> rep, ICache<List<SysDict>> cache)
        {
            this.rep = rep;
            this.cache = cache;
        }

        public List<SysDict> GetDictsByKey(string key)
        {
            string cachekey = PkPmCacheKeys.SysDictByKeyFmt.Fmt(key);
            var dictCategory = cache.Get(cachekey);
            if (dictCategory != null)
            {
                return dictCategory;
            }
            else
            {
                List<SysDict> result = new List<SysDict>();
                dictCategory = rep.GetByCondition(r => r.KeyValue == key && r.CategoryId == -1 && r.Status == 1);
                if (dictCategory != null && dictCategory.Count > 0)
                {
                    result = rep.GetByConditionSort(r => r.CategoryId == dictCategory[0].Id, new SortingOptions<SysDict>(r => new { r.OrderNo }));
                }
                cache.Remove(cachekey);
                cache.Add(cachekey, result);
                return result;
            }
        }

        public string GetDictsByKey(string key , string value)
        {
            if(string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            var dictCategory = GetDictsByKey(key);  
            if (dictCategory != null && dictCategory.Count > 0)
            {
                var valueSysDic = dictCategory.Where(s => s.KeyValue == value).FirstNonDefault();
                if(valueSysDic!=null)
                {
                    return valueSysDic.Name;
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        public string GetDictsByName(string key, string value)
        {
            var dictCategory = GetDictsByKey(key);  
            if (dictCategory != null && dictCategory.Count > 0)
            { 
                var valueSysDic = dictCategory.Where(s => s.Name == value).FirstNonDefault();
                if (valueSysDic != null)
                {
                    return valueSysDic.KeyValue;
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        public bool AddSysDict(SysDict dict, out string errorMsg)
        {
            errorMsg = string.Empty;
            try
            {
                long id = rep.Insert(dict);
                return true;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }

        public bool EditSysDict(SysDict dict, out string errorMsg)
        {
            errorMsg = string.Empty;
            try
            {
                int editCount = rep.Update(dict);
                return true;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }

        public bool DeleteSysDict(int id, out string errorMsg)
        {
            errorMsg = string.Empty;
            try
            {
                int deleteCount = rep.DeleteByCondition(d => d.Id == id || d.CategoryId == id);
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
