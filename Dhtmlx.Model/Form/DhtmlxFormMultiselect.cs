using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhtmlx.Model.Form
{
    public class DhtmlxFormMultiselect : DhtmlxFormWithOptions
    {
        public DhtmlxFormMultiselect():this(string.Empty)
        {

        }

        public DhtmlxFormMultiselect(string label):base()
        {
            AddStringItem("label", label);
        }

        public override string Type
        {
            get
            {
                return "multiselect";
            }
        }
    }
}
