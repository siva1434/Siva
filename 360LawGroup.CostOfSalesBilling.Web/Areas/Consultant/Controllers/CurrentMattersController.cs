using _360LawGroup.CostOfSalesBilling.Models;
using _360LawGroup.CostOfSalesBilling.Web.Controllers;
using _360LawGroup.CostOfSalesBilling.Web.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace _360LawGroup.CostOfSalesBilling.Web.Areas.Consultant.Controllers
{
    [WebAuth]
    public class CurrentMattersController : BaseController
    {
        // GET: Consultant/CurrentMatters
        [HttpGet]
        public ActionResult CurrentConsultantMatters()
        {
            var model = new MatterViewModel();
            var AreaOfList = ApiHelper.GetWithParam<AdminSettingsViewModel>("api/common/common/getsettingbyname", "name", "CLIENTMATTER_AREA_OF_LAW");
            if (AreaOfList.StatusCode == HttpStatusCode.OK)
                ViewBag.AreaOfList = AreaOfList.Data.ParamValue.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            else
                ViewBag.AreaOfList = new List<string>();
            return View("CurrentConsultantMatters",model);
        }
    }
}