using System.Linq;
using _360LawGroup.CostOfSalesBilling.Data;
using _360LawGroup.CostOfSalesBilling.Models;
using Microsoft.AspNet.Identity;

namespace _360LawGroup.CostOfSalesBilling.Web.Api.Controllers
{
    using System.Net.Http;
    using System.Web.Http;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security;
    using System;
    using System.Configuration;
    using _360LawGroup.CostOfSalesBilling.Utilities;
    using System.Linq.Expressions;

    [CustExceptionFilter]
    public class BaseApiController : ApiController
    {
        protected string LocalLoginProvider = "Local";

        private ApplicationUserManager _userManager;

        public UnitOfWorkCore Uow { get; set; }
        
        public int TimeZoneInterval
        {
            get
            {
                return -240; //Estern Daylight Time (EDT)
            }
        }

        public BaseApiController()
        {
            Uow = new UnitOfWorkCore();
        }

        public BaseApiController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat,
            UnitOfWorkCore unitOfwork)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
            Uow = unitOfwork;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ??
                       (_userManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>());
            }
            private set
            {
                _userManager = value;
            }
        }

        public ApplicationUser LoggedInUser
        {
            get
            {
                if (User.Identity.IsAuthenticated)
                { return UserManager.FindById(User.Identity.GetUserId()); }
                return null;
            }
        }

        public virtual string RoleId
        {
            get
            {
                return LoggedInUser != null ? LoggedInUser.Roles.FirstOrDefault().RoleId : "";

            }
        }

        public string WebUrl => ConfigurationManager.AppSettings["WEB_URL"];

        public string ApiUrl => ConfigurationManager.AppSettings["API_URL"];

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        protected Guid CreateNotification(string title, string message, NotificationType notificationType, NotificationState notificationState, string userId, string toUserId)
        {
            /*Notification notification = new Notification();
            if (!string.IsNullOrEmpty(userId))
            {
                notification.NotificationId = Guid.NewGuid();
                notification.CreatedBy = userId;
                notification.CreatedOn = DateTime.UtcNow;
                notification.UpdatedBy = userId;
                notification.UpdatedOn = DateTime.UtcNow;
                notification.Message = message;
                notification.Title = title;
                notification.NotificationType = notificationType.ToString();
                notification.State = (int)notificationState;
                notification.ToUserId = toUserId;
                Uow.NotificationRepository.Insert(notification);
                Uow.Save();
                var notificationcallback = ConfigurationManager.AppSettings["WEB_URL"] + "Base/PushNotification?notificationId=" + notification.NotificationId;
                NofiticationCommon.Callback(notification.NotificationId, NotificationState.Init, notificationcallback);

            }
            return notification.NotificationId;*/
            return Guid.Empty;
        }

        public bool UserIsInRole(string role)
        {
            return User.IsInRole(role);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }
                if (Uow != null)
                {
                    Uow.Dispose();
                    Uow = null;
                }
            }

            base.Dispose(disposing);

        }
    }
}