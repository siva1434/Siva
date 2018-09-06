using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using AutoMapper.QueryableExtensions;
using _360LawGroup.CostOfSalesBilling.Models;
using _360LawGroup.CostOfSalesBilling.Utilities;
using System.Net.Http;
using System.IO;
using System.Net.Http.Headers;
using System.Web;
using _360LawGroup.CostOfSalesBilling.Web.Helper;

namespace _360LawGroup.CostOfSalesBilling.Web.Api.Controllers
{
    [RoutePrefix("common")]
    public class CommonController : BaseApiController
    {
        [AllowAnonymous, HttpGet, Route("getnotification")]
        public NotificationModel GetNotification(Guid notificationId)
        {
            return null; //Uow.NotificationRepository.GetById<NotificationModel>(notificationId);
        }

        [AllowAnonymous, HttpGet, Route("notifyinit")]
        public Guid NotifyInit(string title, string message, NotificationType notificationType, NotificationState notificationState, string userId, string toUserId)
        {
            return Guid.Empty;// CreateNotification(title, message, notificationType, notificationState, userId, toUserId);
        }

        [Authorize, HttpGet, Route("getnotifications")]
        public dynamic GetNotifications(int offset)
        {
            /*var userId = LoggedInUser.Id;
            var list = Uow.NotificationRepository.GetQuery<NotificationModel>(x => x.ToUserId == userId).OrderByDescending(x => x.UpdatedOn);
            var total = list.Count();
            var dataList = list.Skip(offset * 5).Take(5).ToList();*/
            return new { offset, limit = 5, total = 0, rows = new List<dynamic>() };
        }

        [Authorize, HttpGet, Route("removenotification")]
        public bool RemoveNotification(Guid notificationId)
        {
            /*var userId = LoggedInUser.Id;
            var notification = Uow.NotificationRepository.GetQuery(x => x.ToUserId == userId && x.NotificationId == notificationId && x.State >= 2).FirstOrDefault();
            if (notification != null)
            {
                Uow.NotificationRepository.Delete(notification);
                Uow.Save();
                return true;
            }
            else
            {*/
            return false;
            //}
        }

        [Authorize, HttpGet, Route("clearnotifications")]
        public bool ClearNotifications()
        {
            /*var userId = LoggedInUser.Id;
            var notifications = Uow.NotificationRepository.GetQuery(x => x.ToUserId == userId && x.State >= 2).ToList();
            if (notifications != null && notifications.Count > 0)
            {
                foreach (var n in notifications)
                {
                    Uow.NotificationRepository.Delete(n);
                }
                Uow.Save();
                return true;
            }
            else
            {*/
            return false;
            //}
        }

        [AppAuth, HttpGet, Route("getallconsultant")]
        public GenericResponse<List<KeyValuePair<string, string>>> GetAllConsultant()
        {
            var list = Uow.UserRepository.GetQuery(x => !x.IsDeleted && x.AspNetRoles.Any(i => i.Id == RoleExtension.Consultant)).AsEnumerable().Select(x =>
                            new KeyValuePair<string, string>(x.Id, x.FullName)).ToList();
            return new GenericResponse<List<KeyValuePair<string, string>>>
            {
                StatusCode = HttpStatusCode.OK,
                Data = list
            };
        }
        [AppAuth, HttpGet, Route("getallclient")]
        public GenericResponse<List<KeyValuePair<Guid, string>>> GetAllClient()
        {
            var list = Uow.ClientRepository.GetQuery(x => !x.IsDeleted).AsEnumerable().Select(x =>
                            new KeyValuePair<Guid, string>(x.Id, x.FirstName + " " + x.LastName +" "+ (x.IsActive ? "" : " (DEACTIVE)"))).ToList();
            return new GenericResponse<List<KeyValuePair<Guid, string>>>
            {
                StatusCode = HttpStatusCode.OK,
                Data = list
            };
        }

        [Route("getbase64"), HttpPost]
        public DefaultResponse GetFileBase64()
        {
            if (HttpContext.Current.Request.Files.Count != 1)
                throw new HttpRequestValidationException("Attempt to upload chunked file containing more than one fragment per request");
            var status = new DefaultResponse();
            var postedFile = HttpContext.Current.Request.Files[0];
            var fileName = postedFile.FileName;
            var ext = Path.GetExtension(postedFile.FileName);
            using (var ms = new MemoryStream())
            {
                var buffer = new byte[1024];
                var l = postedFile.InputStream.Read(buffer, 0, 1024);
                while (l > 0)
                {
                    ms.Write(buffer, 0, l);
                    l = postedFile.InputStream.Read(buffer, 0, 1024);
                }
                using (var thumbms = new MemoryStream())
                {
                    var imgBytes = ms.ToArray();
                    string commentbase64 = Convert.ToBase64String(imgBytes);
                    status.Data = commentbase64;
                    status.SetCustomMessages(HttpStatusCode.OK, "Uploaded successfully.");
                    thumbms.Flush();
                    thumbms.Close();
                }
                ms.Flush();
                ms.Dispose();
            }
            return status;
        }

        [AppAuth, HttpGet, Route("getsettingbyname")]
        public GenericResponse<AdminSettingsViewModel> GetSettingsByName(string name)
        {
            var ParamList = Uow.AdminSettingsRepository.GetQuery<AdminSettingsViewModel>(x =>
                x.ParamName.Equals(name, StringComparison.OrdinalIgnoreCase));
            return new GenericResponse<AdminSettingsViewModel>
            {
                StatusCode = HttpStatusCode.OK,
                Data = ParamList.FirstOrDefault()
            };
        }
    }
}