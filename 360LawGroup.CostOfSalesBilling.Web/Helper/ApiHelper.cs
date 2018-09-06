using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Web;
using _360LawGroup.CostOfSalesBilling.Models;
using Newtonsoft.Json;

namespace _360LawGroup.CostOfSalesBilling.Web.Helper
{
    public static class ApiHelper
    {
        private static HttpClient GetHttpClient()
        {
            var myHttpClient = new HttpClient();
            string token = null;
            var user = HttpContext.Current.User as ClaimsPrincipal;
            if (user?.Identity != null && user.Identity.IsAuthenticated)
            {
                var acctoken = JsonConvert.DeserializeObject<Token>(user.FindFirst("Token").Value);
                token = $"{acctoken.token_type} {acctoken.access_token}";
            }

            myHttpClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["API_URL"]);
            if (!string.IsNullOrEmpty(token))
            {
                myHttpClient.DefaultRequestHeaders.Add("Authorization", token);
                //myHttpClient.DefaultRequestHeaders.Add("CurrentLocation", user.FindFirst("CurrentLocation").Value);
            }
            return myHttpClient;
        }

        public static Token Login(LoginViewModel model)
        {
            using (var httpClient = GetHttpClient())
            {
                var status = new GenericResponse<Token>();
                HttpContent content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", model.UserName),
                    new KeyValuePair<string, string>("password", model.Password)
                });
                var result = httpClient.PostAsync("360LawGroup/login", content).Result;
                var resultContent = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<Token>(resultContent);
            }
        }

        public static GenericResponse<UserViewModel> GetProfile(string token)
        {
            using (var httpClient = GetHttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", token);
                var result = httpClient.GetAsync("account/userinfo").Result;
                var resultContent = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<GenericResponse<UserViewModel>>(resultContent);
            }
        }

        public static GenericResponse<T> GetWithParam<T>(string url, string paramName, object value = null)
        {
            GenericResponse<T> model = new GenericResponse<T>();
            using (var httpClient = GetHttpClient())
            {
                if (!string.IsNullOrEmpty(paramName))
                {
                    url += "?" + paramName + "=" + value;
                }
                var response = httpClient.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    model = JsonConvert.DeserializeObject<GenericResponse<T>>(response.Content.ReadAsStringAsync().Result);
                }
            }
            return model;
        }

        public static GenericResponse<T> Get<T>(string url)
        {
            return GetWithParam<T>(url, null, null);
        }

        public static GenericResponse<T> Get<T>(string url, object id)
        {
            return GetWithParam<T>(url, "id", id);
        }

        public static NotificationModel GetNotification(Guid notificationId)
        {
            using (var httpClient = GetHttpClient())
            {
                var response = httpClient.GetAsync("common/getnotification?notificationId=" + notificationId).Result;
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<NotificationModel>(response.Content.ReadAsStringAsync().Result);
                }
                return null;
            }
        }

        public static Guid NotifyInit(string title, string message, NotificationType notificationType, NotificationState notificationState, string userId, string toUserId)
        {
            using (var httpClient = GetHttpClient())
            {
                var response = httpClient.GetAsync($"common/notifyinit?title={title}&message={message}&notificationType={(int)notificationType}&notificationState={(int)notificationState}&userId={userId}&toUserId={toUserId}").Result;
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<Guid>(response.Content.ReadAsStringAsync().Result);
                }
                return Guid.Empty;
            }
        }
    }
}
