using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dhtmlx.Model.Toolbar
{
    public class DhtmlxToolbarSliderItem : DhtmlxToolbarItem
    {
        string id;
        int length;
        int minValue;
        int maxValue;
        int valueNow;

        string texMin;
        string textMax;
        string toolTip;

        public DhtmlxToolbarSliderItem(string id,int length,
            int minValue,int maxValue,int valueNow)
        {
            this.id = id;
            this.length = length;
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.valueNow = valueNow;
        }

        public string TextMin
        {
            set
            {
                texMin = value;
            }
        }

        public string TextMax
        {
            set
            {
                textMax = value;
            }
        }

        public string ToolTip
        {
            set
            {
                toolTip = value;
            }
        }

        public override string Type
        {
            get
            {
                return "slider";
            }
        }

        public override XElement BuildDhtmlXml()
        {
            return new XElement("item",
                new XAttribute("id", id),
                new XAttribute("type", Type),
                new XAttribute("length", length),
                new XAttribute("valueMin", minValue),
                new XAttribute("valueMax", maxValue),
                new XAttribute("valueNow", valueNow),
                string.IsNullOrWhiteSpace(texMin) ? null : new XAttribute("textMin", texMin),
                string.IsNullOrWhiteSpace(textMax) ? null : new XAttribute("textMax", textMax),
                string.IsNullOrWhiteSpace(toolTip) ? null : new XAttribute("toolTip", toolTip));
        }
    }
}
