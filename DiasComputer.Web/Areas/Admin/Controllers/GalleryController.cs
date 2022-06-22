using AspNetCoreHero.ToastNotification.Abstractions;
using DiasComputer.Core.DTOs.Admin;
using DiasComputer.Core.OperationResults;
using DiasComputer.Core.Security;
using DiasComputer.Core.Services.Interfaces;
using DiasComputer.Utility.Convertors;
using DiasComputer.Utility.Generator;
using DiasComputer.Utility.Methods;
using Microsoft.AspNetCore.Mvc;

namespace DiasComputer.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [PermissionChecker(3)]
    public class GalleryController : Controller
    {
        private IProductRepository _productRepository;
        private INotyfService _notyfService;

        public GalleryController(IProductRepository productRepository, INotyfService notyfService)
        {
            _productRepository = productRepository;
            _notyfService = notyfService;
        }

        [BindProperty] public GalleriesViewModel Galleries { get; set; }

        #region Index

        /// <summary>
        /// Method will get parameter and show gallery index
        /// </summary>
        public IActionResult GalleryIndex(int productId)
        {
            ViewBag.ProductId = productId;
            return View(_productRepository.GetGalleryByProductId(productId));
        }

        #endregion

        #region Create

        /// <summary>
        /// Method will create new gallery image
        /// </summary>
        public IActionResult AddNewGalleryImage(int productId)
        {
            ViewBag.ProductId = productId;
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult AddNewGalleryImage(GalleriesViewModel gallery)
        {
            if (!ModelState.IsValid)
                return View(gallery);

            if (Galleries.SelectedImage?.Length > 0)
            {
                var newName = StringGenerator.GenerateUniqueCode() +
                              Path.GetExtension(Galleries.SelectedImage.FileName);

                //Defining new image path and thumb path
                var imgPath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images",
                    "productGalleries",
                    newName);

                var imgThumbPath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images",
                    "productThumbnailsGalleries",
                    newName);

                //Saving new image
                using (var stream = new FileStream(imgPath, FileMode.Create))
                {
                    Galleries.SelectedImage.CopyTo(stream);
                }

                //Saving new thumb image
                ImageConvertor img = new ImageConvertor();
                img.Image_resize(imgPath, imgThumbPath, 280);

                gallery.ImgName = newName;

            }

            //Updating gallery
            if (_productRepository.AddImageToGallery(gallery))
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));
            }
            else
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
            }

            return Redirect($"/Admin/Gallery/GalleryIndex?productId={gallery.ProductId}");
        }


        #endregion

        #region Update

        /// <summary>
        /// Method will update gallery image
        /// </summary>
        public IActionResult UpdateGalleryImage(int galleryId)
        {
            return View(_productRepository.GetGalleryViewModel(galleryId));
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult UpdateGalleryImage(GalleriesViewModel gallery)
        {
            if (!ModelState.IsValid)
                return View(gallery);

            if (Galleries.SelectedImage?.Length > 0)
            {

                var imgName = gallery.ImgName.Split(".");

                var newName = imgName[0] + Path.GetExtension(Galleries.SelectedImage.FileName);

                //Defining new image path and thumb path
                var imgPath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images",
                    "productGalleries",
                    newName);

                var imgThumbPath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images",
                    "productThumbnailsGalleries",
                    newName);

                //Old paths
                var oldImgPath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images",
                    "productGalleries",
                    gallery.ImgName);

                var oldImgThumbPath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images",
                    "productThumbnailsGalleries",
                    gallery.ImgName);

                //Checking if the image extension was changed then deleting old images
                if ("." + imgName[1] != Path.GetExtension(Galleries.SelectedImage.FileName))
                {
                    System.IO.File.Delete(oldImgPath);
                    System.IO.File.Delete(oldImgThumbPath);
                }

                //Saving new image
                using (var stream = new FileStream(imgPath, FileMode.Create))
                {
                    Galleries.SelectedImage.CopyTo(stream);
                }

                //Saving new thumb image
                ImageConvertor img = new ImageConvertor();
                img.Image_resize(imgPath, imgThumbPath, 280);


                //Defining new name for saving on database
                gallery.ImgName = newName;
            }

            //Updating gallery
            if (_productRepository.UpdateImageGallery(gallery))
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));
            }
            else
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
            }

            //Returning 
            return Redirect($"/Admin/Gallery/GalleryIndex?productId={gallery.ProductId}");
        }


        #endregion

        #region Delete

        /// <summary>
        /// Method will get parameters delete gallery image
        /// </summary>
        public IActionResult DeleteGalleryImage(int galleryId, int productId)
        {
            //Getting image name
            var galleryImgName = _productRepository.GetGalleryImageByGalleryId(galleryId);

            //Defining image path and thumb path
            var imgPath = Path.Combine(Directory.GetCurrentDirectory(),
                "wwwroot",
                "images",
                "productGalleries",
                galleryImgName);

            var imgThumbPath = Path.Combine(Directory.GetCurrentDirectory(),
                "wwwroot",
                "images",
                "productThumbnailsGalleries",
                galleryImgName);

            //Checking if image was existed then deleting it 
            if (System.IO.Directory.Exists(imgPath) && galleryImgName != "Default.png")
            {
                System.IO.File.Delete(imgPath);
            }

            if (System.IO.Directory.Exists(imgThumbPath) && galleryImgName != "Default.png")
            {
                System.IO.File.Delete(imgThumbPath);
            }

            //Deleting img details from database
            if (_productRepository.DeleteImageGallery(galleryId))
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));
            }
            else
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
            }

            return Redirect($"/Admin/Gallery/GalleryIndex?productId={productId}");
        }

        #endregion
    }
}
