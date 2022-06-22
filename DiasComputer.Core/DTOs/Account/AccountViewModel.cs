using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DiasComputer.Core.DTOs.Account
{
    #region Account

    public class SignUpViewModel
    {
        [Display(Name = "آدرس ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد")]
        public string EmailAddress { get; set; } = string.Empty;

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.Password)]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,20}$", ErrorMessage = "پسورد باید حداقل دارای 6 کارکتر ترکیبی از حرف و عدد باشد !")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "کلمه عبور با تکرار کلمه عبور مغایرت دارد !")]
        [Display(Name = "تکرار کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Display(Name = "قوانین و مقررات")]
        [Required(ErrorMessage = "لطفا {0} را تایید نمایید")]
        public bool AcceptRules { get; set; }
    }

    public class SignInViewModel
    {
        [Display(Name = "آدرس ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد")]
        public string EmailAddress { get; set; } = string.Empty;

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.Password)]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد")]
        public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; }
    }

    public class RecoverPasswordViewModel
    {
        [Display(Name = "آدرس ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد")]
        public string EmailAddress { get; set; } = string.Empty;
    }

    public class SetNewPasswordForRecoveryViewModel
    {
        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.Password)]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,20}$", ErrorMessage = "پسورد باید حداقل دارای 6 کارکتر ترکیبی از حرف و عدد باشد !")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "کلمه عبور با تکرار کلمه عبور مغایرت دارد !")]
        [Display(Name = "تکرار کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    #endregion

    #region Profile

    public class ShowProfileViewModel
    {
        public int UserId { get; set; }
        [Display(Name = "نام و نام خانوادگی")]
        public string? UserName { get; set; }
        [Display(Name = "آدرس ایمیل")]
        public string EmailAddress { get; set; } = string.Empty;
        [Display(Name = "کد ملی")]
        public string? NationalCode { get; set; }
        [Display(Name = "تلفن همراه")]
        public string? Phone { get; set; }
        [Display(Name = "شماره کارت بانکی")]
        public string? BankAccountCardNumber { get; set; }
        [Display(Name = "آواتار")]
        public string? ProfilePhoto { get; set; }

    }

    public class ChangePasswordViewModel
    {
        [Display(Name = "کلمه عبور فعلی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.Password)]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.Password)]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,20}$", ErrorMessage = "پسورد باید حداقل دارای 6 کارکتر ترکیبی از حرف و عدد باشد !")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "کلمه عبور با تکرار کلمه عبور مغایرت دارد !")]
        [Display(Name = "تکرار کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public class UpdateProfileViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string NationalCode { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string BankAccountNumber { get; set; } = string.Empty;
        public IFormFile UserAvatar { get; set; }
        public string? CurrentProfile { get; set; } = string.Empty;
    }

    public class LatestWishListViewModel
    {
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public string ProductTitle { get; set; } = string.Empty;
        public string ProductImage { get; set; } = string.Empty;
        public bool AvailableStatus { get; set; }
        public int ProductPrice { get; set; }
        public int WishListId { get; set; }
    }

    public class UserForProfileMenuViewModel
    {
        public int UserId { get; set; }
        public string UserProfile { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
    }

    public class ReferredOrdersViewModel
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string ReferredCode { get; set; } = string.Empty;
        public int OrderSumPrice { get; set; }
        public bool ReferredStatus { get; set; }
    }

    #endregion
}
