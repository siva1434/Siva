using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace _360LawGroup.CostOfSalesBilling.Utilities
{
    public static class Common
    {

        public const string RegexAlpha = "[A-Za-z]*";
        public const string RegexAlphaSpace = "[A-Za-z ]*";
        public const string RegexName = "[A-Za-z\\/\\s\\.,&'-]*";
        public const string RegexNum = "[0-9]*";
        public const string RegexAlphaNum = "[A-Za-z0-9]*";
        public const string RegexAlphaNumSpace = "[A-Za-z0-9 ]*";
        public const string RegexAlphaNumSpaceDotSlash = @"[A-Za-z0-9 \.\-]*";
        public const string RegexDomainList = @"^(?:(?:\w+(?:-+\w+)*\.)+[a-z]+)(?:,(?:(?:\w+(?:-+\w+)*\.)+[a-z]+))*$";

        public const string RequiredMsg = "This field is required.";

        public static string GetVisitorIpAddress()
        {
            var httpReq = System.Web.HttpContext.Current.Request;
            var visitorIpAddress = httpReq.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(visitorIpAddress))
                visitorIpAddress = httpReq.ServerVariables["REMOTE_ADDR"];

            if (string.IsNullOrEmpty(visitorIpAddress))
                visitorIpAddress = httpReq.UserHostAddress;

            if (string.IsNullOrEmpty(visitorIpAddress) || visitorIpAddress.Trim() == "::1" ||
                 visitorIpAddress.Trim() == "127.0.0.1" || visitorIpAddress.Trim() == "0.0.0.0")
            {
                visitorIpAddress = "127.0.0.1";
            }

            return visitorIpAddress.Split(':')[0];
        }

        public static IpTimeZone GetTimeZone(string ip)
        {
            var handler = new HttpClientHandler();
            var httpClient = new HttpClient(handler);
            var httpRequest = new HttpRequestMessage(new HttpMethod("GET"), "http://freegeoip.net/json/" + ip);
            var response = httpClient.SendAsync(httpRequest).Result;
            var result = response.Content.ReadAsStringAsync().Result;
            IpTimeZone timeZone;
            try
            {
                timeZone = JsonConvert.DeserializeObject<IpTimeZone>(result);
                if (string.IsNullOrEmpty(timeZone?.time_zone))
                {
                    timeZone = GetTimeZone(string.Empty);
                }
            }
            catch
            {
                timeZone = GetTimeZone(string.Empty);
            }
            return timeZone;
        }

        public static string GetValidFileName(string fileName, int len = int.MaxValue, DateTime? addTicks = null)
        {
            if (string.IsNullOrEmpty(fileName))
                return fileName;
            var fileNameWExt = Path.GetFileNameWithoutExtension(fileName);
            fileNameWExt = fileNameWExt.Replace(" ", "-");
            var invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            fileNameWExt = invalid.Aggregate(fileNameWExt, (current, c) => current.Replace(c.ToString(), ""));
            fileNameWExt = fileNameWExt.Length > len ? fileNameWExt.Substring(0, len) : fileNameWExt;
            return (fileNameWExt + (addTicks?.ToString("-yyyyMMddHHmm") ?? "") + Path.GetExtension(fileName)).ToLower().Replace("--", "-");
        }

        public static int GetPageNumber(char qrCodePageData)
        {
            var data = new List<char> {
                '\u0000', '\u0001', '\u0002', '\u0003', '\u0004', '\u0005', '\u0006', '\u0007',
                '\u0008', '\u0009','\u0010','\u0011','\u0012','\u0013','\u0014','\u0015','\u0016','\u0017','\u0018','\u0019','\u0020'
            };
            return data.IndexOf(qrCodePageData);
        }

        public static string GetFinanceYear(DateTime date)
        {
            return (date.Month >= 4 ? date.Year : date.Year - 1).ToString() + "-" + (date.Month >= 4 ? date.Year + 1 : date.Year).ToString().Substring(2, 2);
        }

        public static string GetInvoicePrefix(long locId, DateTime date)
        {
            return "HF-" + locId.ToString("D3") + "/" + Utilities.Common.GetFinanceYear(date) + "/";
        }
    }
}
