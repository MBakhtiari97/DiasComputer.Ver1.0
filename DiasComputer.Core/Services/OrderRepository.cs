using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiasComputer.Core.DTOs.Orders;
using DiasComputer.Core.Services.Interfaces;
using DiasComputer.DataLayer.Context;
using DiasComputer.DataLayer.Entities.Orders;
using DiasComputer.Utility.Convertors;
using Microsoft.EntityFrameworkCore;

namespace DiasComputer.Core.Services
{
    public class OrderRepository : IOrderRepository
    {
        private DiasComputerContext _context;

        public OrderRepository(DiasComputerContext context)
        {
            _context = context;
        }

        public IEnumerable<WalletTransactionsViewModel> GetWalletTransactionsHistory(int userId)
        {
            return _context.TransactionHistories
                .Where(t => t.IsWalletTransaction && t.UserId == userId)
                .Select(t =>
                new WalletTransactionsViewModel()
                {
                    UserId = userId,
                    Amount = t.Amount,
                    Date = t.Date,
                    Description = t.Description,
                    IsApproved = t.IsApproved,
                    PaymentMethod = t.PaymentMethod,
                    TH_Id = t.TH_Id,
                    TypeId = t.TypeId
                })
                .OrderByDescending(t => t.Date)
                .Take(10)
                .ToList();
        }

        public TransactionHistory GetTransactionByTransactionId(int transactionId)
        {
            return _context.TransactionHistories
                .Find(transactionId);
        }

        public void UpdateTransactionHistory(TransactionHistory tHistory)
        {
            _context.TransactionHistories.Update(tHistory);

            _context.Wallets
                .Single(w => w.UserId == tHistory.UserId)
                .Balance += tHistory.Amount;

            _context.SaveChanges();
        }

        public List<OrdersViewModel> GetMyOrders(int userId)
        {
            return _context.Orders
                .Where(o => o.UserId == userId)
                .Select(o => new OrdersViewModel()
                {
                    Date = o.OrderDate,
                    OrderId = o.OrderId,
                    OrderStatus = o.IsFinally,
                    OrderSum = o.OrderSum
                })
                .OrderByDescending(o => o.Date)
                .ToList();
        }

        public List<OrdersViewModel> GetLatestOrders(int userId)
        {
            return _context.Orders
                .Where(o => o.UserId == userId)
                .Select(o => new OrdersViewModel()
                {
                    Date = o.OrderDate,
                    OrderId = o.OrderId,
                    OrderStatus = o.IsFinally,
                    OrderSum = o.OrderSum
                })
                .OrderByDescending(o => o.Date)
                .Take(3)
                .ToList();
        }

        public List<OrderDetailsViewModel> GetOrderDetailsByOrderId(int orderId)
        {
            return _context.OrderDetails.Where(o => o.OrderId == orderId)
                .Include(o => o.Order)
                .Include(o => o.Product)
                .Select(o => new OrderDetailsViewModel()
                {
                    Color = o.Color,
                    Count = o.Count,
                    OrderId = o.OrderId,
                    ProductTitle = o.Product.ProductName,
                    ProductImage = o.Product.ProductImg,
                    ProductId = o.ProductId,
                    ProductPrice = o.ProductPrice,
                    ProductPriceSum = o.ProductPrice * o.Count,
                    Warranty = o.Warranty,
                    IsProcessed = o.Order.IsOrderProcessed
                })
                .ToList();
        }

        public bool DeleteOrder(int orderId)
        {
            try
            {
                var order = GetOrderByOrderId(orderId);
                order.IsDelete = true;
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Tuple<List<OrdersWaitingForActionViewModel>, int> GetOrdersHistory(int? orderId, bool? isFinally, int pageId)
        {
            //Getting all orders
            IQueryable<Order> orders = _context.Orders;

            //Filtering orders
            if (orderId != null)
                orders = orders.Where(o => o.OrderId == orderId);

            if (isFinally == true)
                orders = orders.Where(o => o.IsFinally == isFinally);

            //Pagination
            int take = 10;
            int skip = (pageId - 1) * take;
            int pageCount = orders.Count() / take;

            //Creating final orders
            var finalOrders = orders
                .Include(o => o.User)
                .Select(o => new OrdersWaitingForActionViewModel()
                {
                    UserId = o.UserId,
                    IsFinally = o.IsFinally,
                    OrderDate = o.OrderDate,
                    OrderId = o.OrderId,
                    UserName = o.User.UserName,
                    OrderSum = o.OrderSum
                }).Skip(skip)
                .Take(take)
                .ToList();

            return Tuple.Create(finalOrders, pageCount);
        }

        public int GetAllOrdersThisMonth()
        {
            return _context.Orders.Count(o => o.OrderDate >= GetCustomDates.GetMonthStartDate()
                                              && o.OrderDate <= GetCustomDates.GetMonthEndDate());
        }


        public int GetMonthlyEarning()
        {
            //Getting all earnings of this month
            var OrdersEarnings = _context.Orders
                .Where(o =>
                    o.IsFinally
                    && o.OrderDate >= GetCustomDates.GetMonthStartDate()
                    && o.OrderDate <= GetCustomDates.GetMonthEndDate())
                .Select(o => o.OrderSum)
                .ToList();

            int totalEarning = 0;

            //Summing earnings of month
            foreach (var earnValue in OrdersEarnings)
            {
                totalEarning += earnValue;
            }

            return totalEarning;
        }

        public List<RecentOrdersViewModel> GetRecentOrders()
        {
            return _context.Orders.Where(o => o.IsFinally && !o.IsOrderProcessed).Select(o =>
                new RecentOrdersViewModel()
                {
                    OrderId = o.OrderId,
                    OrderSum = o.OrderSum,
                    PaymentStatus = o.IsFinally,
                    UserId = o.UserId
                }).ToList();
        }


        public Order GetOrderByOrderId(int orderId)
        {
            return _context.Orders.Find(orderId);
        }

        public bool CancelOrder(int orderId)
        {
            try
            {
                var order = GetOrderByOrderId(orderId);
                order.IsCancelled = true;
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public TransactionHistory GetOrderTransaction(int orderId)
        {
            return _context.TransactionHistories
                .SingleOrDefault(t => t.OrderId == orderId);
        }

        public List<OrdersWaitingForActionViewModel> GetOrdersForAction()
        {
            return _context.Orders
                .Include(o => o.User)
                .Where(o => o.IsFinally && !o.IsOrderProcessed)
                .Select(o =>
                new OrdersWaitingForActionViewModel()
                {
                    UserId = o.UserId,
                    IsFinally = o.IsFinally,
                    OrderDate = o.OrderDate,
                    OrderId = o.OrderId,
                    OrderSum = o.OrderSum,
                    UserName = o.User.UserName
                }).ToList();
        }

        public InvoicePaymentDetails GetPaymentDetails(int orderId)
        {
            return _context.TransactionHistories
                .Where(t => t.OrderId == orderId)
                .Select(t =>
                new InvoicePaymentDetails()
                {
                    OrderId = t.OrderId,
                    PaymentMethod = t.PaymentMethod,
                    TrackingCode = t.TrackingCode,
                    PaymentDate = t.Date
                }).Single();
        }

        public bool ChangeOrderStatus(int orderId)
        {
            try
            {
                var order = GetOrderByOrderId(orderId);
                order.IsOrderProcessed = true;
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public Tuple<List<OrdersWaitingForActionViewModel>, int> GetAllOrders(int? orderId, int pageId = 1)
        {
            var orders = _context.Orders
                .OrderByDescending(o => o.OrderDate)
                .Include(o => o.User)
                .Select(o => new OrdersWaitingForActionViewModel()
                {
                    UserId = o.UserId,
                    IsFinally = o.IsFinally,
                    OrderDate = o.OrderDate,
                    OrderId = o.OrderId,
                    OrderSum = o.OrderSum,
                    UserName = o.User.UserName
                });

            if (orderId != null)
            {
                orders = orders.Where(o => o.OrderId == orderId);
            }

            //Pagination
            int take = 10;
            int skip = (pageId - 1) * take;
            int pageCount = orders.Count() / take;

            return Tuple.Create(orders.Skip(skip).Take(take).ToList(), pageCount);
        }

        public List<Order> GetUserOrders(int userId)
        {
            return _context.Orders
                .Where(o => o.UserId == userId && o.IsFinally)
                .OrderByDescending(o => o.OrderDate)
                .ToList();
        }
    }
}
