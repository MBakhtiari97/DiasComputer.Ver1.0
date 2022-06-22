using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiasComputer.DataLayer.Entities.Orders
{
    public class TransactionType
    {
        [Key]
        public int TypeId { get; set; }
        [Required]
        public string TypeTitle { get; set; }

        #region Relation's

        [ForeignKey("TypeId")]
        public List<TransactionToType> TransactionToTypes { get; set; }

        #endregion
    }
}
