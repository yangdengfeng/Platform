using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhtmlx.Model.Form
{
    public class DhtmlxFormBlock : DhtmlxFormItemConatiner
    {
        public DhtmlxFormBlock() : this(string.Empty)
        {

        }

        public DhtmlxFormBlock(string name) : base()
        {
            AddStringItem("name", name);
        }

        public override string Type
        {
            get
            {
                return "block";
            }
        }
    }
}
