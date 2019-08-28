using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dhtmlx.Model.Ribbon
{
   public class DhtmlxRibbon
    {
        List<DhtmlxRibbonItem> items;

        public DhtmlxRibbon()
        {
            items = new List<Ribbon.DhtmlxRibbonItem>();
        }

        public void AddRibbonItem(DhtmlxRibbonItem item)
        {
            items.Add(item);
        }

        public XElement BuildDhtmlXml()
        {
            return new XElement("ribbon",
                from i in items
                select i.BuildDhtmlXml());
        }
    }
}
