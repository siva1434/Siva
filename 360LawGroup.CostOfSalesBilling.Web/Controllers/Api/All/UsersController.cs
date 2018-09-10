using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web.Http;
using _360LawGroup.CostOfSalesBilling.Web.Helper;
using _360LawGroup.CostOfSalesBilling.Data;
using _360LawGroup.CostOfSalesBilling.Data.Common;
using _360LawGroup.CostOfSalesBilling.Models;
using _360LawGroup.CostOfSalesBilling.Utilities;
using Microsoft.AspNet.Identity;

namespace _360LawGroup.CostOfSalesBilling.Web.Controllers.Api.All
{
    [ApiAuth]
    [RoutePrefix("api/all/users")]
    public class UsersController : BaseApiController
    {
        [Route("getall"), HttpPost]
        public GridData<UserViewModel> GetAll(SearchModel model, string roleId)
        {
            int total;
            var mainquery = Uow.UserRepository.GetQuery<UserViewModel>(x => !x.IsDeleted && x.AspNetRoles.Any(y => y.Id == roleId));
            /* if (model.search.ContainsKey("SearchValue"))
             {
                 var value = (model.search["SearchValue"] ?? string.Empty).ToLower();
                 mainquery = mainquery.Where(x => x.FullName.ToLower().Replace(" ", "").Contains(value.Replace(" ", "")) ||
                   x.Email.ToLower().Contains(value) || x.ConsultantName.Contains(value) || x.AreaofLaw.ToLower().Contains(value)
                   || x.JurisdictionsCovered.ToLower().Contains(value) || x.AspNetUser2FullName.ToLower().Contains(value) ||
                   x.UserStatus.ToLower().Contains(value)
                   );
                 model.search.Remove("SearchValue");
             }*/
            var list = mainquery.ApplyFilter(model, out total);
            var gridData = new GridData<UserViewModel>(list, model, total, TimeZoneInterval);
            return gridData;
        }


        [Route("getbyid"), HttpGet]
        public GenericResponse<UserViewModel> GetById(string id)
        {
            var status = new GenericResponse<UserViewModel>
            {
                StatusCode = HttpStatusCode.NotFound,
                Messages = new List<string>() { "User not found." }
            };
            var roleAcess = RoleExtension.GetRoleAccessList(RoleId);
            var query = Uow.UserRepository.GetQuery(x => !x.IsDeleted
                        && x.Id == id && x.AspNetRoles.Any(y => roleAcess.Contains(y.Id)));
            var u = query.FirstOrDefault();
            if (u == null)
                return status;
            status.StatusCode = HttpStatusCode.OK;
            status.Messages = new List<string>();
            status.Data = new UserViewModel()
            {
                FullName = u.FullName,
                FirstName = u.FirstName,
                MiddleName = u.MiddleName,
                LastName = u.LastName,
                Gender = u.Gender,
                Email = u.Email,
                Id = u.Id,
                UserName = u.UserName,
                RoleId = u.AspNetRoles.Select(x => x.Id).FirstOrDefault(),
                EmailConfirmed = u.EmailConfirmed,
                PhoneNumber = u.PhoneNumber,
                PinCode = u.PinCode,
                PhoneNumberConfirmed = u.PhoneNumberConfirmed,
                IsActive = u.IsActive,
                ModifiedOn = u.ModifiedOn,
                Address = u.Address,
                CreatedBy = u.CreatedBy,
                // AspNetUserFullName = u.AspNetUser1.FullName,
                ModifiedBy = u.ModifiedBy,
                AspNetUser1FullName = u.AspNetUser2?.FullName,
                CreatedOn = u.CreatedOn,
                ConsultantCompany = u.ConsultantCompany,
                AreaofLaw = u.AreaofLaw,
                JurisdictionsCovered = u.JurisdictionsCovered,
                BusinessPhone = u.BusinessPhone,
                EmailAddress_360BusinessLaw = u.EmailAddress_360BusinessLaw,
                ConsultantUserId = u.ConsultantUserId,
                EmailAddress_360LawServices = u.EmailAddress_360LawServices,
                EmailAddress_PrivateClientLaw = u.EmailAddress_PrivateClientLaw,
                City = u.City,
                Country = u.Country,
                HomePhone = u.HomePhone,                
                Notes = u.Notes,
                Attachments = u.Attachments,
                SubscriptionHourlyRate = u.SubscriptionHourlyRate,
                MemberHourlyRate = u.MemberHourlyRate,
                PrivateClientHourlyRate = u.PrivateClientHourlyRate,
                LitigationHourlyRate = u.LitigationHourlyRate,
                RegulatedHourlyRate = u.RegulatedHourlyRate,
                OverseasHourlyRate = u.OverseasHourlyRate,
                LastUpdated = u.LastUpdated,
                IsSendAnEmailIntro = u.IsSendAnEmailIntro,
                UserStatus = u.UserStatus,
                AddresslLine1 = u.AddresslLine1,
                Region = u.Region,
                ClientId = u.ClientId ?? Guid.Empty
            };
            return status;
        }

        [Route("create"), HttpPost]
        public DefaultResponse Create(UserViewModel model)
        {
            return ValidateAndSave(model);
        }

        [Route("edit"), HttpPost]
        public DefaultResponse Edit(UserViewModel model)
        {
            return ValidateAndSave(model);
        }

        private DefaultResponse ValidateAndSave(UserViewModel model)
        {
            var userCount = (Uow.UserRepository.GetQuery(x => !x.IsDeleted && x.AspNetRoles.Any(y => y.Id == model.RoleId)).Count() + 1);
            var isError = false;
            if (!string.IsNullOrEmpty(model.Id))
            {
                ModelState.Remove("model.Password");
                ModelState.Remove("model.ConfirmPassword");
            }
            ModelState.Remove("model.CreatedBy");
            ModelState.Remove("model.CreatedOn");
            if (ModelState.IsValid)
            {
                var roleAcess = RoleExtension.GetRoleAccessList(RoleId);
                if (!roleAcess.Contains(model.RoleId))
                {
                    ModelState.AddModelError("RoleId", $"You don't have rights to create {model.RoleId} from here.");
                    isError = true;
                }
                else if (Uow.UserRepository.GetQuery(x => x.Id != model.Id && x.Email.Equals(model.Email, StringComparison.OrdinalIgnoreCase)).Any())
                {
                    ModelState.AddModelError("Email", "Email '" + model.Email + "' is already taken.");
                    isError = true;
                }
                else if (string.IsNullOrEmpty(model.Id))//create
                {
                    var user = model.To<ApplicationUser>(-TimeZoneInterval);
                    user.Id = Guid.NewGuid().ToString();
                    user.UserName = model.Email; //for login 
                    user.CreatedBy = LoggedInUser.Id;
                    user.CreatedOn = DateTime.UtcNow;
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.AddresslLine1 = model.AddresslLine1;
                    user.Region = model.Region;
                    user.Attachments = model.Attachments;
                    user.EmailConfirmed = true;
                    user.LockoutEndDateUtc = null;
                    user.UserStatus = model.UserStatus;
                    user.ModifiedOn = null;
                    user.ModifiedBy = null;
                    user.LastUpdated = null;
                    user.IsActive = true;
                    if (model.ClientId != Guid.Empty)
                        user.ClientId = model.ClientId;
                    else
                        user.ClientId = null;
                    user.ConsultantUserId = !string.IsNullOrEmpty(model.ConsultantUserId) ? model.ConsultantUserId : null;
                    var result = UserManager.CreateAsync(user, model.Password).Result;
                    if (result.Succeeded)
                    {
                        model.Id = user.Id;
                        //send register email
                        SendNewUserMail(model);
                    }
                    else
                    {
                        foreach (var err in result.Errors)
                        {
                            ModelState.AddModelError("UserName", err);
                        }
                        isError = true;
                    }
                }
                else//Edit
                {
                    var query = Uow.UserRepository.GetQuery(x => !x.IsDeleted
                        && x.Id == model.Id);
                    var extUser = query.FirstOrDefault();
                    if (extUser == null)
                    {
                        return new DefaultResponse(HttpStatusCode.NotFound, "User not found for update.");
                    }
                    var createdBy = extUser.CreatedBy;
                    var createdOn = extUser.CreatedOn;
                    var userName = extUser.UserName;
                    var emailConfirmed = extUser.EmailConfirmed;
                    var passwordhash = extUser.PasswordHash;
                    var user = model.To(extUser, -TimeZoneInterval);
                    user.PasswordHash = passwordhash;
                    user.UserName = userName;
                    user.UserStatus = model.UserStatus;
                    if (model.ClientId != Guid.Empty)
                        user.ClientId = model.ClientId;
                    else
                        user.ClientId = null;

                    user.ConsultantUserId = !string.IsNullOrEmpty(model.ConsultantUserId) ? model.ConsultantUserId : null;
                    user.CreatedBy = createdBy;
                    user.CreatedOn = createdOn;
                    user.IsActive = true;
                    user.AddresslLine1 = model.AddresslLine1;
                    user.Region = model.Region;
                    user.Attachments = model.Attachments;
                    user.ModifiedOn = DateTime.UtcNow;
                    user.ModifiedBy = LoggedInUser.Id;
                    user.EmailConfirmed = emailConfirmed;
                    user.AspNetRoles.Clear();
                    Uow.UserRepository.Update(user);
                    isError = Uow.Save(this) == 0;
                    if (!isError)
                    {
                        if (user.IsActive && !user.EmailConfirmed && model.RoleId != RoleExtension.SuperAdmin)
                        {
                            SendEmailConfirmation(user);
                        }
                    }
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
                var result = UserManager.AddToRoleAsync(model.Id, model.RoleId).Result;
                var user = Uow.UserRepository.GetById(model.Id);
                Uow.Save(this);
                return new DefaultResponse(HttpStatusCode.OK, "User successfully saved.");
            }
            var res = new DefaultResponse();
            res.SetErrorMessages(this);
            return res;
        }

        private void SendNewUserMail(UserViewModel user)
        {
            var code = UserManager.GeneratePasswordResetToken(user.Id);
            var callbackUrl = $"{WebUrl}Account/ResetPassword?code={code}&userId={user.Id}";
            UserManager.SendEmail(user.Id, "360LawGroup - New Account Created", $"Dear {user.FirstName},<br/>New {user.RoleId} account created. Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
        }

        private void SendEmailConfirmation(AspNetUser user)
        {
            var suAdmins = Uow.UserRepository.GetQuery(x => !x.IsDeleted && x.IsActive
                                                            && x.AspNetRoles.Any(y => y.Id == RoleExtension.SuperAdmin)).ToList();
            var token = UserManager.GenerateEmailConfirmationToken(user.Id);
            var callbackUrl = $"{WebUrl}/Account/ConfirmEmail?code={token}&userId={user.Id}";
            var role = user.AspNetRoles.Select(x => x.Id).FirstOrDefault();
            /*foreach (var su in suAdmins)
            {
                UserManager.SendEmail(su.Id, $"360LawGroup - New {role} Registered",
                    $"Hello {su.FirstName},<br/> New {role} registration approved for '{user.Institution.Name}'. You can confirm user's email by clicking <a href=\"{callbackUrl}\">here</a>.");
            }
            var instAdmins = Uow.UserRepository.GetQuery(x => !x.IsDeleted && x.IsActive && x.InstitutionId == user.InstitutionId
                                                              && x.AspNetRoles.Any(y => y.Id == RoleExtension.InstitutionAdmin)).ToList();
            foreach (var ins in instAdmins)
            {
                UserManager.SendEmail(ins.Id, $"360LawGroup - New {role} Registered",
                    $"Hello {ins.FirstName},<br/> New {role} registration approved. You can confirm user's email by clicking <a href=\"{callbackUrl}\">here</a>.");
            }*/
            UserManager.SendEmail(user.Id, $"360LawGroup - New {role} Registered",
                $"Hello {user.FirstName},<br/> Your account is approved. Now you can login after confirming your email by clicking <a href=\"{callbackUrl}\">here</a>.");
        }

        [Route("delete"), HttpGet]
        public DefaultResponse Delete(string id)
        {
            var status = new DefaultResponse(HttpStatusCode.OK, "User deleted successfully.");
            var roleAcess = RoleExtension.GetRoleAccessList(RoleId);
            var query = Uow.UserRepository.GetQuery(x => !x.IsDeleted
                        && x.Id == id && x.AspNetRoles.Any(y => roleAcess.Contains(y.Id)));
            var user = query.FirstOrDefault();
            if (user == null)
            { return status; }

            user.UserName = "#DELETED#" + user.UserName;
            user.Email = "#DELETED#" + user.Email;
            user.IsDeleted = true;
            Uow.UserRepository.Update(user);
            if (Uow.Save(this) == 0)
            {
                status.SetErrorMessages(this);
            }
            return status;
        }

        [Route("getroles"), HttpGet]
        public GenericResponse<List<KeyValuePair<string, string>>> GetRoles()
        {
            var userRoles = Uow.RoleRepository.GetQuery().AsEnumerable().Select(x =>
                   new KeyValuePair<string, string>(x.Id, x.Name)).ToList();
            if (!UserIsInRole(RoleExtension.SuperAdmin))
                userRoles.RemoveAll(x => x.Key == RoleExtension.SuperAdmin);
            return new GenericResponse<List<KeyValuePair<string, string>>>
            {
                StatusCode = HttpStatusCode.OK,
                Data = userRoles
            };
        }

        [Route("updatepassword"), HttpPost]
        public DefaultResponse UpdatePassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var status = new DefaultResponse();
                var roleAcess = RoleExtension.GetRoleAccessList(RoleId);
                var query = Uow.UserRepository.GetQuery(x => !x.IsDeleted
                        && x.Id == model.Id && x.AspNetRoles.Any(y => roleAcess.Contains(y.Id)));
                var user = query.FirstOrDefault();
                var code = UserManager.GeneratePasswordResetTokenAsync(user.Id).Result;
                var chngResult = UserManager.ResetPasswordAsync(user.Id, code, model.Password).Result;
                status.StatusCode = chngResult.Succeeded ? HttpStatusCode.OK : HttpStatusCode.NotAcceptable;
                status.Messages = chngResult.Succeeded ? new List<string>() { "Password successfully saved." } : chngResult.Errors.ToList();
                return status;
            }
            var res = new DefaultResponse();
            res.SetErrorMessages(this);
            return res;
        }
    }
}