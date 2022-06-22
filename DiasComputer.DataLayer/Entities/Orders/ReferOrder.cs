using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiasComputer.DataLayer.Entities.Users;

namespace DiasComputer.DataLayer.Entities.Orders
{
    public class ReferOrder
    {
        [Key]
        public int ReferId { get; set; }
        [Required]
        public int OrderId { get; set; }
        [Required]
        public DateTime ReferSubmitDate { get; set; }
        [Required]
        [MaxLength(50)]
        public string ReferCode { get; set; }
        public bool ReferStatus { get; set; }

        #region Relations

        public Order Order { get; set; }

        #endregion
    }
}
