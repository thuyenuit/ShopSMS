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
    // table thể loại bài đăng
    [Table("PostCategories")]
    public class PostCategory: Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PostCategoryID { get; set; }

        [Required]
        [MaxLength(255)]
        public string PostCategoryName { get; set; }

        [Required]
        [MaxLength(255)]
        [Column(TypeName = "varchar")]
        public string PostCategoryAlias { get; set; }

        [MaxLength(255)]
        public string PostCategoryDescription { get; set; }

        public int? PostCategoryParentID { get; set; }

        public int? PostCategoryDisplayOrder { get; set; }

        public string PostCategoryImage { get; set; }

        public bool? PostCategoryHomeFlag { get; set; }

        public virtual IEnumerable<Post> Posts { get; set; }
    }
}
