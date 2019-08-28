using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dhtmlx.Model.Grid
{
   public class DhtmlxGridSettings
    {
        string colWidth;
        public DhtmlxGridSettings()
        {
            colWidth = string.Empty;
        }

        public string ColmnWidth
        {
            set
            {
                colWidth = value;
            }
        }

        public XElement BuildDhtmlXml()
        {
            return new XElement("settings",
                new XElement("colwidth", colWidth));
        }
    }
}
