using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

namespace _360LawGroup.CostOfSalesBilling.Web
{
    public class DateTimeModelBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            ValidateBindingContext(bindingContext);

            if (!bindingContext.ValueProvider.ContainsPrefix(bindingContext.ModelName) ||
                !CanBindType(bindingContext.ModelType))
            {
                return false;
            }

            var modelName = bindingContext.ModelName;
            var attemptedValue = bindingContext.ValueProvider
                .GetValue(modelName).AttemptedValue;

            try
            {
                bindingContext.Model = DateTime.Parse(attemptedValue);
            }
            catch (FormatException e)
            {
                bindingContext.ModelState.AddModelError(modelName, e);
            }

            return true;
        }

        private static void ValidateBindingContext(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException("bindingContext");
            }

            if (bindingContext.ModelMetadata == null)
            {
                throw new ArgumentException("ModelMetadata cannot be null", "bindingContext");
            }
        }

        public static bool CanBindType(Type modelType)
        {
            return modelType == typeof(DateTime) || modelType == typeof(DateTime?);
        }
    }

    public class DateTimeModelBinderProvider : ModelBinderProvider
    {
        readonly DateTimeModelBinder binder = new DateTimeModelBinder();

        public override IModelBinder GetBinder(HttpConfiguration configuration, Type modelType)
        {
            if (DateTimeModelBinder.CanBindType(modelType))
            {
                return binder;
            }

            return null;
        }
    }
}