using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HealthFreak.Utilities;
using HealthFreak.Web.Controllers;

namespace HealthFreak.Web.Areas.SuperAdmin.Controllers
{
    [AppAuth( RoleExtension.SuperAdmin)]
    public class LogsController : BaseController
    {
        // GET: SuperAdmin/Logs
        public ActionResult Index()
        {
            return View();
        }
    }
}