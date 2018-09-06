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
    [RoutePrefix("api/all/matter")]
    public class MatterController : BaseApiController
    {
         [Route("getall"), HttpPost]
        public GridData<MatterViewModel> GetAll(SearchModel model)
        {
            int total;
            var newmodel = model.Clone();
            var query = Uow.MatterRepository.GetQuery<MatterViewModel>(x => !x.IsDeleted);

            if (model.search.ContainsKey("SearchValue"))
            {
                var value = (model.search["SearchValue"] ?? string.Empty).ToLower();
                query = query.Where(x => x.AspNetUser1FullName.ToLower().Contains(value)
                || x.AreaofLaw.ToLower().Contains(value) || x.ConsultantId.Contains(value) || x.Status.ToLower().Contains(value));

                model.search.Remove("SearchValue");
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
            var instance = Uow.MatterRepository.GetQuery(x => x.Id == id).FirstOrDefault();
            if (instance == null)
                return matters;
            matters.StatusCode = HttpStatusCode.OK;
            matters.Messages = new List<string>();
            matters.Data = instance.To<MatterViewModel>(TimeZoneInterval);
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
            //var userCount = (Uow.MatterRepository.GetQuery(x => !x.IsDeleted).Count() + 1);
            var isError = false;
            ModelState.Remove("model.CreatedBy");
            ModelState.Remove("model.CreatedOn");
            if (ModelState.IsValid)
            {
                //if (Uow.ClientRepository.GetQuery(x => x.Id != model.Id && x.Email.Equals(model.Email, StringComparison.OrdinalIgnoreCase)).Any())
                //{
                //    ModelState.AddModelError("Email", "Email '" + model.Email + "' is already taken.");
                //    isError = true;
                //}
                if (model.Id == Guid.Empty) //create   
                {
                    var instance = model.To<Matter>(-TimeZoneInterval);
                    instance.Id = Guid.NewGuid();
                    instance.CreatedBy = LoggedInUser.Id;
                    instance.CreatedOn = DateTime.UtcNow;
                    instance.ModifiedBy = null;
                    instance.ModifiedOn = null;
                    instance.IsActive = true;
                    Uow.MatterRepository.Insert(instance);
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
                    var matter = model.To(extMatter, -TimeZoneInterval);
                    matter.CreatedBy = createdBy;
                    matter.CreatedOn = createdOn;
                    matter.ModifiedOn = DateTime.UtcNow;
                    matter.ModifiedBy = LoggedInUser.Id;
                    matter.IsActive = true;
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
                Uow.Save(this);
                return new DefaultResponse(HttpStatusCode.OK, "matter successfully saved.");
            }
            var res = new DefaultResponse();
            res.SetErrorMessages(this);
            return res;
        }
    }
}
