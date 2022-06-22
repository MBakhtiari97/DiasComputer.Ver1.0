using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiasComputer.DataLayer.Entities.SiteResources
{
    public class EmailType
    {
        [Key]
        public int EmailTypeId { get; set; }
        [Required]
        [MaxLength(200)]
        public string TypeTitle { get; set; }

        #region Relation

        [ForeignKey("EmailTypeId")]
        public List<EmailHistory> Email { get; set; }

        #endregion

    }
}
