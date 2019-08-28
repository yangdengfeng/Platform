using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhtmlx.Model.Form
{
    public class DhtmlxFormImage : DhtmlxFormItem
    {
        public DhtmlxFormImage():this(string.Empty,string.Empty)
        {

        }

        public DhtmlxFormImage(string name,string url):base()
        {
            AddStringItem("name", name).AddStringItem("url", url);
        }

        public override string Type
        {
            get
            {
                return "image";
            }
        }
    }
}
