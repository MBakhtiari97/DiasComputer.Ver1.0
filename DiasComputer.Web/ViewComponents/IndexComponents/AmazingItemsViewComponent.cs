using DiasComputer.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DiasComputer.Web.ViewComponents.IndexComponents
{
    public class AmazingItemsViewComponent:ViewComponent
    {
        private IProductRepository _productRepository;

        public AmazingItemsViewComponent(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult((IViewComponentResult)View("/Views/Shared/ViewComponent/IndexComponents/AmazingItems.cshtml",_productRepository.GetLatestAmazingProducts()));

        }
    }
}
