using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

// ReSharper disable once CheckNamespace
namespace _360LawGroup.CostOfSalesBilling.Models {
    public class GenericResponse<T> {
        public GenericResponse() {
            this.Messages = new List<string>();
        }

        public HttpStatusCode StatusCode { get; set; }

        public string Status => StatusCode.ToString();

        private T _data;

        public T Data {
            get {
                return _data;
            }
            set {
                _data = value;
                if ( _data != null )
                    _data = _data.NullToEmpty();
            }
        }

        public List<string> Messages { get; set; }
    }

    public class DefaultResponse : GenericResponse<string> {
        public DefaultResponse() {
            Data = "";
        }

        public DefaultResponse(HttpStatusCode statusCode, string message) {
            this.StatusCode = statusCode;
            this.Messages = new List<string>() { message };
        }

        public DefaultResponse(HttpStatusCode statusCode, string[] messages) {
            this.StatusCode = statusCode;
            this.Messages = messages.ToList();
        }
    }

    public static class ResponseHelper {
        public static TObject NullToEmpty<TObject>(this TObject obj) {
            var propList = obj.GetType().GetProperties().Where(x => x.PropertyType == typeof(string) && x.GetValue(obj, null) == null);
            foreach ( var p in propList ) {
                p.SetValue(obj, string.Empty);
            }
            return obj;
        }
    }
}
