using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSMS.Model.Model
{
    [Table("SystemConfigs")]
    public class SystemConfig
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SystemConfigID { get; set; }

        [Required]
        [MaxLength(255)]
        public string SystemConfigContent { get; set; }

        [Required]
        public bool SystemConfigStatus { get; set; }


    }
}
