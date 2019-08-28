using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dhtmlx.Model.Toolbar
{
    public class DhtmlxToolbarButtonSelectItem : DhtmlxToolbarItem
    {
        string id;
        string text;
        string img;
        string imgdis;
        List<DhtmlxToolbarItem> items;

        public DhtmlxToolbarButtonSelectItem(string id,string text)
        {
            this.id = id;
            this.text = text;
            items = new List<Toolbar.DhtmlxToolbarItem>();
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

        public override string Type
        {
            get
            {
                return "buttonSelect";
            }
        }

        public void AddOptionItem(DhtmlxToolbarItem item)
        {
            items.Add(item);
        }

        public override XElement BuildDhtmlXml()
        {
            return new XElement("item",
                new XAttribute("id", id),
                new XAttribute("type", Type),
                new XAttribute("text", text),
                string.IsNullOrWhiteSpace(img) ? null : new XAttribute("img", img),
                string.IsNullOrWhiteSpace(imgdis) ? null : new XAttribute("imgdis", imgdis),
                from i in items
                select i.BuildDhtmlXml());
        }
    }
}
