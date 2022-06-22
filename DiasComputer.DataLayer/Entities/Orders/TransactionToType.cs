using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiasComputer.DataLayer.Entities.Orders
{
    public class TransactionToType
    {
        [Key]
        public int TT_Id { get; set; }
        [Required]
        public int TH_Id { get; set; }
        [Required]
        public int TypeId { get; set; }

        #region Relation's

        public TransactionHistory TransactionHistory { get; set; }
        public TransactionType TransactionType { get; set; }

        #endregion
    }
}
