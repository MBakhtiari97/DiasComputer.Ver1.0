using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiasComputer.Core.DTOs.Admin;
using DiasComputer.Core.Services.Interfaces;
using DiasComputer.DataLayer.Context;
using DiasComputer.DataLayer.Entities.SiteResources;

namespace DiasComputer.Core.Services
{
    public class BannerRepository : IBannerRepository
    {
        DiasComputerContext _context;

        public BannerRepository(DiasComputerContext context)
        {
            _context = context;
        }

        public Banner GetTopBanner()
        {
            return _context.Banners.SingleOrDefault(b => b.BannerPosition == "Top" && b.IsActive);
        }

        public List<Banner> GetTopMiddleBanner()
        {
            return _context.Banners.Where(b => b.BannerPosition == "TopMiddle" && b.IsActive).ToList();
        }

        public Banner GetBottomBanner()
        {
            return _context.Banners.SingleOrDefault(b => b.BannerPosition == "Bottom" && b.IsActive);
        }

        public List<Banner> GetBottomMiddleBanners()
        {
            return _context.Banners.Where(b => b.BannerPosition == "BottomMiddle" && b.IsActive).ToList();
        }

        public List<Banner> GetSideBanners()
        {
            return _context.Banners.Where(b => b.BannerPosition == "Side" && b.IsActive).ToList();
        }

        public List<Slider> GetSliders()
        {
            return _context.Sliders.ToList();
        }

        public List<BannersViewModel> GetAllBanners(int pageId = 1)
        {
            //Getting banners
            var banners = _context.Banners
                .Select(b => new BannersViewModel()
                {
                    IsActive = b.IsActive,
                    BannerRedirectTo = b.BannerRedirectTo,
                    BannerAltName = b.BannerAltName,
                    BannerImg = b.BannerImg,
                    BannerPosition = b.BannerPosition,
                    BannerSize = b.BannerSize,
                    BannerId = b.BannerId
                });


            return banners.ToList();
        }

        public BannersViewModel GetBanner(int bannerId)
        {
            return _context.Banners.Where(b => b.BannerId == bannerId)
                .Select(b => new BannersViewModel()
                {
                    IsActive = b.IsActive,
                    BannerRedirectTo = b.BannerRedirectTo,
                    BannerAltName = b.BannerAltName,
                    BannerImg = b.BannerImg,
                    BannerPosition = b.BannerPosition,
                    BannerSize = b.BannerSize,
                    BannerId = b.BannerId
                }).Single();
        }

        public bool UpdateBanner(BannersViewModel banner)
        {
            try
            {
                //Getting banner
                var currentBanner = GetBannerById(banner.BannerId);

                //Changing banner values
                currentBanner.BannerImg = banner.BannerImg;
                currentBanner.BannerAltName = banner.BannerAltName;
                currentBanner.BannerRedirectTo = banner.BannerRedirectTo;
                currentBanner.IsActive = banner.IsActive;

                //Saving new values
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Banner GetBannerById(int bannerId)
        {
            return _context.Banners.Find(bannerId);
        }

        public bool DeactivateBanner(int bannerId)
        {
            try
            {
                var banner = GetBannerById(bannerId);
                banner.IsActive = false;

                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ActivateBanner(int bannerId)
        {
            try
            {
                var banner = GetBannerById(bannerId);
                banner.IsActive = true;

                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<SliderViewModel> GetAllSlides()
        {
            return _context.Sliders
                .Select(s => new SliderViewModel()
                {
                    SlideAltName = s.SlideAltName,
                    SlideRedirectTo = s.SlideRedirectTo,
                    SlideImg = s.SlideImg,
                    SlideId = s.SlideId
                })
                .ToList();
        }

        public bool AddNewSlide(SliderViewModel slide)
        {
            try
            {
                //Creating a new instance of Slider and filling it
                var newSlide = new Slider()
                {
                    SlideAltName = slide.SlideAltName,
                    SlideRedirectTo = slide.SlideRedirectTo,
                    SlideImg = slide.SlideImg
                };

                //Adding new slide
                _context.Sliders.Add(newSlide);

                //Saving changes
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateSlide(SliderViewModel slide)
        {
            try
            {
                //Getting slide
                var currentSlide = GetSlideBySlideId(slide.SlideId);

                //Updating slide
                currentSlide.SlideAltName = slide.SlideAltName;
                currentSlide.SlideRedirectTo = slide.SlideRedirectTo;
                currentSlide.SlideImg = slide.SlideImg;

                //Saving changes
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteSlide(int slideId)
        {
            try
            {
                var slide = GetSlideBySlideId(slideId);
                _context.Sliders.Remove(slide);
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public Slider GetSlideBySlideId(int slideId)
        {
            return _context.Sliders.Find(slideId);
        }

        public SliderViewModel GetSlideViewModelBySlideId(int slideId)
        {
            return _context.Sliders
                .Where(s => s.SlideId == slideId)
                .Select(s => new SliderViewModel()
                {
                    SlideAltName = s.SlideAltName,
                    SlideRedirectTo = s.SlideRedirectTo,
                    SlideImg = s.SlideImg,
                    SlideId = s.SlideId

                })
                .Single();
        }

        public string GetSlideNameBySlideId(int slideId)
        {
            return _context.Sliders
                .Find(slideId)
                .SlideImg;
        }
    }
}
