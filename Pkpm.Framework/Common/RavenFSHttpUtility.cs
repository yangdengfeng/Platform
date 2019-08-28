using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Framework.Common
{
    public static class RavenFSHttpUtility
    {
        public static bool UploadFile(string uploadUrl, Stream stream, out string errorMsg)
        {
            errorMsg = string.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uploadUrl);

            try
            {
                request.Method = "PUT";
                request.AllowWriteStreamBuffering = true; ;
                request.SendChunked = true;
                request.KeepAlive = false;
                using (Stream reqStream = request.GetRequestStream())
                {
                    stream.CopyTo(reqStream);
                }


                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created)
                {
                    errorMsg = string.Format("{0}-[StatusCode:{1}-StatusDescription:{2}]", "上传失败", response.StatusCode, response.StatusDescription);
                    return false;
                }
            }
            catch (Exception ex)
            {
                errorMsg = string.Format("[ErrorMsg:{0}]-[StackTrace:{1}]-[Source:{2}]", ex.Message, ex.StackTrace, ex.Source);
                return false;
            }
            return true;
        }

        public static byte[] DownloadFile(string fileUrl)
        {
            try
            {
                WebClient wc = new WebClient();
                wc.Proxy = null;
                return wc.DownloadData(fileUrl);
            }
           catch
            {
                return null;
            }        
        }
    }
}
