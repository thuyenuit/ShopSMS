using ShopSMS.Common.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopSMS.Web.Provider
{
    public class UserInfoInstance
    {
        public static string UserCode { get; set; }
        public static string UserName { get; set; }
        public static string FullName { get; set; }
        public static string Email { get; set; }
        public static string Phone { get; set; }

        public static List<ListStatus> ListStatus { get; set; }
        public static List<ListGroupMenu> ListGroupMenu { get; set; }
    }
}