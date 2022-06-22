using DiasComputer.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DiasComputer.Web.ViewComponents.IndexComponents
{
    public class SuggestionWidgetViewComponent:ViewComponent
    {
        private IProductRepository _productRepository;

        public SuggestionWidgetViewComponent(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult((IViewComponentResult)View("/Views/Shared/ViewComponent/IndexComponents/SuggestionWidget.cshtml",_productRepository.GetLatestAmazingProducts()));
        }
    }
}
