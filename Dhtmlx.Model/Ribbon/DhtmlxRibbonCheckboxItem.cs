using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dhtmlx.Model.Ribbon
{
    public class DhtmlxRibbonCheckboxItem : DhtmlxRibbonItemWithIDText
    {
        bool isChecked;
        bool disabled;
        int width;

        public DhtmlxRibbonCheckboxItem(string id, string text) : base(id, text)
        {
            isChecked = false;
            disabled = false;
            width = 0;
        }

        public bool IsChecked
        {
            set
            {
                isChecked = value;
            }
        }

        public bool Disabled
        {
            set
            {
                disabled = value;
            }
        }

        public int Width
        {
            set
            {
                width = value;
            }
        }

        public override string Type
        {
            get
            {
                return "checkbox";
            }
        }

        public override XElement BuildDhtmlXml()
        {
            return new XElement("item",
                new XAttribute("id", id),
                new XAttribute("text", text),
                new XAttribute("type", Type),
                isChecked ? new XAttribute("checked", isChecked) : null,
                disabled ? new XAttribute("disable", disabled) : null,
                width > 0 ? new XAttribute("width", width) : null);
        }
    }
}
