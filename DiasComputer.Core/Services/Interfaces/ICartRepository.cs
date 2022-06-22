using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiasComputer.Core.DTOs.Cart;
using DiasComputer.DataLayer.Entities.Orders;
using DiasComputer.DataLayer.Entities.Users;

namespace DiasComputer.Core.Services.Interfaces
{
    public interface ICartRepository
    {

        #region Cart

        List<CartItemsViewModel> GetCartItems(int userId);
        public bool RemoveFromCart(int detailId, int orderId);
        public bool AddToCart(int productId, string color, int userId);
        ProductDetailsForAddToCartViewModel GetProductDetails(int productId);

        #endregion

        #region Orders

        int GetTotalOrderPriceWithShipping(int userId);
        public Order GetLatestOrderByUserId(int userId);
        bool IsOrderExist(int userId);

        #endregion

        #region Delivery

        PostDetail? GetPostDetails(int userId);
        bool InsertOrUpdatePostDetails(PostDetail post);
        Tuple<PostDetail, List<CartItemsViewModel>> GetCheckOutDetails(int userId);

        #endregion

        #region Payment

        public int MakePayment(int userId, int orderId, int amount, string description, bool isApproved = false);
        public TransactionHistory GetTransactionHistoryForPayment(int transactionHistoryId);
        public void UpdateTransactionAndOrder(TransactionHistory transactionHistory);
        public PaymentConclusionViewModel GetPaymentDetails(int userId);
        void UpdateTransactionHistoryForWalletPayment(int thId);
        bool CheckWalletBalance(int userId, int totalPrice);
        bool PayWithWallet(int userId, int totalPrice);
        int CheckAndGetCouponValue(string coupon);

        #endregion
    }
}
