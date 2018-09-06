using _360LawGroup.CostOfSalesBilling.Data.Common;
using _360LawGroup.CostOfSalesBilling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace _360LawGroup.CostOfSalesBilling.Web.Controllers.Api.All
{
    [ApiAuth]
    [RoutePrefix("api/all/consultants")]
    public class ConsultantsController : BaseApiController
    {
        [Route("getall"), HttpPost]
        public GridData<ConsultantHourViewModel> GetAll(SearchModel model)
        {
            int total;
            var newmodel = model.Clone();
            var query = Uow.ConsultantHourRepository.GetQuery<ConsultantHourViewModel>(x => !x.IsDeleted);

            //if (model.search.ContainsKey("SearchValue"))
            //{
            //    var value = (model.search["SearchValue"] ?? string.Empty).ToLower();
            //    query = query.Where(x => x.FullName.ToLower().Replace(" ", "").Contains(value.Replace(" ", "")) || x.Company.ToLower().Contains(value)
            //    || x.Email.ToLower().Contains(value) || x.JobTitle.Contains(value) || x.BusinessPhone.ToLower().Contains(value));

            //    model.search.Remove("SearchValue");

            //}
            var list = query.ApplyFilter(model, out total);
            var gridData = new GridData<ConsultantHourViewModel>(list, model, total, TimeZoneInterval);
            return gridData;
        }
    }
}
