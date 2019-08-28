using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhtmlx.Model.Form
{
    public class DhtmlxFormBtn2state : DhtmlxFormItemConatiner
    {
        public DhtmlxFormBtn2state() : this(string.Empty, string.Empty)
        {

        }

        public DhtmlxFormBtn2state(string label, string name) : base()
        {
            AddStringItem("label", label).AddStringItem("name", name);
        }

        public override string Type
        {
            get
            {
                return "btn2state";
            }
        }
    }
}
