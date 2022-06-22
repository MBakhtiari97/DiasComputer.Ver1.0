using DiasComputer.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DiasComputer.Web.ViewComponents.IndexComponents
{
    public class SliderViewComponent:ViewComponent
    {
        IBannerRepository _bannerRepository;

        public SliderViewComponent(IBannerRepository bannerRepository)
        {
            _bannerRepository = bannerRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult((IViewComponentResult)View("/Views/Shared/ViewComponent/IndexComponents/Slider.cshtml",_bannerRepository.GetSliders()));

        }
    }
}
