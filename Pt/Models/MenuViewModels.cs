using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PkpmGX.Models
{
    public class SearchMenuViewModel
    {
        public string MenuName { get; set; }
    }

    public class NewMenuViewModel : SearchMenuViewModel
    {
        public string MenuUrl { get; set; }
        public int MenuOrderNo { get; set; }
        public int IsCategory { get; set; }
        public int CategoryId { get; set; }
        public int MenuStatus { get; set; }
        public string MenuIcon { get; set; }
    }

    public class NewActonViewModel
    {
        public string ActionName { get; set; }
        public string ActionuUrl { get; set; }
        public int ActionStatus { get; set; }
        public int PathId { get; set; }
    }

    public class EditAcionViewModel : NewActonViewModel
    {
        public int ActionId { get; set; }
    }

    public class EditMenuViewModel : NewMenuViewModel
    {
        public int PathId { get; set; }
    }
}