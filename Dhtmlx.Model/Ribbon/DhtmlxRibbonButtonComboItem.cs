using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dhtmlx.Model.Ribbon
{
    public class DhtmlxRibbonButtonComboItem : DhtmlxRibbonItemWithIDText
    {
        int width;
        string comboType;
        string comboImagePath;
        string comboDefaultImage;
        string comboDefaultImageDis;
        List<DhtmlxRibbonOption> options;

        public DhtmlxRibbonButtonComboItem(string id, string text) : base(id, text)
        {
            width = 0;
            comboType = comboImagePath = comboDefaultImage = comboDefaultImageDis = string.Empty;
            options = new List<Ribbon.DhtmlxRibbonOption>();
        }

        public int Width
        {
            set
            {
                width = value;
            }
        }

        public string ComboType
        {
            set
            {
                comboType = value;
            }
        }

        public string ComboImagePath
        {
            set
            {
                comboImagePath = value;
            }
        }

        public string ComboDefaultImage
        {
            set
            {
                comboDefaultImage = value;
            }
        }

        public string ComboDefalutImageDis
        {
            set
            {
                comboDefaultImageDis = value;
            }
        }

        public void AddOption(string value, string text, bool isSelected = false)
        {
            options.Add(new Ribbon.DhtmlxRibbonOption(value, text, isSelected));
        }
        
        public void SetOptionSelectedByValue(string value)
        {
            foreach (var item in options)
            {
                if (item.Value == value)
                {
                    item.IsSelected = true;
                }
                else
                {
                    item.IsSelected = false;
                }
            }
        }

        public void SetOptionSelectedByText(string text)
        {
            foreach (var item in options)
            {
                if (item.Text == text)
                {
                    item.IsSelected = true;
                }
                else
                {
                    item.IsSelected = false;
                }
            }
        }

        public override string Type
        {
            get
            {
                return "buttonCombo";
            }
        }

        public override XElement BuildDhtmlXml()
        {
            return new XElement("item",
                new XAttribute("id", id),
                new XAttribute("text", text),
                width > 0 ? new XAttribute("width", width) : null,
                string.IsNullOrWhiteSpace(comboType) ? null : new XAttribute("comboType", comboType),
                string.IsNullOrWhiteSpace(comboImagePath) ? null : new XAttribute("comboImagePath", comboImagePath),
                string.IsNullOrWhiteSpace(comboDefaultImage) ? null : new XAttribute("comboDefaultImage", comboDefaultImage),
                string.IsNullOrWhiteSpace(comboDefaultImageDis) ? null : new XAttribute("comboDefaultImageDis", comboDefaultImageDis),
                options.Count == 0 ? null :
                new XElement("complete",
                  from o in options
                  select o.BuildDhtmlXml()));
        }
    }

    public class DhtmlxRibbonOption
    {
        public string Value { get; set; }
        public string Text { get; set; }
        public bool IsSelected { get; set; }

        public DhtmlxRibbonOption(string value, string text) : this(value, text, false)
        {

        }

        public DhtmlxRibbonOption(string value, string text, bool isSelected)
        {
            this.Value = value;
            this.Text = text;
            this.IsSelected = isSelected;
        }

        public XElement BuildDhtmlXml()
        {
            return new XElement("option",
                new XAttribute("value", Value),
                IsSelected ? new XAttribute("selected", IsSelected) : null,
                new XCData(Text));
        }
    }


}
