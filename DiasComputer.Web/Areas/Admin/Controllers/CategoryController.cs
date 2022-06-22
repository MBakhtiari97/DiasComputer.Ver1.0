using AspNetCoreHero.ToastNotification.Abstractions;
using DiasComputer.Core.OperationResults;
using DiasComputer.Core.Security;
using DiasComputer.Core.Services.Interfaces;
using DiasComputer.DataLayer.Entities.Groups;
using DiasComputer.Utility.Methods;
using Microsoft.AspNetCore.Mvc;

namespace DiasComputer.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [PermissionChecker(10)]
    public class CategoryController : Controller
    {
        private ICategoryRepository _categoryRepository;
        private INotyfService _notyfService;

        public CategoryController(ICategoryRepository categoryRepository, INotyfService notyfService)
        {
            _categoryRepository = categoryRepository;
            _notyfService = notyfService;
        }

        #region Index

        /// <summary>
        /// Method will show category index
        /// </summary>
        public IActionResult Index()
        {
            return View(_categoryRepository.GetAllCategories());
        }

        #endregion

        #region Create

        /// <summary>
        /// Method will create new category
        /// </summary>
        public ActionResult AddCategory(int? groupId)
        {
            if (groupId != null)
            {
                ViewBag.GroupId = groupId;
            }

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCategory(Group group)
        {
            if (!ModelState.IsValid)
                return View(group);

            if (_categoryRepository.AddNewCategory(group))
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

        #region Update

        /// <summary>
        /// Method will update category
        /// </summary>
        public ActionResult UpdateCategory(int groupId)
        {
            return View(_categoryRepository.GetGroupById(groupId));
        }
        [HttpPost]
        public ActionResult UpdateCategory(Group group)
        {
            if (!ModelState.IsValid)
                return View(group);

            if (_categoryRepository.UpdateCategory(group))
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

        #region Delete

        /// <summary>
        /// Method will remove category
        /// </summary>
        public ActionResult DeleteCategory(int groupId)
        {
            if (_categoryRepository.DeleteCategory(groupId))
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
