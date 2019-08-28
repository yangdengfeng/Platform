﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dhtmlx.Model.Toolbar
{
    public class DhtmlxToolbarButtonTwoStateItem : DhtmlxToolbarBaseButtonItem
    {

        public DhtmlxToolbarButtonTwoStateItem(string id, string text) : base(id, text)
        {

        }


        public override string Type
        {
            get
            {
                return "buttonTwoState";
            }
        }
    }
}
