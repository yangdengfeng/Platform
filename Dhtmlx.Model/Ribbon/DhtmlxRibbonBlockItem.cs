using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dhtmlx.Model.Ribbon
{
    public class DhtmlxRibbonBlockItem : DhtmlxRibbonItemWithIDText
    {
        DhtmlxRibbonTextPos text_pos;
        DhtmlxRibbonMode mode;
        List<DhtmlxRibbonItem> items;

        public DhtmlxRibbonBlockItem(string id, string text) : base(id, text)
        {
            text_pos = DhtmlxRibbonTextPos.bottom;
            mode = DhtmlxRibbonMode.cols;
            items = new List<Ribbon.DhtmlxRibbonItem>();
        }

        public DhtmlxRibbonMode Mode
        {
            set
            {
                mode = value;
            }
        }

        public DhtmlxRibbonTextPos Pos
        {
            set
            {
                text_pos = value;
            }
        }

        public void AddRibbonItem(DhtmlxRibbonItem item)
        {
            items.Add(item);
        }

        public override string Type
        {
            get
            {
                return "block";
            }
        }

        public override XElement BuildDhtmlXml()
        {
            return new XElement("item",
                new XAttribute("id", id),
                new XAttribute("type", Type),
                new XAttribute("text", text),
                new XAttribute("mode", mode.ToString()),
                new XAttribute("text_pos", text_pos.ToString()),
                from i in items
                select i.BuildDhtmlXml());
        }
    }

    public enum DhtmlxRibbonTextPos
    {
        top,
        bottom
    }

    public enum DhtmlxRibbonMode
    {
        cols,
        rows
    }


}
