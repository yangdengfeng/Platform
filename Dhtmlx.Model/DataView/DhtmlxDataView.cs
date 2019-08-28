using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dhtmlx.Model.DataView
{
    public class DhtmlxDataView
    {
        List<DhtmlxDataViewItem> items;

        public DhtmlxDataView()
        {
            items = new List<DataView.DhtmlxDataViewItem>();
        }

        public void AddDataViewItem(DhtmlxDataViewItem item)
        {
            items.Add(item);
        }

        public XElement BuildDhtmlXml()
        {
            return new XElement("data",
                from i in items
                select i.BuildDhtmlXml());
        }

    }
}
