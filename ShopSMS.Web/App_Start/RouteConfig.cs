using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ShopSMS.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*botdetect}", new { botdetect = @"(.*)BotDetectCaptcha\.ashx" });

            //routes.MapRoute(
            //   name: "Category",
            //   url: "dtdd-{id}",
            //   defaults: new { controller = "Category", action = "Index", id = UrlParameter.Optional },
            //   namespaces: new string[] { "ShopSMS.Web.Controllers" }
            //);

            routes.MapRoute(
               name: "Dien thoai",
               url: "dien-thoai",
               defaults: new { controller = "Category", action = "SmartPhoneView", id = UrlParameter.Optional },
               namespaces: new string[] { "ShopSMS.Web.Controllers" }
            );

            routes.MapRoute(
               name: "May tinh bang",
               url: "may-tinh-bang",
               defaults: new { controller = "Category", action = "TabletView", id = UrlParameter.Optional },
               namespaces: new string[] { "ShopSMS.Web.Controllers" }
            );

            routes.MapRoute(
               name: "Laptop",
               url: "laptop",
               defaults: new { controller = "Category", action = "LaptopView", id = UrlParameter.Optional },
               namespaces: new string[] { "ShopSMS.Web.Controllers" }
            );

            routes.MapRoute(
               name: "Phu kien",
               url: "phu-kien",
               defaults: new { controller = "Category", action = "AccessoriesView", id = UrlParameter.Optional },
               namespaces: new string[] { "ShopSMS.Web.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "ShopSMS.Web.Controllers" }
            );
        }
    }
}
