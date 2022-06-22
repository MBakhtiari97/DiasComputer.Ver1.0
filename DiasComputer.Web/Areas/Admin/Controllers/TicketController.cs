using AspNetCoreHero.ToastNotification.Abstractions;
using DiasComputer.Core.OperationResults;
using DiasComputer.Core.Security;
using DiasComputer.Core.Services.Interfaces;
using DiasComputer.DataLayer.Entities.SiteResources;
using DiasComputer.Utility.Methods;
using Microsoft.AspNetCore.Mvc;

namespace DiasComputer.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [PermissionChecker(1025)]
    public class TicketController : Controller
    {
        private ISiteRepository _siteRepository;
        private INotyfService _notyfService;

        public TicketController(ISiteRepository siteRepository, INotyfService notyfService)
        {
            _siteRepository = siteRepository;
            _notyfService = notyfService;
        }

        #region Index

        /// <summary>
        /// Method will show tickets index
        /// </summary>
        public IActionResult Index(bool isRead, bool isAnswered, int pageId = 1)
        {
            ViewBag.PageId = pageId;

            return View(_siteRepository.GetAllTickets(isRead, isAnswered, pageId));
        }

        #endregion

        #region Update

        /// <summary>
        /// Method will update ticket status
        /// </summary>
        public IActionResult UpdateTicketStatus(int ticketId)
        {
            return View(_siteRepository.GetTicketByTicketId(ticketId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateTicketStatus(Ticket ticket)
        {
            if (_siteRepository.UpdateTicket(ticket))
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
        /// Method will delete ticket
        /// </summary>
        public IActionResult DeleteTicket(int ticketId)
        {
            if (_siteRepository.DeleteTicket(ticketId))
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
