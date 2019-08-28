using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dhtmlx.Model.TreeView
{
   public class DhtmlxTreeView
    {
        List<DhtmlxTreeViewItem> items;

        public DhtmlxTreeView()
        {
            items = new List<DhtmlxTreeViewItem>();
        }

        public void AddTreeViewItem(DhtmlxTreeViewItem item)
        {
            this.items.Add(item);
        }

        public XElement BuildDhtmlXml()
        {
            return new XElement("tree",
                from i in items
                select i.BuildDhtmlXml());
        }
    }
}
