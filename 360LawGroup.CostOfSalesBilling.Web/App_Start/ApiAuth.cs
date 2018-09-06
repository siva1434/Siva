using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using _360LawGroup.CostOfSalesBilling.Models;
using Microsoft.AspNet.Identity;
using _360LawGroup.CostOfSalesBilling.Data;

namespace _360LawGroup.CostOfSalesBilling.Web
{
    public class ApiAuthAttribute : AuthorizeAttribute
    {

        public ApiAuthAttribute(params string[] roles)
        {
            this.Roles = string.Join(",", roles);
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    base.OnAuthorization(actionContext);
                    return;
                }
                using (var Uow = new UnitOfWorkCore())
                {
                    var user = Uow.UserRepository.GetById(userId);
                    if (user == null || !user.IsActive || user.IsDeleted)
                    {
                        var status = new DefaultResponse { StatusCode = HttpStatusCode.NonAuthoritativeInformation };
                        status.Messages = new List<string> { "Your account deactivated. Please contact administrator for more info." };
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.OK, status);
                    }
                    else
                    {
                        base.OnAuthorization(actionContext);
                    }
                }
            }
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            var status = new DefaultResponse { StatusCode = HttpStatusCode.Unauthorized };
            status.Messages = new List<string> { "Unauthorized Access. Please contact administrator for more info." };
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.OK, status);
        }
    }
}