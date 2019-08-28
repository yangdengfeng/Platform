using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhtmlx.Model.Form
{
    public class DhtmlxFromCalendar : DhtmlxFormItem
    {
        public DhtmlxFromCalendar() : this(string.Empty, string.Empty, string.Empty, string.Empty)
        {

        }

        public DhtmlxFromCalendar(string name, string label, string dateFormate, string value):base()
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                AddStringItem("name", name);
            }

            if (!string.IsNullOrWhiteSpace(label))
            {
                AddStringItem("label", label);
            }

            if (!string.IsNullOrWhiteSpace(dateFormate))
            {
                AddStringItem("dateFormat", dateFormate);
            }

            if (!string.IsNullOrWhiteSpace(value))
            {
                AddStringItem("value", value);
            }
        }

        public override string Type
        {
            get
            {
                return "calendar";
            }
        }
    }
}
