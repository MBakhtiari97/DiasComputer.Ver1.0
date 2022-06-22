using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using DiasComputer.DataLayer.Entities.Groups;
using DiasComputer.DataLayer.Entities.Orders;
using DiasComputer.DataLayer.Entities.Users;

namespace DiasComputer.DataLayer.Entities.Products
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        public int GroupId { get; set; }
        public int? SubGroupId { get; set; }
        [Required]
        [MaxLength(200)]
        public string ProductName { get; set; }
        [Required]
        [MaxLength(200)]
        public string ProductEngName { get; set; }
        [Required]
        [MaxLength(200)]
        public string? ProductImg { get; set; }
        [Required]
        [MaxLength(1000)]
        public string ProductColors { get; set; }
        [MaxLength(200)]
        public string? ProductBrand { get; set; }
        [Required]
        public int ProductPrice { get; set; }
        public bool AvailableStatus { get; set; }
        public int? BrandId { get; set; }
        [Required]
        public int AvailableCount { get; set; }
        [MaxLength(300)]
        public string? WarrantyName { get; set; }
        [Required]
        [MaxLength(300)]
        [MinLength(200)]
        public string ShortDescription { get; set; }

        [DataType(DataType.MultilineText)]
        public string? FullDescription { get; set; }
        [MaxLength(1200)]
        public string? Tags { get; set; }
        [Required]
        public DateTime AddedDate { get; set; }
        public bool IsOnSale { get; set; }
        public int? SalePercent { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }

        #region Relation's

        public List<WishList> WishLists { get; set; }
        public List<Feature> Features { get; set; }
        public List<Gallery> Galleries { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        public List<Review> Reviews { get; set; }
        [ForeignKey("GroupId")]
        public Group Group { get; set; }
        [ForeignKey("SubGroupId")]
        public Group SubGroup { get; set; }

        #endregion


    }
}
