using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopSMS.Web.Models
{
    public class ProductCategoryViewModel
    {
        public int ProductCategoryID { get; set; }

        public string ProductCategoryName { get; set; }

        public int CategoryID { get; set; }

        public int? DisplayOrder { get; set; }

        public DateTime? CreateDate { get; set; }

        public string CreateBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public string UpdateBy { get; set; }

        public string MetaKeyword { get; set; }

        public string MetaDescription { get; set; }

        public bool Status { get; set; }

        public virtual CategoryViewModel Categories { get; set; }

        public virtual IEnumerable<ProductViewModel> Products { get; set; }

        public string CategoryName { get; set; }
        public string CreateByUser { get; set; }
        public int IntStatusID { get; set; }
    }
}