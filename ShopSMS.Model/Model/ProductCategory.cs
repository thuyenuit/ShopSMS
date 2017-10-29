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
    [Table("ProductCategories")]
    public class ProductCategory : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductCategoryID { get; set; }

        [Required]
        [MaxLength(255)]
        public string ProductCategoryName { get; set; }

        public int CategoryID { get; set; }

        public int? DisplayOrder { get; set; }

        public bool? HomeFlag { get; set; }

        [ForeignKey("CategoryID")]
        public virtual Category Categories { get; set; }

        public virtual IEnumerable<Product> Products { get; set; }
    }
}
