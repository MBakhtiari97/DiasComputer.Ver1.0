using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DiasComputer.Core.DTOs.Admin
{
    public class BrandsViewModel
    {
        public int LogoId { get; set; }

        [Display(Name = "عنوان برند")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string LogoTitle { get; set; }

        [Display(Name = "لوگوی برند")]
        public string? LogoImg { get; set; }

        [Display(Name = "هدایت به آدرس")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد")]
        public string? RedirectTo { get; set; }

        [Display(Name = "لوگوی برند")]
        public IFormFile? BrandImg { get; set; }

        [Display(Name = "برند را نمایش بده")]
        public bool IsActive { get; set; }
    }
}
