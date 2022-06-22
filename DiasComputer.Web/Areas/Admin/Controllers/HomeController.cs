using System.ComponentModel;
using AspNetCoreHero.ToastNotification.Abstractions;
using DiasComputer.Core.DTOs.Admin;
using DiasComputer.Core.OperationResults;
using DiasComputer.Core.Security;
using DiasComputer.Core.Services.Interfaces;
using DiasComputer.Utility.Convertors;
using DiasComputer.Utility.Generator;
using DiasComputer.Utility.Methods;
using DiasComputer.Utility.Security;
using Microsoft.AspNetCore.Mvc;

namespace DiasComputer.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [PermissionChecker(1)]
    public class HomeController : Controller
    {
        ISiteRepository _siteRepository;
        private INotyfService _notyfService;
        private IBannerRepository _bannerRepository;
        private IOrderRepository _orderRepository;
        private IProductRepository _productRepository;
        private IUserRepository _userRepository;

        public HomeController(ISiteRepository siteRepository, INotyfService notyfService, IBannerRepository bannerRepository, IOrderRepository orderRepository, IProductRepository productRepository, IUserRepository userRepository)
        {
            _siteRepository = siteRepository;
            _notyfService = notyfService;
            _bannerRepository = bannerRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
        }

        [BindProperty]
        public BrandsViewModel Brand { get; set; }

        #region AdminIndex

        /// <summary>
        /// Method will show admins index
        /// </summary>
        public IActionResult Index()
        {
            #region RequiredViewBags

            ViewBag.TotalMonthOrders = _orderRepository.GetAllOrdersThisMonth();
            ViewBag.TotalMonthReviews = _productRepository.GetThisMonthReviewsCount();
            ViewBag.TotalMonthProductRefers = _productRepository.GetThisMonthReferProductRequestCount();
            ViewBag.LatestUsersCount = _userRepository.GetLatestRegisteredUsersCount();
            ViewBag.LatestUsers = _userRepository.GetLatestRegisteredUsers();
            ViewBag.MonthEarnings = _orderRepository.GetMonthlyEarning();
            ViewBag.MonthSoldProducts = _productRepository.GetMonthSoldProductCount();
            ViewBag.RecentOrders = _orderRepository.GetRecentOrders();
            ViewBag.TotalSoldProducts = _productRepository.GetAllSoldProductsCount();
            ViewBag.RecentCommentsCount = _productRepository.GetRecentCommentsCount();

            #endregion

            return View();
        }

        #endregion

        #region Brands

        #region BrandsIndex

        /// <summary>
        /// Method will show brands index
        /// </summary>
        [Route("/Admin/Brands")]
        public ActionResult BrandsIndex()
        {
            return View(_siteRepository.GetAllBrands());
        }

        #endregion

        #region Create

        /// <summary>
        /// Method will create new brand
        /// </summary>
        [Route("/Admin/Brands/New")]
        public ActionResult CreateBrand()
        {
            return View();
        }

        [Route("/Admin/Brands/New")]
        [HttpPost]
        public ActionResult CreateBrand(BrandsViewModel brand)
        {
            if (!ModelState.IsValid)
                return View(brand);

            string logoName;

            if (Brand.BrandImg?.Length > 0)
            {
                if (!Brand.BrandImg.IsImage())
                {
                    _notyfService.Warning(OperationResultText.ShowResult(OperationResult.Result.InvalidExtension.ToString()));
                    return View(brand);
                }

                //Defining logo name
                logoName = brand.LogoTitle + "Logo" + Path.GetExtension(Brand.BrandImg.FileName);

                //Defining temporary image path
                var tempSavingPath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images",
                    "temp",
                    logoName);

                //Defining saving logo path
                var logoSavingPath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images",
                    "brand",
                    logoName);

                //Saving temporary logo
                using (var stream = new FileStream(tempSavingPath, FileMode.Create))
                {
                    Brand.BrandImg.CopyTo(stream);
                }

                //Saving logo by ImageConvertor With 205px as Width
                ImageConvertor img = new ImageConvertor();
                img.Image_resize(tempSavingPath, logoSavingPath, 205);

                //Deleting temporary logo
                System.IO.File.Delete(tempSavingPath);
            }
            else
            {
                //Setting default logo image if it was null
                logoName = "Default.png";
            }

            //Setting logo name and redirect address in order to saving on database
            brand.LogoImg = logoName;
            brand.RedirectTo = "/Search?searchPhrase=" + brand.LogoTitle;

            //Adding brand
            if (_siteRepository.AddNewBrand(brand))
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));
            }
            else
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
            }

            return RedirectToAction("BrandsIndex");
        }

        #endregion

        #region Update

        /// <summary>
        /// Method will update brand
        /// </summary>
        [Route("/Admin/Brands/Update")]
        public ActionResult UpdateBrand(int brandId)
        {
            return View(_siteRepository.GetBrand(brandId));
        }

        [Route("/Admin/Brands/Update")]
        [HttpPost]
        public ActionResult UpdateBrand(BrandsViewModel brand)
        {
            if (!ModelState.IsValid)
                return View(brand);


            if (Brand.BrandImg?.Length > 0)
            {
                if (!Brand.BrandImg.IsImage())
                {
                    _notyfService.Warning(OperationResultText.ShowResult(OperationResult.Result.InvalidExtension.ToString()));
                    return View(brand);
                }

                //Defining logo name
                var logoName = brand.LogoTitle + "Logo" + Path.GetExtension(Brand.BrandImg.FileName);

                //Defining temporary image path
                var tempSavingPath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images",
                    "temp",
                    logoName);

                //Defining saving logo path
                var logoSavingPath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images",
                    "brand",
                    logoName);

                //Defining old logo path
                var oldLogoPath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images",
                    "brand",
                    brand.LogoImg);

                //First deleting old logo from server
                if (System.IO.File.Exists(oldLogoPath) && brand.LogoImg != "Default.png")
                {
                    System.IO.File.Delete(oldLogoPath);
                }

                //Saving temporary logo
                using (var stream = new FileStream(tempSavingPath, FileMode.Create))
                {
                    Brand.BrandImg.CopyTo(stream);
                }

                //Saving logo by ImageConvertor With 205px as Width
                ImageConvertor img = new ImageConvertor();
                img.Image_resize(tempSavingPath, logoSavingPath, 205);

                //Deleting temporary logo
                System.IO.File.Delete(tempSavingPath);

                //If new logo was selected then defining new logo name in order to saving on database
                brand.LogoImg = logoName;
            }

            //Setting redirect address in order to saving on database
            brand.RedirectTo = "/Search?searchPhrase=" + brand.LogoTitle;

            //Updating brand
            if (_siteRepository.UpdateBrand(brand))
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));
            }
            else
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
            }

            return RedirectToAction("BrandsIndex");
        }

        #endregion

        #region Delete

        /// <summary>
        /// Method will delete brand
        /// </summary>
        public ActionResult DeleteBrand(int brandId)
        {
            if (_siteRepository.DeleteBrand(brandId))
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));
            }
            else
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
            }

            return RedirectToAction("BrandsIndex");
        }

        #endregion

        #endregion
    }
}
