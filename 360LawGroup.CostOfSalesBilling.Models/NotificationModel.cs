using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _360LawGroup.CostOfSalesBilling.Models
{
    public class NotificationModel
    {
        public Guid NotificationId { get; set; }

        public string NotificationType { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public NotificationState State { get; set; }

        public string ToUserId { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public string UpdatedBy { get; set; }
    }

    public enum NotificationType
    {
        DataImport,
    }
    public enum NotificationState
    {
        Init = 0,
        Processing = 1,
        Success = 2,
        Failed = 3
    }
}
