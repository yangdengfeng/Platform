using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dhtmlx.Model.Ribbon
{
    public class DhtmlxRibbonInputItem : DhtmlxRibbonItemWithIDText
    {
        public string Img { get; set; }
        public string Imgdis { get; set; }
        public bool Disabled { get; set; }
        public string  value { get; set; }


        public DhtmlxRibbonInputItem(string id, string text) : base(id, text)
        {
        }

        public override string Type
        {
            get
            {
                return "input";
            }
        }

        public override XElement BuildDhtmlXml()
        {
            return new XElement("item",
                new XAttribute("type", Type),
                new XAttribute("id", id),
                new XAttribute("text", text),
                string.IsNullOrWhiteSpace(Img) ? null : new XAttribute("img", Img),
                string.IsNullOrWhiteSpace(Imgdis) ? null : new XAttribute("imgdis", Imgdis),
                Disabled ? new XAttribute("disabled", Disabled) : null,
                string.IsNullOrWhiteSpace(value) ? null : new XAttribute("value", value));
        }
    }
}
