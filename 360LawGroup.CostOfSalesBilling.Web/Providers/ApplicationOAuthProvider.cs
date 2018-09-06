using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _360LawGroup.CostOfSalesBilling.Data;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using _360LawGroup.CostOfSalesBilling.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;

namespace _360LawGroup.CostOfSalesBilling.Web.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            _publicClientId = publicClientId;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();
            var userName = context.UserName ?? string.Empty;
            if (userName.Contains("@"))
            {
                var userFound = userManager.FindByEmail(userName);
                if (userFound != null)
                    userName = userFound.UserName;
            }
            var user = await userManager.FindAsync(userName, context.Password);
            if (user == null || user.IsDeleted)
            {
                context.SetError("invalid_grant", "invlid grant");
                context.Response.Headers.Add("X-Unauthorized", new[] { "Invalid username or password." });
                return;
            }
            if (!user.IsActive)
            {
                context.SetError("invalid_grant", "invlid grant");
                context.Response.Headers.Add("X-Unauthorized", new[] { "User is inactive or not approved by administrator. Please contact administrator for more information." });
                return;
            }
            using (var uow = new UnitOfWorkCore())
            {
                var aspnetUser = uow.UserRepository.GetById(user.Id);
                if (!user.EmailConfirmed)
                {
                    context.SetError("invalid_grant", "invlid grant");
                    context.Response.Headers.Add("X-Unauthorized", new[] { "Your email address is not confirmed. We already sent you an email. Please check your inbox. If you didn't get any email, you can regenerate by using forgot password." });
                    return;
                }
                var oAuthIdentity = await user.GenerateUserIdentityAsync(userManager,
                    OAuthDefaults.AuthenticationType);
                var cookiesIdentity = await user.GenerateUserIdentityAsync(userManager,
                    CookieAuthenticationDefaults.AuthenticationType);
                var properties = CreateProperties(aspnetUser);
                var ticket = new AuthenticationTicket(oAuthIdentity, properties);
                context.Validated(ticket);
                context.Request.Context.Authentication.SignIn(cookiesIdentity);
                context.Response.Headers.Add("X-Authorized", new[] { "" });
            }
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (var property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                //Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                //if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(AspNetUser u)
        {

            var userProfile = new UserViewModel()
            {
                FirstName = u.FirstName,
                MiddleName = u.MiddleName,
                LastName = u.LastName,
                Gender = u.Gender,
                Email = u.Email,
                Id = u.Id,
                UserName = u.UserName,
                RoleId = u.AspNetRoles.Select(x => x.Id).FirstOrDefault(),
                EmailConfirmed = u.EmailConfirmed,
                PhoneNumber = u.PhoneNumber,
                PhoneNumberConfirmed = u.PhoneNumberConfirmed,
                Address = u.Address,
                IsActive = u.IsActive,
                ModifiedOn = u.ModifiedOn,
                CreatedBy = u.CreatedBy,
                ModifiedBy = u.ModifiedBy,
                CreatedOn = u.CreatedOn,
            };

            var data = new Dictionary<string, string>
            {
                { "profile", JsonConvert.SerializeObject(userProfile) },
                { "role", userProfile.RoleId },
                { "error", "" }
            };
            return new AuthenticationProperties(data);
        }
    }
}