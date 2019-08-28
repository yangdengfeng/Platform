using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhtmlx.Model.Form
{
    public class DhtmlxFromFieldset : DhtmlxFormItemConatiner
    {

        public DhtmlxFromFieldset()
        {

        }

        public DhtmlxFromFieldset(string name,string label):base()
        {
            AddStringItem("name", name).AddStringItem("label", label);
        }

        public override string Type
        {
            get
            {
                return "fieldset";
            }
        }
    }
}
