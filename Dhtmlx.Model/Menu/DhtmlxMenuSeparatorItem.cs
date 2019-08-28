using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dhtmlx.Model.Menu
{
    public class DhtmlxMenuSeparatorItem : DhtmlxMenuItem
    {
        string id;

        public DhtmlxMenuSeparatorItem(string id)
        {
            this.id = id;
        }

        public override XElement BuildDhtmlXml()
        {
            return new XElement("item",
                new XAttribute("id", id),
                new XAttribute("type", "separator"));
        }
    }
}
