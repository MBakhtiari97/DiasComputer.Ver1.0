using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiasComputer.Core.DTOs.Admin;
using DiasComputer.DataLayer.Entities.SiteResources;

namespace DiasComputer.Core.Services.Interfaces
{
    public interface IBannerRepository
    {
        #region Banners
        /// <summary>
        /// Method will get top position banner
        /// </summary>
        Banner GetTopBanner();

        /// <summary>
        /// Method will all top middle position banners
        /// </summary>
        List<Banner> GetTopMiddleBanner();

        /// <summary>
        /// Method will get bottom position banner
        /// </summary>
        Banner GetBottomBanner();

        /// <summary>
        /// Method will get all bottom middle position banners
        /// </summary>
        List<Banner> GetBottomMiddleBanners();

        /// <summary>
        /// Method will get all side position banners
        /// </summary>
        List<Banner> GetSideBanners();

        #endregion

        #region Slider

        /// <summary>
        /// Method will returns all slides
        /// </summary>
        List<Slider> GetSliders();

        #endregion

        #region Admin

        #region Banner

        /// <summary>
        /// Method will return all banners
        /// </summary>
        List<BannersViewModel> GetAllBanners(int pageId = 1);

        /// <summary>
        /// Method will get a banner id and return a banner view model
        /// </summary>
        BannersViewModel GetBanner(int bannerId);

        /// <summary>
        /// Method will get a banner and update it
        /// </summary>
        bool UpdateBanner(BannersViewModel banner);

        /// <summary>
        /// Method will get a banner id and return a banner
        /// </summary>
        Banner GetBannerById(int bannerId);

        /// <summary>
        /// Method will get a banner id and deactivate it
        /// </summary>
        bool DeactivateBanner(int bannerId);

        /// <summary>
        /// Method will get a banner id and activate it
        /// </summary>
        bool ActivateBanner(int bannerId);


        #endregion

        #region Slider

        /// <summary>
        /// Method will return all slides
        /// </summary>
        List<SliderViewModel> GetAllSlides();

        /// <summary>
        /// Method will get a slider view model and create a new slide
        /// </summary>
        /// <returns>boolean</returns>
        bool AddNewSlide(SliderViewModel slide);

        /// <summary>
        /// Method will get a slider view model and update it
        /// </summary>
        /// <returns>boolean</returns>
        bool UpdateSlide(SliderViewModel slide);

        /// <summary>
        /// Method will get a slider id and deleting it
        /// </summary>
        /// <returns>boolean</returns>
        bool DeleteSlide(int slideId);

        /// <summary>
        /// Method will get a slide id and return a slide 
        /// </summary>
        Slider GetSlideBySlideId(int slideId);

        /// <summary>
        /// Method will get a slide id and return a slider view model
        /// </summary>
        SliderViewModel GetSlideViewModelBySlideId(int slideId);

        /// <summary>
        /// Method will get a slide id and returns the name of slide
        /// </summary>
        /// <returns>string</returns>
        string GetSlideNameBySlideId(int slideId);

        #endregion

        #endregion
    }
}
