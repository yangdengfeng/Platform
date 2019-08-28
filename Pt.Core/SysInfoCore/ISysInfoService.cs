using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Core.SysInfoCore
{
    public interface ISysInfoService
    {
        bool EditInfo(int id, string informationName, string Content, string type,out string errorMsg);

        bool CreateInfo(string informationName, string Content, string type, string addTiem, out string errorMsg);

        bool DeleteInfo(int id, out string errorMsg);
    }
}
