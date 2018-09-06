using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Routing;
using Elmah.Contrib.WebApi;
using Microsoft.Owin.Security.OAuth;
using System;

namespace _360LawGroup.CostOfSalesBilling.Web {
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            // Web API configuration and services
            // Locally only you will be able to see the exception errors
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.LocalOnly;

            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            config.Services.Add(typeof(IExceptionLogger), new ElmahExceptionLogger());

            //config.BindParameter(typeof(DateTime), new DateTimeModelBinder());
            //config.BindParameter(typeof(DateTime?), new DateTimeModelBinder());
            // Web API routes                        
            config.MapHttpAttributeRoutes();            

            //config.Routes.MapHttpRoute("AXD", "{resource}.axd/{*pathInfo}", null, null, new StopRoutingHandler());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "360api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
