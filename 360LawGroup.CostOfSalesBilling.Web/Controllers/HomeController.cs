using System;
using System.Web.Mvc;
using _360LawGroup.CostOfSalesBilling.Utilities;
using _360LawGroup.CostOfSalesBilling.Models;
using System.Net;
using _360LawGroup.CostOfSalesBilling.Web.Helper;
using System.Collections.Generic;
using Microsoft.Owin.Security.Cookies;
using System.Web;

namespace _360LawGroup.CostOfSalesBilling.Web.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Home
        public ActionResult Index()
        {
            if (User.IsInRole(RoleExtension.SuperAdmin))
                return RedirectToAction("Index", "Home", new { Area = "SuperAdmin" });
            else if (User.IsInRole(RoleExtension.Admin))
                return RedirectToAction("Index", "Home", new { Area = "Admin" });
            else if (User.IsInRole(RoleExtension.Consultant))
                return RedirectToAction("Index", "Home", new { Area = "Consultant" });
            else if (User.IsInRole(RoleExtension.ClientUser))
                return RedirectToAction("Index", "Home", new { Area = "ClientUser" });

            else
            {
                Request.GetOwinContext().Authentication.SignOut("ApplicationCookie");
                return RedirectToAction("Login", "Account", new { Area = "" });
            }
        }

        public ActionResult Swagger()
        {
            return Redirect(ViewBag.ApiUrl + "swagger/ui/index?token=" + ViewBag.UserToken);
        }
        /*
        public ActionResult DashboardCounters()
        {
            var model = new CounterViewModel();
            var status = ApiHelper.Get<CounterViewModel>("dashboard/getallcounter");
            if (status.StatusCode == HttpStatusCode.OK)
            {
                model = status.Data;
            }
            return PartialView("_DashboardCounters", model);
        }
        */
    }
}