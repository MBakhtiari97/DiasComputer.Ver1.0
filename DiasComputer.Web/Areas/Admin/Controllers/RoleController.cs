using AspNetCoreHero.ToastNotification.Abstractions;
using DiasComputer.Core.OperationResults;
using DiasComputer.Core.Security;
using DiasComputer.Core.Services.Interfaces;
using DiasComputer.DataLayer.Entities.Users;
using DiasComputer.Utility.Methods;
using Microsoft.AspNetCore.Mvc;

namespace DiasComputer.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [PermissionChecker(5)]
    public class RoleController : Controller
    {
        IPermissionService _permissionService;
        private INotyfService _notyfService;

        public RoleController(IPermissionService permissionService, INotyfService notyfService)
        {
            _permissionService = permissionService;
            _notyfService = notyfService;
        }

        #region Index

        /// <summary>
        /// Method will show roles index
        /// </summary>
        public IActionResult Index()
        {
            return View(_permissionService.GetRoles());
        }

        #endregion

        #region Create

        /// <summary>
        /// Method will create role
        /// </summary>
        public IActionResult CreateRole()
        {
            ViewBag.Permissions = _permissionService.GetAllPermissions();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateRole(Role role, List<int> selectedPermission)
        {
            //Validating Model
            if (!ModelState.IsValid)
                return View(role);


            //Create Role
            if (_permissionService.AddNewRole(role))
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));
            }
            else
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
            }

            _permissionService.AddPermission(role.RoleId, selectedPermission);

            return RedirectToAction("Index");
        }

        #endregion

        #region Update

        /// <summary>
        /// Method will update role
        /// </summary>
        public IActionResult UpdateRole(int roleId)
        {
            ViewBag.Permissions = _permissionService.GetAllPermissions();
            ViewBag.SelectedPermissions = _permissionService.GetSelectedPermissions(roleId);

            return View(_permissionService.GetRoleDetails(roleId));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateRole(Role role, List<int> selectedPermission)
        {
            //Validating Model
            if (!ModelState.IsValid)
                return View(role);

            //Updating Role
            if (_permissionService.UpdateRole(role))
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));
            }
            else
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
            }

            _permissionService.UpdatePermissionRole(role.RoleId, selectedPermission);

            return RedirectToAction("Index");
        }

        #endregion

        #region Delete

        /// <summary>
        /// Method will delete role
        /// </summary>
        public IActionResult DeleteRole(int roleId)
        {
            if (_permissionService.DeleteRole(roleId))
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));
            }
            else
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
            }
            return RedirectToAction("Index");
        }

        #endregion
    }
}
