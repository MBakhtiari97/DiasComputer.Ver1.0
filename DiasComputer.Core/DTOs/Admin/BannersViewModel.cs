using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DiasComputer.Core.DTOs.Admin
{
    public class BannersViewModel
    {
        public int BannerId { get; set; }
        [Display(Name = "ابعاد بنر")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string? BannerSize { get; set; }
        [Display(Name = "موقعیت بنر")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد")]
        public string? BannerPosition { get; set; }
        [Display(Name = "لینک هدایت به")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد")]
        public string? BannerRedirectTo { get; set; }
        [Display(Name = "نام جایگزین بنر")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string BannerAltName { get; set; } = string.Empty;
        [Display(Name = "تصویر بنر")]
        public string? BannerImg { get; set; }
        [Display(Name = "تصویر بنر")]
        public IFormFile? NewBannerImg { get; set; }
        [Display(Name = "وضعیت فعال بودن بنر")]
        public bool IsActive { get; set; }
    }

    public class SliderViewModel
    {
        public int SlideId { get; set; }
        [Display(Name = "لینک هدایت اسلایدر")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد")]
        public string? SlideRedirectTo { get; set; }
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد")]
        [Display(Name = "نام جایگزین")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string SlideAltName { get; set; } = string.Empty;
        [Display(Name = "اسلاید")]
        public string? SlideImg { get; set; }
        [Display(Name = "اسلاید")]
        public IFormFile? Slide { get; set; }
    }
}
