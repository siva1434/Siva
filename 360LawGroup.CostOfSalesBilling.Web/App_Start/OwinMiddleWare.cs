using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;
using _360LawGroup.CostOfSalesBilling.Models;
using Microsoft.Owin;
using Newtonsoft.Json;
using Owin;

namespace _360LawGroup.CostOfSalesBilling.Web {
    public class OwinExceptionHandlerMiddleware : OwinMiddleware {

        public OwinExceptionHandlerMiddleware(OwinMiddleware next)
            : base(next) {
        }

        public override async Task Invoke(IOwinContext context) {
            //await Next.Invoke(context);            
            await Next.Invoke(context);
            if (context.Response.StatusCode != 200 && context.Response.StatusCode != 301
                 && context.Response.StatusCode != 302)
            {
                if (!context.Response.Headers.ContainsKey("X-Unauthorized"))
                    throw new HttpException(context.Response.StatusCode, context.Response.ReasonPhrase);
                var msg = context.Response.Headers.GetValues("X-Unauthorized");
                var t = new Token { error = msg.FirstOrDefault() };
                throw new HttpException((int)HttpStatusCode.OK, JsonConvert.SerializeObject(t));
            }
        }
    }

    public static class OwinExceptionHandlerMiddlewareAppBuilderExtensions {
        public static void UseOwinExceptionHandler(this IAppBuilder app) {
            app.Use<OwinExceptionHandlerMiddleware>();
        }
    }

    public static class NLogExceptionLogger {

        public static DefaultResponse GetResponse(Exception exe) {
            DefaultResponse status;
            if ( exe is HttpException ) {
                status = new DefaultResponse
                {
                    StatusCode = (HttpStatusCode) (exe as HttpException).GetHttpCode(),
                    Messages = new List<string> {exe.Message}
                };
            } else {
                status = ExecuteError(exe);
            }
            return status;
        }

        private static DefaultResponse ExecuteError(Exception exe) {
            var status = new DefaultResponse();
            if ( exe is DbEntityValidationException ) {
                status.Messages = ( exe as DbEntityValidationException ).EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage).ToList();
            } else {
                if ( exe.Message.Contains("A potentially dangerous Request.Form value was detected from the client") ) {
                    status.Messages.Clear();
                    status.Messages.Add("We have identify you are trying to post html/invalid data in fields. Please enter valid data & try again.");
                } else {
                    if ( status.Messages.Count == 0 )
                        status.Messages.Add("Oops! something went wrong. Please try again.");
                    status.Messages[0] += exe.Message + "-" + exe.StackTrace;
                    if ( exe.InnerException != null ) {
                        status.Messages.Add(exe.InnerException.Message + "-" + exe.InnerException.StackTrace);
                    }
                }
            }
            return status;
        }
    }

    public class CustExceptionFilterAttribute : ExceptionFilterAttribute {
        public override void OnException(HttpActionExecutedContext context) {
            var status = NLogExceptionLogger.GetResponse(context.Exception);
            var response = context.Request.CreateResponse(HttpStatusCode.OK, status);
            context.Response = response;
        }
    }
}