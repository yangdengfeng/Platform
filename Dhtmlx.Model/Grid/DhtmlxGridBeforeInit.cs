using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dhtmlx.Model.Grid
{
    public class DhtmlxGridBeforeInit
    {
        List<DhtmlxGridCommand> commands;

        public DhtmlxGridBeforeInit()
        {
            commands = new List<Grid.DhtmlxGridCommand>();
        }

        public void AddCommand(DhtmlxGridCommand command)
        {
            this.commands.Add(command);
        }

        public XElement BuildDhtmlXml()
        {
            return new XElement("beforeInit",
                from c in commands
                select c.BuildDhtmlXml());
        }
    }
}
