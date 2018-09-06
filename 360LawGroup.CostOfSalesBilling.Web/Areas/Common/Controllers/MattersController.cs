﻿using _360LawGroup.CostOfSalesBilling.Models;
using _360LawGroup.CostOfSalesBilling.Web.Controllers;
using _360LawGroup.CostOfSalesBilling.Web.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace _360LawGroup.CostOfSalesBilling.Web.Areas.Common.Controllers
{
    [WebAuth]
    public class MattersController : BaseController
    {
        // GET: Common/Matters
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
            var model = new MatterViewModel();
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.Title = "Create New Matter";
            }
            else
            {
                //ViewBag.Title = "Edit Matter";
                //var status = ApiHelper.Get<MatterViewModel>("common/client/getbyid", id);
                //if (status.StatusCode == HttpStatusCode.OK)
                //{
                //    model = status.Data;
                //    // ViewBag.SelectedRole = model.RoleId;
                //}
                //else
                //    return Content("");
            }
            var overseascountrylist = ApiHelper.GetWithParam<AdminSettingsViewModel>("api/common/getsettingbyname", "name", "CLIENTMATTER_OVERSEAS_COUNTRY");
            if (overseascountrylist.StatusCode == HttpStatusCode.OK)
                ViewBag.overseascountrylist = overseascountrylist.Data.ParamValue.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            else
                ViewBag.overseascountrylist = new List<string>();

            var AreaOfList = ApiHelper.GetWithParam<AdminSettingsViewModel>("api/common/getsettingbyname", "name", "CLIENTMATTER_AREA_OF_LAW");
            if (AreaOfList.StatusCode == HttpStatusCode.OK)
                ViewBag.AreaOfList = AreaOfList.Data.ParamValue.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            else
                ViewBag.AreaOfList = new List<string>();
            return PartialView("Editor",model);
        }
    }
}