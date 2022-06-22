using AspNetCoreHero.ToastNotification.Abstractions;
using DiasComputer.Core.DTOs.Admin;
using DiasComputer.Core.OperationResults;
using DiasComputer.Core.Security;
using DiasComputer.Core.Services.Interfaces;
using DiasComputer.Utility.Convertors;
using DiasComputer.Utility.Methods;
using Microsoft.AspNetCore.Mvc;

namespace DiasComputer.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [PermissionChecker(1022)]
    public class CouponController : Controller
    {
        private ISiteRepository _siteRepository;
        private INotyfService _notyfService;

        public CouponController(ISiteRepository siteRepository, INotyfService notyfService)
        {
            _siteRepository = siteRepository;
            _notyfService = notyfService;
        }


        #region Index

        /// <summary>
        /// Method will show coupons index with or without filters
        /// </summary>
        public IActionResult CouponsIndex(string? startDate, string? endDate, string filter = "", int pageId = 1)
        {
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;
            ViewBag.Filter = filter;
            ViewBag.PageId = pageId;

            DateTime? startGlobalDate = null, endGlobalDate = null;
            if (startDate != null)
            {
                startGlobalDate = DateConvertor.ToMiladi(startDate);
            }

            if (endDate != null)
            {
                endGlobalDate = DateConvertor.ToMiladi(endDate);
            }

            return View(_siteRepository.GetAllCoupons(startGlobalDate, endGlobalDate, filter, pageId));
        }

        #endregion

        #region Create

        /// <summary>
        /// Method will create new coupon
        /// </summary>
        public IActionResult CreateCoupon()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCoupon(CouponsViewModel coupon, string? startDate, string? endDate)
        {
            if (!ModelState.IsValid)
                return View(coupon);



            if (startDate != null)
            {
                coupon.ActiveFrom = DateConvertor.ToMiladi(startDate);
            }

            if (endDate != null)
            {
                coupon.ActiveTill = DateConvertor.ToMiladi(endDate);
            }

            if (_siteRepository.AddCoupon(coupon))
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));
            }
            else
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
            }

            return RedirectToAction("CouponsIndex");
        }


        #endregion

        #region Update

        /// <summary>
        /// Method will update coupon
        /// </summary>
        public IActionResult UpdateCoupon(int couponId)
        {
            return View(_siteRepository.GetCouponViewModel(couponId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateCoupon(CouponsViewModel coupon, string? startDate, string? endDate)
        {
            if (!ModelState.IsValid)
                return View(coupon);

            if (startDate != null)
            {
                coupon.ActiveFrom = DateConvertor.ToMiladi(startDate);
            }

            if (endDate != null)
            {
                coupon.ActiveTill = DateConvertor.ToMiladi(endDate);
            }

            if (_siteRepository.UpdateCoupon(coupon))
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));
            }
            else
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
            }

            return RedirectToAction("CouponsIndex");
        }


        #endregion

        #region Delete

        /// <summary>
        /// Method will delete coupon
        /// </summary>
        public IActionResult DeleteCoupon(int couponId)
        {

            if (_siteRepository.DeleteCoupon(couponId))
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));
            }
            else
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
            }

            return RedirectToAction("CouponsIndex");
        }

        #endregion
    }
}
