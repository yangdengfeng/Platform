using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhtmlx.Model.Grid
{
    public class LayUIGrid
    {
        public int Code { get; set; }
        public int Count { get; set; }
        public string Msg { get; set; }
        List<LayUIGridRow> Rows;
        Dictionary<string, int> Pagings;

        public LayUIGrid()
        {
            Msg = "";
            Code = 0;
            Count = 0;
            Rows = new List<LayUIGridRow>();
            Pagings = new Dictionary<string, int>();
        }

        public void AddGridRow(LayUIGridRow row)
        {
            Rows.Add(row);
        }

        public void AddPaging(int total_count, int pos)
        {
            Pagings["total_count"] = total_count;
            Pagings["pos"] = pos;
        }

        public JObject BuildRowXml()
        {

            return new JObject(
                              new JProperty("code", Code),
                              new JProperty("count", Count),
                              new JProperty("msg", Msg),
                              new JProperty("data",
                                  new JArray(
                                      from p in Rows
                                      select p.BuildRowJson())));
        }
    }
}
