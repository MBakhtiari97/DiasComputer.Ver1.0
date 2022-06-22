using System.Security.Claims;
using AspNetCoreHero.ToastNotification.Abstractions;
using DiasComputer.Core.DTOs.Account;
using DiasComputer.Core.OperationResults;
using DiasComputer.Core.Services.Interfaces;
using DiasComputer.DataLayer.Entities.Users;
using DiasComputer.Utility.Generator;
using DiasComputer.Utility.Methods;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiasComputer.Web.Areas.UserPanel
{
    [Authorize]
    [Area("UserPanel")]
    public class UserController : Controller
    {
        IUserRepository _userRepository;
        private INotyfService _notyfService;
        IProductRepository _productRepository;
        private IOrderRepository _orderRepository;

        public UserController(IUserRepository userRepository, INotyfService notyfService, IProductRepository productRepository, IOrderRepository orderRepository)
        {
            _userRepository = userRepository;
            _notyfService = notyfService;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
        }

        [BindProperty]
        public UpdateProfileViewModel UpdateProfile { get; set; }

        #region Profile

        #region UserProfile

        /// <summary>
        /// Method will show user profile
        /// </summary>
        [Route("MyProfile")]
        public IActionResult MyProfile()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = _userRepository.GetUserForProfile(userId);

            if (user.UserId != userId)
            {
                return NotFound();
            }

            ViewBag.LatestWishLists = _productRepository.GetLatestWishLists(userId);
            ViewBag.MyLatestOrders = _orderRepository.GetLatestOrders(userId);
            return View(user);
        }

        #endregion

        #region ChangePassword

        /// <summary>
        /// Method will change user password
        /// </summary>
        [Authorize]
        [Route("MyProfile/ChangePassword")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Route("MyProfile/ChangePassword")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(ChangePasswordViewModel changePassword)
        {
            if (!ModelState.IsValid)
            {
                return View(changePassword);
            }
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (_userRepository.ChangeUserPassword(changePassword, userId))
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));
                return Redirect("/SignOut");
            }
            else
            {
                ModelState.AddModelError("CurrentPassword", "کلمه عبور فعلی صحیح نمی باشد و یا خطایی رخ داده است !");
                return View(changePassword);
            }
        }

        #endregion


        #region ChangeProfileDetails

        /// <summary>
        /// Method will change the user profile details
        /// </summary>
        [Route("MyProfile/ChangeDetails")]
        public IActionResult ChangeProfileDetails()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());

            var userDetails = _userRepository.GetUserProfile(userId);
            if (userDetails.UserId != userId)
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.UnAuthorized.ToString()));
                return RedirectToAction("MyProfile");
            }
            return View(userDetails);
        }

        [Route("MyProfile/ChangeDetails")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeProfileDetails(UpdateProfileViewModel update)
        {
            if (!ModelState.IsValid)
            {
                return View(update);
            }

            var user = _userRepository
                   .GetUserById(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
                   .ToString()));


            if (UpdateProfile.UserAvatar.FileName != null)
            {
                if (user.UserAvatar != null && user.UserAvatar != "Default.png")
                {
                    var oldImagePath = Path.Combine(
                        Directory.GetCurrentDirectory()
                        , "wwwroot/images/userAvatars",
                        user.UserAvatar);

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                var newAvatarName = StringGenerator.GenerateUniqueCode() +
                                    Path.GetExtension(UpdateProfile.UserAvatar.FileName);

                var newImagePath = Path.Combine(Directory.GetCurrentDirectory()
                    , "wwwroot/images/userAvatars",
                    newAvatarName);

                using (var stream = new FileStream(newImagePath, FileMode.Create))
                {
                    UpdateProfile.UserAvatar.CopyTo(stream);
                }

                user.UserAvatar = newAvatarName;
            }

            user.EmailAddress = update.EmailAddress;
            user.UserName = update.UserName;
            user.BankAccountNumber = update.BankAccountNumber;
            user.NationalCode = update.NationalCode;
            user.PhoneNumber = update.PhoneNumber;

            if (_userRepository.UpdateUserDetailsProfile(user))
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));
            }
            else
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
            }

            return RedirectToAction("MyProfile");
        }

        #endregion

        #endregion

        #region WishList

        #region UserWishList

        /// <summary>
        /// Method will show user wish list items
        /// </summary>
        [Route("/MyProfile/WishList")]
        public IActionResult MyWishList()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());
            return View(_productRepository.GetAllWishLists(userId));
        }

        #endregion

        #region AddToWishList

        /// <summary>
        /// Method will add new item to user wish list
        /// </summary>
        [Route("/MyProfile/WishList/Add/{productId}")]
        public IActionResult AddToWishList(int productId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());

            if (_userRepository.AddToWishList(productId, userId))
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

        #region RemoveFromWishList

        /// <summary>
        /// Method will remove item from user wish list
        /// </summary>
        [Route("/MyProfile/WishList/Remove/{wishListId}")]
        public IActionResult RemoveFromWishlist(int wishListId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());
            if (_userRepository.RemoveFromWishList(wishListId, userId))
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));
                return RedirectToAction("MyWishList");
            }
            else
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
                return RedirectToAction("MyWishList");
            }
        }

        #endregion

        #endregion

        #region PostDetails

        #region PostalDetails

        /// <summary>
        /// Method will show user postal details
        /// </summary>
        [Route("/MyProfile/PostDetails")]
        public IActionResult MyPostDetails()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());
            return View(_userRepository.GetPostDetails(userId));
        }

        #endregion

        #region UpdatePostalDetails

        /// <summary>
        /// Method will update user postal details
        /// </summary>
        [Route("/MyProfile/PostDetails/Update/{id}")]
        public IActionResult UpdatePostDetails(int id)
        {
            return View(_userRepository.GetUserPostDetailById(id));
        }

        [HttpPost]
        [Route("/MyProfile/PostDetails/Update/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult UpdatePostDetails(PostDetail post)
        {
            if (_userRepository.UpdatePostDetails(post))
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));
            }
            else
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
            }
            return RedirectToAction("MyPostDetails");
        }

        #endregion

        #region InsertPostalDetails

        /// <summary>
        /// Method will insert user postal details
        /// </summary>
        [Route("/MyProfile/PostDetails/Insert")]
        public IActionResult InsertPostDetails()
        {
            ViewBag.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());
            return View();
        }

        [Route("/MyProfile/PostDetails/Insert")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult InsertPostDetails(PostDetail postDetail)
        {
            if (postDetail.UserId != int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString()))
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.UnAuthorized.ToString()));
                return RedirectToAction("MyPostDetails");
            }

            if (_userRepository.AddPostDetails(postDetail))
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));
            }
            else
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
            }

            return RedirectToAction("MyPostDetails");
        }


        #endregion


        #endregion
    }
}
