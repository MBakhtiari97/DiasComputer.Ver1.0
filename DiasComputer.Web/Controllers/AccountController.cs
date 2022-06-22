using System.Security.Claims;
using AspNetCoreHero.ToastNotification.Abstractions;
using DiasComputer.Core.DTOs.Account;
using DiasComputer.Core.OperationResults;
using DiasComputer.Core.Services.Interfaces;
using DiasComputer.DataLayer.Entities.Users;
using DiasComputer.Utility.Generator;
using DiasComputer.Utility.Methods;
using DiasComputer.Utility.SendEmail;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiasComputer.Web.Controllers
{
    public class AccountController : Controller
    {
        IUserRepository _userRepository;
        private INotyfService _notyfService;
        IViewRenderService _viewRenderService;
        private IGoogleRecaptcha _recaptcha;


        public AccountController(IUserRepository userRepository, INotyfService notyfService, IViewRenderService viewRenderService, IGoogleRecaptcha recaptcha)
        {
            _userRepository = userRepository;
            _notyfService = notyfService;
            _viewRenderService = viewRenderService;
            _recaptcha = recaptcha;
        }

        #region SignUp

        /// <summary>
        /// This method will register new user
        /// </summary>
        [Route("SignUp")]
        public IActionResult SignUp()
        {
            return View();
        }
        [Route("SignUp")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUpViewModel signUp)
        {
            if (!await _recaptcha.IsUserValid())
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.RecaptchaFailed.ToString()));
                return View(signUp);
            }

            if (!ModelState.IsValid)
            {
                return View(signUp);
            }
            //1 For successful register 2 for existed email 3 for other unexpected errors
            int addNewUser = await _userRepository.AddNewUser(signUp);

            if (addNewUser == 1)
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.SuccessSignUp.ToString()));
            }
            else if (addNewUser == 2)
            {
                ModelState.AddModelError("EmailAddress", "آدرس ایمیل تکراری است");
                return View(signUp);
            }
            else
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
            }

            return Redirect("/");
        }

        /// <summary>
        /// This method will Activate user account
        /// </summary>
        [Route("/ActivateProfile/{emailAddress}/{identifierCode}")]
        public IActionResult ActiveAccount(string emailAddress, string identifierCode)
        {
            if (!_userRepository.ActivateUserProfile(emailAddress, identifierCode))
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.UnAuthorized.ToString()));
                return Redirect("/");
            }
            else
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));
                return Redirect("/SignIn");
            }
        }

        #endregion

        #region SignIn

        /// <summary>
        /// This method will login the user
        /// </summary>
        [Route("SignIn")]
        public IActionResult SignIn()
        {
            return View();
        }

        [Route("SignIn")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInViewModel signIn)
        {
            if (!await _recaptcha.IsUserValid())
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.RecaptchaFailed.ToString()));
                return View(signIn);
            }

            if (!ModelState.IsValid)
            {
                return View(signIn);
            }

            //Getting user for login
            User user = _userRepository.SignInUser(signIn);
            if (user == null)
            {
                ModelState.AddModelError("EmailAddress", "حساب کاربری با اطلاعات وارد شده یافت نشد");
                return View(signIn);
            }

            //Checking user active account status
            if (!user.IsActive)
            {
                _notyfService.Warning(OperationResultText.ShowResult(OperationResult.Result.DisabledAccount.ToString()));
                return View(signIn);
            }
            //Signing in User
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.EmailAddress.ToString()),
                new Claim(ClaimTypes.Surname, user.UserName.ToString()),
                new Claim(ClaimTypes.Actor, user.UserAvatar.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var properties = new AuthenticationProperties
            {
                IsPersistent = signIn.RememberMe
            };

            await HttpContext.SignInAsync(principal, properties);

            _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Welcome.ToString()));
            return Redirect("/");
        }

        #endregion

        #region SignOut

        /// <summary>
        /// This method will logout the user
        /// </summary>
        [Route("SignOut")]
        public IActionResult SignOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/SignIn");
        }

        #endregion

        #region RecoverPassword

        /// <summary>
        /// This method will send recover email 
        /// </summary>
        [Route("RecoverPassword")]
        public IActionResult RecoverPassword()
        {
            return View();
        }

        [Route("RecoverPassword")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordViewModel recover)
        {
            if (!await _recaptcha.IsUserValid())
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.RecaptchaFailed.ToString()));
                return View(recover);
            }

            if (!ModelState.IsValid)
            {
                return View(recover);
            }

            var user = _userRepository
                .GetUserByEmailAddress(FixedText.FixEmail(recover.EmailAddress));

            if (user == null)
            {
                ModelState.AddModelError("EmailAddress", "کاربری با آدرس ایمیل وارد شده یافت نشد !");
                return View(recover);
            }

            var body = await _viewRenderService.RenderToStringAsync("Account/Emails/_ForgotPasswordEmail", user);
            SendEmail.Send(FixedText.FixEmail(recover.EmailAddress), "بازیابی کلمه عبور", body);

            _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Recovery.ToString()));
            return View(recover);
        }

        /// <summary>
        /// This method will set a new password
        /// </summary>
        [Route("RecoverPassword/ChangePassword/{emailAddress}/{identifierCode}")]
        public IActionResult SetNewPasswordInRecoverMode(string emailAddress, string identifierCode)
        {
            var user = _userRepository.CheckUserForRecoverPasswordByIdentifier(emailAddress, identifierCode);
            if (user == null)
            {
                _notyfService.Error("مشکلی در اعتبارسنجی درخواست شما وجود دارد !");
                return Redirect("/");
            }

            return View();
        }

        [Route("RecoverPassword/ChangePassword/{emailAddress}/{identifierCode}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SetNewPasswordInRecoverMode(string emailAddress, string identifierCode, SetNewPasswordForRecoveryViewModel newPassword)
        {

            if (!ModelState.IsValid)
            {
                return View(newPassword);
            }

            if (_userRepository.SerNewPasswordForRecovery(emailAddress, identifierCode, newPassword.Password))
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));
                return Redirect("/SignIn");
            }
            else
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
            }

            return View();
        }
        #endregion

    }
}
