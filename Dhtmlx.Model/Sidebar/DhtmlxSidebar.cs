using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dhtmlx.Model.Sidebar
{
   public class DhtmlxSidebar
    {
        List<DhtmlxSidebarBaseItem> items;

        public string Template { get; set; }
        public string IconPath { get; set; }
        public int Width { get; set; }

        public DhtmlxSidebar()
        {
            items = new List<Sidebar.DhtmlxSidebarBaseItem>();
            Template = "text";
            IconPath = string.Empty;
            Width = 0;
        }

        public void AddSidebarItem(DhtmlxSidebarBaseItem item)
        {
            items.Add(item);
        }

        public XElement BuildDhtmlXml()
        {
            return new XElement("sidebar",
                new XAttribute("template", Template),
                string.IsNullOrWhiteSpace(IconPath) ? null : new XAttribute("icons_path", IconPath),
                Width <= 0 ? null : new XAttribute("width", Width),
                from i in items
                select i.BuildDhtmlXml());
        }


    }
}
