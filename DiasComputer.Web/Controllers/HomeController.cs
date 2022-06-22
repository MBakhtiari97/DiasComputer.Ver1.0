using DiasComputer.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using AspNetCoreHero.ToastNotification.Abstractions;
using DiasComputer.Core.DTOs.Product;
using DiasComputer.Core.OperationResults;
using DiasComputer.Core.Services.Interfaces;
using DiasComputer.DataLayer.Entities.Products;
using DiasComputer.DataLayer.Entities.SiteResources;
using DiasComputer.Utility.Methods;
using Microsoft.AspNetCore.HttpOverrides;

namespace DiasComputer.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        ISiteRepository _siteRepository;
        private INotyfService _notyfService;
        IProductRepository _productRepository;
        private IGoogleRecaptcha _recaptcha;

        public HomeController(ILogger<HomeController> logger, ISiteRepository siteRepository, INotyfService notyfService, IProductRepository productRepository, IGoogleRecaptcha recaptcha)
        {
            _logger = logger;
            _siteRepository = siteRepository;
            _notyfService = notyfService;
            _productRepository = productRepository;
            _recaptcha = recaptcha;
        }

        #region Index

        /// <summary>
        /// This method will returns home index of application
        /// </summary>
        public IActionResult Index()
        {
            return View();
        }

        #endregion

        #region NotFoundAndServerError
        /// <summary>
        /// This method will return's the not found page (404) 
        /// </summary>
        [Route("/NotFound")]
        public IActionResult PageNotFound()
        {
            return View();
        }

        [Route("/ServerError")]
        public IActionResult PageServerError()
        {
            return View();
        }

        #endregion

        #region ContactUs

        /// <summary>
        /// This method will Submit a contact us form
        /// </summary>
        [Route("ContactUs")]
        public IActionResult ContactUs()
        {
            ViewBag.ContactInfo = _siteRepository.GetShopContactDetails();
            return View();
        }

        [Route("ContactUs")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactUs(Ticket ticket)
        {
            if (!await _recaptcha.IsUserValid())
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.RecaptchaFailed.ToString()));
                return View(ticket);
            }

            if (!ModelState.IsValid)
                return View(ticket);

            ticket.TicketSenderIP = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            if (_siteRepository.AddTicket(ticket))
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.Success.ToString()));
            }
            else
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
            }
            return Redirect("/");
        }

        #endregion

        #region AboutUs

        /// <summary>
        /// This method will show the about us page
        /// </summary>
        [Route("AboutUs")]
        public IActionResult AboutUs()
        {
            return View();
        }

        #endregion

        #region PrivacyPolicy

        /// <summary>
        /// This method will show the privacy policy page
        /// </summary>
        [Route("Privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        #endregion

        #region Refer

        /// <summary>
        /// This method will show the refer instruction page
        /// </summary>
        [Route("Refer")]
        public IActionResult Refer()
        {
            return View();
        }

        #endregion

        #region Newsletter

        /// <summary>
        /// This method will submit a new newsletter user
        /// </summary>
        public IActionResult JoinToNewsletter(string email)
        {
            //Subscribe and unsubscribe will be done with this method
            if (_siteRepository.JoinToNewsletter(FixedText.FixEmail(email)))
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.SuccessNewsletter.ToString()));
            }
            else
            {
                _notyfService.Warning(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
            }
            return Redirect("/");
        }

        #endregion

        #region Comment

        /// <summary>
        /// This method will submit a new comment
        /// </summary>
        public IActionResult AddComment(AddCommentViewModel comment)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());

            Review newComment = new Review()
            {
                ParentId = comment.ParentId,
                ProductId = comment.ProductId,
                ReviewDate = DateTime.Now,
                ReviewText = comment.CommentText,
                UserId = userId
            };

            if (_productRepository.AddComment(newComment))
            {
                _notyfService.Success(OperationResultText.ShowResult(OperationResult.Result.SuccessComment.ToString()));
            }
            else
            {
                _notyfService.Error(OperationResultText.ShowResult(OperationResult.Result.Failure.ToString()));
            }

            return Redirect($"/Products/{@comment.ProductId}");
        }

        #endregion

        #region Error

        /// <summary>
        /// This method will config the default error page
        /// </summary>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #endregion
    }
}