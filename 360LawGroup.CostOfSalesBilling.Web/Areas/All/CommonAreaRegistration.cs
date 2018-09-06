using System.Web.Mvc;

namespace _360LawGroup.CostOfSalesBilling.Web.Areas.All
{
    public class CommonAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "All";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "All_default",
                "All/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "_360LawGroup.CostOfSalesBilling.Web.Areas.All.Controllers" }
            );
        }
    }
}