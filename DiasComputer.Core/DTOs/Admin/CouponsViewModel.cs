using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiasComputer.Core.DTOs.Admin
{
    public class CouponsViewModel
    {
        [Display(Name = "شناسه کد تخفیف")]
        public int CouponId { get; set; }
        [Display(Name = "کد تخفیف")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string CouponCode { get; set; } = string.Empty;
        [Display(Name = "وضعیت فعال بودن")]
        public bool IsActive { get; set; }
        [Display(Name = "درصد تخفیف")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public int CouponPercent { get; set; }
        [Display(Name = "فعال از تاریخ")]
        public DateTime? ActiveFrom { get; set; }
        [Display(Name = "فعال تا تاریخ")]
        public DateTime? ActiveTill { get; set; }
        [Display(Name = "تعداد کد های معتبر")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public int CouponCount { get; set; }
    }
}
