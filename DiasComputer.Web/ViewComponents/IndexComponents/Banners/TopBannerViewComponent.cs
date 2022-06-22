using DiasComputer.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DiasComputer.Web.ViewComponents.IndexComponents.Banners
{
    public class TopBannerViewComponent : ViewComponent
    {
        private IBannerRepository _bannerRepository;

        public TopBannerViewComponent(IBannerRepository bannerRepository)
        {
            _bannerRepository = bannerRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult((IViewComponentResult)View("/Views/Shared/ViewComponent/IndexComponents/Banners/TopBanner.cshtml",_bannerRepository.GetTopBanner()));

        }
    }
}
