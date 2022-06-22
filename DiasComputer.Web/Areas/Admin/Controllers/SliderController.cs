using AspNetCoreHero.ToastNotification.Abstractions;
using DiasComputer.Core.DTOs.Admin;
using DiasComputer.Core.OperationResults;
using DiasComputer.Core.Security;
using DiasComputer.Core.Services.Interfaces;
using DiasComputer.Utility.Generator;
using DiasComputer.Utility.Methods;
using DiasComputer.Utility.Security;
using Microsoft.AspNetCore.Mvc;

namespace DiasComputer.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [PermissionChecker(1024)]
    public class SliderController : Controller
    {
        private IBannerRepository _bannerRepository;
        private INotyfService _notyfService;

        public SliderController(IBannerRepository bannerRepository, INotyfService notyfService)
        {
            _bannerRepository = bannerRepository;
            _notyfService = notyfService;
        }

        [BindProperty]
        public SliderViewModel Slider { get; set; }

        #region Index

        /// <summary>
        /// Method will show slides index
        /// </summary>
        public IActionResult Index()
        {
            return View(_bannerRepository.GetAllSlides());
        }

        #endregion

        #region Create

        /// <summary>
        /// Method will create new slide
        /// </summary>
        public IActionResult AddNewSlide()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddNewSlide(SliderViewModel newSlide)
        {
            if (!ModelState.IsValid)
                return View(newSlide);

            if (Slider.Slide?.Length > 0)
            {
                if (Slider.Slide.IsImage())
                {
                    //Setting new name for slider in order to save
                    var newNameForSaving = StringGenerator.GenerateUniqueCode() + Path.GetExtension(Slider.Slide.FileName);

                    //Defining saving path
                    var newSlidePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "slider", newNameForSaving);

                    //Saving slide banner
                    using (var stream = new FileStream(newSlidePath, FileMode.Create))
                    {
                        Slider.Slide.CopyTo(stream);
                    }

                    //Setting new image name for storing on database
                    newSlide.SlideImg = newNameForSaving;
                }
                else
                {
                    _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.InvalidExtension.ToString()));
                    return View(newSlide);
                }
            }
            else
            {
                //Setting the default name if the banner was not selected
                newSlide.SlideImg = "Default.png";
            }

            //Adding the slider banner
            if (_bannerRepository.AddNewSlide(newSlide))
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));
                return RedirectToAction("Index");
            }
            else
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
                return View(newSlide);
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// Method will update slide
        /// </summary>
        public IActionResult UpdateSlide(int slideId)
        {
            return View(_bannerRepository.GetSlideViewModelBySlideId(slideId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateSlide(SliderViewModel updateSlide)
        {
            if (!ModelState.IsValid)
                return View(updateSlide);

            if (Slider.Slide?.Length > 0)
            {
                if (Slider.Slide.IsImage())
                {
                    //Setting new name for slider in order to save
                    var newNameForSaving = StringGenerator.GenerateUniqueCode() + Path.GetExtension(Slider.Slide.FileName);

                    //Defining saving path
                    var newSlidePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "slider", newNameForSaving);

                    //Defining old photo path
                    var oldSlidePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "slider", updateSlide.SlideImg);

                    //Checking if the slide was not equal to default slide name then trying to deleting it
                    if (updateSlide.SlideImg != "Default.png")
                    {
                        //Deleting old slide if it exists
                        if (System.IO.File.Exists(oldSlidePath))
                        {
                            System.IO.File.Delete(oldSlidePath);
                        }
                    }

                    //Saving slide banner
                    using (var stream = new FileStream(newSlidePath, FileMode.Create))
                    {
                        Slider.Slide.CopyTo(stream);
                    }

                    //Setting new image name for storing on database
                    updateSlide.SlideImg = newNameForSaving;
                }
                else
                {
                    _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.InvalidExtension.ToString()));
                    return View(updateSlide);
                }
            }

            if (_bannerRepository.UpdateSlide(updateSlide))
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));
                return RedirectToAction("Index");
            }
            else
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
                return View(updateSlide);
            }
        }

        #endregion

        #region Delete

        /// <summary>
        /// Method will delete slide
        /// </summary>
        public IActionResult DeleteSlide(int slideId)
        {
            var slideImageName = _bannerRepository.GetSlideNameBySlideId(slideId);

            if (_bannerRepository.DeleteSlide(slideId))
            {

                var slidePath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images",
                    "slider",
                    slideImageName);

                if (System.IO.File.Exists(slidePath) && slideImageName != "Default.png")
                {
                    System.IO.File.Delete(slidePath);
                }

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
