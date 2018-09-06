using System;
using System.Configuration;
using System.Security.Claims;
using System.Web.Mvc;
using _360LawGroup.CostOfSalesBilling.Models;
using _360LawGroup.CostOfSalesBilling.Web.Signalr;
using Newtonsoft.Json;
using System.Collections.Generic;
using _360LawGroup.CostOfSalesBilling.Utilities;

namespace _360LawGroup.CostOfSalesBilling.Web.Controllers
{
    public class BaseController : Controller
    {
        public string BaseUrl => $"{Request.Url.Scheme}://{Request.Url.Authority}{Url.Content("~")}";

        public ClaimsPrincipal CurrentUser => User as ClaimsPrincipal;

        public int TimeZoneInterval
        {
            get
            {
                return 330; //IST 
            }
        }

        public bool IsValidRole(string role)
        {
            var token = JsonConvert.DeserializeObject<Token>(CurrentUser.FindFirst("Token").Value);
            var profile = JsonConvert.DeserializeObject<UserViewModel>(token.profile);
            return RoleExtension.IsValidRole(profile.RoleId, role);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.ApiUrl = ConfigurationManager.AppSettings["API_URL"];
            ViewBag.BasePath = BaseUrl;

            if (CurrentUser != null && CurrentUser.Identity.IsAuthenticated)
            {
                var token = JsonConvert.DeserializeObject<Token>(CurrentUser.FindFirst("Token").Value);
                var profile = JsonConvert.DeserializeObject<UserViewModel>(token.profile);
                ViewBag.Username = profile.UserName;
                ViewBag.UserToken = $"{token.token_type} {token.access_token}";
                ViewBag.UserId = profile.Id;
                ViewBag.FullName = $"{profile.FirstName}";
                if (!string.IsNullOrEmpty(profile.LastName))
                    ViewBag.FullName = ViewBag.FullName + " " + profile.LastName.Substring(0, 1) + ".";
                ViewBag.Role = profile.RoleId;
                ViewBag.UserProfile = profile;
                if (CurrentUser.HasClaim(x => x.Type == "CurrentLocation"))
                    ViewBag.CurrentLocation = CurrentUser.FindFirst("CurrentLocation").Value;
                else
                    ViewBag.CurrentLocation = null;
            }
            else
            {
                ViewBag.Username = null;
                ViewBag.UserToken = null;
                ViewBag.UserId = null;
                ViewBag.FullName = null;
                ViewBag.Role = null;
                ViewBag.LocationIds = null;
                ViewBag.LocationNames = null;
                ViewBag.CurrentLocation = null;
                ViewBag.UserProfile = new UserViewModel();
            }
            ViewBag.TimeZoneInterval = TimeZoneInterval;
        }

        [AllowAnonymous, HttpGet]
        public JsonResult PushNotification(Guid notificationId)
        {
            var notification = NotificationHelper.NotifyInit(notificationId);
            if (notification != null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PopupMsg(string title, string msg, bool notify = false)
        {
            return PartialView("PopupMsg", new[] { title, msg, notify.ToString().ToLower() });
        }
    }
}