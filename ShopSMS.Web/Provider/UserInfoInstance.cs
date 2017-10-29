using ShopSMS.Common.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopSMS.Web.Provider
{
    public class UserInfoInstance
    {
        public static string UserCodeInstance { get; set; }
        public static string UserNameInstance { get; set; }
        public static string FullNameInstance { get; set; }
        public static string EmailInstance { get; set; }
        public static string PhoneInstance { get; set; }

        public static List<ListStatus> ListStatus { get; set; }
        public static List<ListGroupMenu> ListGroupMenu { get; set; }
    }
}