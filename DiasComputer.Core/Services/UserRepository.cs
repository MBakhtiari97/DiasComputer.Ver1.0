using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiasComputer.Core.DTOs.Account;
using DiasComputer.Core.DTOs.Admin;
using DiasComputer.Core.DTOs.Orders;
using DiasComputer.Core.Services.Interfaces;
using DiasComputer.DataLayer.Context;
using DiasComputer.DataLayer.Entities.Orders;
using DiasComputer.DataLayer.Entities.Users;
using DiasComputer.Utility.Convertors;
using DiasComputer.Utility.Generator;
using DiasComputer.Utility.Methods;
using DiasComputer.Utility.Security;
using DiasComputer.Utility.SendEmail;
using Microsoft.EntityFrameworkCore;

namespace DiasComputer.Core.Services
{
    public class UserRepository : IUserRepository
    {
        private DiasComputerContext _context;
        IViewRenderService _viewRenderService;

        public UserRepository(DiasComputerContext context, IViewRenderService viewRenderService)
        {
            _context = context;
            _viewRenderService = viewRenderService;
        }

        public async Task<int> AddNewUser(SignUpViewModel signUp)
        {
            try
            {
                //1 For successful register 2 for existed email 3 for other errors
                if (_context.Users.Any(u => u.EmailAddress == FixedText.FixEmail(signUp.EmailAddress)))
                {
                    return 2;
                }

                //Creating a new user
                User newUser = new User()
                {
                    EmailAddress = FixedText.FixEmail(signUp.EmailAddress),
                    Password = PasswordHelper.EncodePasswordMd5(signUp.Password),
                    IsActive = false,
                    IsDelete = false,
                    RegisterDate = DateTime.Now,
                    UniqueCode = StringGenerator.GenerateUniqueCode(),
                    UserAvatar = "Default.png"
                };

                //Adding new user
                _context.Users.Add(newUser);
                _context.SaveChanges();

                //Creating new user role 
                UserRole newUserRole = new UserRole()
                {
                    RoleId = 1,
                    UserId = newUser.UserId
                };
                
                //Adding new user role
                _context.UserRoles.Add(newUserRole);
                _context.SaveChanges();

                var body = await _viewRenderService.RenderToStringAsync("Account/Emails/_ActivationEmail", newUser);
                SendEmail.Send(newUser.EmailAddress, "تایید حساب کاربری", body);

                return 1;
            }
            catch
            {
                return 3;
            }
        }

        public User? SignInUser(SignInViewModel signIn)
        {

            var emailAddress = FixedText.FixEmail(signIn.EmailAddress);
            var password = PasswordHelper.EncodePasswordMd5(signIn.Password);

            return _context.Users
                .SingleOrDefault(u => u.EmailAddress == emailAddress && u.Password == password);
        }

        public User? GetUserByEmailAddress(string emailAddress)
        {
            return _context.Users
                .SingleOrDefault(u => u.EmailAddress == emailAddress);

        }

        public User? CheckUserForRecoverPasswordByIdentifier(string emailAddress, string identifierCode)
        {
            var user = _context.Users
                .SingleOrDefault(u =>
                u.EmailAddress == emailAddress && u.UniqueCode == identifierCode);

            return user;
        }

        public bool SerNewPasswordForRecovery(string emailAddress, string identifierCode, string newPassword)
        {
            try
            {
                //First getting user
                var user = _context.Users
                    .SingleOrDefault(u =>
                        u.EmailAddress == emailAddress && u.UniqueCode == identifierCode);

                if (user != null)
                {
                    //changing user password and updating user
                    user.Password = PasswordHelper.EncodePasswordMd5(newPassword);
                    user.UniqueCode = StringGenerator.GenerateUniqueCode();

                    if (!user.IsActive)
                    {
                        user.IsActive = true;
                    }

                    _context.Users.Update(user);
                    _context.SaveChanges();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

        }

        public bool ActivateUserProfile(string emailAddress, string identifierCode)
        {
            try
            {
                var user = _context.Users
                    .SingleOrDefault(u =>
                        u.EmailAddress == emailAddress && u.UniqueCode == identifierCode);
                if (user != null)
                {
                    user.IsActive = true;
                    user.UniqueCode = StringGenerator.GenerateUniqueCode();
                    _context.Users.Update(user);
                    _context.SaveChanges();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

        }

        public bool IsUserAdmin(int userId)
        {

            var user = _context.Users
                .Where(u => u.UserId == userId)
                .Include(u => u.UserRole).Single();


            if (user.UserRole.First().RoleId != 1)
                return true;

            return false;

        }

        public ShowProfileViewModel GetUserForProfile(int userId)
        {
            return _context.Users
                .Where(u => u.UserId == userId)
                .Select(u => new ShowProfileViewModel()
                {
                    EmailAddress = u.EmailAddress,
                    BankAccountCardNumber = u.BankAccountNumber,
                    NationalCode = u.NationalCode,
                    Phone = u.PhoneNumber,
                    UserId = userId,
                    UserName = u.UserName,
                    ProfilePhoto = u.UserAvatar
                }).First();
        }

        public bool ChangeUserPassword(ChangePasswordViewModel change, int userId)
        {
            try
            {
                var user = _context.Users.Find(userId);

                var currentPassword = PasswordHelper.EncodePasswordMd5(change.CurrentPassword);
                var newPassword = PasswordHelper.EncodePasswordMd5(change.Password);

                if (user.Password == currentPassword)
                {
                    user.Password = newPassword;
                    _context.Users.Update(user);
                    _context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

        }

        public bool UpdateUserDetailsProfile(User user)
        {
            try
            {
                _context.Users.Update(user);
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public User GetUserById(int userId)
        {
            return _context.Users.Find(userId);
        }

        public UpdateProfileViewModel GetUserProfile(int userId)
        {
            return _context.Users.Where(u => u.UserId == userId).Select(u => new UpdateProfileViewModel()
            {
                PhoneNumber = u.PhoneNumber,
                BankAccountNumber = u.BankAccountNumber,
                CurrentProfile = u.UserAvatar,
                EmailAddress = u.EmailAddress,
                NationalCode = u.NationalCode,
                UserId = u.UserId,
                UserName = u.UserName
            }).First();
        }

        public List<PostDetail> GetPostDetails(int userId)
        {
            return _context.PostDetails
                .Where(p => p.UserId == userId)
                .ToList();
        }

        public PostDetail GetUserPostDetailById(int id)
        {
            return _context.PostDetails.Find(id);
        }

        public bool UpdatePostDetails(PostDetail postDetail)
        {
            try
            {
                _context.PostDetails.Update(postDetail);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool AddPostDetails(PostDetail postDetail)
        {
            try
            {
                _context.PostDetails.Add(postDetail);
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool RemoveFromWishList(int wishListId, int userId)
        {
            try
            {
                if (_context.WishLists.Find(wishListId).UserId == userId)
                {
                    var wishListItem = _context.WishLists.Find(wishListId);
                    _context.WishLists.Remove(wishListItem);
                    _context.SaveChanges();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool AddToWishList(int productId, int userId)
        {
            try
            {
                //Checking if there wasnt exist a wish list with this product then add it , otherwise return false
                if (!_context.WishLists.Any(w => w.UserId == userId && w.ProductId == productId))
                {
                    var newWishList = new WishList()
                    {
                        UserId = userId,
                        ProductId = productId
                    };

                    _context.Add(newWishList);
                    _context.SaveChanges();
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }

        }

        public Tuple<List<UserViewModel.ManageUsersViewModel>, int> GetAllUsers(string? emailAddress, string? bankAccountNumber, string? nationalCode, string? phoneNumber, string? userName, int pageId)
        {
            //Getting all users
            IQueryable<User> users = _context.Users;

            //Applying active filters
            if (emailAddress != null)
            {
                users = users.Where(u => u.EmailAddress.Contains(emailAddress));
            }

            if (bankAccountNumber != null)
            {
                users = users.Where(u => u.BankAccountNumber.Contains(bankAccountNumber));
            }

            if (userName != null)
            {
                users = users.Where(u => u.UserName.Contains(userName));
            }

            if (phoneNumber != null)
            {
                users = users.Where(u => u.PhoneNumber.Contains(phoneNumber));
            }

            if (nationalCode != null)
            {
                users = users.Where(u => u.NationalCode.Contains(nationalCode));
            }

            //Pagination
            int take = 10;
            int skip = (pageId - 1) * take;
            int usersPageCount = users.Count() / take;

            //Creating final list of users
            var finalUsers = users
                .Select(u => new UserViewModel.ManageUsersViewModel()
                {
                    BankAccountNumber = u.BankAccountNumber,
                    EmailAddress = u.EmailAddress,
                    IsActive = u.IsActive,
                    NationalCode = u.NationalCode,
                    PhoneNumber = u.PhoneNumber,
                    RegisterDate = u.RegisterDate,
                    UserAvatar = u.UserAvatar,
                    UserId = u.UserId,
                    UserName = u.UserName
                })
                .Skip(skip)
                .Take(take)
                .ToList();

            return Tuple.Create(finalUsers, usersPageCount);
        }

        public bool JoinNewUser(UserViewModel.ManageUsersViewModel user)
        {
            try
            {
                var emailAddress = FixedText.FixEmail(user.EmailAddress);
                if (CheckEmailExists(emailAddress))
                {
                    return false;
                }

                User newUser = new User()
                {
                    BankAccountNumber = user.BankAccountNumber,
                    EmailAddress = emailAddress,
                    IsActive = true,
                    IsDelete = false,
                    NationalCode = user.NationalCode,
                    Password = PasswordHelper.EncodePasswordMd5(user.Password),
                    PhoneNumber = user.PhoneNumber,
                    RegisterDate = DateTime.Now,
                    UniqueCode = StringGenerator.GenerateUniqueCode(),
                    UserAvatar = user.UserAvatar,
                    UserName = user.UserName
                };
                _context.Users.Add(newUser);
                _context.SaveChanges();

                UserRole newUserRole = new UserRole()
                {
                    RoleId = user.RoleId,
                    UserId = newUser.UserId
                };
                _context.UserRoles.Add(newUserRole);
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<Role> GetRoles()
        {
            return _context.Roles
                .ToList();
        }

        public bool CheckEmailExists(string emailAddress)
        {
            if (_context.Users.Any(u => u.EmailAddress == emailAddress))
                return true;

            return false;
        }

        public bool UpdateExistedUser(UserViewModel.ManageUsersViewModel user)
        {
            try
            {
                //Getting current user details
                var currentUser = GetUserById(user.UserId);

                //Fixing email address
                var emailAddress = FixedText.FixEmail(user.EmailAddress);
                if (currentUser != null)
                {
                    //Setting new details
                    currentUser.NationalCode = user.NationalCode;
                    currentUser.PhoneNumber = user.PhoneNumber;
                    currentUser.UserAvatar = user.UserAvatar;
                    currentUser.UserName = user.UserName;
                    currentUser.BankAccountNumber = user.BankAccountNumber;
                    currentUser.UniqueCode = StringGenerator.GenerateUniqueCode();
                    currentUser.IsActive = user.IsActive;

                    //Checking if the email address is unique then updating it
                    if (currentUser.EmailAddress != emailAddress)
                    {
                        if (!CheckEmailExists(emailAddress))
                        {
                            currentUser.EmailAddress = emailAddress;
                        }
                        else
                        {
                            return false;
                        }
                    }

                    //Updating user role
                    var userRole = GetUserRoleByUserId(user.UserId);
                    userRole.RoleId = user.RoleId;

                    //Saving changes
                    _context.SaveChanges();
                    return true;

                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public UserRole GetUserRoleByUserId(int userId)
        {
            return _context.UserRoles
                .Single(ur => ur.UserId == userId);
        }

        public UserViewModel.ManageUsersViewModel GetSingleUserForAdmin(int userId)
        {
            return _context.Users
                .Include(u => u.UserRole)
                .Where(u => u.UserId == userId)
                .Select(u => new UserViewModel.ManageUsersViewModel()
                {
                    BankAccountNumber = u.BankAccountNumber,
                    EmailAddress = u.EmailAddress,
                    IsActive = u.IsActive,
                    NationalCode = u.NationalCode,
                    PhoneNumber = u.PhoneNumber,
                    RoleId = u.UserRole.First().RoleId,
                    UserAvatar = u.UserAvatar,
                    UserId = u.UserId,
                    UserName = u.UserName
                }).Single();
        }

        public bool RemoveUser(int userId)
        {
            try
            {
                var user = _context.Users.SingleOrDefault(u => u.UserId == userId);
                if (user != null)
                {
                    user.IsDelete = true;
                    _context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public PostDetail? GetDeliveryDetailsByUserId(int userId)
        {
            return _context.PostDetails
                .SingleOrDefault(u => u.UserId == userId);
        }

        public bool DeleteDeliveryDetails(int pdId)
        {
            try
            {
                var details = _context.PostDetails.Find(pdId);
                _context.PostDetails.Remove(details);
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ChangeWalletStatus(int walletId)
        {
            try
            {
                var userWallet = _context.Wallets.SingleOrDefault(w => w.WalletId == walletId);
                if (userWallet != null)
                {
                    if (userWallet.IsActive == false)
                    {
                        userWallet.IsActive = true;
                    }
                    else
                    {
                        userWallet.IsActive = false;
                    }

                    _context.SaveChanges();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public List<TransactionHistory> GetUserTransactionsByUserId(int userId)
        {
            return _context.TransactionHistories
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.Date)
                .ToList();
        }

        public UserDetailsForProcessOrderViewModel GetUserDeliveryDetails(int userId)
        {
            return _context.PostDetails.Include(p => p.User).Where(p => p.UserId == userId).Select(p =>
                new UserDetailsForProcessOrderViewModel()
                {
                    Address = p.Address,
                    City = p.City,
                    EmailAddress = p.User.EmailAddress,
                    NationalCode = p.NationalCode,
                    PhoneNumber = p.PhoneNumber,
                    UserId = p.UserId,
                    UserName = p.UserName,
                    ZipCode = p.ZipCode
                }).Single();
        }

        public List<UserViewModel.LatestRegisteredUsers> GetLatestRegisteredUsers()
        {
            var latestUsers = _context.Users.Where(u =>
                    u.RegisterDate >= GetCustomDates.GetMonthStartDate()
                    && u.RegisterDate <= GetCustomDates.GetMonthEndDate())
                .Select(u => new UserViewModel.LatestRegisteredUsers()
                {
                    RegisterDate = u.RegisterDate,
                    UserAvatar = u.UserAvatar,
                    UserId = u.UserId,
                    UserName = u.UserName
                })
                .OrderByDescending(u => u.RegisterDate)
                .Take(6)
                .ToList();

            return latestUsers;
        }

        public int GetLatestRegisteredUsersCount()
        {
            return _context.Users.Count(u =>
                    u.RegisterDate >= GetCustomDates.GetMonthStartDate()
                    && u.RegisterDate <= GetCustomDates.GetMonthEndDate());
        }

        public UserForProfileMenuViewModel GetUserDetailsForProfileMenu(string emailAddress)
        {
            return _context.Users
                .Where(u => u.EmailAddress == emailAddress)
                .Select(u => new UserForProfileMenuViewModel()
                {
                    UserId = u.UserId,
                    UserName = u.UserName,
                    UserProfile = u.UserAvatar
                })
                .Single();
        }

        public List<ReferredOrdersViewModel> GetReferredOrdersByUserId(int userId)
        {
            return _context.ReferOrders
                .Include(r => r.Order)
                .Where(r => r.Order.UserId == userId)
                .Select(r => new ReferredOrdersViewModel()
                {
                    OrderDate = r.Order.OrderDate,
                    OrderId = r.OrderId,
                    OrderSumPrice = r.Order.OrderSum,
                    ReferredCode = r.ReferCode,
                    ReferredStatus = r.ReferStatus
                })
                .ToList();
        }

        public Wallet? GetUserWallet(int userId)
        {
            return _context.Wallets
                .SingleOrDefault(w => w.UserId == userId);
        }

        public bool CreateWallet(int userId)
        {
            try
            {
                if (!_context.Wallets.Any(w => w.UserId == userId))
                {
                    _context.Wallets.Add(new Wallet()
                    {
                        Balance = 0,
                        IsActive = false,
                        UserId = userId
                    });
                    _context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch
            {
                return false;
            }
        }

        public bool ActivateWallet(int walletId)
        {
            try
            {
                _context.Wallets.Find(walletId).IsActive = true;
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public int ChargeWallet(int userId, int amount, string description, bool isApproved = false)
        {
            TransactionHistory transactionHistory = new TransactionHistory()
            {
                UserId = userId,
                Amount = amount,
                Date = DateTime.Now,
                IsApproved = isApproved,
                Description = description,
                IsWalletTransaction = true,
                PaymentMethod = "درگاه پرداخت زرین پال",
                TypeId = 1,
                TrackingCode = "0"
            };

            _context.TransactionHistories.Add(transactionHistory);
            _context.SaveChanges();
            return transactionHistory.TH_Id;
        }

        public int GetUserWalletBalance(int userId)
        {
            if (_context.Wallets.Any(w => w.UserId == userId))
            {
                return _context.Wallets.Single(w => w.UserId == userId).Balance;
            }

            return 0;
        }

        public int GetUserIdByEmailAddress(string emailAddress)
        {
            return _context.Users
                .Single(u => u.EmailAddress == emailAddress)
                .UserId;
        }
        public List<string> GetActiveNewsletters()
        {
            return _context.NewsLetters.Where(n => !n.IsCanceled).Select(n => n.EmailAddress).ToList();
        }

        public List<string> GetAdminsEmails()
        {
            var users = _context.UserRoles
                .Include(ur => ur.User)
                .Where(ur => ur.RoleId == 4)
                .Select(ur => ur.User);

            return users.Select(u => u.EmailAddress).ToList();
        }
    }
}
