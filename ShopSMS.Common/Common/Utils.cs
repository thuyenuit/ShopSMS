using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShopSMS.Common.Common
{
    public static class Utils
    {
        #region // GetListInt
        public static List<int> GetListInt(IDictionary<string, object> dic, string key, List<int> def = null)
        {
            if (!dic.ContainsKey(key))
            {
                return def = new List<int>();
            }
            return GetListInt(dic[key]);
        }
        public static List<int> GetListInt(object val)
        {
            List<int> def = new List<int>();
            if (val == null)
            {
                return def;
            }

            if (val is List<int>)
                return (List<int>)val;
            else
            {             
               throw new Exception("GetInt error, object is not a List<Int> value");
            }

        }
        #endregion

        #region // GetInt
        public static int GetInt(IDictionary<string, object> dic, string key, int def = 0)
        {
            if (!dic.ContainsKey(key))
            {
                return def;
            }
            return GetInt(dic[key], def);
        }
        public static int GetInt(object val, int def = 0)
        {
            if (val == null)
            {
                return def;
            }

            if (val is int)
                return (int)val;
            else
            {
                try {
                    return Int32.Parse(val.ToString());
                } catch {
                    throw new Exception("GetInt error, object is not a Int value");
                }
            }

        }
        #endregion

        #region // GetString
        public static string GetString(IDictionary<string, object> dic, string key, string def = null)
        {
            if (!dic.ContainsKey(key))
            {
                return def;
            }
            return GetString(dic[key], def);
        }
        public static string GetString(object val, string def = null)
        {
            if (val == null)
            {
                return def;
            }

            if (val is string)
                return val.ToString();
            else
            {
                try
                {
                    return val.ToString();
                }
                catch
                {
                    throw new Exception("GetString error, object is not a String value");
                }
            }

        }
        #endregion

        #region //GetDecimal
        public static decimal GetDecimal(IDictionary<string, object> dic, string key, decimal def = 0)
        {
            if (!dic.ContainsKey(key))
            {
                return def;
            }
            return GetDecimal(dic[key], def);
        }
        public static decimal GetDecimal(object val, decimal def = 0)
        {
            if (val == null)
            {
                return def;
            }

            if (val is decimal)
                return (decimal)val;
            else
            {
                try
                {
                    return decimal.Parse(val.ToString());
                }
                catch
                {
                    throw new Exception("GetDecimal error, object is not a Decimal value");
                }
            }
        }
        #endregion

        public static string FormatDecimal(this decimal? val, CultureInfo custom = null, string formatString = "#.##")
        {
            if (custom == null)
            {
                custom = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
                custom.NumberFormat.NumberDecimalSeparator = ".";
            }
            return val.HasValue ? val.Value.ToString(formatString, custom) : "";
        }

        public static bool IsDateTime(string val)
        {
            DateTime temp;
            return DateTime.TryParse(val, out temp);
        }

        public static bool IsInt32(string val)
        {
            int temp;
            return Int32.TryParse(val, out temp);
        }

        public static bool IsDecimal(string val)
        {
            decimal temp;
            return Decimal.TryParse(val, out temp);
        }

    }
}
