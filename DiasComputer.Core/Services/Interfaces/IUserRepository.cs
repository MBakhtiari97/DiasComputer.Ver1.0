using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiasComputer.Core.DTOs.Account;
using DiasComputer.Core.DTOs.Admin;
using DiasComputer.Core.DTOs.Orders;
using DiasComputer.DataLayer.Entities.Orders;
using DiasComputer.DataLayer.Entities.Users;
using DiasComputer.Utility.Generator;
using DiasComputer.Utility.Methods;
using DiasComputer.Utility.Security;
using Microsoft.AspNetCore.Identity;

namespace DiasComputer.Core.Services.Interfaces
{
    public interface IUserRepository
    {
        #region Account

        public Task<int> AddNewUser(SignUpViewModel signUp);
        public User? SignInUser(SignInViewModel signIn);
        public User? GetUserByEmailAddress(string emailAddress);
        public User? CheckUserForRecoverPasswordByIdentifier(string emailAddress, string identifierCode);
        public bool SerNewPasswordForRecovery(string emailAddress, string identifierCode, string newPassword);
        public bool ActivateUserProfile(string emailAddress, string identifierCode);
        bool IsUserAdmin(int userId);

        #endregion

        #region Profile

        public ShowProfileViewModel GetUserForProfile(int userId);
        public bool ChangeUserPassword(ChangePasswordViewModel change, int userId);
        public bool UpdateUserDetailsProfile(User user);
        public User GetUserById(int userId);
        public UpdateProfileViewModel GetUserProfile(int userId);
        public List<PostDetail> GetPostDetails(int userId);
        public PostDetail GetUserPostDetailById(int id);
        public bool UpdatePostDetails(PostDetail postDetail);
        public bool AddPostDetails(PostDetail postDetail);
        public UserForProfileMenuViewModel GetUserDetailsForProfileMenu(string emailAddress);
        public List<ReferredOrdersViewModel> GetReferredOrdersByUserId(int userId);

        #endregion

        #region Wallet

        public Wallet? GetUserWallet(int userId);
        public bool CreateWallet(int userId);
        public bool ActivateWallet(int walletId);
        public int ChargeWallet(int userId, int amount, string description, bool isApproved = false);
        int GetUserWalletBalance(int userId);

        #endregion

        #region User

        int GetUserIdByEmailAddress(string emailAddress);
        public bool RemoveFromWishList(int wishListId, int userId);
        bool AddToWishList(int productId, int userId);

        #endregion

        #region Admin

        #region Users

        Tuple<List<UserViewModel.ManageUsersViewModel>, int> GetAllUsers
            (string? emailAddress, string? bankAccountNumber, string? nationalCode, string? phoneNumber, string? userName, int pageId);
        bool UpdateExistedUser(UserViewModel.ManageUsersViewModel user);
        bool CheckEmailExists(string emailAddress);
        UserViewModel.ManageUsersViewModel GetSingleUserForAdmin(int userId);
        bool RemoveUser(int userId);
        bool ChangeWalletStatus(int walletId);
        List<TransactionHistory> GetUserTransactionsByUserId(int userId);

        List<UserViewModel.LatestRegisteredUsers> GetLatestRegisteredUsers();
        int GetLatestRegisteredUsersCount();

        #endregion

        #region Roles

        List<Role> GetRoles();
        UserRole GetUserRoleByUserId(int userId);

        #endregion

        #region Delivery

        PostDetail? GetDeliveryDetailsByUserId(int userId);
        bool DeleteDeliveryDetails(int pdId);
        UserDetailsForProcessOrderViewModel GetUserDeliveryDetails(int userId);

        #endregion

        #region NewsLetter

        List<string> GetActiveNewsletters();
        List<string> GetAdminsEmails();
        bool JoinNewUser(UserViewModel.ManageUsersViewModel user);

        #endregion

        #endregion
    }
}
