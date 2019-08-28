using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhtmlx.Model.Form
{
    public class DhtmlxFormSelect : DhtmlxFormWithOptions
    {
        public DhtmlxFormSelect():this(string.Empty,string.Empty)
        {

        }

        public DhtmlxFormSelect(string name,string label):base()
        {
            AddStringItem("name", name).AddStringItem("label", label);
        }

        public override string Type
        {
            get
            {
                return "select";
            }
        }
    }
}
