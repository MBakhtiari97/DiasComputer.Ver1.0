using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiasComputer.DataLayer.Entities.SiteResources
{
    public class EmailHistory
    {
        [Key]
        public int EmailId { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [Display(Name = "مخاطبان")]
        public int EmailTypeId { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [Display(Name = "موضوع ایمیل")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [Display(Name = "متن ایمیل")]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }
        public bool IsDelete { get; set; }

        #region Relation

        public EmailType EmailType { get; set; }

        #endregion
    }
}
