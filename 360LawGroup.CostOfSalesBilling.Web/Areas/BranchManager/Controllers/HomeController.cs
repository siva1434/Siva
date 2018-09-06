using HealthFreak.Utilities;
using HealthFreak.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HealthFreak.Web.Areas.BranchManager.Controllers
{
    [AppAuth(RoleExtension.SuperAdmin, RoleExtension.BranchManager)]
    public class HomeController : BaseController
    {
        // GET: BranchManager/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}