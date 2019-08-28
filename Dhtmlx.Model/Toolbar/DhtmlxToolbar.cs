using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dhtmlx.Model.Toolbar
{
   public class DhtmlxToolbar
    {
        List<DhtmlxToolbarItem> items;

        public DhtmlxToolbar()
        {
            items = new List<DhtmlxToolbarItem>();
        }

        public void AddToolbarItem(DhtmlxToolbarItem item)
        {
            items.Add(item);
        }

        public  XElement BuildDhtmlXml()
        {
            return new XElement("toolbar",
                from i in items
                select i.BuildDhtmlXml());
        }
    }
}
