using System.Threading.Tasks;
using DiasComputer.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DiasComputer.Web.ViewComponents
{
    public class ResponsiveMenuItemsViewComponent:ViewComponent
    {
        private ICategoryRepository _categoryRepository;

        public ResponsiveMenuItemsViewComponent(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult((IViewComponentResult)View("/Views/Shared/ViewComponent/ResponsiveMenuItems.cshtml", _categoryRepository.GetAllCategories()));
        }
    }
}
