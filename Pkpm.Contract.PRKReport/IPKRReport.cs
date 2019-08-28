using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Contract.PRKReport
{
    [ServiceContract]
    public interface IPKRReport
    {
        [OperationContract]
        byte[] BuildPdfFromPKR(string pkrPath);

        [OperationContract]
        byte[] BuildImageFromPKR(string pkrPath);

        [OperationContract]
        string BuildHtmlImageFromPKR(string pkrPath);

        [OperationContract]
        string BuildHtmlImageFromPKRUrl(string pkrPath, string url);
    }
}
