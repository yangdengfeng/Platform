using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dhtmlx.Model.Sidebar
{
    public class DhtmlxSidebarItem : DhtmlxSidebarBaseItem
    {
        public string Icon { get; set; }
        public bool IsSelected { get; set; }

        string id;
        string text;
        string bubble;

        public DhtmlxSidebarItem(string id, string text, string bubble)
        {
            this.id = id;
            this.text = text;
            this.bubble = bubble;
        }

        public override XElement BuildDhtmlXml()
        {
            return new XElement("item",
                  new XAttribute("id", id),
                  new XAttribute("text", text),
                  string.IsNullOrWhiteSpace(bubble) ? null : new XAttribute("bubble", bubble),
                  string.IsNullOrWhiteSpace(Icon) ? null : new XAttribute("icon", Icon),
                  IsSelected == false ? null : new XAttribute("selected", IsSelected));
        }

    }
}
