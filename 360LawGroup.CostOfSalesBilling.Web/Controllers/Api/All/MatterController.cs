using _360LawGroup.CostOfSalesBilling.Data;
using _360LawGroup.CostOfSalesBilling.Data.Common;
using _360LawGroup.CostOfSalesBilling.Models;
using _360LawGroup.CostOfSalesBilling.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace _360LawGroup.CostOfSalesBilling.Web.Controllers.Api.All
{
    [ApiAuth]
    [RoutePrefix("api/all/matter")]
    public class MatterController : BaseApiController
    {
        [Route("getall"), HttpPost]
        public GridData<MatterViewModel> GetAll(SearchModel model)
        {
            int total;
            var query = Uow.MatterRepository.GetQuery<MatterViewModel>(x => !x.IsDeleted);
            //if (model.search.ContainsKey("SearchValue"))
            //{
            //    var value = (model.search["SearchValue"] ?? string.Empty).ToLower();
            //    query = query.Where(x => x.ClientFullName.ToLower().Contains(value) || x.MatterName.ToLower().Contains(value) || x.Box.ToLower().Contains(value) ||
            //    x.AspNetUsers.Any(i => i.FullName.Contains(value)) || x.AreaofLaw.ToLower().Contains(value) || x.WorkRateRateType.Contains(value) || x.Status.ToLower().Contains(value));

            //    model.search.Remove("SearchValue");
            //}
            if (model.search.ContainsKey("ConsultantIds"))
            {
                var ids = (model.search["ConsultantIds"] ?? string.Empty).Split(new[] { ',' },
                    StringSplitOptions.RemoveEmptyEntries);
                if (ids.Any())
                    foreach (var id in ids)
                        query = query.Where(x => x.AspNetUsers.Any(y => y.Id == id));
                model.search.Remove("ConsultantIds");
            }
            var list = query.ApplyFilter(model, out total);
            var gridData = new GridData<MatterViewModel>(list, model, total, TimeZoneInterval);
            return gridData;
        }

        [Route("getbyid"), HttpGet]
        public GenericResponse<MatterViewModel> GetById(Guid id)
        {
            var matters = new GenericResponse<MatterViewModel>
            {
                StatusCode = HttpStatusCode.NotFound,
                Messages = new List<string>() { "Matters not found." }
            };

            var data = new MatterViewModel();
            var instance = Uow.MatterRepository.GetQuery(x => x.Id == id).FirstOrDefault();

            if (instance == null)
                return matters;
            matters.StatusCode = HttpStatusCode.OK;
            matters.Messages = new List<string>();
            matters.Data = instance.To<MatterViewModel>(TimeZoneInterval);
            matters.Data.ConsultantIds = instance.AspNetUsers.Select(x => x.Id).ToList();
            return matters;
        }

        [Route("create"), HttpPost]
        public DefaultResponse Create(MatterViewModel model)
        {
            return ValidateAndSave(model);
        }

        [Route("edit"), HttpPost]
        public DefaultResponse Edit(MatterViewModel model)
        {
            return ValidateAndSave(model);
        }


        private DefaultResponse ValidateAndSave(MatterViewModel model)
        {
            var isError = false;
            ModelState.Remove("model.CreatedBy");
            ModelState.Remove("model.CreatedOn");
            Matter instance = null;
            var oldShareEmailList = new List<string>();
            var newShareEmailList = new List<string>();
            if (ModelState.IsValid)
            {
                if (Uow.MatterRepository.GetQuery(x => x.Id != model.Id && x.MatterName.Equals(model.MatterName, StringComparison.OrdinalIgnoreCase)).Any())
                {
                    ModelState.AddModelError("Name", "Name '" + model.MatterName + "' is exists.");
                    isError = true;
                }
                else if (model.Id == Guid.Empty) //create   
                {
                    instance = model.To<Matter>(-TimeZoneInterval);
                    instance.Id = Guid.NewGuid();
                    instance.CreatedBy = LoggedInUser.Id;
                    instance.CreatedOn = DateTime.UtcNow;
                    instance.ModifiedBy = null;
                    instance.ModifiedOn = null;
                    instance.IsActive = true;
                    foreach (var id in model.ConsultantIds)
                    {
                        var consult = Uow.UserRepository.GetById(id);
                        instance.AspNetUsers.Add(consult);
                        newShareEmailList.Add(consult.Email);
                    }
                    Uow.MatterRepository.Insert(instance);
                    //var client = Uow.ClientRepository.GetById(model.ClientId);
                    //if (client != null) newShareEmailList.Add(client.Email);
                    newShareEmailList.Add(LoggedInUser.Email);
                    isError = Uow.Save(this) == 0;
                    model = instance.To<MatterViewModel>(TimeZoneInterval);
                }
                else
                {
                    var query = Uow.MatterRepository.GetQuery(x => !x.IsDeleted && x.Id == model.Id);
                    var extMatter = query.FirstOrDefault();
                    if (extMatter == null)
                    {
                        return new DefaultResponse(HttpStatusCode.NotFound, "matter not found for update.");
                    }

                    var createdBy = extMatter.CreatedBy;
                    var createdOn = extMatter.CreatedOn;

                    foreach (var o in extMatter.AspNetUsers)
                        oldShareEmailList.Add(o.Email);
                    oldShareEmailList.Add(extMatter.AspNetUser.Email);
                    //oldShareEmailList.Add(extMatter.Client.Email);

                    extMatter.AspNetUsers.Clear();
                    instance = model.To(extMatter, -TimeZoneInterval);
                    instance.CreatedBy = createdBy;
                    instance.CreatedOn = createdOn;
                    instance.ModifiedOn = DateTime.UtcNow;
                    instance.ModifiedBy = LoggedInUser.Id;
                    instance.IsActive = true;
                    foreach (var id in model.ConsultantIds)
                    {
                        var consult = Uow.UserRepository.GetById(id);
                        instance.AspNetUsers.Add(consult);
                        newShareEmailList.Add(consult.Email);
                    }
                    //var client = Uow.ClientRepository.GetById(model.ClientId);
                    //if (client != null) newShareEmailList.Add(client.Email);
                    newShareEmailList.Add(LoggedInUser.Email);
                    Uow.MatterRepository.Update(extMatter);
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
                var matterFolder = OneDriveHelper.CreateClientMatterFolderIfNotExist(instance.MatterName);
                instance.Box = matterFolder.WebUrl;
                Uow.MatterRepository.Update(instance);
                Uow.Save(this);
                // share permission logic
                var oldPermissionDeleteList = oldShareEmailList.Where(x => !newShareEmailList.Contains(x)).ToList();
                var newPermissionAddList = newShareEmailList.Where(x => !oldPermissionDeleteList.Contains(x)).ToList();

                // update permision for folder
                matterFolder.UpdateFolderPermissions(newPermissionAddList);

                return new DefaultResponse(HttpStatusCode.OK, "matter successfully saved.");
            }
            var res = new DefaultResponse();
            res.SetErrorMessages(this);
            return res;
        }

        [Route("getcurrentmatterdata"), HttpPost]
        public GridData<MatterViewModel> GetCurrentMatterData(SearchModel model)
        {
            int total;
            var query = Uow.MatterRepository.GetQuery<MatterViewModel>(x => !x.IsDeleted);
            if (model.search.ContainsKey("ConsultantIds"))
            {
                var ids = (model.search["ConsultantIds"] ?? string.Empty).Split(new[] { ',' },
                    StringSplitOptions.RemoveEmptyEntries);
                if (ids.Any())
                    foreach (var id in ids)
                        query = query.Where(x => x.AspNetUsers.Any(y => y.Id == id));
                model.search.Remove("ConsultantIds");
            }
            if (model.search.ContainsKey("SearchValue"))
            {
                var value = (model.search["SearchValue"] ?? string.Empty).ToLower();
                query = query.Where(x => x.Client_Matter.ToLower().Contains(value)
                || x.Status.ToLower().Contains(value) || x.WorkRateRateType.Contains(value));

                model.search.Remove("SearchValue");

            }
            query = query.Where(x => x.WorkHours > 0);
            var list = query.ApplyFilter(model, out total);
            var gridData = new GridData<MatterViewModel>(list, model, total, TimeZoneInterval);
            return gridData;
        }

        [Route("getcurrentconsultantbymatters"), HttpPost]
        public GridData<MatterViewModel> GetCurrentConsultantByMatters(SearchModel model)
        {
            int total;
            var query = Uow.MatterRepository.GetQuery<MatterViewModel>(x => !x.IsDeleted && x.Status != "Completed");
            if (UserIsInRole(RoleExtension.Consultant))
            {
                query = query.Where(x => x.AspNetUsers.Any(i => i.Id == LoggedInUser.Id));
            }
            if (model.search.ContainsKey("ConsultantIds"))
            {
                var ids = (model.search["ConsultantIds"] ?? string.Empty).Split(new[] { ',' },
                    StringSplitOptions.RemoveEmptyEntries);
                if (ids.Any())
                    foreach (var id in ids)
                        query = query.Where(x => x.AspNetUsers.Any(y => y.Id == id));
                model.search.Remove("ConsultantIds");
            }
            var list = query.ApplyFilter(model, out total);
            var gridData = new GridData<MatterViewModel>(list, model, total, TimeZoneInterval);
            return gridData;
        }
    }
}
