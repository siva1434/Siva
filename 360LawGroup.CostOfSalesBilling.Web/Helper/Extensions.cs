using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using _360LawGroup.CostOfSalesBilling.Models;
using System;
using System.Web.Http;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace _360LawGroup.CostOfSalesBilling.Web
{
    public static class Extensions
    {
        public static GenericResponse<T> SetErrorMessages<T>(this GenericResponse<T> status, Controller controller)
        {
            var msgList = new List<string>();
            foreach (var item in controller.ModelState.Values)
            {
                foreach (var error in item.Errors)
                {
                    if (string.IsNullOrEmpty(error.ErrorMessage) && error.Exception != null)
                        msgList.Add(error.Exception.Message);
                    else
                        msgList.Add(error.ErrorMessage);
                }
            }
            status.StatusCode = HttpStatusCode.NotAcceptable;
            status.Messages = msgList;
            return status;
        }

        public static GenericResponse<T> SetCustomMessages<T>(this GenericResponse<T> status, HttpStatusCode statusCode, IEnumerable<string> message)
        {
            status.Messages.Clear();
            status.Messages = new List<string>(message);
            status.StatusCode = statusCode;
            return status;
        }

        public static GenericResponse<T> SetCustomMessages<T>(this GenericResponse<T> status, HttpStatusCode statusCode, string message)
        {
            status.Messages.Clear();
            status.Messages.Add(message);
            status.StatusCode = statusCode;
            return status;
        }

        public static GenericResponse<T> SetException<T>(this GenericResponse<T> status, Exception exe)
        {
            status.Messages.Clear();
            status.Messages.Add("Message:" + exe.Message);
            status.Messages.Add("Stack Trace:" + exe.StackTrace);
            if (exe.InnerException != null)
            {
                status.Messages.Add("Inner Exception Message:" + exe.Message);
                status.Messages.Add("Inner Exception Stack Trace:" + exe.StackTrace);
            }
            status.StatusCode = HttpStatusCode.InternalServerError;
            return status;
        }

        public static DefaultResponse AsDefault<T>(this GenericResponse<T> status)
        {
            return status as DefaultResponse;
        }

        public static GenericResponse<T> SetErrorMessages<T>(this GenericResponse<T> status, ApiController controller) {
            var msgList = new List<string>();
            foreach ( var item in controller.ModelState.Values ) {
                foreach ( var error in item.Errors ) {
                    if ( string.IsNullOrEmpty(error.ErrorMessage) && error.Exception != null )
                        msgList.Add(error.Exception.Message);
                    else
                        msgList.Add(error.ErrorMessage);
                }
            }
            status.StatusCode = HttpStatusCode.NotAcceptable;
            status.Messages = msgList;
            return status;
        }        
    }
}