using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhtmlx.Model.Ribbon
{
  public abstract class DhtmlxRibbonItemWithIDText : DhtmlxRibbonItem
    {
        protected string id;
        protected string text;

        public DhtmlxRibbonItemWithIDText(string id,string text)
        {
            this.id = id;
            this.text = text;
        }
    }
}
