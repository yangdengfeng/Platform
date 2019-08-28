using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dhtmlx.Model.Menu
{
   public abstract class DhtmlxMenuItem
    {
        public abstract XElement BuildDhtmlXml();
    }
}
