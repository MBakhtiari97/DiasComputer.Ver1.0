using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiasComputer.DataLayer.Entities.Products;

namespace DiasComputer.DataLayer.Entities.Orders
{
    public class OrderDetail
    {
        [Key]
        public int DetailId { get; set; }
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int ProductPrice { get; set; }
        [Required]
        public int ProductPriceSum { get; set; }
        [Required]
        public int Count { get; set; }
        public string? Color { get; set; }
        public string? Warranty { get; set; }

        #region Relation's

        public Order Order { get; set; }
        public Product Product { get; set; }

        #endregion
    }
}
