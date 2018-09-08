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
    public class ClientsController : BaseController
    {
        // GET: Common/Clients
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return Edit(string.Empty);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            var model = new ClientViewModel
            {
                MemberChargeRate = 135,
                PrivateClientChargeRate = 165,
                LitigationChargeRate = 220,
                RegulatedChargeRate = 220,
                ContractRenewalDate = DateTime.UtcNow,
                SubscriptionDate = DateTime.UtcNow
            };
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.Title = "Create New client";
            }
            else
            {
                ViewBag.Title = "Edit client";
                var status = ApiHelper.Get<ClientViewModel>("api/all/client/getbyid", id);
                if (status.StatusCode == HttpStatusCode.OK)
                {
                    model = status.Data;
                    // ViewBag.SelectedRole = model.RoleId;
                }
                else
                    return Content("");
            }
            var subscriptionbasislist = ApiHelper.GetWithParam<AdminSettingsViewModel>("api/common/common/getsettingbyname", "name", "CLIENT_SUBSCRIPTION_BASIS");
            if (subscriptionbasislist.StatusCode == HttpStatusCode.OK)
                ViewBag.SubscrptionBasisList = subscriptionbasislist.Data.ParamValue.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            else
                ViewBag.SubscrptionBasisList = new List<string>();
            //var roles = ApiHelper.Get<List<KeyValuePair<string, string>>>("common/users/getroles");
            //ViewBag.Roles = roles.StatusCode == HttpStatusCode.OK ? roles.Data : new List<KeyValuePair<string, string>>();
            return PartialView("Editor", model);
        }

        [HttpGet]
        public ActionResult Show(string id)
        {
            var model = new ClientViewModel();
            var status = ApiHelper.Get<ClientViewModel>("api/all/client/getbyid", id);
            if (status.StatusCode == HttpStatusCode.OK)
            {
                model.Id = status.Data.Id;
                model.Company = status.Data.Company;
                model.FullName = status.Data.FullName;
                model.Email = status.Data.Email;
                model.JobTitle = status.Data.JobTitle;
                model.BusinessPhone = status.Data.BusinessPhone;
                model.FaxNumber = status.Data.FaxNumber;
                model.MobileNumber = status.Data.MobileNumber;
                model.Address = status.Data.Address;
                model.ContractedCountries = status.Data.ContractedCountries;
                model.ContractRenewalDate = status.Data.ContractRenewalDate;
                model.Notes = status.Data.Notes;
                model.Attechments = status.Data.Attechments;
                model.MonthlySubscription = status.Data.MonthlySubscription;
                model.PaymentMonths = status.Data.PaymentMonths;
                model.MemberChargeRate = status.Data.MemberChargeRate;
                model.PrivateClientChargeRate = status.Data.PrivateClientChargeRate;
                model.RegulatedChargeRate = status.Data.RegulatedChargeRate;
                model.LitigationChargeRate = status.Data.LitigationChargeRate;
            }
            return PartialView("Show", model);
        }

        [HttpGet]
        public ActionResult SubscriptionClient()
        {
            var model = new ClientViewModel();
            var status = ApiHelper.Get<ClientViewModel>("api/all/client/getallsubscriptionclient");
            if (status.StatusCode == HttpStatusCode.OK)
            {
                model = status.Data;
            }
            return View();
        }
    }
}