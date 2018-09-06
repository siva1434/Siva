using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using _360LawGroup.CostOfSalesBilling.Models;
using _360LawGroup.CostOfSalesBilling.Utilities;
using _360LawGroup.CostOfSalesBilling.Web.Controllers;
using _360LawGroup.CostOfSalesBilling.Web.Helper;

namespace _360LawGroup.CostOfSalesBilling.Web.Areas.All.Controllers
{
    [WebAuth]
    public class UsersController : BaseController
    {
        // GET: Users
        public ActionResult Index(string selectedRole = null)
        {
            var roles = ApiHelper.Get<List<KeyValuePair<string, string>>>("api/all/users/getroles");
            ViewBag.Roles = roles.StatusCode == HttpStatusCode.OK ? roles.Data : new List<KeyValuePair<string, string>>();
            if (!IsValidRole(selectedRole))
                return Redirect("~/");
            return View();
        }

        [HttpGet]
        public ActionResult Create(string selectedRole = null)
        {
            ViewBag.SelectedRole = selectedRole;
            if (!IsValidRole(selectedRole))
               return Content("");
            return Edit(string.Empty);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            var model = new UserViewModel();
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.Title = "Create New User";
            }
            else
            {
                ViewBag.Title = "Edit User";
                var status = ApiHelper.Get<UserViewModel>("api/all/users/getbyid", id);
                if (status.StatusCode == HttpStatusCode.OK)
                {
                    model = status.Data;                    
                    ViewBag.SelectedRole = model.RoleId;
                }
                else
                    return Content("");
            }
            var roles = ApiHelper.Get<List<KeyValuePair<string, string>>>("api/all/users/getroles");
            ViewBag.Roles = roles.StatusCode == HttpStatusCode.OK ? roles.Data : new List<KeyValuePair<string, string>>();
            return PartialView("Editor", model);
        }

        [HttpGet]
        public ActionResult SetPassword(string id)
        {
            UserViewModel user = null;
            var status = ApiHelper.Get<UserViewModel>("api/all/users/getbyid", id);
            if (status.StatusCode == HttpStatusCode.OK)
                user = status.Data;
            return PartialView("_SetPassword", user != null ? new SetPasswordViewModel() { Id = id, UserName = user.UserName } : new SetPasswordViewModel() { Id = id });
        }
    }
}