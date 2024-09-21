using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Models;
using Models.ViewModel;
using System.Text.Encodings.Web;

namespace Hotels_Booking.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IEmailSender emailSender) : Controller
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IEmailSender _emailSender = emailSender;
        public IActionResult Index()
        {
            return View();
        }

        #region User Contacts
        public IActionResult NewContacts() =>
            View(new ContactReply
            {
                Contacts = _unitOfWork.Contacts.Get(c => c.Status == Utility.Status.Pending,
                a => a.ApplicationUser).ToList()
            });
        public IActionResult DeleteContact(int ContactId, string ViewNavigation)
        {
            var contact = _unitOfWork.Contacts.GetOne(id => id.ContactId == ContactId);
            if (contact != null)
                _unitOfWork.Contacts.Remove(contact);
            if (Utility.ViewNavigation.NewContacts.ToString() == ViewNavigation)
                return RedirectToAction("NewContacts");
            else if (Utility.ViewNavigation.ContactHistory.ToString() == ViewNavigation)
                return RedirectToAction("ContactsHistory");
            else
                return NotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReplyToContact(ContactReply contactReply)
        {
            if (ModelState.IsValid)
            {
                var contact = _unitOfWork.Contacts.GetOne(id => id.ContactId == contactReply.contactId, u => u.ApplicationUser);
                if (contact != null)
                {
                    await _emailSender.SendEmailAsync(
                        contact.ApplicationUser.Email ?? "",
                        "Response to Your Hotel Booking Inquiry",
                        $@"
                        <p>Dear {contact.Name},</p>
                        
                        <p>Thank you for reaching out to us. We appreciate your patience.</p>
                        
                        <p>Here is the response to your query:</p>
                        
                        <p>{contactReply.Reply}</p>
                        
                        <p>If you have any further questions or need additional assistance, feel free to contact us.</p>
                        
                        <p>Best regards,<br>
                        Hotel Booking Team</p>
                        "
                        );
                    contact.Reply = contactReply.Reply;
                    contact.Status = Utility.Status.Answered;
                    _unitOfWork.Contacts.Update(contact);
                }
                TempData["ConfirmReply"] = "Your Reply has been submitted.";
            }
            else
                TempData["ConfirmReply"] = "Error! Your Reply has Not been submitted.";

            return RedirectToAction("NewContacts");
        }
        public IActionResult ContactsHistory() => View(_unitOfWork.Contacts.Get(c => c.Status == Utility.Status.Answered,
                a => a.ApplicationUser).ToList());
        #endregion
        #region Hotel Request
        public IActionResult NewHotelRequest() =>
            View(new HotelRequestView
            {
                HotelRequests =
                _unitOfWork.HotelRequests.Get(
                s => s.Status == Utility.Status.Pending,
                e => e.State,
                e => e.State.Country,
                e => e.ApplicationUser
                ).ToList()
            });
        public IActionResult DeleteHotelRequest(int HotelRequestId, string ViewNavigation)
        {
            var Request = _unitOfWork.HotelRequests.GetOne(e => e.HotelRequestId == HotelRequestId);
            if (Request == null) return NotFound();
            _unitOfWork.HotelRequests.Remove(Request);

            if (Utility.ViewNavigation.HotelRequest.ToString() == ViewNavigation)
                return RedirectToAction("NewHotelRequest");
            else if (Utility.ViewNavigation.HotelRequestHistory.ToString() == ViewNavigation)
                return RedirectToAction("HotelRequestHistory");
            else
                return NotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> replytohotelrequest(HotelRequestView hotelRequestView)
        {
            if (ModelState.IsValid)
            {
                var request = _unitOfWork.HotelRequests.GetOne(id => id.HotelRequestId == hotelRequestView.RequestId, u => u.ApplicationUser);
                if (request != null)
                {
                    await _emailSender.SendEmailAsync(
                        request.ApplicationUser.Email ?? "",
                        "Response to Your Hotel Request",
                        $@"
                        <p>{(hotelRequestView.status == Utility.Status.Approved ? "Request Approved" : "Request Rejected")},</p>
                        
                        <p>{hotelRequestView.Reply}</p>
                        
                        <p>If you have any further questions or need additional assistance, feel free to contact us.</p>
                        
                        <p>Best regards,<br>
                        Hotel Booking Team</p>
                        "
                        );
                    request.Reply = hotelRequestView.Reply;
                    request.Status = hotelRequestView.status;
                    _unitOfWork.HotelRequests.Update(request);
                    #region Add New Hotel
                    if (hotelRequestView.status == Utility.Status.Approved)
                    {
                        var newHotel = new Hotel
                        {
                            HotelName = request.HotelName,
                            StateId = request.StateId,
                            Stars = request.Stars,
                            LocationLink = request.LocationLink,
                            Address = request.Address,
                            HotelHotLine = request.HotelHotLine,
                            HotelContactEmail = request.HotelContactEmail,
                            Rating = 0
                        };

                        _unitOfWork.Hotels.Add(newHotel);

                        //add 1st manger to the hotel
                        var user = await _userManager.FindByIdAsync(request.UserId);
                        if (user != null)
                        {
                            var currentRoles = await _userManager.GetRolesAsync(user);
                            await _userManager.RemoveFromRolesAsync(user, currentRoles);
                            await _userManager.AddToRoleAsync(user, "HotelManager");

                            user.HotelId = newHotel.HotelId;
                            await _userManager.UpdateAsync(user);
                        }                        
                    }
                    #endregion
                    TempData["ConfirmReply"] = "Your Reply has been submitted.";
                }
                else
                    TempData["ConfirmReply"] = "Error! Your Reply has Not been submitted.";               
            }
            return RedirectToAction("NewHotelRequest");           
        }
        public IActionResult HotelRequestHistory() => 
            View(_unitOfWork.HotelRequests.Get(
                s => s.Status == Utility.Status.Approved ||
                s.Status == Utility.Status.Rejected,
                e => e.State,
                e => e.State.Country,
                e => e.ApplicationUser
                ).ToList());
        #endregion
    }
}
