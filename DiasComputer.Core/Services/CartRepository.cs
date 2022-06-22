using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspNetCore;
using DiasComputer.Core.DTOs.Cart;
using DiasComputer.Core.Services.Interfaces;
using DiasComputer.DataLayer.Context;
using DiasComputer.DataLayer.Entities.Orders;
using DiasComputer.DataLayer.Entities.Users;
using DiasComputer.Utility.Compute;
using Microsoft.EntityFrameworkCore;

namespace DiasComputer.Core.Services
{
    public class CartRepository : ICartRepository
    {
        DiasComputerContext _context;

        public CartRepository(DiasComputerContext context)
        {
            _context = context;
        }

        public List<CartItemsViewModel> GetCartItems(int userId)
        {
            var order = _context.Orders.Include(o => o.OrderDetails)
                .ThenInclude(o => o.Product)
                .SingleOrDefault(o => o.UserId == userId && !o.IsFinally);
            if (order != null)
            {
                return order.OrderDetails
                    .Select(o => new CartItemsViewModel()
                    {
                        ProductPrice = ((o.Product.IsOnSale) ? (int)Compute.ComputeSalePrice(o.Product.ProductPrice, o.Product.SalePercent) : o.Product.ProductPrice),
                        Count = o.Count,
                        ProductId = o.ProductId,
                        ProductName = o.Product.ProductName,
                        ProductImage = o.Product.ProductImg,
                        ProductColor = o.Color,
                        ProductWarranty = o.Warranty,
                        OrderId = o.OrderId,
                        DetailId = o.DetailId
                    }).ToList();
            }

            return null;
        }

        public int GetTotalOrderPriceWithShipping(int userId)
        {
            var order = _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(o => o.Product)
                .SingleOrDefault(o => o.UserId == userId && !o.IsFinally);


            //Checking if the order was exist
            if (order != null)
            {
                //Initializing a variable for total price
                int totalPrice=0;


                foreach (var orderDetail in order.OrderDetails)
                {
                    //if the product was on sale then total price will sum with the off price
                    if (orderDetail.Product.IsOnSale)
                    {
                        totalPrice += ((int)Compute.ComputeSalePrice(orderDetail.Product.ProductPrice, orderDetail.Product.SalePercent) * orderDetail.Count);
                    }
                    //if not , then total price will sum with the product price
                    else
                    {
                        totalPrice += (orderDetail.Product.ProductPrice*orderDetail.Count);
                    }
                }

                //checking if the total price was under the 300000 then summing it with 25000 for post cost
                if (totalPrice < 300000)
                {
                    totalPrice += 25000;
                }

                return totalPrice;
            }
            return 0;
        }

        public PostDetail? GetPostDetails(int userId)
        {
            return _context.PostDetails
                .SingleOrDefault(p => p.UserId == userId);
        }

        public bool InsertOrUpdatePostDetails(PostDetail post)
        {
            try
            {
                if (post.PD_Id == 0)
                {
                    _context.PostDetails.Add(post);
                    _context.SaveChanges();
                    return true;
                }
                else
                {
                    _context.PostDetails.Update(post);
                    _context.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public Tuple<PostDetail, List<CartItemsViewModel>> GetCheckOutDetails(int userId)
        {
            var postDetails = _context.PostDetails.Single(p => p.UserId == userId);

            var order = _context.Orders.Include(o => o.OrderDetails)
                .ThenInclude(o => o.Product)
                .SingleOrDefault(o => o.UserId == userId && !o.IsFinally);

            var cartItems = order.OrderDetails
                .Select(o => new CartItemsViewModel()
                {
                    ProductPrice = o.ProductPrice,
                    Count = o.Count,
                    ProductId = o.ProductId,
                    ProductName = o.Product.ProductName,
                    ProductImage = o.Product.ProductImg,
                    ProductColor = o.Color,
                    ProductWarranty = o.Warranty,
                    OrderId = o.OrderId,
                    DetailId = o.DetailId
                }).ToList();

            return Tuple.Create(postDetails, cartItems);
        }

        public int MakePayment(int userId, int orderId, int amount, string description, bool isApproved = false)
        {
            if (!_context.TransactionHistories.Any(o => o.OrderId == orderId))
            {
                TransactionHistory transactionHistory = new TransactionHistory()
                {
                    UserId = userId,
                    Amount = amount,
                    Date = DateTime.Now,
                    IsApproved = isApproved,
                    Description = description,
                    IsWalletTransaction = false,
                    PaymentMethod = "درگاه پرداخت زرین پال",
                    TypeId = 1,
                    TrackingCode = "0",
                    OrderId = orderId
                };
                _context.TransactionHistories.Add(transactionHistory);
                _context.SaveChanges();
                return transactionHistory.TH_Id;
            }
            else
            {
                return _context.TransactionHistories.Single(t => t.OrderId == orderId).TH_Id;
            }
        }

        public TransactionHistory GetTransactionHistoryForPayment(int transactionHistoryId)
        {
            return _context.TransactionHistories.Find(transactionHistoryId);
        }

        public Order GetLatestOrderByUserId(int userId)
        {
            return _context.Orders.Single(o => o.UserId == userId && !o.IsFinally);
        }

        public void UpdateTransactionAndOrder(TransactionHistory transactionHistory)
        {
            _context.TransactionHistories.Update(transactionHistory);

            var order = _context.Orders.Find(transactionHistory.OrderId);
            order.IsFinally = true;
            order.OrderSum = transactionHistory.Amount;
            order.IsOrderProcessed = false;

            _context.SaveChanges();
        }

        public PaymentConclusionViewModel GetPaymentDetails(int userId)
        {
            return _context.Orders.Where(o => o.UserId == userId && o.IsFinally)
                .OrderByDescending(o => o.OrderId)
                .Include(o => o.User)
                .ThenInclude(o => o.PostDetails)
                .Select(o => new PaymentConclusionViewModel()
                {
                    PhoneNumber = o.User.PostDetails.Single().PhoneNumber,
                    Address = o.User.PostDetails.Single().Address,
                    IsApproved = o.IsFinally,
                    OrderId = o.OrderId,
                    TotalPrice = o.OrderSum,
                    Username = o.User.PostDetails.Single().UserName
                }).First();
        }

        public bool RemoveFromCart(int detailId, int orderId)
        {
            try
            {
                var order = _context.Orders.Single(o => o.OrderId == orderId);
                var orderDetail = _context.OrderDetails.Single(o => o.OrderId == orderId && o.DetailId == detailId);
                if (orderDetail.Count > 1)
                {
                    orderDetail.Count -= 1;
                }
                else
                {
                    _context.OrderDetails.Remove(orderDetail);
                }

                order.OrderSum -= orderDetail.ProductPrice;
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool AddToCart(int productId, string color, int userId)
        {
            try
            {
                //First checking if the user have an open order !
                if (IsOrderExist(userId))
                {
                    //Getting the order
                    var order = _context.Orders.Single(o => o.UserId == userId && !o.IsFinally);

                    //Checking if the order has the order detail with product id of product !
                    if (_context.OrderDetails.Any(o => o.ProductId == productId && o.OrderId == order.OrderId && o.Color == color))
                    {
                        //Getting the order detail and increasing the count of it and also increasing the order sum price
                        var orderDetail = _context.OrderDetails
                            .Single(o => o.ProductId == productId && o.OrderId == order.OrderId && o.Color == color);

                        orderDetail.Count += 1;
                        orderDetail.ProductPriceSum += orderDetail.ProductPrice;
                        order.OrderSum += orderDetail.ProductPrice;

                        _context.SaveChanges();
                    }

                    //If the order does not have an order detail for that product
                    else
                    {
                        //Getting product details
                        var productDetails = GetProductDetails(productId);

                        if (productDetails != null)
                        {
                            //Creating a new order detail
                            var orderDetail = new OrderDetail()
                            {
                                Count = 1,
                                Color = color,
                                OrderId = order.OrderId,
                                ProductId = productId,
                                ProductPrice = ((productDetails.IsOnSale) ? (int)Compute.ComputeSalePrice(productDetails.ProductPrice, productDetails.SalePercent) : productDetails.ProductPrice),
                                ProductPriceSum = ((productDetails.IsOnSale) ? (int)Compute.ComputeSalePrice(productDetails.ProductPrice, productDetails.SalePercent) : productDetails.ProductPrice),
                                Warranty = productDetails.ProductWarranty
                            };

                            //Adding created order detail and updating order total price
                            _context.OrderDetails.Add(orderDetail);
                            order.OrderSum += orderDetail.ProductPrice;

                            _context.SaveChanges();
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                //If the user does not have an open order
                else
                {
                    //Creating a new open order and adding it
                    var newOrder = new Order()
                    {
                        IsFinally = false,
                        OrderDate = DateTime.Now,
                        OrderSum = 0,
                        ReferSubmitted = false,
                        UserId = userId
                    };

                    _context.Orders.Add(newOrder);
                    _context.SaveChanges();

                    //Getting product info for adding it
                    var productDetails = GetProductDetails(productId);

                    if (productDetails != null)
                    {
                        //Creating a new order detail and adding it and updating order total price
                        var orderDetail = new OrderDetail()
                        {
                            Count = 1,
                            Color = color,
                            OrderId = newOrder.OrderId,
                            ProductId = productId,
                            ProductPrice = ((productDetails.IsOnSale) ? (int)Compute.ComputeSalePrice(productDetails.ProductPrice, productDetails.SalePercent) : productDetails.ProductPrice),
                            ProductPriceSum = ((productDetails.IsOnSale) ? (int)Compute.ComputeSalePrice(productDetails.ProductPrice, productDetails.SalePercent) : productDetails.ProductPrice),
                            Warranty = productDetails.ProductWarranty
                        };

                        _context.OrderDetails.Add(orderDetail);
                        newOrder.OrderSum += orderDetail.ProductPrice;

                        _context.SaveChanges();
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public ProductDetailsForAddToCartViewModel GetProductDetails(int productId)
        {
            return _context.Products
                .Where(p => p.ProductId == productId)
                .Select(p => new ProductDetailsForAddToCartViewModel()
                {
                    ProductPrice = p.ProductPrice,
                    ProductWarranty = p.WarrantyName,
                    SalePercent = p.SalePercent,
                    IsOnSale = p.IsOnSale
                }).SingleOrDefault();
        }

        public bool IsOrderExist(int userId)
        {
            if (_context.Orders.Any(o =>
                    o.UserId == userId && !o.IsFinally))
            {
                return true;
            }

            return false;
        }

        public void UpdateTransactionHistoryForWalletPayment(int thId)
        {
            _context.TransactionHistories.Find(thId).IsWalletTransaction = true;
            _context.SaveChanges();
        }

        public bool CheckWalletBalance(int userId, int totalPrice)
        {
            var wallet = _context.Wallets.SingleOrDefault(w => w.UserId == userId);
            if (wallet == null)
                return false;

            if (wallet.Balance >= totalPrice)
                return true;

            return false;

        }

        public bool PayWithWallet(int userId, int totalPrice)
        {
            if (CheckWalletBalance(userId, totalPrice))
            {
                var userWallet = _context.Wallets.Single(w => w.UserId == userId);
                userWallet.Balance -= totalPrice;

                return true;
            }
            else
            {
                return false;
            }
        }

        public int CheckAndGetCouponValue(string coupon)
        {
            var getCoupon = _context.Coupons.SingleOrDefault(c => c.CouponCode == coupon);

            if (getCoupon != null
                && getCoupon.IsActive
                && getCoupon.ActiveFrom <= DateTime.Now
                && getCoupon.ActiveTill >= DateTime.Now)
            {
                return _context.Coupons.Single(c => c.CouponCode == coupon).CouponPercent;
            }
            else
            {
                return 0;
            }
        }
    }
}
