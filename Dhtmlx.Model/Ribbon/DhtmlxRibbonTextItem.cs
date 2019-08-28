using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dhtmlx.Model.Ribbon
{
    public class DhtmlxRibbonTextItem : DhtmlxRibbonItemWithIDText
    {
        public bool  IsBig { get; set; }

        public DhtmlxRibbonTextItem(string id, string text) : base(id, text)
        {
            IsBig = false;
        }

        public override string Type
        {
            get
            {
                return "text";
            }
        }

        public override XElement BuildDhtmlXml()
        {
            return new XElement("item",
                new XAttribute("id", id),
                new XAttribute("text", text),
                new XAttribute("type", Type),
                IsBig == false ? null : new XAttribute("isbig", IsBig));
        }
    }
}
