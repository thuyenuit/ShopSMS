using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSMS.Model.Model
{
    [Table("Tags")]
    public class Tag
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TagID { get; set; }

        [Required]
        [MaxLength(100)]
        public string TagName { get; set; }

        [Required]
        [MaxLength(100)]
        public string TagType { get; set; }

        public virtual IEnumerable<PostTag> PostTags { set; get; }

        public virtual IEnumerable<ProductTag> ProductTags { set; get; }
    }
}
