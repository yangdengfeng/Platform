using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dhtmlx.Model.Ribbon
{
    public class DhtmlxRibbonSilderItem : DhtmlxRibbonItemWithIDText
    {
        public int Size { get; set; }
        public bool Vertical { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public int Value { get; set; }
        public int Step { get; set; }
        public bool Disabled { get; set; }
        public bool EnableTooltip { get; set; }

        public DhtmlxRibbonSilderItem(string id, string text) : base(id, text)
        {
            Min = 0;
            Max = 99;
            Step = 1;
            Value = 0;
            Vertical = false;
            Disabled = false;
            EnableTooltip = false;
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
                new XAttribute("text", text),
                new XAttribute("type", Type),
                new XAttribute("size", Size),
                Vertical ? new XAttribute("vertical", true) : null,
                Min == 0 ? null : new XAttribute("min", Min),
                Max == 99 ? null : new XAttribute("max", Max),
                Step == 1 ? null : new XAttribute("step", Step),
                Disabled == false ? null : new XAttribute("disabled", Disabled),
                EnableTooltip == false ? null : new XAttribute("enableTooltip", EnableTooltip));
        }
    }
}
