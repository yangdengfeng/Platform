using Pkpm.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Core.SysDictCore
{
   public interface ISysDictService
    {
        List<SysDict> GetDictsByKey(string key);
        string GetDictsByKey(string key, string value);
        string GetDictsByName(string key, string value);

        bool AddSysDict(SysDict dict, out string errorMsg);
        bool EditSysDict(SysDict dict, out string errorMsg);
        bool DeleteSysDict(int id, out string errorMsg);
    }
}
