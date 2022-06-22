using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiasComputer.DataLayer.Entities.Users;

namespace DiasComputer.DataLayer.Entities.Products
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }
        [Required]
        [MaxLength(1200)]
        public string ReviewText { get; set; }
        [Required]
        public DateTime ReviewDate { get; set; }
        [Required]
        public bool IsConfirmed { get; set; }
        public int? ParentId { get; set; }
        public bool IsReported { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public bool IsDelete { get; set; }
        public bool? IsAnswered { get; set; }

        #region Relation's

        [ForeignKey("ParentId")]
        public Review Reviews { get; set; }
        public User User { get; set; }
        public Product Product { get; set; }

        #endregion
    }
}
