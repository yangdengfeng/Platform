using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dhtmlx.Model.Menu
{
   public class DhtmlxMenu
    {
        List<DhtmlxMenuItem> items;

        public DhtmlxMenu()
        {
            items = new List<Menu.DhtmlxMenuItem>();
        }

        public void AddItem(DhtmlxMenuItem item)
        {
            items.Add(item);
        }

        public XElement BuildDhtmlXml()
        {
            return new XElement("menu",
                  from i in items
                  select i.BuildDhtmlXml());
        }
    }
}
