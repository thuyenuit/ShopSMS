using ShopSMS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopSMS.Web.ViewModelClient
{
    public class CCategoryViewModel
    {
        public ProductCategoryViewModel ObjectPCFirst { get; set; }
        public ProductCategoryViewModel ObjectPCSecond { get; set; }
        public ProductCategoryViewModel ObjectPCThird{ get; set; }

        public IEnumerable<ProductCategoryViewModel> ListCategoryProduct1 { get; set; }
    }
}