using DiasComputer.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DiasComputer.Web.ViewComponents.IndexComponents.Banners
{
    public class BottomBannerViewComponent : ViewComponent
    {
        private IBannerRepository _bannerRepository;

        public BottomBannerViewComponent(IBannerRepository bannerRepository)
        {
            _bannerRepository = bannerRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult((IViewComponentResult)View("/Views/Shared/ViewComponent/IndexComponents/Banners/BottomBanner.cshtml",_bannerRepository.GetBottomBanner()));

        }
    }
}
