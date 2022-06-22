using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiasComputer.DataLayer.Entities.Users;

namespace DiasComputer.DataLayer.Entities.Orders
{
    public class TransactionHistory
    {
        [Key]
        public int TH_Id { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        public int TypeId { get; set; }
        [Required]
        public int UserId { get; set; }
        [MaxLength(1000)]
        public string? Description { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [MaxLength(200)]
        public string? PaymentMethod { get; set; }
        public int? OrderId { get; set; }
        [Required]
        public string TrackingCode { get; set; }
        public bool IsApproved { get; set; }
        public bool IsWalletTransaction { get; set; }

        #region Relation's

        public Order Order { get; set; }
        public User User { get; set; }

        [ForeignKey("TH_Id")]
        public List<TransactionToType> TransactionToType { get; set; }

        #endregion

    }
}
