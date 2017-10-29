using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSMS.Model.Abstract
{
    public interface IAuditable
    {
        DateTime? CreateDate { get; set; }
        string CreateBy { get; set; }
        DateTime? UpdateDate { get; set; }
        string UpdateBy { get; set; }
        string MetaKeyword { get; set; }
        string MetaDescription { get; set; }
        bool Status { get; set; }
    }

    public class Auditable: IAuditable
    {
        public DateTime? CreateDate { get; set; }

        [MaxLength(255)]
        public string CreateBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        [MaxLength(1000)]
        public string UpdateBy { get; set; }

        [MaxLength(500)]
        public string MetaKeyword { get; set; }

        [MaxLength(500)]
        public string MetaDescription { get; set; }

        public bool Status { get; set; }
    }
}
