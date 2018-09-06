using System.Web.Mvc;

namespace _360LawGroup.CostOfSalesBilling.Web.Areas.Consultant
{
    public class ConsultantAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Consultant";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Consultant_default",
                "Consultant/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}