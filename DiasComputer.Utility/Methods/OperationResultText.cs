using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiasComputer.Utility.Methods
{
    public static class OperationResultText
    {
        public static string ShowResult(string status)
        {
            var result = "";
            switch (status)
            {
                case "Success":
                    {
                        result = "عملیات با موفقیت انجام شد";
                    }
                    break;

                case "Failure":
                    {
                        result = "عملیات با شکست روبرو شد";
                    }
                    break;
                case "DuplicateEmail":
                    {
                        result = "ایمیل وارد شده تکراری است";
                    }
                    break;
                case "InvalidExtension":
                    {
                        result = "پسوند فایل انتخاب شده معتبر نیست";
                    }
                    break;
                case "UnAuthorized":
                    {
                        result = "درخواست نامعتبر";
                    }
                    break;
                case "ExistedWallet":
                    {
                        result = "کیف پول شما فعال است";
                    }
                    break;
                case "SuccessSignUp":
                    {
                        result = "ثبت نام شما با موفقیت انجام شد لطفا نسبت به فعالسازی حساب کاربری خود اقدام نمایید";
                    }
                    break;
                case "DisabledAccount":
                    {
                        result = "حساب کاربری شما فعال نیست ؛ لطفا با تایید آدرس ایمیل خود حساب کاربری خود را فعال نمایید";
                    }
                    break;
                case "Welcome":
                    {
                        result = "ورود موفقیت آمیز ؛ خوش آمدید";
                    }
                    break;
                case "Recovery":
                    {
                        result = "ایمیل بازیابی کلمه عبور برای شما ارسال شد";
                    }
                    break;
                case "SuccessNewsletter":
                    {
                        result = "وضعیت عضویت شما در سرویس خبرنامه با موفقیت تغییر یافت";
                    }
                    break;
                case "SuccessComment":
                {
                    result = "نظر شما با موفقیت ثبت شد و پس از تایید در سایت نمایش داده خواهد شد !";
                }
                    break;
                case "SuccessReport":
                {
                    result = "درخواست بازنگری کامنت با موفقیت ثبت شد و به زودی بررسی خواهد شد !";
                }
                    break;
                case "RecaptchaFailed":
                {
                    result = "اعتبارسنجی شما تایید نشد !";
                }
                    break;
            }
            return result;
        }
    }
}
