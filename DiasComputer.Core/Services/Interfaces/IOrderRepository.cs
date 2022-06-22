using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiasComputer.Core.DTOs.Orders;
using DiasComputer.DataLayer.Entities.Orders;

namespace DiasComputer.Core.Services.Interfaces
{
    public interface IOrderRepository
    {
        #region Wallet

        IEnumerable<WalletTransactionsViewModel> GetWalletTransactionsHistory(int userId);
        TransactionHistory GetTransactionByTransactionId(int transactionId);
        void UpdateTransactionHistory(TransactionHistory tHistory);

        #endregion

        #region Orders

        List<OrdersViewModel> GetMyOrders(int userId);
        List<OrdersViewModel> GetLatestOrders(int userId);
        Order GetOrderByOrderId(int orderId);


        #endregion

        #region Admin

        #region Orders

        List<Order> GetUserOrders(int userId);
        bool CancelOrder(int orderId);
        List<OrdersWaitingForActionViewModel> GetOrdersForAction();
        bool ChangeOrderStatus(int orderId);
        Tuple<List<OrdersWaitingForActionViewModel>, int> GetAllOrders(int? orderId, int pageId = 1);
        List<OrderDetailsViewModel> GetOrderDetailsByOrderId(int orderId);
        bool DeleteOrder(int orderId);
        Tuple<List<OrdersWaitingForActionViewModel>, int> GetOrdersHistory(int? orderId, bool? isFinally, int pageId);
        int GetAllOrdersThisMonth();
        int GetMonthlyEarning();
        List<RecentOrdersViewModel> GetRecentOrders();

        #endregion

        #region Payment

        TransactionHistory GetOrderTransaction(int orderId);
        InvoicePaymentDetails GetPaymentDetails(int orderId);

        #endregion

        #endregion
    }
}
