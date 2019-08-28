using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dhtmlx.Model.Accordion
{
   public class DhtmlxAccordion
    {
        public string IconPaths { get; set; }

        List<DhtmlxAccordionCell> cells;
        bool multiMode;

        public DhtmlxAccordion() : this(false)
        {

        }

        public DhtmlxAccordion(bool multiMode)
        {
            this.multiMode = multiMode;
            cells = new List<Accordion.DhtmlxAccordionCell>();
        }

        public void AddAccCell(DhtmlxAccordionCell cell)
        {
            cells.Add(cell);
        }

        public XElement BuildDhtmlXml()
        {
            return new XElement("accordion",
                new XAttribute("multiMode", multiMode),
                string.IsNullOrWhiteSpace(IconPaths) ? null : new XAttribute("iconsPath", IconPaths),
                from c in cells
                select c.BuildDhtmlXml());
        }
    }
}
