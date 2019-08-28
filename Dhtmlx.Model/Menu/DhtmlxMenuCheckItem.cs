using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dhtmlx.Model.Menu
{
    public class DhtmlxMenuCheckItem : DhtmlxMenuItem
    {
        string id;
        string text;
        bool isChecked;
        bool enabled;

        public bool IsChecked
        {
            set
            {
                isChecked = value;
            }
        }

        public bool Enabled
        {
            set
            {
                enabled = value;
            }
        }

        public DhtmlxMenuCheckItem(string id,string text)
        {
            this.id = id;
            this.text = text;
            isChecked = false;
            enabled = true;
        }

        public override XElement BuildDhtmlXml()
        {
            return new XElement("item",
                new XAttribute("id", id),
                new XAttribute("text", text),
                new XAttribute("type", "checkbox"),
                isChecked ? new XAttribute("checked", isChecked) : null,
                enabled ? null : new XAttribute("enabled", enabled));
        }
    }
}
