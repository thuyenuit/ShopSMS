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

        public int? ProductCategoryID { get; set; }

        public string Avatar { get; set; }

        public string MoreImages { get; set; }

        public decimal PriceInput { get; set; }

        public decimal PriceSell { get; set; }

        public decimal? PromotionPrice { get; set; }

        public int? Warranty { get; set; }

        public int? Quantity { get; set; }

        public string Description { get; set; }

        public bool? ProductHomeFlag { get; set; }

        public bool? ProductHotFlag { get; set; }

        public bool? ProductNew { get; set; }

        public bool? ProductSellingGood { get; set; }

        public int? ProductViewCount { get; set; }

        public DateTime? CreateDate { get; set; }

        public string CreateBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public string UpdateBy { get; set; }

        public string MetaKeyword { get; set; }

        public string MetaDescription { get; set; }

        public bool Status { get; set; }

        public decimal? TaxVAT { get; set; }

        public string ProductCategoryName { get; set; }

        public int? ProducerID { get; set; }

        public string ProducerName { get; set; }

        //public virtual ProductCategoryViewModel ProductCategories { get; set; }

        //public virtual IEnumerable<ProductTagViewModel> ProductTags { set; get; }

        //public virtual IEnumerable<OrderDetail> OrderDetails { set; get; }
    }

    public class ListError
    {
        public int OrderInExcel { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
    }
}