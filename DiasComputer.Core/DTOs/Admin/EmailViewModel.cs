using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiasComputer.Core.DTOs.Admin
{
    public class PrivateEmailViewModel
    {
        [EmailAddress(ErrorMessage = "ایمیل وار دشده معتبر نمی باشد")]
        [Display(Name = "آدرس ایمیل")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string EmailAddress { get; set; } = string.Empty;
        [Display(Name = "موضوع")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string Subject { get; set; } = string.Empty;
        [Display(Name = "شرح")]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }
    }

    public class PublicEmailViewModel
    {
        [Display(Name = "موضوع")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string Subject { get; set; } = string.Empty;
        [Display(Name = "شرح")]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }
    }
}
