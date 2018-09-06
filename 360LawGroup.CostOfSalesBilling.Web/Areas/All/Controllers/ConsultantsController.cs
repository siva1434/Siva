using _360LawGroup.CostOfSalesBilling.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _360LawGroup.CostOfSalesBilling.Web.Areas.All.Controllers
{
    [WebAuth]
    public class ConsultantsController : BaseController
    {
        // GET: All/Consultant
        public ActionResult ConsultantHours()
        {
            return View();
        }
    }
}