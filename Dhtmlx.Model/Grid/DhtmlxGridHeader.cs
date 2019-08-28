using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dhtmlx.Model.Grid
{
   public class DhtmlxGridHeader
    {
        List<DhtmlxGridColumn> columns;
        DhtmlxGirdAfterInit afterInit;
        DhtmlxGridBeforeInit beforeInit;
        DhtmlxGridSettings settings;

        public DhtmlxGridHeader()
        {
            columns = new List<Grid.DhtmlxGridColumn>();
           
        }

        public void AddColumn(DhtmlxGridColumn column)
        {
            this.columns.Add(column);
        }

        public DhtmlxGirdAfterInit AfterInit
        {
            set
            {
                afterInit = value;     
            }
        }

        public DhtmlxGridBeforeInit BeforeInit
        {
            set
            {
                beforeInit = value;
            }
        }

        public DhtmlxGridSettings Settings
        {
            set
            {
                settings = value;
            }
        }

        public XElement BuildDhtmlXml()
        {
            return  new XElement("head",
                from c in columns
                select c.BuildDhtmlXml(),
                afterInit == null ? null : afterInit.BuildDhtmlXml(),
                beforeInit == null ? null : beforeInit.BuildDhtmlXml(),
                settings == null ? null : settings.BuildDhtmlXml());

            
        }


    }
}
