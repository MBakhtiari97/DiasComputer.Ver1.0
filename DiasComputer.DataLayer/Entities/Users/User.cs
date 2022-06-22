using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiasComputer.DataLayer.Entities.Orders;
using DiasComputer.DataLayer.Entities.Products;

namespace DiasComputer.DataLayer.Entities.Users
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [MaxLength(200)]
        public string? UserName { get; set; }
        [Required]
        [MaxLength(200)]
        public string EmailAddress { get; set; }
        [Required]
        [MaxLength(200)]
        public string Password { get; set; }
        [MaxLength(50)]
        public string? PhoneNumber { get; set; }
        [MaxLength(50)]
        public string? UserAvatar { get; set; }
        [MaxLength(200)]
        public string? BankAccountNumber { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public DateTime RegisterDate { get; set; }
        [Required]
        public bool IsDelete { get; set; }
        [Required]
        [MaxLength(200)]
        public string UniqueCode { get; set; }
        [MaxLength(50)]
        public string? NationalCode { get; set; }

        #region Relation's

        public List<UserRole> UserRole { get; set; }
        public Wallet Wallet { get; set; }
        public List<WishList> WishLists { get; set; }
        public List<Order> Order { get; set; }
        public List<TransactionHistory> Transaction { get; set; }
        public List<Review> Reviews { get; set; }
        public List<PostDetail> PostDetails { get; set; }

        #endregion

    }
}
