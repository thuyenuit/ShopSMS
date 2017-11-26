using ShopSMS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopSMS.Web.ViewModelClient
{
    public class HomeViewModel
    {
        public IEnumerable<ProductViewModel> ListProduct { get; set; }
    }
}