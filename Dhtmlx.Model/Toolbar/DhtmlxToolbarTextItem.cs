using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dhtmlx.Model.Toolbar
{
    public class DhtmlxToolbarTextItem : DhtmlxToolbarItem
    {
        string id;
        string text;

        public DhtmlxToolbarTextItem(string id,string text)
        {
            this.id = id;
            this.text = text;
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
                new XAttribute("type", Type),
                new XAttribute("text", text));
        }
    }
}
