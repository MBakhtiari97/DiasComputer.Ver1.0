using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiasComputer.DataLayer.Entities.Products;

namespace DiasComputer.DataLayer.Entities.Users
{
    public class WishList
    {
        [Key]
        public int WishListId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int ProductId { get; set; }

        #region Relation's

        public User User { get; set; }
        public Product Product { get; set; }

        #endregion
    }
}
