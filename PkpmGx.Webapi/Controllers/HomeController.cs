using Pkpm.Entity;
using Pkpm.Framework.Repsitory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PkpmGx.Webapi.Controllers
{
    public class HomeController : Controller
    { 
        public ActionResult Index()
        {
 

            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
