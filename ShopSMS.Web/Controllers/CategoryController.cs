using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopSMS.Web.Controllers
{
    public class CategoryController : Controller
    {
        // GET: CCategory
        public ActionResult Index(int id)
        {
            return View();
        }

        public ActionResult SmartPhoneView()
        {
            return View();
        }

        public ActionResult TabletView()
        {
            return View();
        }

        public ActionResult LaptopView()
        {
            return View();
        }

        public ActionResult AccessoriesView()
        {
            return View();
        }

    }
}