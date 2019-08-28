using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Core.ApplyQualifySixCore
{
    public interface IApplyQualifyService
    {
        bool ImportPeople(List<string> strs, string unitcode, string pid, out string errorMsg);
        bool Delete(string id, out string errorMsg);
    }
}
