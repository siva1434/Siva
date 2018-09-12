using Microsoft.Graph;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace _360LawGroup.CostOfSalesBilling.Utilities
{
    public class OneDriveTokenResponse
    {
        public string error { get; set; }
        public string error_description { get; set; }
        public string[] error_codes { get; set; }
        public DateTime? timestamp { get; set; }
        public string trace_id { get; set; }
        public string correlation_id { get; set; }
        public string token_type { get; set; }
        public string scope { get; set; }
        public int expires_in { get; set; }
        public int ext_expires_in { get; set; }
        public string access_token { get; set; }
    }

    public static class OneDriveHelper
    {
        private static string oneDriveAccessToken = null;

        private static string oneDriveAccessTokenError = null;

        private static GraphServiceClient MsGraphClient
        {
            get
            {
                return new GraphServiceClient(new DelegateAuthenticationProvider((requestMessage) =>
            {
                requestMessage
                    .Headers
                    .Authorization = new AuthenticationHeaderValue("bearer", OneDriveAccessToken);
                return Task.FromResult(0);
            }));
            }
        }

        private static string OneDriveAccessToken
        {
            get
            {
                if (string.IsNullOrEmpty(oneDriveAccessToken))
                {
                    using (var client = new HttpClient())
                    {
                        var nvc = new List<KeyValuePair<string, string>>();
                        nvc.Add(new KeyValuePair<string, string>("grant_type", "password"));
                        nvc.Add(new KeyValuePair<string, string>("username", ConfigurationManager.AppSettings["OneDrive:UserName"]));
                        nvc.Add(new KeyValuePair<string, string>("password", ConfigurationManager.AppSettings["OneDrive:Password"]));
                        nvc.Add(new KeyValuePair<string, string>("client_id", ConfigurationManager.AppSettings["OneDrive:ClientId"]));
                        nvc.Add(new KeyValuePair<string, string>("client_secret", ConfigurationManager.AppSettings["OneDrive:ClientSecret"]));
                        nvc.Add(new KeyValuePair<string, string>("scope", ConfigurationManager.AppSettings["OneDrive:Scope"]));
                        var req = new HttpRequestMessage(HttpMethod.Post, ConfigurationManager.AppSettings["OneDrive:AuthTokenUrl"])
                        { Content = new FormUrlEncodedContent(nvc) };
                        var res = client.SendAsync(req).Result;
                        var response = res.Content.ReadAsStringAsync().Result;
                        var tokenResponse = JsonConvert.DeserializeObject<OneDriveTokenResponse>(response);
                        if (string.IsNullOrEmpty(tokenResponse.error))
                            oneDriveAccessToken = tokenResponse.access_token;
                        else
                            oneDriveAccessTokenError = tokenResponse.error;
                    }
                }
                return oneDriveAccessToken;
            }
        }

        private static DriveItem CreateMainFolderIfNotExist()
        {
            var graphServiceClient = MsGraphClient;
            var drives = graphServiceClient.Me.Drive.Root.Children.Request().GetAsync().Result;
            if (!drives.Any(i => i.Name.Equals("CITRAS", StringComparison.OrdinalIgnoreCase)))
            {
                var folder = new DriveItem();
                folder.Name = "CITRAS";
                folder.Folder = new Folder();
                return graphServiceClient.Me.Drive.Root.Children.Request().AddAsync(folder).Result;
            }

            return drives.FirstOrDefault(i => i.Name.Equals("CITRAS", StringComparison.OrdinalIgnoreCase));
        }

        private static DriveItem CreateRootFolderIfNotExist()
        {
            var mainFolder = CreateMainFolderIfNotExist();
            var rootFolderName = ConfigurationManager.AppSettings["OneDrive:RootFolderName"];
            var graphServiceClient = MsGraphClient;
            var subFolders = graphServiceClient.Me.Drive.Items[mainFolder.Id].Children.Request().GetAsync().Result;
            if (!subFolders.Any(i => i.Name.Equals(rootFolderName, StringComparison.OrdinalIgnoreCase)))
            {
                var folder = new DriveItem();
                folder.Name = rootFolderName;
                folder.Folder = new Folder();
                return graphServiceClient.Me.Drive.Items[mainFolder.Id].Children.Request().AddAsync(folder).Result;
            }
            return subFolders.FirstOrDefault(i => i.Name.Equals(rootFolderName, StringComparison.OrdinalIgnoreCase));
        }

        public static DriveItem CreateClientMatterFolderIfNotExist(string matterName)
        {
            var rootFolder = CreateRootFolderIfNotExist();
            var graphServiceClient = MsGraphClient;
            var subFolders = graphServiceClient.Me.Drive.Items[rootFolder.Id].Children.Request().GetAsync().Result;
            var matterFolderName = ConfigurationManager.AppSettings["OneDrive:MatterFolderName"];
            DriveItem folder = new DriveItem();
            if (!subFolders.Any(i => i.Name.Equals(matterFolderName, StringComparison.OrdinalIgnoreCase)))
            {
                folder.Name = matterFolderName;
                folder.Folder = new Folder();
                folder = graphServiceClient.Me.Drive.Items[rootFolder.Id].Children.Request().AddAsync(folder).Result;
            }
            else
                folder = subFolders.FirstOrDefault(i => i.Name.Equals(matterFolderName, StringComparison.OrdinalIgnoreCase));
            var matters = graphServiceClient.Me.Drive.Items[folder.Id].Children.Request().GetAsync().Result;
            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            //remove illigal characters
            foreach (char c in invalid)
            {
                matterName = matterName.Replace(c.ToString(), "");
            }
            var oldFolder = matters.FirstOrDefault(i => i.Name.Equals(matterName, StringComparison.OrdinalIgnoreCase));
            if (oldFolder != null)
            {
                graphServiceClient.Me.Drive.Items[oldFolder.Id].Request().DeleteAsync();
            }
            var clientMatters = graphServiceClient.Me.Drive.Items[rootFolder.Id].Children.Request().GetAsync().Result;
            if (!clientMatters.Any(i => i.Name.Equals(matterName, StringComparison.OrdinalIgnoreCase)))
            {
                var matterfolder = new DriveItem();
                matterfolder.Name = matterName;
                matterfolder.Folder = new Folder();
                return graphServiceClient.Me.Drive.Items[folder.Id].Children.Request().AddAsync(matterfolder).Result;
            }
            return clientMatters.FirstOrDefault(i => i.Name.Equals(matterName, StringComparison.OrdinalIgnoreCase));
            // in api controller where you just need to call createclientmatterfolderifnotexist..
            // taking response you just need to store path / name or find out way what we need to store in column so when they click
            // we can redirect to one drive folder in new tab. 

            //return null;
        }

        public static DriveItem UpdateFolderPermissions(this DriveItem item, List<string> addUserList)
        {
            var graphServiceClient = MsGraphClient;            
            if (addUserList != null && addUserList.Any())
            {
                var newRecList = new List<DriveRecipient>();
                string[] roles = { "read", "write" };
                foreach (var email in addUserList)
                {
                    newRecList.Add(new DriveRecipient { Email = email });
                }
                var iitem = graphServiceClient.Me.Drive.Items[item.Id].Invite(newRecList, true, roles, true).Request().PostAsync().Result;
            }
            return item;
        }

        public static DriveItem UploadConsultantAttachment(string tempfileId, string oldFileName)
        {
            var rootFolder = CreateRootFolderIfNotExist();
            var graphServiceClient = MsGraphClient;
            var subFolders = graphServiceClient.Me.Drive.Items[rootFolder.Id].Children.Request().GetAsync().Result;
            var consultantFolderName = ConfigurationManager.AppSettings["OneDrive:ConsultantFolderName"];
            DriveItem folder = new DriveItem();
            if (!subFolders.Any(i => i.Name.Equals(consultantFolderName, StringComparison.OrdinalIgnoreCase)))
            {
                folder.Name = consultantFolderName;
                folder.Folder = new Folder();
                folder = graphServiceClient.Me.Drive.Items[rootFolder.Id].Children.Request().AddAsync(folder).Result;
            }
            else
                folder = subFolders.FirstOrDefault(i => i.Name.Equals(consultantFolderName, StringComparison.OrdinalIgnoreCase));
            var consultFolder = graphServiceClient.Me.Drive.Items[folder.Id].Children.Request().GetAsync().Result;
            var oldFile = consultFolder.FirstOrDefault(i => i.Name.Equals(oldFileName, StringComparison.OrdinalIgnoreCase));
            if (oldFile != null)
            {
                graphServiceClient.Me.Drive.Items[oldFile.Id].Request().DeleteAsync();
            }
            var tempfile = graphServiceClient.Me.Drive.Items[tempfileId].Request().GetAsync().Result;
            return graphServiceClient.Me.Drive.Items[tempfile.Id].Request().UpdateAsync(new DriveItem
            {
                Name = tempfile.Name,
                ParentReference = new ItemReference { Id = folder.Id }
            }).Result;
        }

        public static DriveItem UploadClientAttachment(string tempfileId, string oldFileName)
        {
            var rootFolder = CreateRootFolderIfNotExist();
            var graphServiceClient = MsGraphClient;
            var subFolders = graphServiceClient.Me.Drive.Items[rootFolder.Id].Children.Request().GetAsync().Result;
            var clientFolderName = ConfigurationManager.AppSettings["OneDrive:ClientFolderName"];
            DriveItem folder = new DriveItem();
            if (!subFolders.Any(i => i.Name.Equals(clientFolderName, StringComparison.OrdinalIgnoreCase)))
            {
                folder.Name = clientFolderName;
                folder.Folder = new Folder();
                folder = graphServiceClient.Me.Drive.Items[rootFolder.Id].Children.Request().AddAsync(folder).Result;
            }
            else
                folder = subFolders.FirstOrDefault(i => i.Name.Equals(clientFolderName, StringComparison.OrdinalIgnoreCase));
            var consultFolder = graphServiceClient.Me.Drive.Items[folder.Id].Children.Request().GetAsync().Result;
            var oldFile = consultFolder.FirstOrDefault(i => i.Name.Equals(oldFileName, StringComparison.OrdinalIgnoreCase));
            if (oldFile != null)
            {
                graphServiceClient.Me.Drive.Items[oldFile.Id].Request().DeleteAsync();
            }
            var tempfile = graphServiceClient.Me.Drive.Items[tempfileId].Request().GetAsync().Result;
            return graphServiceClient.Me.Drive.Items[tempfile.Id].Request().UpdateAsync(new DriveItem
            {
                Name = tempfile.Name,
                ParentReference = new ItemReference { Id = folder.Id }
            }).Result;
        }

        /*
        public static DriveItem UploadClientAttachment(string base64String, string newFilename, string oldFileName)
        {
            var rootFolder = CreateRootFolderIfNotExist();
            var graphServiceClient = MsGraphClient;
            var subFolders = graphServiceClient.Me.Drive.Items[rootFolder.Id].Children.Request().GetAsync().Result;
            var clientFolderName = ConfigurationManager.AppSettings["OneDrive:ClientFolderName"];
            DriveItem folder = new DriveItem();
            if (!subFolders.Any(i => i.Name.Equals(clientFolderName, StringComparison.OrdinalIgnoreCase)))
            {
                folder.Name = clientFolderName;
                folder.Folder = new Folder();
                folder = graphServiceClient.Me.Drive.Items[rootFolder.Id].Children.Request().AddAsync(folder).Result;
            }
            else
                folder = subFolders.FirstOrDefault(i => i.Name.Equals(clientFolderName, StringComparison.OrdinalIgnoreCase));

            var consultFolder = graphServiceClient.Me.Drive.Items[folder.Id].Children.Request().GetAsync().Result;
            //remove oldfile using oldFileName
            var oldFile = consultFolder.FirstOrDefault(i => i.Name.Equals(oldFileName, StringComparison.OrdinalIgnoreCase));
            if (oldFile != null)
            {
                graphServiceClient.Me.Drive.Items[oldFile.Id].Request().DeleteAsync();
            }

            var file = new DriveItem();
            file.Name = Path.GetFileNameWithoutExtension(newFilename) + "-" + DateTime.UtcNow.Ticks + Path.GetExtension(newFilename);
            file.File = new Microsoft.Graph.File();
            byte[] bytesfile = Convert.FromBase64String(base64String);
            using (var ms = new MemoryStream(bytesfile))
            {
                return graphServiceClient.Me.Drive.Items[folder.Id].ItemWithPath(file.Name).Content.Request().PutAsync<DriveItem>(ms).Result;
            }
        }

        */

        public static DriveItem UploadTempFile(byte[] imgbyte, string newFilename, string oldFileName)
        {
            var rootFolder = CreateRootFolderIfNotExist();
            var graphServiceClient = MsGraphClient;
            var subFolders = graphServiceClient.Me.Drive.Items[rootFolder.Id].Children.Request().GetAsync().Result;
            var tempFolderName = ConfigurationManager.AppSettings["OneDrive:Temp"];
            DriveItem folder = new DriveItem();
            if (!subFolders.Any(i => i.Name.Equals(tempFolderName, StringComparison.OrdinalIgnoreCase)))
            {
                folder.Name = tempFolderName;
                folder.Folder = new Folder();
                folder = graphServiceClient.Me.Drive.Items[rootFolder.Id].Children.Request().AddAsync(folder).Result;
            }
            else
                folder = subFolders.FirstOrDefault(i => i.Name.Equals(tempFolderName, StringComparison.OrdinalIgnoreCase));

            var consultFolder = graphServiceClient.Me.Drive.Items[folder.Id].Children.Request().GetAsync().Result;
            //remove oldfile using oldFileName
            var oldFile = consultFolder.FirstOrDefault(i => i.Name.Equals(oldFileName, StringComparison.OrdinalIgnoreCase));
            if (oldFile != null)
            {
                graphServiceClient.Me.Drive.Items[oldFile.Id].Request().DeleteAsync();
            }
            var file = new DriveItem();
            file.Name = Path.GetFileNameWithoutExtension(newFilename) + "-" + DateTime.UtcNow.Ticks + Path.GetExtension(newFilename);
            file.File = new Microsoft.Graph.File();
            using (var ms = new MemoryStream(imgbyte))
            {
                return graphServiceClient.Me.Drive.Items[folder.Id].ItemWithPath(file.Name).Content.Request().PutAsync<DriveItem>(ms).Result;
            }
        }

    }
}
