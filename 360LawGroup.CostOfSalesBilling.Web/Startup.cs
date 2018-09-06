using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

[assembly: OwinStartup(typeof(_360LawGroup.CostOfSalesBilling.Web.Startup))]

namespace _360LawGroup.CostOfSalesBilling.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseOwinExceptionHandler();
            ConfigureAuth(app);            
        }
    }
}
