using _360LawGroup.CostOfSalesBilling.Web.Helper;
using _360LawGroup.CostOfSalesBilling.Data;
using _360LawGroup.CostOfSalesBilling.Data.Common;
using _360LawGroup.CostOfSalesBilling.Models;
using _360LawGroup.CostOfSalesBilling.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace _360LawGroup.CostOfSalesBilling.Web.Controllers.Api.All
{
    [ApiAuth]
    [RoutePrefix("api/all/client")]
    public class ClientsController : BaseApiController
    {
        [Route("getall"), HttpPost]
        public GridData<ClientViewModel> GetAll(SearchModel model)
        {
            int total;
            var newmodel = model.Clone();
            var query = Uow.ClientRepository.GetQuery<ClientViewModel>(x => !x.IsDeleted);

            if (model.search.ContainsKey("SearchValue"))
            {
                var value = (model.search["SearchValue"] ?? string.Empty).ToLower();
                query = query.Where(x => x.FullName.ToLower().Replace(" ", "").Contains(value.Replace(" ", "")) || x.Company.ToLower().Contains(value)
                || x.Email.ToLower().Contains(value) || x.JobTitle.Contains(value) || x.BusinessPhone.ToLower().Contains(value));

                model.search.Remove("SearchValue");

            }
            var list = query.ApplyFilter(model, out total);
            var gridData = new GridData<ClientViewModel>(list, model, total, TimeZoneInterval);
            return gridData;
        }

        [Route("getbyid"), HttpGet]
        public GenericResponse<ClientViewModel> GetById(Guid id)
        {
            var clients = new GenericResponse<ClientViewModel>
            {
                StatusCode = HttpStatusCode.NotFound,
                Messages = new List<string>() { "Clients not found." }
            };
            var instance = Uow.ClientRepository.GetQuery(x => x.Id == id).FirstOrDefault();
            if (instance == null)
                return clients;
            clients.StatusCode = HttpStatusCode.OK;
            clients.Messages = new List<string>();
            clients.Data = instance.To<ClientViewModel>(TimeZoneInterval);
            return clients;
        }


        [Route("getclientrenewalslist"), HttpGet]
        public List<EventObject<ClientViewModel>> GetClientRenewalsData(DateTime start, DateTime end)
        {
            var status = new GenericResponse<List<EventObject<ClientViewModel>>>();
            var list = Uow.ClientRepository.GetQuery(x => DbFunctions.TruncateTime(x.ContractRenewalDate) >= start && DbFunctions.TruncateTime(x.ContractRenewalDate) <= end).ToList();

            var newlist = list.Select(x => new EventObject<ClientViewModel>
            {
                id = x.Id.ToString(),
                title = x.Company,
                start = x.ContractRenewalDate.GetValueOrDefault(),
                end = x.ContractRenewalDate.GetValueOrDefault(),
                allDay = true,
                otherInfo = x.To<ClientViewModel>(TimeZoneInterval)
            }).ToListMap<EventObject<ClientViewModel>>(TimeZoneInterval);
            return newlist;
        }
        [Route("getallsubscriptionclient"), HttpPost]
        public GridData<ClientViewModel> GetSubscriptionClients(SearchModel model)
        {
            int total;
            var newmodel = model.Clone();
            var query = Uow.ClientRepository.GetQuery<ClientViewModel>(x => !x.IsDeleted && x.IsSubscription);

            if (model.search.ContainsKey("SearchValue"))
            {
                var value = (model.search["SearchValue"] ?? string.Empty).ToLower();
                //query = query.Where(x => x.FullName.ToLower().Replace(" ", "").Contains(value.Replace(" ", "")) || x.Company.ToLower().Contains(value)
                //|| x.Email.ToLower().Contains(value) || x.JobTitle.Contains(value) || x.BusinessPhone.ToLower().Contains(value));

                model.search.Remove("SearchValue");

            }
            var list = query.ApplyFilter(model, out total);
            foreach(var item in list)
            {
                item.TotalMonthlySubscrptionForAll = list.Sum(x => x.MonthlySubscription);
            }
            var gridData = new GridData<ClientViewModel>(list, model, total, TimeZoneInterval);
            return gridData;
        }
        [Route("create"), HttpPost]
        public DefaultResponse Create(ClientViewModel model)
        {
            return ValidateAndSave(model);
        }

        [Route("edit"), HttpPost]
        public DefaultResponse Edit(ClientViewModel model)
        {
            return ValidateAndSave(model);
        }


        private DefaultResponse ValidateAndSave(ClientViewModel model)
        {
            var userCount = (Uow.ClientRepository.GetQuery(x => !x.IsDeleted).Count() + 1);
            var isError = false;
            ModelState.Remove("model.CreatedBy");
            ModelState.Remove("model.CreatedOn");
            if (ModelState.IsValid)
            {
                if (Uow.ClientRepository.GetQuery(x => x.Id != model.Id && x.Email.Equals(model.Email, StringComparison.OrdinalIgnoreCase)).Any())
                {
                    ModelState.AddModelError("Email", "Email '" + model.Email + "' is already taken.");
                    isError = true;
                }
                else if (model.Id == Guid.Empty) //create   
                {
                    var instance = model.To<Client>(-TimeZoneInterval);
                    instance.Id = Guid.NewGuid();
                    instance.CreatedBy = LoggedInUser.Id;
                    instance.CreatedOn = DateTime.UtcNow;
                    instance.ModifiedBy = null;
                    instance.ModifiedOn = null;
                    instance.IsActive = true;
                    Uow.ClientRepository.Insert(instance);
                    isError = Uow.Save(this) == 0;
                    model = instance.To<ClientViewModel>(TimeZoneInterval);
                }
                else
                {
                    var query = Uow.ClientRepository.GetQuery(x => !x.IsDeleted && x.Id == model.Id);
                    var extClient = query.FirstOrDefault();
                    if (extClient == null)
                    {
                        return new DefaultResponse(HttpStatusCode.NotFound, "client not found for update.");
                    }
                    var createdBy = extClient.CreatedBy;
                    var createdOn = extClient.CreatedOn;
                    var client = model.To(extClient, -TimeZoneInterval);
                    client.CreatedBy = createdBy;
                    client.CreatedOn = createdOn;
                    client.ModifiedOn = DateTime.UtcNow;
                    client.ModifiedBy = LoggedInUser.Id;
                    client.IsActive = true;
                    Uow.ClientRepository.Update(extClient);
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
                return new DefaultResponse(HttpStatusCode.OK, "client successfully saved.");
            }
            var res = new DefaultResponse();
            res.SetErrorMessages(this);
            return res;
        }

        [Route("delete"), HttpGet]
        public DefaultResponse Delete(Guid id)
        {
            var status = new DefaultResponse(HttpStatusCode.OK, "client deleted successfully.");
            var query = Uow.ClientRepository.GetQuery(x => !x.IsDeleted && x.Id == id);
            var instance = query.FirstOrDefault();
            if (instance == null)
            {
                status.SetCustomMessages(HttpStatusCode.NotFound, "client not found.");
                return status;
            }
            instance.IsDeleted = true;
            Uow.ClientRepository.Update(instance);
            if (Uow.Save(this) == 0)
            {
                status.SetErrorMessages(this);
            }
            return status;
        }
    }
}
