using AspNetCoreHero.ToastNotification.Abstractions;
using DiasComputer.Core.DTOs.Product;
using DiasComputer.Core.OperationResults;
using DiasComputer.Core.Security;
using DiasComputer.Core.Services.Interfaces;
using DiasComputer.Utility.Methods;
using Microsoft.AspNetCore.Mvc;

namespace DiasComputer.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [PermissionChecker(3)]
    public class FeatureController : Controller
    {
        private IProductRepository _productRepository;
        private INotyfService _notyfService;

        public FeatureController(IProductRepository productRepository, INotyfService notyfService)
        {
            _productRepository = productRepository;
            _notyfService = notyfService;
        }

        #region Index

        /// <summary>
        /// Method will show features index
        /// </summary>
        public ActionResult FeaturesIndex(int productId, int pageId = 1)
        {
            ViewBag.ProductId = productId;
            ViewBag.PageId = pageId;
            return View(_productRepository.GetProductFeatures(productId, pageId));
        }

        #endregion

        #region Create

        /// <summary>
        /// Method will create feature
        /// </summary>
        public ActionResult AddFeatures(int productId, int? featureId)
        {
            if (featureId != null)
            {
                ViewBag.FeatureId = featureId;
            }
            ViewBag.ProductId = productId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddFeatures(ProductFeaturesViewModel feature)
        {

            if (!ModelState.IsValid)
                return View(feature);

            if (_productRepository.AddNewFeature(feature))
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));
            }
            else
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
            }
            return Redirect($"/Admin/Feature/FeaturesIndex?productId={feature.ProductId}");
        }

        #endregion

        #region Update

        /// <summary>
        /// Method will update feature
        /// </summary>
        public ActionResult UpdateFeature(int featureId)
        {
            var model = _productRepository.GetFeatureByFeatureId(featureId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateFeature(ProductFeaturesViewModel feature)
        {
            if (!ModelState.IsValid)
                return View(feature);

            //Updating details
            if (_productRepository.UpdateFeature(feature))
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));
            }
            else
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
            }
            return Redirect($"/Admin/Feature/FeaturesIndex?productId={feature.ProductId}");
        }

        #endregion

        #region Delete

        /// <summary>
        /// Method will delete feature
        /// </summary>
        [HttpGet]
        public ActionResult DeleteFeature(int featureId, int productId)
        {
            if (_productRepository.DeleteFeature(featureId))
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));
            }
            else
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
            }

            return Redirect($"/Admin/Feature/FeaturesIndex?productId={productId}");
        }

        #endregion
    }
}
