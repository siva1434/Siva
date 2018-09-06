using System;
using System.Configuration;
using System.Security.Claims;
using System.Web.Mvc;
using _360LawGroup.CostOfSalesBilling.Models;
using _360LawGroup.CostOfSalesBilling.Web.Signalr;
using Newtonsoft.Json;
using System.Collections.Generic;
using _360LawGroup.CostOfSalesBilling.Utilities;
using System.Web.Mvc.Html;

namespace _360LawGroup.CostOfSalesBilling.Web.Controllers
{
    public class BaseController : Controller
    {
        public string BaseUrl => $"{Request.Url.Scheme}://{Request.Url.Authority}{Url.Content("~")}";

        public bool IsLoggedIn
        {
            get { return UserProfile != null; }
        }

        private Token currentUser = null;
        public Token CurrentUser
        {
            get
            {
                if (currentUser == null)
                {
                    var loginCookies = WebCookie.Get("WebLogin");
                    if (loginCookies != null)
                    {
                        currentUser = JsonConvert.DeserializeObject<Token>(loginCookies);
                    }
                }
                return currentUser;
            }
        }

        private UserViewModel userProfile = null;
        public UserViewModel UserProfile
        {
            get
            {
                if (CurrentUser != null && userProfile == null)
                {
                    userProfile = JsonConvert.DeserializeObject<UserViewModel>(CurrentUser.profile);
                }
                return userProfile;
            }
        }

        public int TimeZoneInterval
        {
            get
            {
                return -240; //IST 
            }
        }

        public bool IsValidRole(string role)
        {
            var profile = userProfile;
            return RoleExtension.IsValidRole(profile.RoleId, role);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.ApiUrl = ConfigurationManager.AppSettings["API_URL"];
            ViewBag.BasePath = BaseUrl;

            if (CurrentUser != null && IsLoggedIn)
            {
                var token = currentUser;
                var profile = userProfile;
                ViewBag.Username = profile.UserName;
                ViewBag.UserToken = $"{token.token_type} {token.access_token}";
                ViewBag.UserId = profile.Id;
                ViewBag.FullName = $"{profile.FirstName}";
                if (!string.IsNullOrEmpty(profile.LastName))
                    ViewBag.FullName = ViewBag.FullName + " " + profile.LastName.Substring(0, 1) + ".";
                ViewBag.Role = profile.RoleId;
                ViewBag.UserProfile = profile;
            }
            else
            {
                ViewBag.Username = null;
                ViewBag.UserToken = null;
                ViewBag.UserId = null;
                ViewBag.FullName = null;
                ViewBag.Role = null;
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

        public bool UserIsInRole(string role)
        {
            return HtmlHelperExt.UserIsInRole(role);
        }
    }
}