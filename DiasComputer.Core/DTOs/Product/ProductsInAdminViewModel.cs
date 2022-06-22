using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DiasComputer.Core.DTOs.Product
{
    public class ProductsForAdminViewModel
    {
        [Display(Name = "شناسه محصول")]
        public int ProductId { get; set; }

        [Display(Name = "گروه اصلی محصول")]
        public int GroupId { get; set; }

        [Display(Name = "گروه فرعی محصول")]
        public int? SubGroupId { get; set; }

        [Display(Name = "نام محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد")]
        public string ProductName { get; set; } = string.Empty;

        [Display(Name = "نام محصول به انگلیسی")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد")]
        public string ProductEngName { get; set; } = string.Empty;

        [Display(Name = "تصویر محصول")]
        public string? ProductImg { get; set; }

        [Display(Name = "رنگ های موجود")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [MaxLength(1000, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد")]
        public string ProductColors { get; set; } = string.Empty;

        [Display(Name = "برند سازنده")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد")]
        public string? ProductBrand { get; set; }

        [Display(Name = "قیمت (تومان)")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public int ProductPrice { get; set; }

        [Display(Name = "وضعیت موجودی")]
        public bool AvailableStatus { get; set; }

        [Display(Name = "تعداد موجودی")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public int AvailableCount { get; set; }

        [Display(Name = "گارانتی محصول")]
        public string? WarrantyName { get; set; }

        [Display(Name = "شرح مختصر")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [MaxLength(300, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد")]
        public string ShortDescription { get; set; } = string.Empty;

        [Display(Name = "شرح")]
        [DataType(DataType.MultilineText)]
        public string? FullDescription { get; set; }

        [Display(Name = "تگ ها")]
        [MaxLength(1200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد")]
        public string? Tags { get; set; }

        [Display(Name = "تاریخ افزودن")]
        public DateTime AddedDate { get; set; }

        [Display(Name = "در تخفیف")]
        public bool IsOnSale { get; set; }

        [Display(Name = "درصد تخفیف(%)")]
        public int? SalePercent { get; set; }

        [Display(Name = "فعال")]
        public bool IsActive { get; set; }

        [Display(Name = "تصویر")]
        public IFormFile? ProductNewImage { get; set; }
    }
}
