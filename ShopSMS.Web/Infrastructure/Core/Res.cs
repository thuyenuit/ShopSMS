using ShopSMS.Web.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Web;

namespace ShopSMS.Web.Infrastructure.Core
{
    public static class Res
    {
        public static string Get(string key)
        {          
            if (string.IsNullOrEmpty(key))
                return key;

            string value = ResourceVI.ResourceManager.GetString(key);
           
            if (string.IsNullOrEmpty(value))
                return key;

            return value;
        }
    }
}