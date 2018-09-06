using System.Web.Mvc;
using _360LawGroup.CostOfSalesBilling.Utilities;
using _360LawGroup.CostOfSalesBilling.Web.Controllers;

namespace _360LawGroup.CostOfSalesBilling.Web.Areas.SuperAdmin.Controllers
{
    [AppAuth( RoleExtension.SuperAdmin)]
    public class HomeController : BaseController
    {
        // GET: SuperAdmin/Home
        public ActionResult Index()
        {
            return View();
        }   
        
       
    }
}