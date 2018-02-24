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
    public class CategoryController : Controller
    {
        IProductService productService;
        IProductCategoryService productCategoryService;
        public CategoryController(IProductService productService,
            IProductCategoryService productCategoryService)
        {
            this.productService = productService;
            this.productCategoryService = productCategoryService;
        }

        // GET: CCategory
        public ActionResult Index(int id)
        {
            return View();
        }

        public ActionResult SmartPhoneView()
        {
            CCategoryViewModel model = new CCategoryViewModel();
            IEnumerable<ProductCategory> lstPC = productCategoryService.GetAll();
            var lstResultPC = Mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(lstPC);
            lstResultPC = lstResultPC.Where(x => x.Status == true).OrderBy(x => x.DisplayOrder);
            model.ListCategoryProduct1 = lstResultPC.ToList();

            var lst = lstResultPC.ToList();
            for (int i = 0; i < lst.Count(); i++)
            {
                if (i == 0)
                {
                    model.ObjectPCFirst = lst[i];
                }
                else if (i == 1)
                {
                    model.ObjectPCSecond = lst[i];
                }
                else if (i == 2)
                {
                    model.ObjectPCThird = lst[i];
                }
                else if (i > 2)
                {
                    break;
                }
            }

            return View(model);
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