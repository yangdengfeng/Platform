using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dhtmlx.Model.Ribbon
{
    public abstract class DhtmlxRibbonItem
    {
        public abstract string Type { get; }

        public abstract XElement BuildDhtmlXml();
    }
}
