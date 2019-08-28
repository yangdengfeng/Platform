using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dhtmlx.Model.Ribbon
{
    public class DhtmlxRibbonButtonTwoStateItem : DhtmlxRibbonItemWithIDText
    {
        public string Img { get; set; }
        public string ImgDis { get; set; }
        public bool IsBig { get; set; }
        public bool Disabled { get; set; }
        public bool State { get; set; }

        public DhtmlxRibbonButtonTwoStateItem(string id, string text) : base(id, text)
        {
        }

        public override string Type
        {
            get
            {
                return "buttonTwoState";
            }
        }

        public override XElement BuildDhtmlXml()
        {
            return new XElement("item",
                new XAttribute("id", id),
                new XAttribute("text", text),
                new XAttribute("type", Type),
                string.IsNullOrWhiteSpace(Img) ? null : new XAttribute("img", Img),
                string.IsNullOrWhiteSpace(ImgDis) ? null : new XAttribute("imgdis", ImgDis),
                IsBig == false ? null : new XAttribute("isbig", IsBig),
                Disabled == false ? null : new XAttribute("disable", Disabled),
                State == false ? null : new XAttribute("state", State));
        }
    }
}
