using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Core.SoftwareVersService
{
    public interface ISoftwareVersService
    {
        bool EditSoftwareVers(int id, string userCode, string userName, string FileVersionDate, string FileVersion, string EndDate, out string errorMsg);
    }
}
