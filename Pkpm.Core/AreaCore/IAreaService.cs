using Pkpm.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Core.AreaCore
{
    public interface IAreaService
    {
        Dictionary<string, string> GetAllDictArea();
        List<AreaModel> GetTreeArea();
        List<t_bp_area> GetAllArea();
        string GetAreasByArea(string Area);
    }
}
