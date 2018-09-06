using _360LawGroup.CostOfSalesBilling.Utilities;
using _360LawGroup.CostOfSalesBilling.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _360LawGroup.CostOfSalesBilling.Web.Areas.Consultant.Controllers
{
    [AppAuth(RoleExtension.Consultant)]
    public class HomeController : BaseController
    {
        // GET: Consultant/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}