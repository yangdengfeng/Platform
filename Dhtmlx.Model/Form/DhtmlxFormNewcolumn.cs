using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhtmlx.Model.Form
{
    public class DhtmlxFormNewcolumn : DhtmlxFormItem
    {
        public DhtmlxFormNewcolumn():base()
        {

        }

        public DhtmlxFormNewcolumn(int offset):base()
        {
            AddIntItem("offset", offset);
        }

        public override string Type
        {
            get
            {
                return "newcolumn";
            }
        }
    }
}
