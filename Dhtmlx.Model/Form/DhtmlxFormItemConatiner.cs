using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dhtmlx.Model.Form
{
   public abstract class DhtmlxFormItemConatiner: DhtmlxFormItem
    {
        List<DhtmlxFormItem> items;

        public DhtmlxFormItemConatiner():base()
        {
            items = new List<DhtmlxFormItem>();
        }

        public void AddFormItem(DhtmlxFormItem item)
        {
            items.Add(item);
        }

        public override XElement BuildDhtmlXml()
        {
            XElement elem = base.BuildDhtmlXml();
            foreach (var item in items)
            {
                elem.Add(item.BuildDhtmlXml());
            }
            return elem;
        }

    }
}
