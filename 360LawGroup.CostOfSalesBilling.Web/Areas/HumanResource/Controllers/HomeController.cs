using HealthFreak.Utilities;
using HealthFreak.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HealthFreak.Web.Areas.HumanResource.Controllers
{
    [AppAuth(RoleExtension.SuperAdmin, RoleExtension.HumanResource)]
    public class HomeController : BaseController
    {
        // GET: HumanResource/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}