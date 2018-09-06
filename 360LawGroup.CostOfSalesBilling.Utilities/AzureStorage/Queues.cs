using System;
using System.Collections.Generic;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using System.Configuration;

namespace _360LawGroup.CostOfSalesBilling.Utilities.AzureStorage
{
    public enum QueueType
    {
        DataImport
    }
    public static class Queues
    {
        private static readonly CloudStorageAccount StorageAccount;
        public static int MaxBlockSize { get; } = 2097152;

        static Queues()
        {
            StorageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
        }

        private static string ApiUrl
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["API_URL"];
                }
                catch { return string.Empty; }
            }
        }

        private static string WebUrl
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["WEB_URL"];
                }
                catch { return string.Empty; }
            }
        }

        private static CloudQueue GetCloudQueue(QueueType queueName)
        {
            if (string.IsNullOrEmpty(queueName.ToString().ToLower()))
                return null;
            var queueClient = StorageAccount.CreateCloudQueueClient();
            var cloudQueue = queueClient.GetQueueReference(queueName.ToString().ToLower());
            cloudQueue.CreateIfNotExists();
            return cloudQueue;
        }

        public static void AddToQueue(QueueType queueName, List<KeyValuePair<string, object>> data, Guid? notificationId = null)
        {
            var queue = GetCloudQueue(queueName);
            data.Add(new KeyValuePair<string, object>("BaseUrl", ApiUrl));
            data.Add(new KeyValuePair<string, object>("notificationid", notificationId));
            if (notificationId.HasValue)
                data.Add(new KeyValuePair<string, object>("notificationcallback", WebUrl + "Base/PushNotification?notificationId=" + notificationId));
            else
                data.Add(new KeyValuePair<string, object>("notificationcallback", ""));
            queue.AddMessage(new CloudQueueMessage(JsonConvert.SerializeObject(data)));
        }
    }
}
