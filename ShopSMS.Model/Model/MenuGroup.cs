using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopSMS.Model.Model
{
    [Table("MenuGroups")]
    public class MenuGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MenuGroupID { get; set; }

        [Required]
        public string MenuGroupName { get; set; }

        public virtual IEnumerable<Menu> Menus { get; set; }
    }
}