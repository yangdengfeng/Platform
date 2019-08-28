using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dhtmlx.Model.Ribbon
{
    public class DhtmlxRibbonButtonItem : DhtmlxRibbonItemWithIDText
    {
        string img;
        string imgdis;
        bool isBig;
        bool disable;

        public DhtmlxRibbonButtonItem(string id, string text) : base(id, text)
        {
            isBig = false;
            disable = false;
        }

        public string Img
        {
            set
            {
                img = value;
            }
        }

        public string Imgdis
        {
            set
            {
                imgdis = value;
            }
        }

        public bool IsBig
        {
            set
            {
                isBig = value;
            }
        }

        public bool Disable
        {
            set
            {
                disable = value;
            }
        }

        public override string Type
        {
            get
            {
                return "button";
            }
        }

        public override XElement BuildDhtmlXml()
        {
            return new XElement("item",
                new XAttribute("id", id),
                new XAttribute("text", text),
                new XAttribute("type", Type),
                string.IsNullOrWhiteSpace(img) ? null : new XAttribute("img", img),
                string.IsNullOrWhiteSpace(imgdis) ? null : new XAttribute("imgdis", imgdis),
                isBig ? new XAttribute("isbig", isBig) : null,
                disable ? new XAttribute("disable", disable) : null);
        }
    }
}
