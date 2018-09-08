using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Elmah;
using _360LawGroup.CostOfSalesBilling.Models;
using Newtonsoft.Json;
using System.Globalization;
using System.Threading;
using System.Web.Http;
using _360LawGroup.CostOfSalesBilling.Data;
using System.Data.Entity;

namespace _360LawGroup.CostOfSalesBilling.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AutoMapperConfig.Init();
            AreaRegistration.RegisterAllAreas();
            RouteTable.Routes.Ignore("{resource}.axd/{*pathInfo}");
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.SuppressHostPrincipal();
            DbConfiguration.SetConfiguration(new RetryConfiguration());
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Unspecified,
                Culture = CultureInfo.GetCultureInfo("en-US")
            };
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exe = Server.GetLastError();
            var httpContext = ((HttpApplication)sender).Context;
            httpContext.Response.Clear();
            httpContext.ClearError();
            var isAjaxCall = string.Equals("XMLHttpRequest", Context.Request.Headers["x-requested-with"], StringComparison.OrdinalIgnoreCase);
            if (exe is HttpException)
            {
                var code = (exe as HttpException).GetHttpCode();
                switch (code)
                {
                    case 404:
                        if (isAjaxCall)
                            AjaxHttpResponse(httpContext, code,
                                "There is no response from server, The request you are looking for is not found.");
                        else
                            Response.Redirect("~/Error/NotFound");
                        return;
                    case 403:
                        if (isAjaxCall)
                            AjaxHttpResponse(httpContext, code,
                                "You don't have an access. Your request is forbidden.");
                        else
                            Response.Redirect("~/Error/Forbidden");
                        return;
                }
            }
            ErrorSignal.FromContext(httpContext).Raise(exe);
            ExecuteErrorController(httpContext, exe, isAjaxCall);
        }

        private void AjaxHttpResponse(HttpContext httpContext, int code, string msg)
        {
            var status = new DefaultResponse((HttpStatusCode)code, msg);
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = 200;
            httpContext.Response.Write(JsonConvert.SerializeObject(status));
            httpContext.Response.End();
        }

        private void ExecuteErrorController(HttpContext httpContext, Exception exception, bool isAjaxCall)
        {
            if (isAjaxCall)
            {
                var status = new DefaultResponse { StatusCode = HttpStatusCode.InternalServerError };
                if (exception.Message.Contains("A potentially dangerous Request.Form value was detected from the client"))
                {
                    status.Messages.Clear();
                    status.Messages.Add("We have identify you are trying to post html/invalid data in fields. Please enter valid data & try again.");
                }
                else
                {
                    if (status.Messages.Count == 0)
                        status.Messages.Add("Oops! something went wrong. Please try again.");
                    status.Messages[0] += "<div style='display:none;'>" + exception.Message + "-" + exception.StackTrace + "</div>";
                }
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = 200;
                httpContext.Response.Write(JsonConvert.SerializeObject(status));
                httpContext.Response.End();
            }
            else
            {
                var routeData = new RouteData();
                routeData.Values["controller"] = "Error";
                routeData.Values["action"] = "Index";
                routeData.Values["exe"] = exception;
                using (Controller controller = new Controllers.ErrorController())
                {
                    ((IController)controller).Execute(new RequestContext(new HttpContextWrapper(httpContext), routeData));
                }
            }
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            var cInfo = new CultureInfo("en-US")
            {
                DateTimeFormat =
                {
                    ShortDatePattern = "MM/dd/yyyy",
                    ShortTimePattern="hh:mm tt",
                    FullDateTimePattern="MM/dd/yyyy hh:mm tt",
                    DateSeparator = "/"
                }
            };
            Thread.CurrentThread.CurrentCulture = cInfo;
            Thread.CurrentThread.CurrentUICulture = cInfo;
        }
    }
}
