using AspNetCoreHero.ToastNotification.Abstractions;
using DiasComputer.Core.DTOs.Admin;
using DiasComputer.Core.OperationResults;
using DiasComputer.Core.Security;
using DiasComputer.Core.Services.Interfaces;
using DiasComputer.DataLayer.Entities.SiteResources;
using DiasComputer.Utility.Generator;
using DiasComputer.Utility.Methods;
using DiasComputer.Utility.Security;
using Microsoft.AspNetCore.Mvc;

namespace DiasComputer.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [PermissionChecker(1018)]
    public class BannerController : Controller
    {
        private INotyfService _notyfService;
        IBannerRepository _bannerRepository;

        public BannerController(INotyfService notyfService, IBannerRepository bannerRepository)
        {
            _notyfService = notyfService;
            _bannerRepository = bannerRepository;
        }

        [BindProperty]
        public BannersViewModel Banner { get; set; }

        #region Index

        /// <summary>
        /// Method will show banner index
        /// </summary>
        [Route("/Admin/Banners")]
        public IActionResult BannersIndex(int pageId = 1)
        {
            ViewBag.pageId = pageId;
            return View(_bannerRepository.GetAllBanners(pageId));
        }

        #endregion

        #region Update

        /// <summary>
        /// Method will update banner
        /// </summary>
        [Route("/Admin/Banners/Update")]
        public IActionResult UpdateBanner(int bannerId)
        {
            return View(_bannerRepository.GetBanner(bannerId));
        }

        [HttpPost]
        [Route("/Admin/Banners/Update")]
        public IActionResult UpdateBanner(BannersViewModel banner)
        {
            if (!ModelState.IsValid)
            {
                return View(banner);
            }

            if (Banner.NewBannerImg?.Length > 0)
            {
                if (banner.NewBannerImg.IsImage())
                {
                    //Defining new name 
                    var newBannerName = StringGenerator.GenerateUniqueCode()
                                        + Path.GetExtension(Banner.NewBannerImg.FileName);

                    //Defining current banner
                    var currentBanner = Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot",
                        "images",
                        "banner",
                        banner.BannerImg);

                    //Defining new banner
                    var newBannerPath = Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot",
                        "images",
                        "banner",
                        newBannerName);

                    //Deleting old banner
                    System.IO.File.Delete(currentBanner);

                    //Saving new one
                    using (var stream = new FileStream(newBannerPath, FileMode.Create))
                    {
                        Banner.NewBannerImg.CopyTo(stream);
                    }

                    //Setting new name as image name
                    banner.BannerImg = newBannerName;
                }
                else
                {
                    //If the extenstion was invalid
                    _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.InvalidExtension.ToString()));
                    return View(banner);
                }
            }

            //Updating database
            if (_bannerRepository.UpdateBanner(banner))
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));
            }
            else
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
            }

            return RedirectToAction("BannersIndex");
        }

        #endregion

        #region Active/Deactive

        /// <summary>
        /// Method will deactivate the banner
        /// </summary>
        public IActionResult DeactivateBanner(int bannerId)
        {
            if (_bannerRepository.DeactivateBanner(bannerId))
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));
            }
            else
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
            }
            return RedirectToAction("BannersIndex");
        }

        /// <summary>
        /// Method will activate the banner
        /// </summary>
        public IActionResult ActivateBanner(int bannerId)
        {
            if (_bannerRepository.ActivateBanner(bannerId))
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));
            }
            else
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
            }
            return RedirectToAction("BannersIndex");
        }

        #endregion

    }
}
