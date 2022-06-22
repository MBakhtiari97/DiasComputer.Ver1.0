using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiasComputer.DataLayer.Entities.Permissions;
using DiasComputer.DataLayer.Entities.Users;

namespace DiasComputer.Core.Services.Interfaces
{
    public interface IPermissionService
    {
        #region Roles

       
        List<Role> GetRoles();
        Role GetRoleDetails(int roleId);
        bool AddNewRole(Role role);
        bool UpdateRole(Role role);
        bool DeleteRole(int roleId);


        #endregion

        #region Permission

        bool CheckPermission(int permissionId, string emailAddress);
        List<Permission> GetAllPermissions();
        void AddPermission(int roleId,List<int> permissions);
        List<int> GetSelectedPermissions(int roleId);
        void UpdatePermissionRole(int roleId,List<int> permissions);

        #endregion
    }
}
