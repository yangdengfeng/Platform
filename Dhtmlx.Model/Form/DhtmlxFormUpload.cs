﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhtmlx.Model.Form
{
    public class DhtmlxFormUpload : DhtmlxFormItem
    {
        public DhtmlxFormUpload() : this(string.Empty, string.Empty)
        {

        }

        public DhtmlxFormUpload(string name, string url) : base()
        {
            AddStringItem("name", name).AddStringItem("url", url);
        }

        public override string Type
        {
            get
            {
                return "upload";
            }
        }
    }
}