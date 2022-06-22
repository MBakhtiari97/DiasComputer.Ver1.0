using System.Security.Claims;
using AspNetCoreHero.ToastNotification.Abstractions;
using DiasComputer.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DiasComputer.Web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    public class OrderController : Controller
    {
        IUserRepository _userRepository;
        IOrderRepository _orderRepository;
        private INotyfService _notyfService;

        public OrderController(IUserRepository userRepository, IOrderRepository orderRepository, INotyfService notyfService)
        {
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _notyfService = notyfService;
        }

        #region RefferedOrders
        /// <summary>
        /// Method will show user referred orders
        /// </summary
        [Route("/MyProfile/Orders/Referred")]
        public IActionResult MyReferredOrders()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());
            return View(_userRepository.GetReferredOrdersByUserId(userId));
        }

        #endregion

        #region Orders

        /// <summary>
        /// Method will show user orders
        /// </summary>
        [Route("/MyProfile/Orders")]
        public IActionResult MyOrders()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());
            return View(_orderRepository.GetMyOrders(userId));
        }

        #endregion

        #region OrderDetails

        /// <summary>
        /// Method will show user order details
        /// </summary>
        [Route("/MyProfile/Orders/ShowDetails/{orderId}")]
        public IActionResult ShowOrderDetails(int orderId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());

            //Checking if user wants to access to a unrelated order id then redirecting it
            if (_orderRepository.GetOrderByOrderId(orderId).UserId != userId)
            {
                _notyfService.Error("درخواست نامعتبر !");
                return Redirect("/MyProfile/Orders");
            }

            return View(_orderRepository.GetOrderDetailsByOrderId(orderId));
        }

        #endregion
    }
}
