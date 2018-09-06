using System.Web.Mvc;

namespace _360LawGroup.CostOfSalesBilling.Web.Areas.ClientUser
{
    public class ClientUserAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ClientUser";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ClientUser_default",
                "ClientUser/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}