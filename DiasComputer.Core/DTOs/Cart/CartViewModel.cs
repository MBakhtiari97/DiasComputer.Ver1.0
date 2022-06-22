using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiasComputer.Core.DTOs.Cart
{
    public class CartItemsViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductImage { get; set; } = string.Empty;
        public string ProductColor { get; set; } = string.Empty;
        public string ProductWarranty { get; set; } = string.Empty;
        public int Count { get; set; }
        public int ProductPrice { get; set; }
        public int OrderId { get; set; }
        public int DetailId { get; set; }

    }

    public class PaymentConclusionViewModel
    {
        public int OrderId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public bool IsApproved { get; set; }
        public int TotalPrice { get; set; }
    }

    public class ProductDetailsForAddToCartViewModel
    {
        public int ProductPrice { get; set; }
        public string? ProductWarranty { get; set; }
        public bool IsOnSale { get; set; }
        public int? SalePercent { get; set; }

    }
}
