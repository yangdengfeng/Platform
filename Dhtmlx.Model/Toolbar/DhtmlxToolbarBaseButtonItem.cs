using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dhtmlx.Model.Toolbar
{
    public abstract class DhtmlxToolbarBaseButtonItem : DhtmlxToolbarItem
    {
        string id;
        string text;
        string img;
        string imgdis;

        public DhtmlxToolbarBaseButtonItem(string id,string text)
        {
            this.id = id;
            this.text = text;
        }

        public string Img
        {
            set
            {
                img = value;
            }
        }

        public string Imgdis
        {

            set
            {
                imgdis = value;
            }
        }

        public override XElement BuildDhtmlXml()
        {
            return new XElement("item",
                new XAttribute("id", id),
                new XAttribute("type", Type),
                new XAttribute("text", text),
                string.IsNullOrWhiteSpace(img) ? null : new XAttribute("img", img),
                string.IsNullOrWhiteSpace(imgdis) ? null : new XAttribute("imgdis", imgdis));
        }

    }
}
