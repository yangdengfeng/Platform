using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dhtmlx.Model.Toolbar
{
    public class DhtmlxToolbarSeparatorItem : DhtmlxToolbarItem
    {
        string id;

        public DhtmlxToolbarSeparatorItem(string id)
        {
            this.id = id;
        }

        public override string Type
        {
            get
            {
                return "separator";
            }
        }

        public override XElement BuildDhtmlXml()
        {
            return new XElement("item",
                new XAttribute("id", id),
                new XAttribute("type", Type));
        }
    }
}
