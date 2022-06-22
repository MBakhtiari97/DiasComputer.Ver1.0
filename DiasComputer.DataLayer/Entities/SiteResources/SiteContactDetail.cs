using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiasComputer.DataLayer.Entities.SiteResources
{
    public class SiteContactDetail
    {
        [Key]
        public int SCD_Id { get; set; }
        [Display(Name = "عنوان اطلاعات تماس با ما")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string SCD_TypeTitle { get; set; }
        [Display(Name = "اطلاعات تماس با ما")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string SCD_Value { get; set; }
    }
}
