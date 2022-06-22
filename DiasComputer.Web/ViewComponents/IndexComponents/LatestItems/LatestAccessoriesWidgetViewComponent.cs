using DiasComputer.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DiasComputer.Web.ViewComponents.IndexComponents.LatestItems
{
    public class LatestAccessoriesWidgetViewComponent : ViewComponent
    {
        private IProductRepository _productRepository;
        ICategoryRepository _categoryRepository;

        public LatestAccessoriesWidgetViewComponent(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var groupId = 17;
            var model = _productRepository.GetLatestProducts(groupId);
            ViewBag.GroupId = groupId;
            ViewBag.GroupTitle = _categoryRepository.GetGroupNameByGroupId(groupId);
            return await Task.FromResult((IViewComponentResult)View("/Views/Shared/ViewComponent/IndexComponents/LatestItems/LatestProductWidget.cshtml", model));
        }
    }
}
