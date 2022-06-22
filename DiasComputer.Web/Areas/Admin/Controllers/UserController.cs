using AspNetCoreHero.ToastNotification.Abstractions;
using DiasComputer.Core.DTOs.Admin;
using DiasComputer.Core.OperationResults;
using DiasComputer.Core.Security;
using DiasComputer.Core.Services.Interfaces;
using DiasComputer.DataLayer.Entities.Users;
using DiasComputer.Utility.Generator;
using DiasComputer.Utility.Methods;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiasComputer.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [PermissionChecker(2)]
    public class UserController : Controller
    {
        IUserRepository _userRepository;
        private INotyfService _notyfService;
        IOrderRepository _orderRepository;

        
        public UserController(IUserRepository userRepository, INotyfService notyfService, IOrderRepository orderRepository)
        {
            _userRepository = userRepository;
            _notyfService = notyfService;
            _orderRepository = orderRepository;
        }

        [BindProperty]
        public UserViewModel.ManageUsersViewModel User { get; set; }

        #region Users

        #region UsersIndex

        /// <summary>
        /// Method will show users index
        /// </summary>
        public ActionResult Index(string? emailAddress, string? bankAccountNumber, string? nationalCode, string? phoneNumber, string? userName, int pageId = 1)
        {
            #region RequiredViewBags

            //Filling View bags for showing if it was not null
            ViewBag.PageId = pageId;
            ViewBag.EmailAddress = emailAddress;
            ViewBag.BankAccountNumber = bankAccountNumber;
            ViewBag.NationalCode = nationalCode;
            ViewBag.PhoneNumber = phoneNumber;
            ViewBag.UserName = userName;

            #endregion

            return View(_userRepository.GetAllUsers(emailAddress, bankAccountNumber, nationalCode, phoneNumber, userName, pageId));
        }

        #endregion

        #region CreateNewUser

        /// <summary>
        /// Method will create new user
        /// </summary>
        public ActionResult Create()
        {
            ViewBag.UserRoles = _userRepository.GetRoles();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserViewModel.ManageUsersViewModel user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(user);
                if (User.Avatar != null && User.Avatar.FileName.Length > 0)
                {
                    var newName = StringGenerator.GenerateUniqueCode()
                                  + Path.GetExtension(User.Avatar.FileName);

                    var path = Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot/images/userAvatars", newName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        User.Avatar.CopyTo(stream);
                    }

                    user.UserAvatar = newName;
                }
                else
                {
                    user.UserAvatar = "Default.png";
                }

                if (_userRepository.JoinNewUser(user))
                {
                    _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));
                }
                else
                {
                    _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.DuplicateEmail.ToString()));
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("Index");
            }

        }

        #endregion

        #region UpdateUser

        /// <summary>
        /// Method will update user
        /// </summary>
        public ActionResult Edit(int id)
        {
            ViewBag.UserRoles = _userRepository.GetRoles();
            return View(_userRepository.GetSingleUserForAdmin(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserViewModel.ManageUsersViewModel user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(user);
                //Getting current details
                var currentUserDetails = _userRepository.GetSingleUserForAdmin(user.UserId);

                //Checking if the admin wants to change the photo
                if (User.Avatar != null && User.Avatar.FileName.Length > 0)
                {
                    //Giving a new name
                    var newName = StringGenerator.GenerateUniqueCode()
                                  + Path.GetExtension(User.Avatar.FileName);

                    //Gathering path
                    var path = Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot/images/userAvatars", newName);

                    //Copying new avatar image
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        User.Avatar.CopyTo(stream);
                    }

                    user.UserAvatar = newName;

                    //Deleting old avatar
                    if (currentUserDetails.UserAvatar != "Default.png")
                    {
                        var oldPath = Path.Combine(Directory.GetCurrentDirectory(),
                            "wwwroot/images/userAvatars", currentUserDetails.UserAvatar);

                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                        }
                    }
                }
                //Otherwise giving the current image name to user
                else
                {
                    user.UserAvatar = currentUserDetails.UserAvatar;
                }

                if (_userRepository.UpdateExistedUser(user))
                {
                    _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));
                }
                else
                {
                    _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.DuplicateEmail.ToString()));
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        #endregion

        #region DeleteUser

        /// <summary>
        /// Method will delete user
        /// </summary>
        public ActionResult Delete(int id)
        {
            if (_userRepository.RemoveUser(id))
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));
            }
            else
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
            }
            return RedirectToAction("Index");
        }

        #endregion

        #endregion

        #region UserDeliveryInfo's

        #region DeliveryDetails

        /// <summary>
        /// Method will show delivery details
        /// </summary>
        public ActionResult DeliveryDetails(int userId)
        {
            return View(_userRepository.GetDeliveryDetailsByUserId(userId));
        }

        #endregion

        #region DeleteDeliveryDetails

        
        public ActionResult DeleteDeliveryDetails(int pdId)
        {
            if (_userRepository.DeleteDeliveryDetails(pdId))
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));
            }
            else
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
            }

            return RedirectToAction("Index");
        }

        #endregion

        #endregion

        #region Wallet

        #region ShowWalletStatus

        /// <summary>
        /// Method will show wallet status
        /// </summary>
        public ActionResult WalletStatus(int userId)
        {
            return View(_userRepository.GetUserWallet(userId));
        }

        #endregion

        #region ChangeWalletStatus

        /// <summary>
        /// Method will change wallet status
        /// </summary>
        public ActionResult ChangeWalletStatus(int walletId)
        {
            if (_userRepository.ChangeWalletStatus(walletId))
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));
            }
            else
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
            }
            return RedirectToAction("Index");
        }

        #endregion

        #endregion

        #region UserOrders

        #region ShowUserOrders

        /// <summary>
        /// Method will show user orders
        /// </summary>
        public ActionResult UserOrders(int userId)
        {
            return View(_orderRepository.GetUserOrders(userId));
        }

        #endregion

        #region ShowOrderDetails

        /// <summary>
        /// Method will show order details
        /// </summary>
        public ActionResult OrderDetails(int orderId, int userId)
        {
            ViewBag.UserId = userId;
            return View(_orderRepository.GetOrderDetailsByOrderId(orderId));
        }

        #endregion

        #region CancelOrder

        /// <summary>
        /// Method will cancel an order
        /// </summary>
        public ActionResult CancelOrder(int orderId, int userId)
        {
            if (_orderRepository.CancelOrder(orderId))
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));
            }
            else
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
            }

            return Redirect($"/Admin/User/UserOrders?userId={userId}");
        }

        #endregion

        #endregion

        #region UserTransactions

        #region ShowUserTransactions

        /// <summary>
        /// Method will show user transactions
        /// </summary>
        public ActionResult UserTransactions(int userId)
        {
            return View(_userRepository.GetUserTransactionsByUserId(userId));
        }

        #endregion

        #region TransactionDetails

        /// <summary>
        /// Method will show transaction details
        /// </summary>
        public ActionResult OrderTransactionDetails(int orderId)
        {
            var transactionDetails = _orderRepository.GetOrderTransaction(orderId);
            return View(transactionDetails);
        }

        #endregion

        #endregion
    }
}
