using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSMS.Model.Model
{
    [Table("Supplier")]
    public class Supplier
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SupplierID { get; set; }

        [Required]
        [MaxLength(200)]
        public string SupplierName { get; set; }

        [MaxLength(20)]
        [Column(TypeName = "varchar")]
        public string Phone { get; set; }

        [MaxLength(250)]
        public string Address { get; set; }

        [MaxLength(100)]
        [Column(TypeName = "varchar")]
        public string TaxCode { get; set; }

        [MaxLength(250)]
        public string Description { get; set; }
    }
}
