using HealthFreak.Models;
using HealthFreak.Web.Controllers;
using HealthFreak.Web.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace HealthFreak.Web.Areas.SuperAdmin.Controllers
{
    public class CampaignsController : BaseController
    {
        // GET: SuperAdmin/Campaigns
        public ActionResult Index()
        {
            var model = new CampaignViewModel() { IsActive = true };
            return View(model);
        }

        [HttpGet]
        public ActionResult Editor(string id = null)
        {
            var model = new CampaignViewModel() { IsActive = true };
            if (!string.IsNullOrEmpty(id))
            {
                var status = ApiHelper.Get<CampaignViewModel>("superadmin/campaigns/GetById", id);
                if (status.StatusCode == HttpStatusCode.OK)
                    model = status.Data;
            }
            return PartialView(model);
        }

        [HttpGet]
        public ActionResult CampaignMember(string id)
        {
            var model = new CampaignMembersViewModel() { IsActive = true };
            var Camp = new CampaignViewModel() { IsActive = true };
            var CampResponse = ApiHelper.Get<CampaignViewModel>("superadmin/campaigns/GetById", id);
            if (CampResponse.StatusCode == HttpStatusCode.OK)
                Camp = CampResponse.Data;
            var status = ApiHelper.Get<CampaignMembersViewModel>("superadmin/campaigns/getbycampaignsmemberid", id);
            if (status.StatusCode == HttpStatusCode.OK)
                model = status.Data;
            if (Camp == null || Camp.Id == Guid.Empty)
                return RedirectToAction("Index");
            ViewBag.Camp = Camp;
            return View(model);
        }
    }
}