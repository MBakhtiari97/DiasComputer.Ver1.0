using System.Security.Claims;
using AspNetCoreHero.ToastNotification.Abstractions;
using DiasComputer.Core.DTOs.Orders;
using DiasComputer.Core.OperationResults;
using DiasComputer.Core.Services.Interfaces;
using DiasComputer.Utility.Methods;
using Microsoft.AspNetCore.Mvc;

namespace DiasComputer.Web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    public class Wallet : Controller
    {
        IUserRepository _userRepository;
        private INotyfService _notyfService;
        IOrderRepository _orderRepository;

        public Wallet(IUserRepository userRepository, INotyfService notyfService, IOrderRepository orderRepository)
        {
            _userRepository = userRepository;
            _notyfService = notyfService;
            _orderRepository = orderRepository;
        }

        #region ShowWallet
        /// <summary>
        /// Method will show user wallet
        /// </summary>
        [Route("/MyProfile/Wallet")]
        public IActionResult ShowWallet()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());

            //Getting user wallet transactions
            ViewBag.WalletTransactions = _orderRepository.GetWalletTransactionsHistory(userId);

            return View(_userRepository.GetUserWallet(userId));
        }

        #endregion

        #region CreateWallet

        /// <summary>
        /// Method will create user wallet
        /// </summary>
        [Route("/Wallet/Create")]
        public IActionResult CreateWallet()
        {

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());

            //Creating user wallet
            if (_userRepository.CreateWallet(userId))
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));
            }
            else
            {
                _notyfService.Warning(OperationResultText.ShowResult(OperationResult.Result.ExistedWallet.ToString()));
            }


            return RedirectToAction("ShowWallet");
        }


        #endregion

        #region ActivateWallet

        /// <summary>
        /// Method will activate user wallet
        /// </summary>
        [Route("/Wallet/Activate")]
        public IActionResult ActivateWallet()
        {
            //Getting current user wallet and activating it
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());
            var userWallet = _userRepository.GetUserWallet(currentUserId);

            if (_userRepository.ActivateWallet(userWallet.WalletId))
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));
            }
            else
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
            }

            return RedirectToAction("ShowWallet");
        }

        #endregion

        #region IncreaseBalance

        /// <summary>
        /// Method will increase user wallet balance
        /// </summary>
        [Route("/Wallet/UpBalance")]
        public IActionResult IncreaseBalance()
        {
            return View();
        }

        [Route("/Wallet/UpBalance")]
        [HttpPost]
        public IActionResult IncreaseBalance(ChargeWalletViewModel charge)
        {
            if (!ModelState.IsValid)
            {
                return View(charge);
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());

            int transactionId = _userRepository.ChargeWallet(userId, charge.Amount, "افزایش موجودی کیف پول");

            #region OnlinePayment

            var payment = new ZarinpalSandbox.Payment(charge.Amount);

            var response =
                payment.PaymentRequest("شارژ کیف پول", "https://localhost:44394/OnlinePayment/" + transactionId);

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
        /// Method will use for paying 
        /// </summary>
        [Route("OnlinePayment/{transactionId}")]
        public IActionResult OnlinePayment(int transactionId)
        {
            if (HttpContext.Request.Query["Status"] != ""
                && HttpContext.Request.Query["Status"].ToString().ToLower() == "ok"
                && HttpContext.Request.Query["Authority"] != "")
            {
                string authority = HttpContext.Request.Query["Authority"];

                var transactionHistory = _orderRepository.GetTransactionByTransactionId(transactionId);

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
                    _orderRepository.UpdateTransactionHistory(transactionHistory);
                }
            }
            return View();
        }


        #endregion

    }
}
