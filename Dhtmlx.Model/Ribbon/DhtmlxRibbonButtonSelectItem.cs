using Dhtmlx.Model.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dhtmlx.Model.Ribbon
{
    public class DhtmlxRibbonButtonSelectItem : DhtmlxRibbonItemWithIDText
    {

        public string Img { get; set; }
        public bool IsBig { get; set; }
        public string IconPath { get; set; }
        public DhtmlxMenu Menu { get; set; }


        public DhtmlxRibbonButtonSelectItem(string id, string text) : base(id, text)
        {
        }

        public override string Type
        {
            get
            {
                return "buttonSelect";
            }
        }

        public override XElement BuildDhtmlXml()
        {
            return new XElement("item",
                new XAttribute("type", Type),
                new XAttribute("id", id),
                new XAttribute("text", text),
                string.IsNullOrWhiteSpace(Img) ? null : new XAttribute("img", Img),
                IsBig ? new XAttribute("isbig", IsBig) : null,
                string.IsNullOrWhiteSpace(IconPath) ? null : new XAttribute("icons_path", IconPath),
                Menu == null ? null : Menu.BuildDhtmlXml());
        }
    }
}
