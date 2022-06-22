using System.Threading.Tasks;
using DiasComputer.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DiasComputer.Web.ViewComponents
{
    public class BasketViewComponent : ViewComponent
    {
        private ICartRepository _cartRepository;
        IUserRepository _userRepository;

        public BasketViewComponent(ICartRepository cartRepository, IUserRepository userRepository)
        {
            _cartRepository = cartRepository;
            _userRepository = userRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = _userRepository.GetUserIdByEmailAddress(User.Identity.Name);
            return await Task.FromResult((IViewComponentResult)View("/Views/Shared/ViewComponent/Basket.cshtml",_cartRepository.GetCartItems(userId)));
        }
    }
}
