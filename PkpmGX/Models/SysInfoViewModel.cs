using Pkpm.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PkpmGX.Models
{
    public class SysInfoViewModel
    {
    }
    public class SysInfoSearchModel
    {
       public string InformationName { get; set; }
        public string informationText { get; set; }
        public string informationTime { get; set; }
        public int? posStart { get; set; }
        public int? count { get; set; }
    }

    public class SysInfoSearchUIModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }

    }

    public class SysInfoEditModel
    {
        public string Id { get; set; }
        public string informationName { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
    }

    public class SysInfoEditViewModel
    {
        public t_bp_PkpmJCRU PkpmJCRU { get; set; }
        public List<SysDict> Type { get; set; }
    }

    public class SysInfoCreateModel
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
    }
}