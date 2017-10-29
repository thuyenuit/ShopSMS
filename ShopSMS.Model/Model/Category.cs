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
    [Table("Categories")]
    public class Category : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryID { get; set; }

        [Required]
        [MaxLength(255)]
        public string CategoryName { get; set; }

        [Required]
        [MaxLength(255)]
        public string CategoryAlias { get; set; }

        [MaxLength(255)]
        public string CategoryDescription { get; set; }

        public int? CategoryDisplayOrder { get; set; }

        public string CategoryImage { get; set; }

        public bool? CategoryHomeFlag { get; set; }

        public virtual IEnumerable<ProductCategory> ProductCategories { get; set; }
    }
}
