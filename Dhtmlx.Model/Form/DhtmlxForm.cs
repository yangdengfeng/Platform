using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dhtmlx.Model.Form
{
   public class DhtmlxForm
    {
        List<DhtmlxFormItem> items;
        public DhtmlxForm()
        {
            items = new List<Form.DhtmlxFormItem>();
        }

        public void AddDhtmlxFormItem(DhtmlxFormItem item)
        {
            items.Add(item);
        }

        public XElement BuildDhtmlXml()
        {
            return new XElement("items",
                from item in items
                select item.BuildDhtmlXml());
        }
    }
}
