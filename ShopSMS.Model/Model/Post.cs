using ShopSMS.Model.Abstract;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopSMS.Model.Model
{
    // table bài đăng
    [Table("Posts")]
    public class Post : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PostsID { get; set; }

        [Required]
        [MaxLength(255)]
        [Index(IsUnique = true)]
        public string PostCode { get; set; }

        [Required]
        [MaxLength(255)]
        public string PostsName { get; set; }

        [Required]
        [MaxLength(255)]
        [Column(TypeName = "varchar")]
        public string PostsAlias { get; set; }

        [Required]
        public int PostsCategoryID { get; set; }

        [MaxLength(255)]
        public string PostsImage { get; set; }

        [MaxLength(500)]
        public string PostsDescription { get; set; }

        public string PostsContent { get; set; }

        public bool? PostsHomeFlag { get; set; }

        public bool? PostsHotFlag { get; set; }

        public int? PostsViewCount { get; set; }

        [ForeignKey("PostsCategoryID")]
        public virtual PostCategory PostCategory { get; set; }

        public virtual IEnumerable<PostTag> PostTags { set; get; }
    }
}