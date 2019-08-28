using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhtmlx.Model.Form
{
    public class DhtmlxFormButton : DhtmlxFormItem
    {
        public DhtmlxFormButton() : this(string.Empty, string.Empty)
        {

        }

        public DhtmlxFormButton(string name, string value) : base()
        {
            AddStringItem("value", value).AddStringItem("name", name);
        }

        public override string Type
        {
            get
            {
                return "button";
            }
        }


    }
}
