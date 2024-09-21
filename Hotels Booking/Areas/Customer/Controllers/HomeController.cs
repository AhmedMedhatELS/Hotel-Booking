using DataAccess.Repository.IRepository;
using Hotels_Booking.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.ViewModel;
using System.Diagnostics;

namespace Hotels_Booking.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController(ILogger<HomeController> logger,
        UserManager<ApplicationUser> userManager,
        IUnitOfWork unitOfWork) : Controller
    {
        #region Start Up
        private readonly ILogger<HomeController> _logger = logger;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        #endregion
        #region Home
        public IActionResult Index()
        {
            return View();
        }
        #endregion
        #region Contact
        [Authorize(Roles = "User")]
        public IActionResult Contact() => View(new ContactView { States = _unitOfWork.States.Get(null, c => c.Country).ToList() });

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateContact(Contact Contact)
        {
            if (ModelState.IsValid)
            {
                Contact.UserId = _userManager.GetUserId(User) ?? "";
                _unitOfWork.Contacts.Add(Contact);
                TempData["ConfirmContact"] = "Thank you! Your message has been submitted. We'll respond via email shortly.";
            }
            return RedirectToAction("Contact");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateHotelRequest(HotelRequest HotelRequest)
        {
            if (ModelState.IsValid)
            {
                HotelRequest.UserId = _userManager.GetUserId(User) ?? "";
                _unitOfWork.HotelRequests.Add(HotelRequest);
                TempData["ConfirmContact"] = "Thank you! Your Request has been submitted. We'll respond via email shortly.";
            }
            return RedirectToAction("Contact");
        }
        #endregion
        #region #
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion
    }
}
