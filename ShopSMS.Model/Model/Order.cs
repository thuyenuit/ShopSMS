using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSMS.Model.Model
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderID { get; set; }

        [Required]
        [Index(IsUnique = true)]
        public Int32 OrderCode { get; set; }

        [Required]
        [MaxLength(100)]
        public string OrderCustomerName { get; set; }

        [Required]
        [MaxLength(255)]
        public string OrderCustomerAddress { get; set; }

        [Required]
        [MaxLength(20)]
        public string OrderCustomerPhone { get; set; }

        [MaxLength(100)]
        public string OrderCustomerEmail { get; set; }

        [MaxLength(255)]
        public string OrderCustomerMessage { get; set; }

        [MaxLength(255)]
        public string OrderPayment { get; set; }

        public DateTime? OrderCreateDate { get; set; }

        [MaxLength(100)]
        public string OrderCreateBy { get; set; }

        [MaxLength(100)]
        public string OrderPaymentStatus { get; set; }

        public bool OrderStatus { get; set; }

        public virtual IEnumerable<OrderDetail> OrderDetails { set; get; }
    }
}
