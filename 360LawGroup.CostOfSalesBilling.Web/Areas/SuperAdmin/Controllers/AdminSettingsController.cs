using _360LawGroup.CostOfSalesBilling.Models;
using _360LawGroup.CostOfSalesBilling.Web.Controllers;
using _360LawGroup.CostOfSalesBilling.Web.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace _360LawGroup.CostOfSalesBilling.Web.Areas.SuperAdmin.Controllers
{
    [WebAuth]
    public class AdminSettingsController : BaseController
    {
        // GET: SuperAdmin/AdminSettings
        public ActionResult Index(string type)
        {
            ViewBag.ParamType = type;
            return View();
        }

        /*public ActionResult ClientRepresentative()
        {
            var model = new ClientRepresentativeSettingsViewModel();
            return View(model);
        }*/
    }
}