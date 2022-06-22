using AspNetCoreHero.ToastNotification.Abstractions;
using DiasComputer.Core.DTOs.Orders;
using DiasComputer.Core.OperationResults;
using DiasComputer.Core.Security;
using DiasComputer.Core.Services.Interfaces;
using DiasComputer.Utility.Methods;
using Microsoft.AspNetCore.Mvc;
using Stimulsoft.Base;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;

namespace DiasComputer.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [PermissionChecker(9)]
    public class OrderController : Controller
    {
        private IOrderRepository _orderRepository;
        private INotyfService _notyfService;
        private IUserRepository _userRepository;

        public OrderController(IOrderRepository orderRepository, INotyfService notyfService, IUserRepository userRepository)
        {
            _orderRepository = orderRepository;
            _notyfService = notyfService;
            _userRepository = userRepository;
        }

        #region UnProccessOrdersIndex

        /// <summary>
        /// Method will show un processed orders
        /// </summary>
        [Route("/Admin/Order/UnProcess")]
        public ActionResult AwaitingProcessOrders()
        {
            return View(_orderRepository.GetOrdersForAction());
        }

        #endregion

        #region OrdersHistory

        /// <summary>
        /// Method will show the history of orders
        /// </summary>
        public ActionResult OrdersHistory(int? orderId, bool? isFinally, int pageId = 1)
        {
            ViewBag.PageId = pageId;

            if (orderId != null)
            {
                ViewBag.OrderId = orderId;
            }

            if (isFinally != null)
            {
                ViewBag.IsFinally = isFinally;
            }

            return View(_orderRepository.GetOrdersHistory(orderId, isFinally, pageId));
        }

        #endregion

        #region OrderDetailsForProcess

        /// <summary>
        /// Method will get the order details for show
        /// </summary>
        public ActionResult ProcessOrder(int orderId, int userId)
        {
            ViewBag.DeliveryDetails = _userRepository.GetUserDeliveryDetails(userId);
            return View(_orderRepository.GetOrderDetailsByOrderId(orderId));
        }

        #endregion

        #region ProcessOrder

        /// <summary>
        /// Method will change the order status as processed
        /// </summary>
        public ActionResult Process(int orderId)
        {
            if (_orderRepository.ChangeOrderStatus(orderId))
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));
            }
            else
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
            }
            return RedirectToAction("AwaitingProcessOrders");
        }

        #endregion

        #region PrintInvoice

        /// <summary>
        /// Method will print an invoice for order
        /// </summary>
        public ActionResult PrintInvoice(int orderId, int userId)
        {
            //Sending required details for printing invoice
            ViewBag.DeliveryDetails = _userRepository.GetUserDeliveryDetails(userId);
            ViewBag.PaymentDetails = _orderRepository.GetPaymentDetails(orderId);

            return View(_orderRepository.GetOrderDetailsByOrderId(orderId));
        }

        #endregion

        #region OrdersIndex

        /// <summary>
        /// Method will show orders
        /// </summary>
        public ActionResult AllOrders(int? orderId, int pageId = 1)
        {
            ViewBag.PageId = pageId;
            return View(_orderRepository.GetAllOrders(orderId, pageId));
        }

        #endregion

        #region DeleteOrder

        /// <summary>
        /// Method will delete an order
        /// </summary>
        public ActionResult Delete(int orderId)
        {
            if (_orderRepository.DeleteOrder(orderId))
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));
            }
            else
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
            }

            return RedirectToAction("AllOrders");
        }

        #endregion
    }
}
