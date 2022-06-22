using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiasComputer.DataLayer.Entities.Products;

namespace DiasComputer.DataLayer.Entities.Groups
{
    public class Group
    {
        [Key]
        public int GroupId { get; set; }
        [Display(Name = "عنوان دسته بندی")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string GroupTitle { get; set; }
        public int? ParentId { get; set; }
        public bool IsDelete { get; set; }

        #region Relation's

        [ForeignKey("ParentId")]
        public Group Groups { get; set; }

        [InverseProperty("Group")]
        public List<Product> Products { get; set; }
        [InverseProperty("SubGroup")]
        public List<Product> SubProducts { get; set; }
        #endregion
    }
}