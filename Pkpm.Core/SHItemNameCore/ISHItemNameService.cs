using Pkpm.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Core.SHItemNameCore
{
   public  interface ISHItemNameService
    {
        ItemShortInfos GetAllSHItemName();
        string GetItemSHNameFromAll(Dictionary<string, string> data, string itemCode);
    }
}
