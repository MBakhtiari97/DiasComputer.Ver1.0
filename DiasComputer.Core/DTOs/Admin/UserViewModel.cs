using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DiasComputer.Core.DTOs.Admin
{
    public class UserViewModel
    {
        public class ManageUsersViewModel
        {
            public int UserId { get; set; }
            [Display(Name = "نام و نام خانوادگی")]
            [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
            [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد")]
            public string? UserName { get; set; }
            [EmailAddress(ErrorMessage = "ایمیل وار دشده معتبر نمی باشد")]
            [Display(Name = "آدرس ایمیل")]
            [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
            [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد")]
            public string EmailAddress { get; set; } = string.Empty;
            [Display(Name = "شماره تماس")]
            [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد")]
            public string? PhoneNumber { get; set; }
            [Display(Name = "تصویر پروفایل")]
            public string? UserAvatar { get; set; }
            [Display(Name = "شماره کارت بانکی")]
            [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد")]
            public string? BankAccountNumber { get; set; }
            [Display(Name = "کلمه عبور")]
            [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد")]
            public string? Password { get; set; }
            [Display(Name = "وضعیت حساب کاربری")]
            public bool IsActive { get; set; }
            [Display(Name = "تاریخ ثبت نام")]
            public DateTime RegisterDate { get; set; }
            [Display(Name = "کد ملی")]
            [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد")]
            public string? NationalCode { get; set; }
            [Display(Name = "نقش کاربر")]
            public int RoleId { get; set; }
            [Display(Name = "تصویر پروفایل")]
            public IFormFile? Avatar { get; set; }

        }
        public class LatestRegisteredUsers
        {
            public int UserId { get; set; }
            public string? UserName { get; set; }
            public string? UserAvatar { get; set; }
            public DateTime RegisterDate { get; set; }
        }
    }
}
