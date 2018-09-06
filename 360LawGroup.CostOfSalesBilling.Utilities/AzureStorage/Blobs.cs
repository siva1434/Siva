using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace _360LawGroup.CostOfSalesBilling.Utilities.AzureStorage
{
    public enum Containers
    {
        IdProof,
        MemberPhotos,
        ExcelExport,
        MembershipInduction
    }
    public static class Blobs
    {
        private static readonly CloudBlobClient BlobClient;
        public static int MaxBlockSize { get; } = 2097152;

        static Blobs()
        {
            var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            BlobClient = storageAccount.CreateCloudBlobClient();
        }

        public static Containers GetContainerByName(string containerName)
        {
            var names = Enum.GetNames(typeof(Containers));
            var values = Enum.GetValues(typeof(Containers));
            var i = 0;
            for (i = 0; i < values.Length; i++)
            {
                if (containerName.Equals(names[i], StringComparison.Ordinal))
                    break;
            }
            return (Containers)values.GetValue(i);
        }

        public static bool CheckCloudBlockBlobExists(string blockBlobName, Containers containerName)
        {
            if (string.IsNullOrEmpty(blockBlobName) || string.IsNullOrEmpty(containerName.ToString().ToLower()))
                return false;
            var blobContainer = BlobClient.GetContainerReference(containerName.ToString().ToLower());
            return blobContainer.Exists();
        }

        public static CloudBlockBlob GetCloudBlockBlob(string blockBlobName, Containers containerName)
        {
            if (string.IsNullOrEmpty(blockBlobName) || string.IsNullOrEmpty(containerName.ToString().ToLower()))
                return null;
            var blobContainer = BlobClient.GetContainerReference(containerName.ToString().ToLower());
            blobContainer.CreateIfNotExists();
            blobContainer.SetPermissions(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Off
            });
            var cloudBlockBlob = blobContainer.GetBlockBlobReference(blockBlobName);
            return cloudBlockBlob;
        }

        public static CloudBlockBlob GetCloudBlockBlobPublic(string blockBlobName, Containers containerName)
        {
            if (string.IsNullOrEmpty(blockBlobName) || string.IsNullOrEmpty(containerName.ToString().ToLower()))
                return null;
            var blobContainer = BlobClient.GetContainerReference(containerName.ToString().ToLower());
            blobContainer.CreateIfNotExists();
            blobContainer.SetPermissions(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Container
            });
            var cloudBlockBlob = blobContainer.GetBlockBlobReference(blockBlobName);
            return cloudBlockBlob;
        }

        public static bool DeleteCloudBlockBlob(string blockBlobName, Containers containerName)
        {
            if (string.IsNullOrEmpty(blockBlobName) || string.IsNullOrEmpty(containerName.ToString().ToLower()))
                return false;
            CloudBlobContainer blobContainer = BlobClient.GetContainerReference(containerName.ToString().ToLower());
            blobContainer.CreateIfNotExists();
            CloudBlockBlob blob = blobContainer.GetBlockBlobReference(blockBlobName);
            blob.DeleteIfExists();
            return true;
        }

        public static bool UploadStreamToBlob(CloudBlockBlob blob, string contentType, byte[] data)
        {
            bool success;
            try
            {
                using (var stream = new MemoryStream(data))
                {
                    //blob.StreamWriteSizeInBytes = 512 * 1024; //512 kb
                    blob.UploadFromStream(stream);
                }
                success = true;
            }
            catch (Exception)
            {
                success = false;
            }

            return success;
        }

        public static CloudBlobContainer GetContainerRefference(Containers containerName)
        {
            return !string.IsNullOrEmpty(containerName.ToString().ToLower()) ? BlobClient.GetContainerReference(containerName.ToString().ToLower()) : null;
        }

        public static bool DeleteBlobDirectory(Containers containerName)
        {
            // Ensure the container is exist.
            var blobContainer = BlobClient.GetContainerReference(containerName.ToString().ToLower());

            Parallel.ForEach(blobContainer.ListBlobs("/"), item =>
            {
                if (item is CloudBlockBlob || item.GetType().BaseType == typeof(CloudBlockBlob))
                {
                    ((CloudBlockBlob)item).DeleteIfExists();
                }
            });
            return true;
        }

        public static void CloudBlockBlobCleanup(Containers containerName, string prefix = "")
        {
            var blobContainer = BlobClient.GetContainerReference(containerName.ToString().ToLower());
            blobContainer.CreateIfNotExists();
            blobContainer.SetPermissions(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Off
            });
            var blobs = blobContainer.ListBlobs(prefix, true).OfType<CloudBlockBlob>().Where(b => (DateTime.UtcNow.AddDays(-1) > b.Properties.LastModified.Value.DateTime)).ToList();
            Parallel.ForEach(blobs, blob =>
             {
                 blob.DeleteIfExists();
             });
        }

        public static void DeleteAllBlockBlob(Containers containerName, string prefix)
        {
            var blobContainer = BlobClient.GetContainerReference(containerName.ToString().ToLower());
            blobContainer.CreateIfNotExists();
            blobContainer.SetPermissions(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Off
            });
            var blobs = blobContainer.ListBlobs(prefix, true).OfType<CloudBlockBlob>().ToList();
            Parallel.ForEach(blobs, blob =>
            {
                blob.DeleteIfExists();
            });
        }
    }
}
