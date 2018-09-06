using _360LawGroup.CostOfSalesBilling.Web.Helper;
using _360LawGroup.CostOfSalesBilling.Data.Common;
using _360LawGroup.CostOfSalesBilling.Models;
using _360LawGroup.CostOfSalesBilling.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace _360LawGroup.CostOfSalesBilling.Web.Api.Controllers.SuperAdmin
{
    [AppAuth(RoleExtension.SuperAdmin)]
    [RoutePrefix("superadmin/adminsettings")]
    public class AdminSettingsController : BaseApiController
    {
        [Route("getall"), HttpPost]
        public GridData<AdminSettingsViewModel> GetAll(SearchModel model)
        {
            int total;
            var query = Uow.AdminSettingsRepository.GetQuery<AdminSettingsViewModel>();
            var list = query.ApplyFilter(model, out total);
            var gridData = new GridData<AdminSettingsViewModel>(list, model, total, TimeZoneInterval);
            return gridData;
        }

        [Route("saveadminsettings"), HttpGet]
        public DefaultResponse SaveAdminSettings(string paramName, string paramValue)
        {
            var status = new DefaultResponse(HttpStatusCode.OK, "Admin Settings Updated");
            var instance = Uow.AdminSettingsRepository.GetQuery(x => x.ParamName == paramName).FirstOrDefault();
            if (instance == null)
            {
                status.SetCustomMessages(HttpStatusCode.NotFound, "Errors Not Found");
                return status;
            }
            instance.ParamValue = paramValue;
            Uow.AdminSettingsRepository.Update(instance);
            if (Uow.Save(this) == 0)
            {
                status.SetErrorMessages(this);
            }
            status.SetCustomMessages(HttpStatusCode.OK, "Settings saved successfully.");
            return status;
        }
    }
}
