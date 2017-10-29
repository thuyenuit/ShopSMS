using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSMS.Model.Model
{
    [Table("Slides")]
    public class Slide
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SlideID { set; get; }

        [Required]
        [MaxLength(100)]
        public string SlideName { set; get; }

        [MaxLength(255)]
        public string SlideDescription { set; get; }

        [MaxLength(500)]
        public string SlideImage { set; get; }

        [MaxLength(256)]
        public string SlideUrl { set; get; }

        public int? SlideDisplayOrder { set; get; }

        public bool SlideStatus { set; get; }

        [MaxLength(500)]
        public string SlideContent { set; get; }
    }
}
