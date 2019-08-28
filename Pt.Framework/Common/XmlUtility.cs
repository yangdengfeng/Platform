using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Pkpm.Framework.Common
{
    public class XmlUtility
    {
        private static string XmlFileName = string.Empty;

        public static XmlNode GetEntrustXmlTopNode(string FileName,string TopNodeName, string ConfigType)
        {
            XmlNode node = null;
            if (!string.IsNullOrEmpty(FileName))
            {
                XmlFileName = FileName;
            }
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(XmlFileName);
                node = doc.SelectSingleNode(TopNodeName).SelectSingleNode(ConfigType);
            }
            catch (Exception)
            {

            }
            return node;
        }
    }
}
