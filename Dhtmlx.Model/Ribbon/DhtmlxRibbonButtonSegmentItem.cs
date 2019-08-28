using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dhtmlx.Model.Ribbon
{
    public class DhtmlxRibbonButtonSegmentItem : DhtmlxRibbonItemWithIDText
    {
        public string Img { get; set; }
        public string Imgdis { get; set; }
        public bool IsBig { get; set; }
        public bool Disabled { get; set; }
        public bool State { get; set; }

        public DhtmlxRibbonButtonSegmentItem(string id, string text) : base(id, text)
        {
        }

        public override string Type
        {
            get
            {
                return "buttonSegment";
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
                IsBig ? new XAttribute("isbig", IsBig) : null,
                State ? new XAttribute("state", State) : null);
        }
    }
}
