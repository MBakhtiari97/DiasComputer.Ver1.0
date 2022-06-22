using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using DiasComputer.DataLayer.Entities.Users;

namespace DiasComputer.DataLayer.Entities.Orders
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public int OrderSum { get; set; }
        [Required]
        public bool IsFinally { get; set; }
        public bool ReferSubmitted { get; set; }
        public bool? IsCancelled { get; set; }
        public bool IsOrderProcessed { get; set; }
        public bool IsDelete { get; set; }

        #region Relaion's

        public User User { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        public ReferOrder ReferOrder { get; set; }
        public TransactionHistory Transaction { get; set; }

        #endregion
    }
}
