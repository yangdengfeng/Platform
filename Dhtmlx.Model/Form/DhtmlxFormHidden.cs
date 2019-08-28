using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhtmlx.Model.Form
{
    public class DhtmlxFormHidden : DhtmlxFormItem
    {
        public DhtmlxFormHidden() : this(string.Empty, string.Empty)
        {

        }

        public DhtmlxFormHidden(string name, string value) : base()
        {
            AddStringItem("name", name).AddStringItem("value", value);
        }

        public override string Type
        {
            get
            {
                return "hidden";
            }
        }
    }
}
