using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dhtmlx.Model.Form
{
    public abstract class DhtmlxFormWithOptions : DhtmlxFormItem
    {
        List<DhtmlxFormOption> options;

        public DhtmlxFormWithOptions():base()
        {
            options = new List<Form.DhtmlxFormOption>();
        }

        public DhtmlxFormItem SetOptions(Dictionary<string,string> dicOptions)
        {
            foreach (var item in dicOptions)
            {
                options.Add(new Form.DhtmlxFormOption() { Text = item.Key, Value = item.Value });
            }
            return this;
        }

        public DhtmlxFormWithOptions AddOption(string text, string value,
            string image = "",
            bool selected = false)
        {
            options.Add(new Form.DhtmlxFormOption() { Text = text, Value = value, Image = image, Selected = selected });
            return this;
        }

        public DhtmlxFormItem SetOptionSelectedByText(string text)
        {

            foreach (var item in options)
            {
                if(item.Text==text)
                {
                    item.Selected = true;
                }
                else
                {
                    item.Selected = false;
                }
            }
            return this;
        }

        public DhtmlxFormItem SetOptionSelectedByValue(string value)
        {
            foreach (var item in options)
            {
                if(item.Value==value)
                {
                    item.Selected = true;
                }
                else
                {
                    item.Selected = false;
                }
            }
            return this;
        }

        public override XElement BuildDhtmlXml()
        {
            XElement elem= base.BuildDhtmlXml();
            foreach (var item in options)
            {
                elem.Add(item.BuildDhtmlXml());
            }
            return elem;
        }
    }

    public class DhtmlxFormOption
    {
        public string Text { get; set; }
        public string  Value { get; set; }
        public string  Image { get; set; }
        public bool Selected { get; set; }

        public XElement BuildDhtmlXml()
        {
            XElement elem = new XElement("option",
                new XAttribute("text", Text),
                new XAttribute("value", Value),
                string.IsNullOrWhiteSpace(Image) ? null : new XAttribute("img", Image),
                Selected == false ? null : new XAttribute("selected", Selected));

            return elem;
        }

    }
}
