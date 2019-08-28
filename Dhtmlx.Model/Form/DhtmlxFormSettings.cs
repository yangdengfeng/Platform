using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhtmlx.Model.Form
{
    public class DhtmlxFormSettings : DhtmlxFormItem
    {
        public DhtmlxFormSettings():this(string.Empty,string.Empty)
        {
                
        }

        public DhtmlxFormSettings(string positon,string labelAlign):base()
        {
            AddStringItem("position", positon).AddStringItem("labelAlign", labelAlign);
        }

        public override string Type
        {
            get
            {
                return "settings";
            }
        }
    }
}
