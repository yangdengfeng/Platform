using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhtmlx.Model.Form
{
    public class DhtmlxFormPassword : DhtmlxFormItem
    {
        public DhtmlxFormPassword() : this(string.Empty, string.Empty, string.Empty)
        {

        }

        public DhtmlxFormPassword(string name,string label,string value):base()
        {
            AddStringItem("name", name).AddStringItem("label", label).AddStringItem("value", value);
        }

        public override string Type
        {
            get
            {
                return "password";
            }
        }
    }
}
