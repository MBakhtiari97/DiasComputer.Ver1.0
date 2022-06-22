using AspNetCoreHero.ToastNotification.Abstractions;
using DiasComputer.Core.DTOs.Admin;
using DiasComputer.Core.OperationResults;
using DiasComputer.Core.Security;
using DiasComputer.Core.Services.Interfaces;
using DiasComputer.Utility.Methods;
using DiasComputer.Utility.SendEmail;
using Microsoft.AspNetCore.Mvc;

namespace DiasComputer.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [PermissionChecker(18)]
    public class ManageEmail : Controller
    {
        private IViewRenderService _viewRenderService;
        private INotyfService _notyfService;
        private IUserRepository _userRepository;
        private ISiteRepository _siteRepository;

        public ManageEmail(IViewRenderService viewRenderService, INotyfService notyfService, IUserRepository userRepository, ISiteRepository siteRepository)
        {
            _viewRenderService = viewRenderService;
            _notyfService = notyfService;
            _userRepository = userRepository;
            _siteRepository = siteRepository;
        }

        #region PrivateEmail

        /// <summary>
        /// Method will send an email for specific users
        /// </summary>
        public IActionResult SendPrivateEmail()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendPrivateEmail(PrivateEmailViewModel email)
        {
            if (!ModelState.IsValid)
            {
                return View(email);
            }

            var body = await _viewRenderService.RenderToStringAsync("Account/Emails/_PrivateEmail", email);

            //Sending email 
            SendEmail.Send(email.EmailAddress, email.Subject, body);
            _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));

            return RedirectToAction("SentEmailsHistory");
        }


        #endregion

        #region PublicEmail

        /// <summary>
        /// Method will send an email for all newsletter users
        /// </summary>
        public IActionResult SendPublicEmail()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendPublicEmail(PublicEmailViewModel email)
        {
            if (!ModelState.IsValid)
            {
                return View(email);
            }

            var body = await _viewRenderService.RenderToStringAsync("Account/Emails/_PublicEmail", email);

            //Sending email for active newsletter users
            foreach (var emailAddress in _userRepository.GetActiveNewsletters())
            {
                SendEmail.Send(emailAddress, email.Subject, body);
            }
            _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));

            return RedirectToAction("SentEmailsHistory");
        }

        #endregion

        #region AdminEmail

        /// <summary>
        /// Method will send an email for admins
        /// </summary>
        public IActionResult SendAdminsEmail()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendAdminsEmail(PublicEmailViewModel email)
        {
            if (!ModelState.IsValid)
            {
                return View(email);
            }

            var body = await _viewRenderService.RenderToStringAsync("Account/Emails/_PublicEmail", email);
            var emails = _userRepository.GetAdminsEmails();
            //Sending email for active newsletter users
            foreach (var emailAddress in _userRepository.GetAdminsEmails())
            {
                SendEmail.Send(emailAddress, email.Subject, body);
            }
            _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));

            return RedirectToAction("SentEmailsHistory");
        }


        #endregion

        #region EmailHistory

        /// <summary>
        /// Method will show emails history
        /// </summary>
        public ActionResult SentEmailsHistory(int? typeId, string? filter = "", int pageId = 1)
        {
            ViewBag.PageId = pageId;
            ViewBag.Filter = filter;
            return View(_siteRepository.GetEmailHistory(typeId, filter, pageId));
        }

        #endregion

        #region DeleteEmail

        /// <summary>
        /// Method will delete email from history
        /// </summary>
        public ActionResult DeleteEmail(int emailId)
        {
            if (_siteRepository.DeleteEmail(emailId))
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));
            }
            else
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
            }
            return RedirectToAction("SentEmailsHistory");
        }

        #endregion

        #region EmailDetails

        /// <summary>
        /// Method will show an email details
        /// </summary>
        public ActionResult EmailDetails(int emailId)
        {
            return View(_siteRepository.GetEmailByEmailId(emailId));
        }

        #endregion
    }
}
