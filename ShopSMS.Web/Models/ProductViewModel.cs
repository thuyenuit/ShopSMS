using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopSMS.Web.Models
{
    public class ProductViewModel
    {
        public int ProductID { get; set; }

        public string ProductName { get; set; }

        public string ProductCode { get; set; }

        public string ProductAlias { get; set; }

        public int ProductCategoryID { get; set; }

        public string ProductImage { get; set; }

        public string ProductMoreImage { get; set; }

        public decimal ProductPrice { get; set; }

        public decimal? ProductPromotionPrice { get; set; }

        public int? ProductWarranty { get; set; }

        public int? ProductQuantity { get; set; }

        public string ProductDescription { get; set; }

        public string ProductContent { get; set; }

        public bool? ProductHomeFlag { get; set; }

        public bool? ProductHotFlag { get; set; }

        public int? ProductViewCount { get; set; }

        public DateTime? CreateDate { get; set; }

        public string CreateBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public string UpdateBy { get; set; }

        public string MetaKeyword { get; set; }

        public string MetaDescription { get; set; }

        public bool Status { get; set; }

        public virtual ProductCategoryViewModel ProductCategories { get; set; }

        //public virtual IEnumerable<ProductTagViewModel> ProductTags { set; get; }

        //public virtual IEnumerable<OrderDetail> OrderDetails { set; get; }
    }
}