using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dhtmlx.Model.Sidebar
{
    public class DhtmlxSidebarSeparator : DhtmlxSidebarBaseItem
    {
        public override XElement BuildDhtmlXml()
        {
            return new XElement("item",
                new XAttribute("type", "separator"));
        }
    }
}
