using _360LawGroup.CostOfSalesBilling.Data;
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

            if (model.search.ContainsKey("SearchValue"))
            {
                var value = (model.search["SearchValue"] ?? string.Empty).ToLower();
                query = query.Where(x => x.AspNetUser2FullName.ToLower().Replace(" ", "").Contains(value.Replace(" ", ""))
                || x.ClientFullName.ToLower().Contains(value) ||
                x.Description.Contains(value));
                //|| x.BusinessPhone.ToLower().Contains(value));

                model.search.Remove("SearchValue");

            }
            var list = query.ApplyFilter(model, out total);
            var gridData = new GridData<ConsultantHourViewModel>(list, model, total, TimeZoneInterval);
            return gridData;
        }

        [Route("getbyid"), HttpGet]
        public GenericResponse<ConsultantHourViewModel> GetById(Guid id)
        {
            var clients = new GenericResponse<ConsultantHourViewModel>
            {
                StatusCode = HttpStatusCode.NotFound,
                Messages = new List<string>() { "Clients not found." }
            };
            var instance = Uow.ConsultantHourRepository.GetQuery(x => x.Id == id).FirstOrDefault();
            if (instance == null)
                return clients;
            clients.StatusCode = HttpStatusCode.OK;
            clients.Messages = new List<string>();
            clients.Data = instance.To<ConsultantHourViewModel>(TimeZoneInterval);
            return clients;
        }

        [Route("create"), HttpPost]
        public DefaultResponse Create(ConsultantHourViewModel model)
        {
            return ValidateAndSave(model);
        }

        [Route("edit"), HttpPost]
        public DefaultResponse Edit(ConsultantHourViewModel model)
        {
            return ValidateAndSave(model);
        }

        private DefaultResponse ValidateAndSave(ConsultantHourViewModel model)
        {
            var isError = false;
            ModelState.Remove("model.CreatedBy");
            ModelState.Remove("model.CreatedOn");
            if (ModelState.IsValid)
            {
                if (model.Id == Guid.Empty) //create   
                {
                    var instance = model.To<ConsultantHour>(-TimeZoneInterval);
                    instance.Id = Guid.NewGuid();
                    instance.CreatedBy = LoggedInUser.Id;
                    instance.CreatedOn = DateTime.UtcNow;
                    instance.ModifiedBy = null;
                    instance.ModifiedOn = null;
                    instance.IsActive = true;
                    foreach (var name in model.MatterNames)
                    {
                        var matter = Uow.MatterRepository.GetQuery(x => x.MatterName == name && !x.IsDeleted).FirstOrDefault();
                        instance.MatterId = matter.Id;
                    }

                    if (instance.ResetNewMonthId == Guid.Empty)
                        instance.ResetNewMonthId = null;
                    Uow.ConsultantHourRepository.Insert(instance);
                    isError = Uow.Save(this) == 0;
                    model = instance.To<ConsultantHourViewModel>(TimeZoneInterval);
                }
                else
                {
                    var query = Uow.ConsultantHourRepository.GetQuery(x => !x.IsDeleted && x.Id == model.Id);
                    var extClient = query.FirstOrDefault();
                    if (extClient == null)
                    {
                        return new DefaultResponse(HttpStatusCode.NotFound, "consultant hour not found for update.");
                    }
                    var createdBy = extClient.CreatedBy;
                    var createdOn = extClient.CreatedOn;
                    var client = model.To(extClient, -TimeZoneInterval);
                    client.CreatedBy = createdBy;
                    client.CreatedOn = createdOn;
                    client.ModifiedOn = DateTime.UtcNow;
                    client.ModifiedBy = LoggedInUser.Id;
                    client.IsActive = true;
                    Uow.ConsultantHourRepository.Update(extClient);
                    isError = Uow.Save(this) == 0;
                }
            }
            else
            {
                var res1 = new DefaultResponse();
                res1.SetErrorMessages(this);
                return res1;
            }
            if (!isError)
            {
                Uow.Save(this);
                return new DefaultResponse(HttpStatusCode.OK, "consultant hour successfully saved.");
            }
            var res = new DefaultResponse();
            res.SetErrorMessages(this);
            return res;
        }

        [Route("delete"), HttpGet]
        public DefaultResponse Delete(Guid id)
        {
            var status = new DefaultResponse(HttpStatusCode.OK, "consultant hour deleted successfully.");
            var query = Uow.ConsultantHourRepository.GetQuery(x => !x.IsDeleted && x.Id == id);
            var instance = query.FirstOrDefault();
            if (instance == null)
            {
                status.SetCustomMessages(HttpStatusCode.NotFound, "consultant hour not found.");
                return status;
            }
            instance.IsDeleted = true;
            Uow.ConsultantHourRepository.Update(instance);
            if (Uow.Save(this) == 0)
            {
                status.SetErrorMessages(this);
            }
            return status;
        }

        [Route("getconsultantcostsdata"), HttpPost]
        public GridData<ConsultantCostViewModel> GetConsultantsData(SearchModel model)
        {
            int total;
            var query = Uow.ConsultantCostRepository.GetQuery<ConsultantCostViewModel>();
            //if (model.search.ContainsKey("SearchValue"))
            //{
            //    var value = (model.search["SearchValue"] ?? string.Empty).ToLower();
            //    query = query.Where(x => x..ToLower().Contains(value)
            //    || x.Status.ToLower().Contains(value) || x.WorkRateRateType.Contains(value));

            //    model.search.Remove("SearchValue");

            //}
            //query = query.Where(x => x.WorkHours > 0);
            var list = query.ApplyFilter(model, out total);
            var gridData = new GridData<ConsultantCostViewModel>(list, model, total, TimeZoneInterval);
            return gridData;
        }
    }
}
