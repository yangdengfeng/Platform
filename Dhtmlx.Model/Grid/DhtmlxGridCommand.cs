using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dhtmlx.Model.Grid
{
   public class DhtmlxGridCommand
    {
        string command;
        List<string> commandParams;
        public DhtmlxGridCommand(string command)
        {
            this.command = command;
            commandParams = new List<string>();
        }

        public void AddComamndParam(string commandParam)
        {
            this.commandParams.Add(commandParam);
        }

        public XElement BuildDhtmlXml()
        {
            return new XElement("call", new XAttribute("command", command),
                from p in commandParams
                select new XElement("param", p));
        }

    }
}
