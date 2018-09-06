using HealthFreak.Utilities;
using HealthFreak.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HealthFreak.Web.Areas.Member.Controllers
{
    [AppAuth(RoleExtension.SuperAdmin, RoleExtension.TrainerHead)]
    public class HomeController : BaseController
    {
        // GET: Member/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}