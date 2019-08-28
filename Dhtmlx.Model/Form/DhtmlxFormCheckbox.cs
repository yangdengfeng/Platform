using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhtmlx.Model.Form
{
    public class DhtmlxFormCheckbox : DhtmlxFormItemConatiner
    {
        public DhtmlxFormCheckbox() : this(string.Empty, string.Empty, false)
        {

        }

        public DhtmlxFormCheckbox(string name, string label, bool isChecked) : base()
        {
            AddStringItem("name", name).AddStringItem("label", label).AddBoolItem("checked", isChecked);
        }

        public DhtmlxFormCheckbox(string name, string label, string value, bool isChecked) : base()
        {
            AddStringItem("name", name).AddStringItem("label", label).AddStringItem("value", value).AddBoolItem("checked", isChecked);
        }

        public override string Type
        {
            get
            {
                return "checkbox";
            }
        }
    }
}
