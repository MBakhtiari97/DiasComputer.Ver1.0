using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiasComputer.DataLayer.Entities.Permissions;

namespace DiasComputer.DataLayer.Entities.Users
{
    public class Role
    {
        [Key]
        [Display(Name = "کد نقش")]
        public int RoleId { get; set; }
        [Display(Name = "عنوان نقش")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند بیشتر از {1} باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string RoleName { get; set; }
        [Required]
        public bool IsDelete { get; set; }

        #region Relation's

        public List<UserRole> UserRoles { get; set; }
        public List<RolePermission> RolePermissions { get; set; }

        #endregion

    }
}
