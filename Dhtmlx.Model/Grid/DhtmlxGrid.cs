using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dhtmlx.Model.Grid
{
   public class DhtmlxGrid
    {
        public DhtmlxGridHeader Header { get; set; }
        List<DhtmlxGridRow> rows;
        Dictionary<string, int> pagings;

        public DhtmlxGrid()
        {
            rows = new List<Grid.DhtmlxGridRow>();
            pagings = new Dictionary<string, int>();
        }

        public void AddGridRow(DhtmlxGridRow row)
        {
            rows.Add(row);
        }

        public void AddPaging(int total_count,int pos)
        {
            pagings["total_count"] = total_count;
            pagings["pos"] = pos;
        }

        public XElement BuildDhtmlXml()
        {
            return new XElement("rows",
                Header.BuildDhtmlXml(),
                from r in rows
                select r.BuildDhtmlXml());
        }

        public XElement BuildRowXml()
        {
            return new XElement("rows",
                from kv in pagings
                select new XAttribute(kv.Key, kv.Value.ToString()),
                from r in rows
                select r.BuildDhtmlXml());
        }

    }
}
