﻿using _360LawGroup.CostOfSalesBilling.Models;
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
    public class ConsultantsController : BaseController
    {
        // GET: All/Consultant
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
            var model = new ConsultantHourViewModel();
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.Title = "Create New client";
            }
            else
            {
                ViewBag.Title = "Edit client";
                var status = ApiHelper.Get<ConsultantHourViewModel>("api/all/consultants/getbyid", id);
                if (status.StatusCode == HttpStatusCode.OK)
                {
                    model = status.Data;
                }
                else
                    return Content("");
            }
            return PartialView("Editor", model);
        }

        [HttpGet]
        public ActionResult ConsultantCosts()
        {
            return View();
        }
    }
}