using System.Security.Claims;
using AspNetCoreHero.ToastNotification.Abstractions;
using DiasComputer.Core.DTOs.Product;
using DiasComputer.Core.OperationResults;
using DiasComputer.Core.Services.Interfaces;
using DiasComputer.DataLayer.Entities.Products;
using DiasComputer.Utility.Methods;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiasComputer.Web.Controllers
{
    public class ProductsController : Controller
    {
        IProductRepository _productRepository;
        ICategoryRepository _categoryRepository;
        private INotyfService _notyfService;


        public ProductsController(IProductRepository productRepository, ICategoryRepository categoryRepository, INotyfService notyfService)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _notyfService = notyfService;
        }

        #region Index
        /// <summary>
        /// This Method shows the products index and can filter products 
        /// </summary>
        public IActionResult Index(int groupId = 0, int pageId = 1, string searchPhrase = "", int getType = 1, string orderByType = "newest"
            , int minPrice = 0, int maxPrice = 0, int amazingProducts = 0)
        {
            ViewBag.PageId = pageId;
            ViewBag.Groups = groupId;
            ViewBag.Type = getType;
            ViewBag.OrderBy = orderByType;
            ViewBag.Phrase = searchPhrase;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.OnlyAmazing = amazingProducts;
            if (groupId != 0)
            {
                ViewBag.GroupName = _categoryRepository.GetGroupNameByGroupId(groupId);
            }

            return View(_productRepository.GetProductForList(groupId, searchPhrase, amazingProducts, pageId, minPrice, maxPrice, 12, getType, orderByType));
        }


        #endregion

        #region ShowProduct
        /// <summary>
        /// This Method shows a products by its id
        /// </summary>
        [Route("/Products/{productId}")]
        public IActionResult ShowProduct(int productId)
        {
            var productDetails = _productRepository.GetProductDetails(productId);

            if (productDetails == null)
                return NotFound();

            ViewBag.ShortFeatures = _productRepository.GetShortFeature(productId);
            ViewBag.Features = _productRepository.GetProductFeatures(productId);
            ViewBag.Galleries = _productRepository.GetProductGalleries(productId);
            ViewBag.GroupName = _categoryRepository.GetGroupNameByGroupId(productDetails.GroupId);
            ViewBag.SubGroupName = _categoryRepository.GetGroupNameByGroupId(productDetails.SubGroupId);
            return View(productDetails);
        }


        #endregion

        #region SearchProduct
        /// <summary>
        /// This Method shows searched products
        /// </summary>
        [Route("/Search")]
        public IActionResult SearchProduct(string searchPhrase, string orderByType = "newest", int pageId = 1)
        {
            var products = _productRepository.SearchProducts(searchPhrase, orderByType, pageId, 12);
            ViewBag.SearchPhrase = searchPhrase;
            ViewBag.PageId = pageId;

            return View(products);
        }


        #endregion

        #region ShowCommentsProduct
        /// <summary>
        /// This Method shows the products comments
        /// </summary>
        public IActionResult ShowComments(int id, int pageId = 1)
        {
            ViewBag.PageId = pageId;
            var model = _productRepository.GetProductComments(id, pageId);
            return View(model);
        }


        #endregion

        #region ReportComments
        /// <summary>
        /// This Method submit a report for a comment
        /// </summary>
        [Route("/ReportComment/{productId}/{reviewId}")]
        public ActionResult ReportComment(int productId, int reviewId)
        {
            if (_productRepository.ReportComment(reviewId))
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.SuccessReport.ToString()));
            }
            else
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
            }
            return Redirect($"/Products/{productId}");
        }

        #endregion

        #region RemoveCommentByAdmin
        /// <summary>
        /// This Method will allow the admin to directly remove the comment
        /// </summary>

        [Route("/RemoveComment/{productId}/{reviewId}")]
        public ActionResult RemoveCommentDirectlyInPage(int productId, int reviewId)
        {
            if (_productRepository.RemoveComment(reviewId))
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));
            }
            else
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
            }
            return Redirect($"/Products/{productId}");
        }


        #endregion

    }
}
