using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiasComputer.DataLayer.Entities.Products
{
    public class Coupon
    {
        [Key]
        public int CouponId { get; set; }
        [Required]
        [MaxLength(50)]
        public string CouponCode { get; set; }
        public bool IsActive { get; set; }
        [Required]
        public int CouponPercent { get; set; }
        public DateTime? ActiveFrom { get; set; }
        public DateTime? ActiveTill { get; set; }
        public bool IsDelete { get; set; }
        [Required]
        public int CouponsCount { get; set; }

    }
}
