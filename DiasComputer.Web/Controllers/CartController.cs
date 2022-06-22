using System.Security.Claims;
using System.Security.Cryptography;
using AspNetCoreHero.ToastNotification.Abstractions;
using DiasComputer.Core.OperationResults;
using DiasComputer.Core.Services.Interfaces;
using DiasComputer.DataLayer.Entities.Users;
using DiasComputer.Utility.Generator;
using DiasComputer.Utility.Methods;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiasComputer.Web.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private ICartRepository _cartRepository;
        private INotyfService _notyfService;
        IUserRepository _userRepository;

        public CartController(ICartRepository cartRepository, INotyfService notyfService, IUserRepository userRepository)
        {
            _cartRepository = cartRepository;
            _notyfService = notyfService;
            _userRepository = userRepository;
        }

        #region ShowCart
        /// <summary>
        /// This method will returns cart items
        /// </summary>
        [Route("/Cart")]
        public IActionResult ShowCart()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewBag.TotalPrice = _cartRepository.GetTotalOrderPriceWithShipping(userId);
            return View(_cartRepository.GetCartItems(userId));
        }

        #endregion

        #region AddToCart
        /// <summary>
        /// This method will add items to cart
        /// </summary>
        [HttpPost]
        public IActionResult AddToCart(int productId, string color)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (_cartRepository.AddToCart(productId, color, userId))
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));
            }
            else
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
            }
            return Redirect($"/Products/{productId}");
        }


        #endregion

        #region CheckOutForm
        /// <summary>
        /// This method will returns submit user postal information's
        /// </summary>
        [Route("/Cart/Form")]
        public IActionResult CheckOutDetailsForm()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewBag.UserId = userId;
            return View(_cartRepository.GetPostDetails(userId));
        }
        [HttpPost]
        [Route("/Cart/Form")]
        public IActionResult CheckOutDetailsForm(PostDetail post)
        {
            ModelState.Remove("PD_Id");
            if (!ModelState.IsValid)
            {
                return View(post);
            }
            if (!_cartRepository.InsertOrUpdatePostDetails(post))
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
                return RedirectToAction("ShowCart");
            }

            return RedirectToAction("ShowCheckoutDetails");
        }


        #endregion

        #region CheckOutDetails
        /// <summary>
        /// This method will returns show and submit the order details page
        /// </summary>
        [Route("/Cart/Checkout")]
        public IActionResult ShowCheckoutDetails()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            ViewBag.TotalPrice = _cartRepository.GetTotalOrderPriceWithShipping(userId);
            ViewBag.WalletBalance = _userRepository.GetUserWalletBalance(userId);

            return View(_cartRepository.GetCheckOutDetails(userId));
        }

        [Route("/Cart/Checkout")]
        [HttpPost]
        public IActionResult ShowCheckoutDetails(int totalPrice, string paymentMethod, string? coupon)
        {

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());
            //Checking for price again in order to make sure that the price is correct 
            if (_cartRepository.GetTotalOrderPriceWithShipping(userId) != totalPrice)
            {
                _notyfService.Error("درخواست نامعتبر !");
                return RedirectToAction("ShowCart");
            }

            //Checking for coupon
            if (coupon != null)
            {
                //if method return 0 then the coupon is invalid
                if (_cartRepository.CheckAndGetCouponValue(coupon) == 0)
                {
                    _notyfService.Warning("کد تخفیف وارد شده معتبر نمی باشد !");
                    return RedirectToAction("ShowCart");
                }
                //Otherwise the coupon is valid and we need to reducing it from final price
                else
                {
                    var couponPercent = _cartRepository.CheckAndGetCouponValue(coupon);
                    var percent = (decimal)couponPercent / 100;
                    var percentTimesToTotalPrice = percent * totalPrice;
                    var finalPrice = totalPrice - (int)percentTimesToTotalPrice;
                    totalPrice = finalPrice;
                }
            }

            var order = _cartRepository.GetLatestOrderByUserId(userId);
            int transactionId = _cartRepository.MakePayment(userId, order.OrderId, totalPrice, "خرید محصول");



            //Checking if the user wants to pay with wallet balance running these codes
            if (paymentMethod == "wallet")
            {
                //paying with wallet if the balance is enough
                if (_cartRepository.PayWithWallet(userId, totalPrice))
                {
                    //First updating transaction type to wallet transaction
                    _cartRepository.UpdateTransactionHistoryForWalletPayment(transactionId);

                    //Getting transaction history and approving the transaction
                    var transactionHistory = _cartRepository.GetTransactionHistoryForPayment(transactionId);
                    transactionHistory.PaymentMethod = "کیف پول";
                    transactionHistory.IsApproved = true;

                    //Setting the transaction type to withdraw
                    transactionHistory.TypeId = 2;
                    //Setting generated tracking code 
                    transactionHistory.TrackingCode = ReferCodeGenerator.Generate().ToString();


                    //Updating transaction and order status
                    _cartRepository.UpdateTransactionAndOrder(transactionHistory);

                    //Returning result page
                    return Redirect("/Cart/Payment/" + transactionId + "?PaymentStatus=success");
                }
                //if the balance isn't enough then payment is failed 
                else
                {
                    //Returning result page
                    return Redirect("/Cart/Payment/" + transactionId + "?PaymentStatus=failed");
                }
            }

            #region OnlinePayment

            var payment = new ZarinpalSandbox.Payment(totalPrice);

            var response =
                payment.PaymentRequest("خرید محصول", "https://localhost:44394/Cart/Payment/" + transactionId);

            if (response.Result.Status == 100)
            {
                return Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + response.Result.Authority);
            }

            #endregion

            return BadRequest();
        }


        #endregion

        #region OnlinePayment
        /// <summary>
        /// This method will pay the order 
        /// </summary>
        [Route("/Cart/Payment/{transactionId}")]
        public IActionResult OnlineProductPayment(int transactionId)
        {
            if (HttpContext.Request.Query["Status"] != ""
                && HttpContext.Request.Query["Status"].ToString().ToLower() == "ok"
                && HttpContext.Request.Query["Authority"] != "")
            {
                string authority = HttpContext.Request.Query["Authority"];

                var transactionHistory = _cartRepository.GetTransactionHistoryForPayment(transactionId);

                var payment = new ZarinpalSandbox.Payment(transactionHistory.Amount);
                var response = payment.Verification(authority).Result;

                if (response.Status == 100)
                {
                    ViewBag.Code = response.RefId;
                    ViewBag.IsSuccess = true;
                    ViewBag.Date = DateTime.Now;
                    ViewBag.Amount = transactionHistory.Amount;

                    transactionHistory.IsApproved = true;
                    transactionHistory.TrackingCode = response.RefId.ToString();

                    _cartRepository.UpdateTransactionAndOrder(transactionHistory);
                }

            }

            if (HttpContext.Request.Query["PaymentStatus"] == "success")
            {
                ViewBag.IsSuccess = true;
                ViewBag.Date = DateTime.Now;
            }

            if (HttpContext.Request.Query["PaymentStatus"] == "failed")
            {
                ViewBag.FailedWalletPayment = true;
                ViewBag.IsSuccess = false;
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());
            return View(_cartRepository.GetPaymentDetails(userId));
        }

        #endregion

        #region RemoveFromCart
        /// <summary>
        /// This method will remove an item from cart
        /// </summary>
        [Route("/Cart/Remove/{orderId}/{detailId}")]
        public IActionResult RemoveFromCart(int orderId, int detailId)
        {
            if (_cartRepository.RemoveFromCart(detailId, orderId))
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));
            }
            else
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
            }

            return RedirectToAction("ShowCart");
        }

        #endregion

    }
}
