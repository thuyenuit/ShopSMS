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

        [Required]
        [MaxLength(255)]
        [Column(TypeName = "varchar")]
        public string ProductAlias { get; set; }

        public int ProductCategoryID { get; set; }

        [MaxLength(500)]
        public string ProductImage { get; set; }

        [Column(TypeName="xml")]
        public string ProductMoreImage { get; set; }

        public decimal ProductPrice { get; set; }

        public decimal? ProductPromotionPrice { get; set; }

        public int? ProductWarranty { get; set; }

        public int? ProductQuantity { get; set; }

        [MaxLength(255)]
        public string ProductDescription { get; set; }

        [MaxLength(500)]
        public string ProductContent { get; set; }

        public bool? ProductHomeFlag { get; set; }

        public bool? ProductHotFlag { get; set; }

        public int? ProductViewCount { get; set; }

        [ForeignKey("ProductCategoryID")]
        public virtual ProductCategory ProductCategories { get; set; }

        public virtual IEnumerable<ProductTag> ProductTags { set; get; }

        public virtual IEnumerable<OrderDetail> OrderDetails { set; get; }
    }
}
