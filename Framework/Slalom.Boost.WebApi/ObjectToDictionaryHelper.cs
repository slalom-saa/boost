using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Slalom.Boost.WebApi
{
    public static class ObjectToDictionaryHelper
    {
        public static IDictionary<string, string> ToEventLogDictionary(this object source)
        {
            var dictionary = new Dictionary<string, string>();
            foreach (var property in source.GetType().GetProperties())
            {
                if (!property.GetCustomAttributes<SecurePropertyAttribute>().Any())
                {
                    var value = property.GetValue(source);
                    dictionary.Add(property.Name, Convert.ToString(value));
                }
                else
                {
                    dictionary.Add(property.Name, SecurePropertyAttribute.DefaultText);
                }
            }
            return dictionary;
        }
    }
}