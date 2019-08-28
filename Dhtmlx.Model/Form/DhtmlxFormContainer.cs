using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dhtmlx.Model.Form
{
    public class DhtmlxFormContainer : DhtmlxFormItem
    {
        public override string Type
        {
            get
            {
                return "container";
            }
        }

        public DhtmlxFormContainer(string name,string label) : base()
        {
            AddStringItem("name", name).AddStringItem("label", label);
          
        }
       
    }
}
