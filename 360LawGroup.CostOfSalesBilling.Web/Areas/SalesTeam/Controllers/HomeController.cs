using HealthFreak.Utilities;
using HealthFreak.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HealthFreak.Web.Areas.SalesTeam.Controllers
{
    [AppAuth(RoleExtension.SuperAdmin, RoleExtension.SalesTeam,
        RoleExtension.RegionalBranchManager, RoleExtension.BranchManager)]
    public class HomeController : BaseController
    {
        // GET: SalesTeam/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}