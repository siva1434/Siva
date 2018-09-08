using _360LawGroup.CostOfSalesBilling.Models;
using _360LawGroup.CostOfSalesBilling.Web.Controllers;
using _360LawGroup.CostOfSalesBilling.Web.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace _360LawGroup.CostOfSalesBilling.Web.Areas.All.Controllers
{
    [WebAuth]
    public class ClientRenewalsController : BaseController
    {
        public ActionResult ClientRenewals()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ClientRenewalsDetails(string id = null)
        {
            var model = new ClientViewModel();
            if (!string.IsNullOrEmpty(id))
            {
                var status = ApiHelper.Get<ClientViewModel>("api/all/client/getbyid", id);

                if (status.StatusCode == HttpStatusCode.OK)
                {
                    model.Id = status.Data.Id;
                    model.Company = status.Data.Company;
                    model.FullName = status.Data.FullName;
                    model.LastName = status.Data.LastName;
                    model.Email = status.Data.Email;
                    model.JobTitle = status.Data.JobTitle;
                }
            }
            return PartialView(model);
        }
    }
}