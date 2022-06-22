using DiasComputer.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DiasComputer.Web.ViewComponents.IndexComponents
{
    public class BrandsViewComponent : ViewComponent
    {
       ISiteRepository _siteRepository;

       public BrandsViewComponent(ISiteRepository siteRepository)
       {
           _siteRepository = siteRepository;
       }

       public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult((IViewComponentResult)View("/Views/Shared/ViewComponent/IndexComponents/Brands.cshtml", _siteRepository.GetBrands()));

        }
    }
}
