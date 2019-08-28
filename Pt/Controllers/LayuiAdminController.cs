using Pkpm.Core.UserRoleCore;
using PkpmGX.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PkpmGX.Controllers
{
    [Authorize]
    public class LayuiAdminController : PkpmController
    {
        public LayuiAdminController(IUserService userService) : base(userService)
        {
        }


        // GET: LayuiAdmin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Theme()
        {
            return PartialView();
        }

        public ActionResult More()
        {
            return PartialView();
        }

        public ActionResult About()
        {
            return PartialView();
        }
    }
}