using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhtmlx.Model.Form
{
    public class DhtmlxFormRadio : DhtmlxFormItemConatiner
    {
        public DhtmlxFormRadio() : this(string.Empty, string.Empty, string.Empty, false)
        {

        }

        public DhtmlxFormRadio(string name,string value,string label,bool IsChecked):base()
        {
            AddStringItem("name", name).AddStringItem("value", value).AddStringItem("label", label);
            if(IsChecked)
            {
                AddBoolItem("checked", true);
            }
        }

        public override string Type
        {
            get
            {
                return "radio";
            }
        }
    }
}
