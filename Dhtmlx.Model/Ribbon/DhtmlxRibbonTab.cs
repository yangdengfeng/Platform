using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dhtmlx.Model.Ribbon
{
    public class DhtmlxRibbonTab: DhtmlxRibbonItem
    {
        string id;
        string text;
        List<DhtmlxRibbonItem> items;

        public bool Active { get; set; }

        public DhtmlxRibbonTab(string id, string text)
        {
            this.id = id;
            this.text = text;
            items = new List<Ribbon.DhtmlxRibbonItem>();
        }

        public override string Type
        {
            get
            {
                return string.Empty;
            }
        }

        public void AddRibbonItem(DhtmlxRibbonItem item)
        {
            items.Add(item);
        }

        public override XElement BuildDhtmlXml()
        {
            return new XElement("tab",
                new XAttribute("id", id),
                new XAttribute("text", text),
                Active ? new XAttribute("active", Active) : null,
                from i in items
                select i.BuildDhtmlXml());
        }
    }
}
