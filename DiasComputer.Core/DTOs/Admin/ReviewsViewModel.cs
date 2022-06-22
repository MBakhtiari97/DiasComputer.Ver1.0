using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiasComputer.Core.DTOs.Admin
{
    public class AdminReviewsViewModel
    {
        public int ReviewId { get; set; }
        [Display(Name = "متن نظر")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string ReviewText { get; set; } = string.Empty;
        [Display(Name = "تاریخ درج")]
        public DateTime ReviewDate { get; set; }
        public int? ParentId { get; set; }
        [Display(Name = "کد کاربر")]
        public int UserId { get; set; }
        [Display(Name = "نام کاربر")]
        public string? UserName { get; set; }
        [Display(Name = "آدرس ایمیل کاربر")]
        public string? EmailAddress { get; set; }
        [Display(Name = "کد محصول")]
        public int ProductId { get; set; }
        [Display(Name = "نام محصول")]
        public string? ProductName { get; set; }

    }
}
