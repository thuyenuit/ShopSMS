using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopSMS.Web.Models
{
    public class CategoryViewModel
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string CategoryAlias { get; set; }
        public string CategoryDescription { get; set; }
        public int? CategoryDisplayOrder { get; set; }
        public string CategoryImage { get; set; }
        public bool? CategoryHomeFlag { get; set; }      
        public DateTime? CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public string MetaKeyword { get; set; }
        public string MetaDescription { get; set; }
        public bool Status { get; set; }
        public virtual IEnumerable<ProductCategoryViewModel> ProductCategories { get; set; }

    }
}