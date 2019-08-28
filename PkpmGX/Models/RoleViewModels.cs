using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PkpmGX.Models
{
    public class RoleViewModels
    {
    }


    public class SearchRoleViewModel
    {
        public string RoleName { get; set; }
        public string RoleCode { get; set; }
    }

    public class NewRoleViewModel : SearchRoleViewModel
    {
        public string RoleDesc { get; set; }
    }

    public class EditRoleViewModel : NewRoleViewModel
    {
        public int RoleId { get; set; }
    }
}