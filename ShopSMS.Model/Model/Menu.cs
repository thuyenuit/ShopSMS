using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSMS.Model.Model
{
    [Table("Menus")]
    public class Menu
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MenuID { get; set; } 

        [Required]
        [MaxLength(100)]
        public string MenuName { get; set; }

        [Required]
        [MaxLength(255)]
        public string MenuURL { get; set; }
       
        public string ImageURL { get; set; }

        public int? MenuOrderBy { get; set; }

        [Required]
        public int MenuGroupID { get; set; }

        public string MenuTarget { get; set; }

        [Required]
        public bool MenuStatus { get; set; }

        [ForeignKey("MenuGroupID")]
        public virtual MenuGroup MenuGroup { get; set; }
    }
}
