using System;
using System.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using _360LawGroup.CostOfSalesBilling.Models;
using _360LawGroup.CostOfSalesBilling.Web.Providers;

namespace _360LawGroup.CostOfSalesBilling.Web
{
    public partial class Startup
    {

        public void ConfigureAuth(IAppBuilder app)
        {
            app.MapSignalR();

            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Configure the application for OAuth based flow
            PublicClientId = "360LawGroup";
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/360LawGroup/login"),
                Provider = new ApplicationOAuthProvider(PublicClientId),
                AuthorizeEndpointPath = new PathString("/account/360LawGroupextlogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromTicks(new DateTime(2999, 1, 1).Ticks),
                AllowInsecureHttp = true
            };

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);

            //app.UseGoogleAuthentication(GoogleOptions);

            //app.UseMicrosoftAccountAuthentication(MicrosoftOptions);
        }
        
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        /*public static GoogleOAuth2AuthenticationOptions GoogleOptions = new GoogleOAuth2AuthenticationOptions {
            ClientId = ConfigurationManager.AppSettings["Auth-Google:ClientId"],
            ClientSecret = ConfigurationManager.AppSettings["Auth-Google:ClientSecret"],
            Scope = { "https://www.googleapis.com/auth/userinfo.profile", "https://www.googleapis.com/auth/userinfo.email" },
            Provider = new GoogleOAuth2AuthenticationProvider {
                OnAuthenticated = (context) => {
                    context.Identity.AddClaim(new Claim("GoogleAccessToken", context.AccessToken));
                    foreach ( var claim in context.User ) {
                        var claimType = $"urn:google:{claim.Key}";
                        var claimValue = claim.Value.ToString();
                        if ( !context.Identity.HasClaim(claimType, claimValue) )
                            context.Identity.AddClaim(new Claim(claimType, claimValue, "XmlSchemaString", "Facebook"));

                    }
                    return Task.FromResult(0);
                }
            }
        };

        public static MicrosoftAccountAuthenticationOptions MicrosoftOptions = new MicrosoftAccountAuthenticationOptions {
            ClientId = ConfigurationManager.AppSettings["Auth-Microsoft:ClientId"],
            ClientSecret = ConfigurationManager.AppSettings["Auth-Microsoft:ClientSecret"],            
            Provider = new MicrosoftAccountAuthenticationProvider {
                OnAuthenticated = (context) => {
                    context.Identity.AddClaim(new Claim("MicrosoftAccessToken", context.AccessToken));
                    foreach ( var claim in context.User ) {
                        var claimType = $"urn:ms:{claim.Key}";
                        var claimValue = claim.Value.ToString();
                        if ( !context.Identity.HasClaim(claimType, claimValue) )
                            context.Identity.AddClaim(new Claim(claimType, claimValue, "XmlSchemaString", "Microsoft"));

                    }
                    return Task.FromResult(0);
                }
            }
        };
        */
        public static string PublicClientId { get; private set; }
    }
}
