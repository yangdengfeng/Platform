using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dhtmlx.Model.Menu
{
    public class DhtmlMenuRadioItem : DhtmlxMenuItem
    {
        string id;
        string text;
        string group;
        bool isChecked;
        bool enabled;

        public DhtmlMenuRadioItem(string id,string text)
        {
            this.id = id;
            this.text = text;
            group = string.Empty;
            isChecked = false;
            enabled = true;
        }

        public string Group
        {
            set
            {
                group = value;
            }
        }

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

        public override XElement BuildDhtmlXml()
        {
            return new XElement("item",
                new XAttribute("id", id),
                new XAttribute("text", text),
                new XAttribute("type", "radio"),
                string.IsNullOrWhiteSpace(group) ? null : new XAttribute("group", group),
                isChecked ? null : new XAttribute("checked", isChecked),
                enabled ? null : new XAttribute("enabled", enabled));
        }
    }
}
