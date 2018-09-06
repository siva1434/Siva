using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using _360LawGroup.CostOfSalesBilling.Models;
using _360LawGroup.CostOfSalesBilling.Utilities;
using _360LawGroup.CostOfSalesBilling.Web.Helper;
using Microsoft.Owin.Security;
using Newtonsoft.Json;

namespace _360LawGroup.CostOfSalesBilling.Web.Controllers
{
    public class AccountController : BaseController
    {
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (CurrentUser != null && IsLoggedIn)
            {
                return RedirectToAction("Index", "Home", new { Area = "" });
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        private void GenerateIdentity(Token token, bool rememberMe = false)
        {
            token.remember = rememberMe;
            WebCookie.Set("WebLogin", JsonConvert.SerializeObject(token));
        }

        [AllowAnonymous, HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
                return Json(new DefaultResponse().SetErrorMessages(this).AsDefault());
            try
            {
                var token = ApiHelper.Login(model);
                if (!string.IsNullOrEmpty(token.error))
                    return Json(new DefaultResponse(HttpStatusCode.Unauthorized, token.error));
                GenerateIdentity(token, model.RememberMe);
                return Json(new DefaultResponse(HttpStatusCode.OK, "success"));
            }
            catch
            {
                return Json(new DefaultResponse(HttpStatusCode.NotFound, "Invalid email or password."));
            }
        }

        [HttpPost]
        public ActionResult LogOut()
        {
            WebCookie.Set("WebLogin", null);
            return Redirect("~/");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ConfirmEmail(string userId, string code)
        {
            ViewBag.ConfirmUserId = userId;
            ViewBag.ConfirmCode = code;
            return View();
        }


        [AllowAnonymous]
        public ActionResult ResetPassword(string userId, string code)
        {
            ViewBag.ConfirmUserId = userId;
            ViewBag.ConfirmCode = code;
            return View();
        }


        [AllowAnonymous]
        public ActionResult ExternalConfirm()
        {
            try
            {
                var error = Request.QueryString["error"];
                if (!string.IsNullOrEmpty(error))
                {
                    ViewBag.Success = false;
                    ViewBag.Error = error;
                    return View();
                }
                var token = new Token
                {
                    token_type = Request.QueryString["token_type"],
                    access_token = Request.QueryString["access_token"],
                    expires_in = Request.QueryString["expires_in"]
                };
                var profile = ApiHelper.GetProfile($"{token.token_type} {token.access_token}");
                if (profile.StatusCode == HttpStatusCode.OK)
                {
                    token.profile = JsonConvert.SerializeObject(profile.Data);
                    GenerateIdentity(token);
                    ViewBag.Success = true;
                }
                else
                {
                    ViewBag.Success = false;
                    ViewBag.Error = "Unable to connect login at this moment. Try again.";
                }
            }
            catch
            {
                ViewBag.Success = false;
                ViewBag.Error = "Unable to connect login at this moment. Try again.";
            }
            return View();
        }

        [HttpGet, WebAuth]
        public ActionResult ChangePassword()
        {
            return PartialView();
        }

        [HttpGet, WebAuth]
        public ActionResult UpdateProfile()
        {
            var token = CurrentUser;
            var profile = UserProfile;
            var model = new UpdateProfileModel
            {
                FirstName = profile.FirstName,
                MiddleName = profile.MiddleName,
                LastName = profile.LastName,
                Email = profile.Email,
                PhoneNumber = profile.PhoneNumber
            };
            return PartialView(model);
        }

        [HttpPost, WebAuth]
        public void SetUpdatedProfile(UpdateProfileModel model)
        {
            var token = CurrentUser;
            var profile = UserProfile;
            profile.FirstName = model.FirstName;
            profile.MiddleName = model.MiddleName;
            profile.LastName = model.LastName;
            profile.PhoneNumber = model.PhoneNumber;
            token.profile = JsonConvert.SerializeObject(profile);
            //var remember = false;
            //var rememberClaim = CurrentUser.remember;
            //if (rememberClaim != null)
            //  remember = Convert.ToBoolean(rememberClaim.Value);
            WebCookie.Set("WebLogin", null);
            GenerateIdentity(token, CurrentUser.remember);
        }
    }
}