using _360LawGroup.CostOfSalesBilling.Models;
using _360LawGroup.CostOfSalesBilling.Web.Helper;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System;

namespace _360LawGroup.CostOfSalesBilling.Web.Signalr
{
    public static class NotificationHelper
    {
        public static NotificationModel NotifyInit(string title, string message, NotificationType notificationType, string userId)
        {
            var notificationId = ApiHelper.NotifyInit(title, message, notificationType, NotificationState.Init, userId, userId);
            return NotifyInit(notificationId);
        }

        public static NotificationModel NotifyInit(string title, string message, NotificationType notificationType, NotificationState state, string userId, string toUserId)
        {

            var notificationId = ApiHelper.NotifyInit(title, message, notificationType, state, userId, toUserId);
            return NotifyInit(notificationId);
        }

        public static NotificationModel NotifyInit(Guid notificationId)
        {
            var obj = ApiHelper.GetNotification(notificationId);
            if (obj != null)
            {
                var notificationHub = GlobalHost.ConnectionManager.GetHubContext("notificaitonHub");
                foreach (var connectionId in NotificationHub._connections.GetConnections(obj.ToUserId.ToString()))
                {
                    notificationHub.Clients.Client(connectionId).pushMessage(JsonConvert.SerializeObject(new { Type = "NotifyInit", Data = obj }));
                }
            }
            return obj;
        }

        public static void NotifyExcelExport(string url, bool isSuccess, Guid userId, string refurl)
        {
            var notificationHub = GlobalHost.ConnectionManager.GetHubContext("notificaitonHub");
            foreach (var connectionId in NotificationHub._connections.GetConnections(userId.ToString()))
            {
                notificationHub.Clients.Client(connectionId).pushMessage(JsonConvert.SerializeObject(new { Type = "NotifyExcelExport", Data = new { Url = url, IsSuccess = isSuccess, RefUrl = (refurl ?? string.Empty).ToLower() } }));
            }
        }
    }
}