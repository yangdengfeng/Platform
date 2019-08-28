using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PkpmGX.Models
{
    public class SiderBarModel
    {
        public List<SideBarGroup> SiderBarGroups { get; set; }
    }

    public class SideBarGroup
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public List<SiderBarItem> SiderBarItems { get; set; }
    }

    public class SiderBarItem
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
}