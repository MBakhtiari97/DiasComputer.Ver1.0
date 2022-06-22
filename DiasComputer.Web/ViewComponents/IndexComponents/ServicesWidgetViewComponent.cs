using Microsoft.AspNetCore.Mvc;

namespace DiasComputer.Web.ViewComponents.IndexComponents
{
    public class ServicesWidgetViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult((IViewComponentResult)View("/Views/Shared/ViewComponent/IndexComponents/ServicesWidget.cshtml"));

        }
    }
}
