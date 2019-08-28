using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dhtmlx.Model.Sidebar
{
    public abstract class DhtmlxSidebarBaseItem
    {
        public abstract XElement BuildDhtmlXml();
    }
}
