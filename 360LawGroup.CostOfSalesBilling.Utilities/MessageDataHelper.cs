using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace _360LawGroup.CostOfSalesBilling.Utilities
{
    public static class MessageDataHelper
    {
        public static T GetValue<T>(this List<KeyValuePair<string, object>> list, string key)
        {
            var item = list?.FirstOrDefault(x => x.Key != null && x.Key.Equals(key, StringComparison.OrdinalIgnoreCase));
            if (item?.Value == null)
                return default(T);
            return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(Convert.ToString(item?.Value));
        }

        public static T GetValueByCast<T>(this List<KeyValuePair<string, object>> list, string key)
        {
            var item = list?.FirstOrDefault(x => x.Key != null && x.Key.Equals(key, StringComparison.OrdinalIgnoreCase));
            if (item?.Value == null)
                return default(T);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(Convert.ToString(item?.Value));
        }

        public static string GetValue(this List<KeyValuePair<string, object>> list, string key)
        {
            return list.GetValue<string>(key);
        }
    }
}
