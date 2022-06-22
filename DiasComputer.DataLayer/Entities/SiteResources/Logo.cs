using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiasComputer.DataLayer.Entities.Products;

namespace DiasComputer.DataLayer.Entities.SiteResources
{
    public class Logo
    {
        [Key]
        public int LogoId { get; set; }

        [Display(Name = "عنوان برند")]
        [Required]
        [MaxLength(200)]
        public string LogoTitle { get; set; }

        [Display(Name = "لوگوی برند")]
        [Required]
        [MaxLength(200)]
        public string LogoImg { get; set; }

        [Display(Name = "هدایت به آدرس")]
        [Required]
        [MaxLength(200)]
        public string RedirectTo { get; set; }

        [Display(Name = "وضعیت نمایش در سایت")]
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
    }
}
