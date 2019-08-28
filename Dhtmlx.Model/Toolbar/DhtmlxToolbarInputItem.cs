using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dhtmlx.Model.Toolbar
{
    public class DhtmlxToolbarInputItem : DhtmlxToolbarItem
    {
        string id;
        string value;
        int width;
        string title;

        public DhtmlxToolbarInputItem(string id,string value)
        {
            this.id = id;
            this.value = value;
            width = 0;
            title = string.Empty;
        }

        public int Width
        {
            set
            {
                width = value;
            }
        }

        public string Title
        {
            set
            {
                title = value;
            }
        }

        public override string Type
        {
            get
            {
                return "buttonInput";
            }
        }

        public override XElement BuildDhtmlXml()
        {
            return new XElement("item",
                new XAttribute("id", id),
                new XAttribute("type", Type),
                new XAttribute("value", value),
                width <= 0 ? null : new XAttribute("width", width),
                string.IsNullOrWhiteSpace(title) ? null : new XAttribute("title", title));
        }
    }
}
