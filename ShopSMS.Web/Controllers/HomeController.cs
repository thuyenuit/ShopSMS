using AutoMapper;
using ShopSMS.Model.Model;
using ShopSMS.Service.Services;
using ShopSMS.Web.Models;
using ShopSMS.Web.ViewModelClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopSMS.Web.Controllers
{
    public class HomeController : Controller
    {
        IProductService productService;

        public HomeController(IProductService productService) {
            this.productService = productService;
        }

        public ActionResult Index()
        {
            HomeViewModel model = new HomeViewModel();
            IEnumerable<Product> lstProduct = productService.GetAll();
            var lstPC = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(lstProduct);
            model.ListProduct = lstPC;

            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}