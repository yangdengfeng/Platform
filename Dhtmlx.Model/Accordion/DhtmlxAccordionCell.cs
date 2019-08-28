using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dhtmlx.Model.Accordion
{
   public class DhtmlxAccordionCell
    {
        public string Icon { get; set; }
        public bool IsOpen { get; set; }
        public int Height { get; set; }


        string id;
        string text;

        public DhtmlxAccordionCell(string id,string text)
        {
            this.id = id;
            this.text = text;

            IsOpen = false;
            Height = 0;
            Icon = string.Empty;
        }

        public XElement BuildDhtmlXml()
        {
            return new XElement("cell",
                new XAttribute("id", id),
                new XAttribute("open", IsOpen),
                string.IsNullOrWhiteSpace(Icon) ? null : new XAttribute("icon", Icon),
                Height <= 0 ? null : new XAttribute("height", Height),
                new XText(text));
        }
    }
}
