using ShopSMS.Model.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSMS.Model.Model
{
    [Table("Products")]
    public class Product: Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductID { get; set; }

        [Required]
        [MaxLength(255)]
        [Index(IsUnique = true)]
        public string ProductCode { get; set; }

        [Required]
        [MaxLength(255)]
        public string ProductName { get; set; }

        //[Required]
        [MaxLength(255)]
        [Column(TypeName = "varchar")]
        public string ProductAlias { get; set; }      

        [MaxLength(500)]
        public string Avatar { get; set; }

        [Column(TypeName="xml")]
        public string MoreImages { get; set; }

        public decimal PriceSell { get; set; }

        public decimal PriceInput { get; set; }

        public decimal? PromotionPrice { get; set; }

        public int? Warranty { get; set; }

        public int? Quantity { get; set; }

        public decimal? TaxVAT { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public bool? ProductHomeFlag { get; set; }

        public bool? ProductHotFlag { get; set; }

        public bool? ProductNew { get; set; }

        public bool? ProductSellingGood { get; set; }

        public int? ProductViewCount { get; set; }

        public int? ProductCategoryID { get; set; }

        public int? ProducerID { get; set; }

        //[ForeignKey("ProductCategoryID")]
        //public virtual ProductCategory ProductCategories { get; set; }

        public virtual IEnumerable<ProductTag> ProductTags { set; get; }

        public virtual IEnumerable<OrderDetail> OrderDetails { set; get; }
    }
}
