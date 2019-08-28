using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dhtmlx.Model.Grid
{
   public class DhtmlxGridColumn
    {
        Dictionary<string, string> columnProperties;
        string name;
        string id;

        public DhtmlxGridColumn(string name):this(name,name)
        {
           
        }

        public DhtmlxGridColumn(string id,string name)
        {
            this.id = id;
            this.name = name;
            columnProperties = new Dictionary<string, string>();
        }

        public DhtmlxGridColumnAlign ColumnAlign
        {
            set
            {
                columnProperties["align"] = value.ToString();
            }
        }

        public string  ColumnType
        {
            set
            {
                columnProperties["type"] = value;
            }
        }

        public string ColumnSort
        {
            set
            {
                columnProperties["sort"] = value;
            }
        }

        public string Width
        {
            set
            {
                columnProperties["width"] = value;
            }
        }

        public string  ColumnFormat
        {
            set
            {
                columnProperties["format"] = value;
            }
        }


        public XElement BuildDhtmlXml()
        {

            return new XElement("column",
                new XAttribute("id",id),
                from pKv in columnProperties
                select new XAttribute(pKv.Key, pKv.Value),
                new XText(name));
        }
    }

    public enum DhtmlxGridColumnAlign
    {
        left,
        right,
        center
    }
}
