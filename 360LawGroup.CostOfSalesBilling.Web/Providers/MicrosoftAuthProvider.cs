using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security.MicrosoftAccount;

namespace _360LawGroup.CostOfSalesBilling.Web.Providers {
    public class MicrosoftAuthProvider : MicrosoftAccountAuthenticationProvider {
        public override Task Authenticated(MicrosoftAccountAuthenticatedContext context) {
            context.Identity.AddClaim(new Claim("ExternalAccessToken", context.AccessToken));
            return Task.FromResult<object>(null);
        }
    }
}