
using Pkpm.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Framework.Common
{
    public static class SysDictUtility
    {
        public static string GetKeyFromDic(List<SysDict> sysDicts, string key, string notFound = "")
        {
            if (sysDicts == null || sysDicts.Count() == 0 || string.IsNullOrWhiteSpace(key))
            {
                return notFound;
            }

            SysDict dict = sysDicts.Where(s => String.Compare(s.KeyValue, key, true) == 0).FirstOrDefault();
            if (dict == null)
            {
                return notFound;
            }
            else
            {
                return dict.Name;
            }
        }
    }
}
