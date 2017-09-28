using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slalom.Boost.EntityFramework.Logging
{
    public static class LogExtensions
    {
        public static string Trim(this string instance, int maxLength)
        {
            if (instance == null)
            {
                return null;
            }
            var length = instance.Length;
            if (length > maxLength)
            {
                return instance.Substring(0, maxLength);
            }
            return instance;
        }
    }
}
