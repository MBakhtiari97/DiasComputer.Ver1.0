using DiasComputer.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DiasComputer.Web.Areas.UserPanel.ViewComponents
{
    public class ProfileMenuViewComponent:ViewComponent
    {
        IUserRepository _userRepository;

        public ProfileMenuViewComponent(IUserRepository userRepository)
        {
            _userRepository = userRepository;   
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var details = _userRepository.GetUserDetailsForProfileMenu(User.Identity.Name);
            return await Task.FromResult((IViewComponentResult)View("/Areas/UserPanel/Views/Shared/ViewComponents/ProfileMenu.cshtml", details));
        }
    }
}
