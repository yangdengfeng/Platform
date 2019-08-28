using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dhtmlx.Model.Ribbon
{
    public class DhtmlxRibbonNewLevelItem : DhtmlxRibbonItem
    {
        public override string Type
        {
            get
            {
                return "newLevel";
            }
        }

        public override XElement BuildDhtmlXml()
        {
            return new XElement("item",
                new XAttribute("type", Type));
        }
    }
}
