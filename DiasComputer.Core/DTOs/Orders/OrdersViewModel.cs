using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiasComputer.Core.DTOs.Orders
{
    public class WalletTransactionsViewModel
    {
        public int TH_Id { get; set; }
        public int Amount { get; set; }
        public int TypeId { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public string? PaymentMethod { get; set; }
        public bool IsApproved { get; set; }
        public int UserId { get; set; }
    }

    public class ChargeWalletViewModel
    {
        [Display(Name = "مبلغ")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public int Amount { get; set; }
    }

    public class OrdersViewModel
    {
        [Display(Name = "شناسه سفارش")]
        public int OrderId { get; set; }
        [Display(Name = "تاریخ سفارش")]
        public DateTime Date { get; set; }
        [Display(Name = "جمع فاکتور")]
        public int OrderSum { get; set; }
        [Display(Name = "وضعیت سفارش")]
        public bool OrderStatus { get; set; }
    }

    public class OrderDetailsViewModel
    {
        [Display(Name = "شناسه سفارش")]
        public int OrderId { get; set; }
        [Display(Name = "شناسه محصول")]
        public int ProductId { get; set; }
        [Display(Name = "عنوان محصول")]
        public string ProductTitle { get; set; } = string.Empty;
        [Display(Name = "تصویر محصول")]
        public string ProductImage { get; set; } = string.Empty;
        [Display(Name = "قیمت محصول")]
        public int ProductPrice { get; set; }
        [Display(Name = "جمع")]
        public int ProductPriceSum { get; set; }
        [Display(Name = "تعداد")]
        public int Count { get; set; }
        [Display(Name = "رنگ")]
        public string Color { get; set; } = string.Empty;
        [Display(Name = "گارانتی")]
        public string Warranty { get; set; } = string.Empty;
        public bool IsProcessed { get; set; }

    }

    public class OrdersWaitingForActionViewModel
    {
        [Display(Name = "شناسه سفارش")]
        public int OrderId { get; set; }
        [Display(Name = "شناسه کاربر")]
        public int UserId { get; set; }
        [Display(Name = "تاریخ سفارش")]
        public DateTime OrderDate { get; set; }
        [Display(Name = "جمع فاکتور")]
        public int OrderSum { get; set; }
        [Display(Name = "وضعیت پرداخت")]
        public bool IsFinally { get; set; }
        [Display(Name = "نام کاربر")]
        public string? UserName { get; set; }
    }

    public class UserDetailsForProcessOrderViewModel
    {
        [Display(Name = "شناسه کاربر")]
        public int UserId { get; set; }
        [Display(Name = "آدرس ایمیل")]
        public string EmailAddress { get; set; } = string.Empty;
        [Display(Name = "آدرس پستی")]
        public string Address { get; set; } = string.Empty;
        [Display(Name = "تلفن همراه")]
        public string PhoneNumber { get; set; } = string.Empty;
        [Display(Name = "شهر")]
        public string City { get; set; } = string.Empty;
        [Display(Name = "کد پستی")]
        public string ZipCode { get; set; } = string.Empty;
        [Display(Name = "کد ملی")]
        public string NationalCode { get; set; } = string.Empty;
        [Display(Name = "نام کاربری")]
        public string UserName { get; set; } = string.Empty;
    }

    public class InvoicePaymentDetails
    {
        public int? OrderId { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string TrackingCode { get; set; } = string.Empty;
        public DateTime PaymentDate { get; set; }
    }

    public class RecentOrdersViewModel
    {
        public int OrderId { get; set; }
        public int OrderSum { get; set; }
        public bool PaymentStatus { get; set; }
        public int UserId { get; set; }
    }
}
