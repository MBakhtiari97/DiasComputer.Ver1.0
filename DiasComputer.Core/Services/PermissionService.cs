using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiasComputer.Core.Services.Interfaces;
using DiasComputer.DataLayer.Context;
using DiasComputer.DataLayer.Entities.Permissions;
using DiasComputer.DataLayer.Entities.Users;

namespace DiasComputer.Core.Services
{
    public class PermissionService : IPermissionService
    {
        DiasComputerContext _context;

        public PermissionService(DiasComputerContext context)
        {
            _context = context;
        }

        public bool CheckPermission(int permissionId, string emailAddress)
        {
            var userId = _context.Users
                .Single(u => u.EmailAddress == emailAddress)
                .UserId;

            List<int> userRoles = _context.UserRoles
                .Where(r => r.UserId == userId)
                .Select(r => r.RoleId)
                .ToList();

            if (!userRoles.Any())
                return false;

            List<int> rolesPermission = _context.RolePermissions
                .Where(p => p.PermissionId == permissionId)
                .Select(p => p.RoleId)
                .ToList();
            return rolesPermission.Any(p => userRoles.Contains(p));
        }

        public List<Permission> GetAllPermissions()
        {
            return _context.Permissions.ToList();
        }

        public void AddPermission(int roleId, List<int> permissions)
        {
            foreach (var permission in permissions)
            {
                _context.RolePermissions.Add(new RolePermission()
                {
                    RoleId = roleId,
                    PermissionId = permission
                });
                _context.SaveChanges();
            }
        }

        public List<int> GetSelectedPermissions(int roleId)
        {
            return _context.RolePermissions
                .Where(r => r.RoleId == roleId)
                .Select(r=>r.PermissionId)
                .ToList();
        }

        public void UpdatePermissionRole(int roleId, List<int> permissions)
        {
            foreach (var rolePermission in _context.RolePermissions.Where(r=>r.RoleId==roleId).ToList())
            {
                _context.RolePermissions.Remove(rolePermission);
            }

            foreach (var permission in permissions)
            {
                _context.RolePermissions.Add(new RolePermission()
                {
                    RoleId = roleId,
                    PermissionId = permission
                });
            }

            _context.SaveChanges();
        }

        public List<Role> GetRoles()
        {
            return _context.Roles.ToList();
        }

        public Role GetRoleDetails(int roleId)
        {
            return _context.Roles.Find(roleId);
        }

        public bool AddNewRole(Role role)
        {
            try
            {
                _context.Roles.Add(role);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateRole(Role role)
        {
            try
            {
                _context.Roles.Update(role);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteRole(int roleId)
        {
            try
            {
                var role = GetRoleDetails(roleId);
                role.IsDelete = true;
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
