using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using _360LawGroup.CostOfSalesBilling.Models;
using Microsoft.AspNet.Identity;
using _360LawGroup.CostOfSalesBilling.Data;
using Newtonsoft.Json;
using System.Web;
using System.Linq;
using System.Web.Mvc;

namespace _360LawGroup.CostOfSalesBilling.Web
{
    public class WebAuthAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        private List<string> WebRoles = new List<string>();

        public WebAuthAttribute()
        {

        }

        public WebAuthAttribute(params string[] roles)
        {
            this.WebRoles = roles.ToList();
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            var loginCookies = WebCookie.Get("WebLogin");
            if (loginCookies != null)
            {
                var token = JsonConvert.DeserializeObject<Token>(loginCookies);
                if (token != null)
                {
                    var profile = JsonConvert.DeserializeObject<UserViewModel>(token.profile);
                    if (profile != null)
                    {
                        var userId = profile.Id;
                        if (!string.IsNullOrEmpty(userId))
                        {
                            authorize = WebRoles == null || WebRoles.Count == 0 || WebRoles.Contains(profile.RoleId);
                        }
                    }
                }
            }
            return authorize;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new HttpUnauthorizedResult();
        }
    }
}