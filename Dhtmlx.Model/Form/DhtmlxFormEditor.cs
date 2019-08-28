using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhtmlx.Model.Form
{
    public class DhtmlxFormEditor : DhtmlxFormItem
    {
        public DhtmlxFormEditor() : this(string.Empty, string.Empty)
        {

        }

        public DhtmlxFormEditor(string name,string label):base()
        {
            AddStringItem("name", name).AddStringItem("label", label);
        }

        public override string Type
        {
            get
            {
                return "editor";
            }
        }
    }
}
