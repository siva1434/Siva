using HealthFreak.Api.Helper;
using HealthFreak.Data;
using HealthFreak.Data.Common;
using HealthFreak.Models;
using HealthFreak.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HealthFreak.Api.Controllers.SuperAdmin
{
    [AppAuth( RoleExtension.SuperAdmin)]
    [RoutePrefix("superadmin/locations")]
    public class LocationsController : BaseApiController
    {
        [Route("getall"), HttpPost]
        public GridData<LocationsViewModel> GetAll(SearchModel model)
        {
            int total;
            var query = Uow.LocationRepository.GetQuery<LocationsViewModel>(x => !x.IsDeleted
                    && !x.Country.IsDeleted && !x.State.IsDeleted && !x.City.IsDeleted);
            var list = query.ApplyFilter(model, out total);
            var gridData = new GridData<LocationsViewModel>(list, model, total, TimeZoneInterval);
            return gridData;
        }

        [Route("GetById"), HttpGet]
        public GenericResponse<LocationsViewModel> GetById(long id)
        {
            var status = new GenericResponse<LocationsViewModel>
            {
                StatusCode = HttpStatusCode.NotFound,
                Messages = new List<string>() { "Branch not found." }
            };
            var instance = Uow.LocationRepository.GetQuery(x => !x.IsDeleted
                    && !x.Country.IsDeleted && x.Id == id && !x.State.IsDeleted && !x.City.IsDeleted).FirstOrDefault();
            if (instance == null)
                return status;
            status.StatusCode = HttpStatusCode.OK;
            status.Messages = new List<string>();
            status.Data = instance.To<LocationsViewModel>(TimeZoneInterval);
            return status;
        }

        [Route("create"), HttpPost]
        public GenericResponse<LocationsViewModel> Create(LocationsViewModel model)
        {
            return ValidateAndSave(model);
        }

        [Route("edit"), HttpPost]
        public GenericResponse<LocationsViewModel> Edit(LocationsViewModel model)
        {
            return ValidateAndSave(model);
        }

        private GenericResponse<LocationsViewModel> ValidateAndSave(LocationsViewModel model)
        {
            var status = new GenericResponse<LocationsViewModel>();
            var isError = false;
            ModelState.Remove("model.CreatedOn");
            ModelState.Remove("model.CreatedBy");
            if (ModelState.IsValid)
            {
                //check country and state not deleted and equal with same name if any found then city exist
                if (Uow.LocationRepository.GetQuery(x => x.Id != model.Id && x.CountryId == model.CountryId && x.StateId == model.StateId &&
                x.CityId == model.CityId &&
                    x.Name.Equals(model.Name, StringComparison.OrdinalIgnoreCase) && !x.IsDeleted && !x.State.IsDeleted &&
                    !x.Country.IsDeleted).Any())

                {
                    ModelState.AddModelError("Name", "Branch already exists with selected Country and State and City.");
                    isError = true;
                }
                //check state is not deleted and Active with same State id if any not found then State not exist
                else if (!Uow.StateRepository.GetQuery(x => !x.IsDeleted && x.IsActive && x.Id == model.StateId).Any())
                {
                    ModelState.AddModelError("Name", "Selected state is not exist.");
                    isError = true;
                }
                //check country is not deleted and Active with same country id if any not found then country not exist
                else if (!Uow.CountryRepository.GetQuery(x => !x.IsDeleted && x.IsActive && x.Id == model.CountryId).Any())
                {
                    ModelState.AddModelError("Name", "Selected country is not exist.");
                    isError = true;
                }

                //check country is not deleted and Active with same City id if any not found then City not exist
                else if (!Uow.CityRepository.GetQuery(x => !x.IsDeleted && x.IsActive && x.Id == model.CityId).Any())
                {
                    ModelState.AddModelError("Name", "Selected City is not exist.");
                    isError = true;
                }

                else if (model.Id == 0)//create
                {
                    var instance = model.To<Location>(-TimeZoneInterval);
                    instance.CreatedBy = LoggedInUser.Id;
                    instance.CreatedOn = DateTime.UtcNow;
                    instance.ModifiedBy = null;
                    instance.ModifiedOn = null;
                    Uow.LocationRepository.Insert(instance);
                    isError = Uow.Save(this) == 0;
                    model = instance.To<LocationsViewModel>(TimeZoneInterval);
                }
                else//edit
                {
                    var query = Uow.LocationRepository.GetQuery(x => !x.IsDeleted && x.Id == model.Id);
                    var Extlocation = query.FirstOrDefault();
                    if (Extlocation == null)
                    {
                        status.SetCustomMessages(HttpStatusCode.NotFound, "Branch not found for update.");
                        return status;
                    }
                    var createdBy = Extlocation.CreatedBy;
                    var createdOn = Extlocation.CreatedOn;
                    var instance = model.To(Extlocation, -TimeZoneInterval);

                    instance.CreatedBy = createdBy;
                    instance.CreatedOn = createdOn;
                    instance.ModifiedOn = DateTime.UtcNow;
                    instance.ModifiedBy = LoggedInUser.Id;
                    Uow.LocationRepository.Update(instance);
                    isError = Uow.Save(this) == 0;
                    model = instance.To<LocationsViewModel>(TimeZoneInterval);
                }
            }
            else
            {
                isError = true;
            }
            if (!isError)
            {
                status.Data = model;
                status.SetCustomMessages(HttpStatusCode.OK, "Branch successfully saved.");
                return status;
            }
            status.SetErrorMessages(this);
            return status;

        }

        [Route("delete"), HttpGet]
        public DefaultResponse Delete(long id)
        {
            var status = new DefaultResponse(HttpStatusCode.OK, "Branch deleted successfully.");

            var query = Uow.LocationRepository.GetQuery(x => !x.IsDeleted && !x.State.IsDeleted &&  
                    !x.Country.IsDeleted && !x.City.IsDeleted &&  x.Id == id);

            var instance = query.FirstOrDefault();
            if (instance == null)
            {
                status.SetCustomMessages(HttpStatusCode.NotFound, "Branch not found.");
                return status;
            }
            instance.IsDeleted = true;
            Uow.LocationRepository.Update(instance);
            if (Uow.Save(this) == 0)
            {
                status.SetErrorMessages(this);
            }
            return status;
        }

    }
}
