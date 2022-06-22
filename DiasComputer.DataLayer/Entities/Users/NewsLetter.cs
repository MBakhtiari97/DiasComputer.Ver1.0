using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiasComputer.DataLayer.Entities.Users
{
    public class NewsLetter
    {
        [Key]
        public int NewsletterId { get; set; }
        [Required]
        [MaxLength(250)]
        public string EmailAddress { get; set; }
        public bool IsCanceled { get; set; }
    }
}
