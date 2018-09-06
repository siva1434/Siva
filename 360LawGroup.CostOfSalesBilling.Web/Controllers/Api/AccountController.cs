using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using _360LawGroup.CostOfSalesBilling.Web.Helper;
using _360LawGroup.CostOfSalesBilling.Web.Providers;
using _360LawGroup.CostOfSalesBilling.Data;
using _360LawGroup.CostOfSalesBilling.Models;
using _360LawGroup.CostOfSalesBilling.Web.Results;
using _360LawGroup.CostOfSalesBilling.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;

namespace _360LawGroup.CostOfSalesBilling.Web.Api.Controllers
{
    [RoutePrefix("account")]
    public class AccountController : BaseApiController
    {
        /// <summary>
        /// Get Current Loggedin user information
        /// </summary>
        /// <returns>Response with Data: UserInfo</returns>
        [AppAuth]
        [Route("userinfo")]
        public GenericResponse<UserViewModel> GetUserInfo()
        {
            var status = new GenericResponse<UserViewModel>();
            try
            {
                status.StatusCode = HttpStatusCode.OK;
                var u = Uow.UserRepository.GetById(LoggedInUser.Id);
                status.Data = u.To<UserViewModel>(TimeZoneInterval);
                status.Data.Password = "";
                status.Data.ConfirmPassword = "";
                status.Data.RoleId = string.Join(",", u.AspNetRoles.Select(x => x.Id));
            }
            catch (Exception e)
            {
                status.SetException(e);
            }
            return status;
        }

        /// <summary>
        /// Logout and clear server credentials
        /// </summary>
        [Route("logout")]
        public DefaultResponse Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return new DefaultResponse(HttpStatusCode.OK, "Successfully logged out.");
        }

        // GET Account/ExternalLogin
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("360LawGroupextlogin", Name = "360LawGroupextlogin")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult GetExternalLogin(string provider, string error = null)
        {
            var redirecturl = HttpContext.Current.Request.QueryString["redirect_uri"];
            if (string.IsNullOrEmpty(redirecturl))
            {
                redirecturl = HttpContext.Current.Request.Url.AbsolutePath;
            }
            if (redirecturl.Contains("?"))
                redirecturl += "&";
            else
                redirecturl += "?";
            if (error != null)
            {
                return Redirect(redirecturl + "error=" + Uri.EscapeDataString(error));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            var externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity, provider);

            if (externalLogin == null)
            {
                return Redirect(redirecturl + "error=Unable to access your profile.");
            }
            
            if (externalLogin.LoginProvider != provider)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            var user = UserManager.Find(new UserLoginInfo(externalLogin.LoginProvider,
                externalLogin.ProviderKey));

            var hasRegistered = user != null;

            if (!hasRegistered)
            {
                var claims = externalLogin.GetClaims();
                var identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
                Authentication.SignIn(identity);
                user = UserManager.FindByEmail(externalLogin.EmailAddress);
                if (user == null)
                {
                    return Redirect(redirecturl + "error=Unable to find your linked account try to login with username and password.");
                }
                var logins = UserManager.GetLogins(user.Id);
                if (logins.Any(x => x.LoginProvider != provider))
                {
                    var result = UserManager.AddLogin(user.Id, Authentication.GetExternalLoginInfo().Login);
                    if (!result.Succeeded)
                    {
                        return GetErrorResult(result);
                    }
                }
            }

            var aspnetUser = Uow.UserRepository.GetById(user.Id);
            if (aspnetUser.IsDeleted)
                error = "Unable to find your linked account try to login with username and password.";
            else if (!aspnetUser.IsActive)
                error = "User is inactive or not approved by administrator.Please contact administrator for more information.";
            if (!string.IsNullOrEmpty(error))
                return Redirect(redirecturl + $"error={error}");
            if (!aspnetUser.EmailConfirmed)
            {
                aspnetUser.EmailConfirmed = true;
                Uow.Save(this);
            }
            Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var cookiesIdentity = UserManager.CreateIdentity(user, CookieAuthenticationDefaults.AuthenticationType);
            Authentication.SignIn(cookiesIdentity);
            Request.GetOwinContext().Authentication.SignIn(cookiesIdentity);
            redirecturl =
                $"{redirecturl}provider={externalLogin.LoginProvider}&username={user.UserName}&email={user.Email}&id={user.Id}&roles={string.Join(",", user.Roles.Select(x => x.RoleId))}";
            var localToken = GenerateLocalAccessTokenResponse(user);
            var properties = typeof(Token).GetProperties();
            foreach (var prop in properties)
            {
                redirecturl += $"&{prop.Name}={prop.GetValue(localToken, null)}";
            }
            return Redirect(redirecturl);
        }

        private Token GenerateLocalAccessTokenResponse(ApplicationUser user)
        {
            var totalExpiration = TimeSpan.FromTicks(new DateTime(2999, 1, 1).Ticks);
            var at = new AuthenticationTicket(new ClaimsIdentity("Bearer", "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"),
                new AuthenticationProperties
                {
                    AllowRefresh = true,
                    IsPersistent = true,
                    IssuedUtc = DateTime.UtcNow,
                    ExpiresUtc = new DateTime(2999, 1, 1).ToUniversalTime()
                });

            at.Identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
            at.Identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
            at.Identity.AddClaim(new Claim(ClaimTypes.Role, string.Join(",", user.Roles.Select(x => x.RoleId))));

            var accessToken = Startup.OAuthOptions.AccessTokenFormat.Protect(at);
            var tokenResponse = new Token
            {
                access_token = accessToken,
                token_type = "bearer",
                role = user.Roles.Select(x => x.RoleId).FirstOrDefault(),
                expires_in = totalExpiration.TotalSeconds.ToString(CultureInfo.InvariantCulture)
            };
            return tokenResponse;
        }

        // GET Account/ExternalLogins?returnUrl=%2F&generateState=true
        [AllowAnonymous]
        [HttpGet]
        [Route("360LawGroupextloginlist")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public GenericResponse<List<ExternalLoginViewModel>> GetExternalLogins(string returnUrl, bool generateState = false)
        {
            var descriptions = Authentication.GetExternalAuthenticationTypes();
            var logins = new List<ExternalLoginViewModel>();

            string state;

            if (generateState)
            {
                const int strengthInBits = 256;
                state = RandomOAuthStateGenerator.Generate(strengthInBits);
            }
            else
            {
                state = null;
            }

            foreach (var description in descriptions)
            {
                var login = new ExternalLoginViewModel
                {
                    Name = description.Caption,
                    ClientId = Startup.PublicClientId,
                    Provider = description.AuthenticationType,
                    ResponseType = "token",
                    State = state,
                    Url = Url.Route("360LawGroupextlogin", new
                    {
                        provider = description.AuthenticationType,
                        response_type = "token",
                        client_id = Startup.PublicClientId,
                        redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
                        state
                    })
                };
                logins.Add(login);
            }

            return new GenericResponse<List<ExternalLoginViewModel>>
            {
                StatusCode = HttpStatusCode.OK,
                Data = logins
            };
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("forgotpassword")]
        public DefaultResponse ForgotPassword(ForgotPasswordViewModel model)
        {
            var status = new DefaultResponse();
            if (ModelState.IsValid)
            {
                var user = UserManager.FindByEmail(model.Email);
                if (user == null)
                {
                    status.SetCustomMessages(HttpStatusCode.NotFound,
                        "We didn't find your are email address in our system.");
                    return status;
                }
                var code = UserManager.GeneratePasswordResetToken(user.Id);
                var callbackUrl = $"{WebUrl}Account/ResetPassword?code={code}&userId={user.Id}";
                UserManager.SendEmail(user.Id, "360LawGroup - Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                status.SetCustomMessages(HttpStatusCode.OK,
                    "Reset password link sent to your email address. Please check your inbox and use same link to reset your password.");
            }
            else
            {
                status.SetErrorMessages(this);
            }

            return status;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("resetpassword")]
        public DefaultResponse ResetPassword(SetPasswordBindingModel model)
        {
            var status = new DefaultResponse();
            if (!ModelState.IsValid)
            {
                status.SetErrorMessages(this);
                return status;
            }
            var user = UserManager.FindById(model.UserId);
            if (user == null || user.IsDeleted)
            {
                status.SetCustomMessages(HttpStatusCode.NotFound,
                    "We didn't find you are email address in our system.");
                return status;
            }
            var aspnetUser = Uow.UserRepository.GetById(user.Id);
            var result = UserManager.ResetPassword(user.Id, model.Code, model.NewPassword);
            if (result.Succeeded)
            {
                if (aspnetUser != null)
                {
                    aspnetUser.EmailConfirmed = true;
                    Uow.Save(this);
                }
                if (aspnetUser.IsActive)
                    status.SetCustomMessages(HttpStatusCode.OK,
                        "Your password successfully reset. You can now login with new password.");
                else
                    status.SetCustomMessages(HttpStatusCode.OK,
                    "Your password successfully reset. Contact to your Institution Admin to make your account active. You can then login with new password.");
            }
            else
            {
                status.SetCustomMessages(HttpStatusCode.NotFound,
                    "Invalid code or link is expired. Please try to regenerate forgot password link.");
            }
            return status;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("confirmemail")]
        public DefaultResponse ConfirmEmail(string userId, string code)
        {
            var status = new DefaultResponse();
            if (!ModelState.IsValid)
            {
                status.SetErrorMessages(this);
                return status;
            }
            var user = UserManager.FindById(userId);
            if (user == null || !user.IsActive || user.IsDeleted)
            {
                status.SetCustomMessages(HttpStatusCode.NotFound,
                    "We didn't find you are email address in our system.");
                return status;
            }
            var aspnetUser = Uow.UserRepository.GetById(user.Id);
            var result = UserManager.ConfirmEmail(userId, code);
            if (result.Succeeded)
            {
                status.SetCustomMessages(HttpStatusCode.OK,
                    "Your Email successfully confirmed. You can now login with your credentials.");
            }
            else
            {
                status.SetCustomMessages(HttpStatusCode.NotFound,
                    "Invalid code or link is expired. Please try to use forgot password link to confirm email.");
            }
            return status;
        }

        [AppAuth]
        [HttpPost]
        [Route("changepassword")]
        public DefaultResponse ChangePassword(ChangePasswordBindingModel model)
        {
            var status = new DefaultResponse();
            if (!ModelState.IsValid)
            {
                status.SetErrorMessages(this);
                return status;
            }
            var result = UserManager.ChangePassword(LoggedInUser.Id, model.OldPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                status.SetCustomMessages(HttpStatusCode.NotAcceptable, result.Errors);
            }
            else
            {
                status.SetCustomMessages(HttpStatusCode.OK, "Password changed successfully.");
            }
            return status;
        }

        [AppAuth]
        [HttpPost]
        [Route("updateprofile")]
        public GenericResponse<UpdateProfileModel> UpdateProfile(UpdateProfileModel model)
        {
            var status = new GenericResponse<UpdateProfileModel>();
            if (!ModelState.IsValid)
            {
                status.SetErrorMessages(this);
                return status;
            }
            var user = Uow.UserRepository.GetById(LoggedInUser.Id);
            if (user == null)
                status.SetCustomMessages(HttpStatusCode.NotFound, "User not found.");
            else
            {
                user.FirstName = model.FirstName;
                user.MiddleName = model.MiddleName;
                user.LastName = model.LastName;
                user.PhoneNumber = model.PhoneNumber;
                Uow.UserRepository.Update(user);
                Uow.Save(this);
                status.Data = model;
                status.SetCustomMessages(HttpStatusCode.OK, "Profile updated successfully.");
            }
            return status;
        }        

        [AllowAnonymous, HttpGet, Route("testmail")]
        public void testmail()
        {
            EmailProvider.SendMail("jay.ashara@techstern.com", null, null, "test", "test");
        }

        #region Helpers

        private IAuthenticationManager Authentication => Request.GetOwinContext().Authentication;

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (result.Succeeded)
                return null;
            if (result.Errors != null)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Error", error);
                }
            }

            var errors = new DefaultResponse();
            errors.SetErrorMessages(this);
            return Ok(errors.Messages);
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; private set; }
            public string ProviderKey { get; private set; }
            public string EmailAddress { get; private set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (!string.IsNullOrEmpty(EmailAddress))
                {
                    claims.Add(new Claim(ClaimTypes.Email, EmailAddress, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity, string provider)
            {
                var providerKeyClaim = identity?.FindFirst(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(providerKeyClaim?.Issuer) || string.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                var externalData = new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    EmailAddress = identity.FindFirstValue(ClaimTypes.Email)
                };

                if (provider == "Microsoft")
                {
                    externalData.EmailAddress = identity.FindFirstValue("urn:ms:userPrincipalName");
                }
                else if (provider == "Google")
                {
                    var email = identity.FindFirstValue("urn:google:emails");
                    if (email != null)
                    {
                        var emailObj = JsonConvert.DeserializeObject<dynamic>(email);
                        externalData.EmailAddress = Convert.ToString(emailObj[0].value);
                    }
                }

                return externalData.NullToEmpty();
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static readonly RandomNumberGenerator Random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", nameof(strengthInBits));
                }

                var strengthInBytes = strengthInBits / bitsPerByte;

                var data = new byte[strengthInBytes];
                Random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion
    }
}