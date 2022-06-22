using System.Security.Claims;
using AspNetCoreHero.ToastNotification.Abstractions;
using DiasComputer.Core.DTOs.Admin;
using DiasComputer.Core.OperationResults;
using DiasComputer.Core.Security;
using DiasComputer.Core.Services.Interfaces;
using DiasComputer.Utility.Methods;
using Microsoft.AspNetCore.Mvc;

namespace DiasComputer.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [PermissionChecker(1023)]
    public class ReviewController : Controller
    {
        IProductRepository _productRepository;
        private INotyfService _notyfService;

        public ReviewController(IProductRepository productRepository, INotyfService notyfService)
        {
            _productRepository = productRepository;
            _notyfService = notyfService;
        }

        #region AwaitingConfirmationReviews

        /// <summary>
        /// Method will show awaiting for confirmation reviews
        /// </summary>
        public ActionResult AwaitingReviews(int pageId = 1)
        {
            ViewBag.PageId = pageId;
            return View(_productRepository.GetAwaitingReviews(pageId));
        }

        #endregion

        #region ReplyComment

        /// <summary>
        /// Method will submit reply for comment
        /// </summary>
        public ActionResult ReplyComment(int reviewId, int productId)
        {
            //Set Replier id and Review id 
            ViewBag.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());
            ViewBag.ProductId = productId;
            ViewBag.ReviewId = reviewId;

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReplyComment(AdminReviewsViewModel reply)
        {
            if (!ModelState.IsValid)
            {
                return View(reply);
            }

            if (_productRepository.ReplyComment(reply))
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));
            }
            else
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
                return View(reply);
            }
            return RedirectToAction("AwaitingReviews");
        }

        #endregion

        #region SwitchCommentStatus

        /// <summary>
        /// Method will switch the comment status
        /// </summary>
        public ActionResult SwitchCommentStatus(int reviewId)
        {
            if (_productRepository.SwitchCommentStatus(reviewId))
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));
            }
            else
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
            }
            return RedirectToAction("AwaitingReviews");
        }

        #endregion

        #region DeleteComment

        /// <summary>
        /// Method will delete comment
        /// </summary>
        public ActionResult RemoveComment(int reviewId)
        {
            if (_productRepository.RemoveComment(reviewId))
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));
            }
            else
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
            }
            return RedirectToAction("AwaitingReviews");
        }

        #endregion

        #region Report/UnReportComment

        /// <summary>
        /// Method will report and un report the comment
        /// </summary>
        public ActionResult ReportedComments(int pageId = 1)
        {
            ViewBag.PageId = pageId;
            return View(_productRepository.GetReportedComments(pageId));
        }

        public ActionResult UnReportComment(int reviewId)
        {
            if (_productRepository.UnReportComment(reviewId))
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));
            }
            else
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
            }

            return RedirectToAction("ReportedComments");
        }

        #endregion
    }
}
