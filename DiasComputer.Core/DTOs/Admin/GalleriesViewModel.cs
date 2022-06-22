using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DiasComputer.Core.DTOs.Admin
{
    public class GalleriesViewModel
    {
        public int GalleryId { get; set; }

        [Display(Name = "شناسه محصول")]
        public int ProductId { get; set; }

        [Display(Name = "تصویر")]
        public string? ImgName { get; set; }

        [Display(Name = "نام جایگزین")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد")]
        public string ImgAltName { get; set; } = string.Empty;

        [Display(Name = "تصویر")]
        public IFormFile? SelectedImage { get; set; }
    }
}
