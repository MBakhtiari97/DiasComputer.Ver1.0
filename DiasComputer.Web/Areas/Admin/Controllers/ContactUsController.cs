using AspNetCoreHero.ToastNotification.Abstractions;
using DiasComputer.Core.OperationResults;
using DiasComputer.Core.Security;
using DiasComputer.Core.Services.Interfaces;
using DiasComputer.DataLayer.Entities.SiteResources;
using DiasComputer.Utility.Methods;
using Microsoft.AspNetCore.Mvc;

namespace DiasComputer.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [PermissionChecker(1021)]
    public class ContactUsController : Controller
    {
        ISiteRepository _siteRepository;
        private INotyfService _notyfService;

        public ContactUsController(ISiteRepository siteRepository, INotyfService notyfService)
        {
            _siteRepository = siteRepository;
            _notyfService = notyfService;
        }

        #region Index

        /// <summary>
        /// Method will show contact us page details
        /// </summary>
        public IActionResult Index()
        {
            return View(_siteRepository.GetShopContactDetails());
        }

        #endregion

        #region Create

        /// <summary>
        /// Method will create new contact us information
        /// </summary>
        public IActionResult AddNewDetail()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddNewDetail(SiteContactDetail detail)
        {
            if (!ModelState.IsValid)
            {
                return View(detail);
            }

            if (_siteRepository.AddNewDetail(detail))
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));
            }
            else
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
            }

            return RedirectToAction("Index");
        }

        #endregion

        #region Update

        /// <summary>
        /// Method will update contact us information
        /// </summary>
        public IActionResult UpdateDetail(int SCD_Id)
        {
            return View(_siteRepository.GetDetailBySiteContactDetailId(SCD_Id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateDetail(SiteContactDetail detail)
        {
            if (!ModelState.IsValid)
            {
                return View(detail);
            }

            if (_siteRepository.UpdateDetail(detail))
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));
            }
            else
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
            }

            return RedirectToAction("Index");
        }

        #endregion

        #region Delete

        /// <summary>
        /// Method will remove contact us information
        /// </summary>
        public IActionResult DeleteDetail(int SCD_Id)
        {
            if (_siteRepository.DeleteDetail(SCD_Id))
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));
            }
            else
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
            }

            return RedirectToAction("Index");
        }

        #endregion
    }
}
