using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dhtmlx.Model.DataView
{
   public class DhtmlxDataViewItem
    {
        string id;

        public string Package { get; set; }
        public string Version { get; set; }
        public string Maintainer { get; set; }

        public DhtmlxDataViewItem(string id)
        {
            this.id = id;
           Package = Version = Maintainer = string.Empty;
        }
        

        public XElement BuildDhtmlXml()
        {
            return new XElement("item", new XAttribute("id", id),
                new XElement("Package", new XCData(Package)),
                new XElement("Version", new XCData(Version)),
                new XElement("Maintainer", new XCData(Maintainer)));
        }

    }
}
