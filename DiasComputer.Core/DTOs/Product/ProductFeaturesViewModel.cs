using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiasComputer.Core.DTOs.Product
{
    public class ProductFeaturesViewModel
    {

        public int ProductId { get; set; }
        public int FeatureId { get; set; }
        [Display(Name = "مجموعه ویژگی ها")]
        public int? ParentId { get; set; }
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [Display(Name = "عنوان ویژگی")]
        public string FeatureTitle { get; set; } = string.Empty;
        [MaxLength(1200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد")]
        [Display(Name = "توضیح ویژگی")]
        public string? FeatureDescription { get; set; }
        [Display(Name = "جزو ویژگی های خلاصه باشد")]
        public bool IsShortFeature { get; set; }
    }
}
