using _360LawGroup.CostOfSalesBilling.Models;
using System;

namespace _360LawGroup.CostOfSalesBilling.Data.Common
{
    public static class NofiticationCommon
    {
        public static NotificationModel Callback(Guid? notificaitonId, NotificationState state, string callback, string title = null, string message = null)
        {
            if (!notificaitonId.HasValue) return null;
            using (var uow = new UnitOfWorkCore())
            {
                /*var notificaiton = uow.NotificationRepository.GetById(notificaitonId);
                if (notificaiton != null)
                {
                    notificaiton.State = (int)state;
                    if (!string.IsNullOrEmpty(title))
                        notificaiton.Title = title;
                    if (!string.IsNullOrEmpty(message))
                        notificaiton.Message = message;
                    uow.Save();
                    using (var client = new HttpClient())
                    {
                        var result = client.GetAsync(callback).Result;
                    }
                }
                return notificaiton.To<NotificationModel>();*/
                return null;
            }
        }
    }
}
