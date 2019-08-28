using Pkpm.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dhtmlx.Model.Grid
{
    public class DhtmlxGridRow
    {
        string id;
        List<DhtmlxGridCell> cells;
        List<DhtmlxGridRow> rows;

        public DhtmlxGridRow(int id)
        {
            this.id = id.ToString();
            cells = new List<DhtmlxGridCell>();
            rows = new List<Grid.DhtmlxGridRow>();
        }

        public DhtmlxGridRow(string id)
        {
            this.id = id;
            cells = new List<DhtmlxGridCell>();
            rows = new List<Grid.DhtmlxGridRow>();
        }

        public void AddRow(DhtmlxGridRow row)
        {
            rows.Add(row);
        }

        public void AddCells(List<string> cells)
        {
            foreach (var item in cells)
            {
                AddCell(item);
            }
        }

        public void AddDouble(double d)
        {
            string text = d.ToString();
            AddCell(text);
        }

        public void AddDouble(double? d)
        {
            string text = CommonUtils.GetStrFromDouble(d);
            AddCell(text);
        }

        public void AddCell(int i)
        {
            string text = i.ToString();
            AddCell(text);
        }

        public void AddCell(int? i)
        {
            string text = CommonUtils.GetStrFromInt(i);
            AddCell(text);
        }

        public void AddCell(DateTime dt,string format="yyyy-MM-dd")
        {
            string text = CommonUtils.GetStrFromDt(dt, format);
            AddCell(text);
        }

        public void AddCell(DateTime? dt, string format="yyyy-MM-dd")
        {
            string text = CommonUtils.GetStrFromDt(dt, format);
            AddCell(text);
        }

        public void AddCell(string text, bool hasHtml = false)
        {
            this.cells.Add(new DhtmlxGridCell(text, hasHtml));
        }

        public void AddEmptyCell()
        {
            string text = string.Empty;
            AddCell(text);
        }

        public void AddLinkCell(string linkText,string url,string target= "_blank")
        {
            string text = string.Format("{0}^{1}^{2}", linkText, url, target);
            AddCell(text);
        }

        public void AddLinkJsCells(Dictionary<string, string> linkWithUrls, string title = "操作")
        {
            string text = string.Empty;
            if (linkWithUrls != null && linkWithUrls.Count > 0)
            {
                text = string.Join("~", linkWithUrls.Select(x => string.Format("{0}^javascript:{1};", x.Key, x.Value)));
            }
            DhtmlxGridCell cell = new Grid.DhtmlxGridCell(text, false);
            cell.AddCellAttribute("title", title);
            AddCell(cell);
        }

        public void AddLinkJsCell<T>(T linkText, string url, string fontColor = "black")
        {
            string text = string.Format("{0}^^javascript:{1};^^{2}", linkText, url, fontColor);
            AddCell(text);
        }

         
        public void AddCell(DhtmlxGridCell cell)
        {
            this.cells.Add(cell);
        }

        public XElement BuildDhtmlXml()
        {
            return new XElement("row",
                new XAttribute("id", id),
                from c in cells
                select c.BuildDhtmlXml(),
                from r in rows
                select r.BuildDhtmlXml());
        }
         
    }

    public class DhtmlxGridRowUserData
    {
        string name;
        string userData;

        public DhtmlxGridRowUserData(string name, string value)
        {
            this.name = name;
            this.userData = value;
        }

        public XElement BuildDhtmlXml()
        {
            return new XElement("userdata",
                 new XAttribute("name", name),
                 new XText(userData));
        }
    }

    public class DhtmlxGridCell
    {
        string text;
        bool hasHtml;
        Dictionary<string, string> cellAttrs;

        public DhtmlxGridCell(string text, bool hasHtml)
        {
            this.text = string.IsNullOrWhiteSpace(text) ? string.Empty : text;
            this.hasHtml = hasHtml;
            cellAttrs = new Dictionary<string, string>();
        }

        public DhtmlxGridCell AddCellAttribute(string key,string value)
        {
            cellAttrs[key] = value;
            return this;
        }

        public XElement BuildDhtmlXml()
        {
            return new XElement("cell",
                from cellKv in cellAttrs
                select new XAttribute(cellKv.Key, cellKv.Value),
                hasHtml ? new XCData(text) : new XText(text));
        }
    }
}
