using AspNetCoreHero.ToastNotification.Abstractions;
using DiasComputer.Core.DTOs.Product;
using DiasComputer.Core.OperationResults;
using DiasComputer.Core.Security;
using DiasComputer.Core.Services.Interfaces;
using DiasComputer.DataLayer.Entities.Products;
using DiasComputer.Utility.Convertors;
using DiasComputer.Utility.Generator;
using DiasComputer.Utility.Methods;
using DiasComputer.Utility.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiasComputer.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [PermissionChecker(3)]
    public class ProductsController : Controller
    {
        private IProductRepository _productRepository;
        ICategoryRepository _categoryRepository;
        private INotyfService _notyfService;

        public ProductsController(IProductRepository productRepository, ICategoryRepository categoryRepository, INotyfService notyfService)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _notyfService = notyfService;
        }

        [BindProperty] public ProductsForAdminViewModel Products { get; set; }

        #region Index

        /// <summary>
        /// Method will show products index
        /// </summary>
        public ActionResult Index(string? filter, bool? isAvailable, bool? isActive, bool? isOnSale, int pageId = 1)
        {
            ViewBag.PageId = pageId;
            ViewBag.Filter = filter;
            return View("ProductsIndex", _productRepository.GetProductsForAdmin(filter, isAvailable, isActive, isOnSale, pageId));
        }

        #endregion

        #region Create

        /// <summary>
        /// Method will create a new product
        /// </summary>
        public ActionResult Create()
        {
            ViewBag.Groups = _categoryRepository.GetAllCategories();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductsForAdminViewModel products)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(products);

                if (Products.ProductNewImage != null && Products.ProductNewImage.FileName.Length > 0)
                {
                    if (Products.ProductNewImage.IsImage())
                    {
                        var newName = StringGenerator.GenerateUniqueCode() +
                                      Path.GetExtension(Products.ProductNewImage.FileName);

                        //New actual size image saving path
                        var newPath = Path.Combine(Directory.GetCurrentDirectory(),
                            "wwwroot/images/productImages", newName);

                        //New thumbnail size image saving path
                        var newThumbPath = Path.Combine(Directory.GetCurrentDirectory(),
                            "wwwroot/images/productThumbnailImages", newName);

                        using (var stream = new FileStream(newPath, FileMode.Create))
                        {
                            Products.ProductNewImage.CopyTo(stream);
                        }

                        ImageConvertor convert = new ImageConvertor();
                        convert.Image_resize(newPath, newThumbPath, 150);

                        products.ProductImg = newName;
                    }
                    else
                    {
                        _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.InvalidExtension.ToString()));
                        return View(products);
                    }
                }
                else
                {
                    products.ProductImg = "Default.png";
                }

                if (_productRepository.AddNewProduct(products))
                {
                    _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));
                }
                else
                {
                    _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        #endregion

        #region Details

        /// <summary>
        /// Method will shows the product details
        /// </summary>
        public ActionResult Details(int productId)
        {
            return View(_productRepository.GetProductDetailsForAdmin(productId));
        }

        #endregion

        #region Update

        /// <summary>
        /// Method will update product
        /// </summary>
        public ActionResult Edit(int productId)
        {
            ViewBag.Groups = _categoryRepository.GetAllCategories();
            return View(_productRepository.GetProductDetailsForAdmin(productId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductsForAdminViewModel products)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(products);

                //Gathering current details
                var currentDetails = _productRepository
                    .GetProductByProductId(products.ProductId);


                //Checking for if the image is selected or not
                if (Products.ProductNewImage != null && Products.ProductNewImage.FileName.Length > 0)
                {
                    if (Products.ProductNewImage.IsImage())
                    {

                        #region CurrentImagePath

                        //Gathering current image paths
                        var currentPath = Path.Combine(Directory.GetCurrentDirectory(),
                            "wwwroot/images/productImages", currentDetails.ProductImg);

                        var currentThumbPath = Path.Combine(Directory.GetCurrentDirectory(),
                            "wwwroot/images/productThumbnailImages", currentDetails.ProductImg);

                        #endregion

                        //Creating a new name
                        var newName = StringGenerator.GenerateUniqueCode() +
                                      Path.GetExtension(Products.ProductNewImage.FileName);

                        #region NewImagePath

                        //New actual size image saving path
                        var newPath = Path.Combine(Directory.GetCurrentDirectory(),
                            "wwwroot/images/productImages", newName);

                        //New thumbnail size image saving path
                        var newThumbPath = Path.Combine(Directory.GetCurrentDirectory(),
                            "wwwroot/images/productThumbnailImages", newName);

                        #endregion

                        //Saving new image
                        using (var stream = new FileStream(newPath, FileMode.Create))
                        {
                            Products.ProductNewImage.CopyTo(stream);
                        }

                        //Saving thumb image
                        ImageConvertor convert = new ImageConvertor();
                        convert.Image_resize(newPath, newThumbPath, 150);

                        products.ProductImg = newName;

                        //Deleting old product images
                        if (System.IO.File.Exists(currentPath) && currentDetails.ProductImg != "Default.png")
                        {
                            System.IO.File.Delete(currentPath);
                        }
                        if (System.IO.File.Exists(currentThumbPath) && currentDetails.ProductImg != "Default.png")
                        {
                            System.IO.File.Delete(currentThumbPath);
                        }
                    }
                    else
                    {
                        _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.InvalidExtension.ToString()));
                        return View(products);
                    }
                }
                else
                {
                    products.ProductImg = currentDetails.ProductImg;
                }

                products.AddedDate = currentDetails.AddedDate;
                if (_productRepository.UpdateProduct(products))
                {
                    _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));
                }
                else
                {
                    _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
                return View(products);
            }
        }

        #endregion

        #region Delete

        /// <summary>
        /// Method will delete a product
        /// </summary>
        public ActionResult Delete(int productId)
        {
            if (_productRepository.RemoveProduct(productId))
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
