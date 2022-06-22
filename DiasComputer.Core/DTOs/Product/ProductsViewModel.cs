using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiasComputer.Core.DTOs.Product
{
    public class AmazingProductsSliderViewModel
    {
        public int ProductId { get; set; }
        public string ProductTitle { get; set; } = string.Empty;
        public int ProductPrice { get; set; }
        public int? ProductSalePercent { get; set; }
        public string ProductImage { get; set; } = string.Empty;
        public int? SaleCount { get; set; }
    }
    public class LatestProductsSliderViewModel
    {
        public int ProductId { get; set; }
        public string ProductTitle { get; set; } = string.Empty;
        public int ProductPrice { get; set; }
        public int? ProductSalePercent { get; set; }
        public string ProductImage { get; set; } = string.Empty;
        public bool IsOnSale { get; set; }
    }
    public class OrderDetailItemsForReferViewModel
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string ReferCode { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public int OrderSum { get; set; }
    }
    public class SingleProductViewModel
    {
        public int ProductId { get; set; }
        public string ProductImage { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string ProductEngName { get; set; } = string.Empty;
        public int ProductPrice { get; set; }
        public bool AvailableStatus { get; set; }
        public string? Brand { get; set; }
        public string Color { get; set; } = string.Empty;
        public string? Warranty { get; set; }
        public string ShortDescription { get; set; } = string.Empty;
        public string? FullDescription { get; set; }
        public bool IsOnSale { get; set; }
        public int? SalePercent { get; set; }
        public int GroupId { get; set; }
        public int? SubGroupId { get; set; }
    }
    public class SingleProductInListViewModel
    {
        public int ProductId { get; set; }
        public string ProductImage { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public int ProductPrice { get; set; }
        public bool AvailableStatus { get; set; }
        public bool IsOnSale { get; set; }
        public int? SalePercent { get; set; }
        public int TotalProductsCount { get; set; }
    }

    public class AddCommentViewModel
    {
        public int ProductId  { get; set; }
        public int? ParentId  { get; set; }
        public string CommentText  { get; set; } = string.Empty;
    }

    #region Features

    public class ShortFeaturesViewModel
    {
        public string FeatureTitle { get; set; } = string.Empty;
        public string FeatureDescription { get; set; } = string.Empty;
    }

    #endregion
}
