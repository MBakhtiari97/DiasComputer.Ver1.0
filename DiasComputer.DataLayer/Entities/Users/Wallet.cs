using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace DiasComputer.DataLayer.Entities.Users
{
    public class Wallet
    {
        [Key]
        public int WalletId { get; set; }
        [Required]
        [Display(Name = "موجودی")]
        public int Balance { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        [Display(Name = "وضعیت فعالسازی")]
        public bool IsActive { get; set; }

        #region Relation's

        public User User { get; set; }

        #endregion

    }
}
