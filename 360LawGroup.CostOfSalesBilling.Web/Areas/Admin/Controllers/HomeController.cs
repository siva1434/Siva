using _360LawGroup.CostOfSalesBilling.Utilities;
using _360LawGroup.CostOfSalesBilling.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _360LawGroup.CostOfSalesBilling.Web.Areas.Admin.Controllers
{
    [WebAuth(RoleExtension.Admin)]
    public class HomeController : BaseController
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}