using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dhtmlx.Model.Ribbon
{
    public class DhtmlxRibbonGroupItem : DhtmlxRibbonItem
    {
        string id;
        bool disabled;
        List<DhtmlxRibbonItem> items;

        public DhtmlxRibbonGroupItem(string id)
        {
            this.id = id;
            disabled = false;
            items = new List<Ribbon.DhtmlxRibbonItem>();
        }

        public bool Disabled
        {
            set
            {
                disabled = value;
            }
        }

        public override string Type
        {
            get
            {
                return "group";
            }
        }

        public void AddRibbonItem(DhtmlxRibbonItem item)
        {
            items.Add(item);
        }

        public override XElement BuildDhtmlXml()
        {
            return new XElement("item",
                new XAttribute("type", Type),
                new XAttribute("id", id),
                disabled ? new XAttribute("diabled", disabled) : null,
                from i in items
                select i.BuildDhtmlXml());
        }
    }
}
