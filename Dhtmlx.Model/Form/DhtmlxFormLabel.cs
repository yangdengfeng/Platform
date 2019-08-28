using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhtmlx.Model.Form
{
    public class DhtmlxFormLabel : DhtmlxFormItem
    {
        public DhtmlxFormLabel():this(string.Empty)
        {

        }

        public DhtmlxFormLabel(string label):base()
        {
            AddStringItem("label", label);
        }

        public override string Type
        {
            get
            {
                return "label";
            }
        }
    }
}
