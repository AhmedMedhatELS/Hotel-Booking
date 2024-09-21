using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Models;
using Models.ViewModel;
using System;
using Utility;

namespace Hotels_Booking.Areas.HotelManager.Controllers
{
    [Area("HotelManager")]
    [Authorize(Roles = "HotelManager")]
    public class HotelDataController(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork) : Controller
    {
        #region Start Up
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private int HotelId;
        private Status Status;
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)   HotelId = user.HotelId ?? 0;

            Status = _unitOfWork.Hotels.GetOne(e => e.HotelId == HotelId)?.Status ?? Status;
            if (Status == Utility.Status.AdminReview)            
                TempData["EditData"] = "No Changes Can Be Made";
            
            await next();
        }
        #endregion
        #region Hotel General Data
        public IActionResult HotelGeneralData()
        {
            ViewData["states"] = _unitOfWork.States.Get(null, e => e.Country).ToList();

            return View(
                _unitOfWork.Hotels.GetOne(
                    e => e.HotelId == HotelId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditHotel(Hotel hotel)
        {
            if(Status == Status.AdminReview) return RedirectToAction("HotelGeneralData");

            if (!ModelState.IsValid) return BadRequest();

            var currentHotel = _unitOfWork.Hotels.GetOne(e => e.HotelId == HotelId);
            if (currentHotel == null) return NotFound();

            currentHotel.HotelHotLine = hotel.HotelHotLine;
            currentHotel.Address = hotel.Address;
            currentHotel.HotelContactEmail = hotel.HotelContactEmail;
            currentHotel.HotelName = hotel.HotelName;
            currentHotel.LocationLink = hotel.LocationLink;
            currentHotel.Stars = hotel.Stars;
            currentHotel.StateId = hotel.StateId;

            _unitOfWork.Hotels.Update(currentHotel);

            return RedirectToAction("HotelGeneralData");
        }
        #endregion
        #region Hotel Facilities
        public IActionResult HotelFacilities() =>
            View(new FacilitiesView 
            { Facilities = 
                _unitOfWork.Facilities.Get(e => e.HotelId == HotelId).ToList()});
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddFacility(Facility NewFacility)
        {
            if (Status == Status.AdminReview) return RedirectToAction("HotelFacilities");

            if (!ModelState.IsValid) return BadRequest();

            NewFacility.HotelId = HotelId;
            _unitOfWork.Facilities.Add(NewFacility);

            return RedirectToAction("HotelFacilities");
        }
        public IActionResult DeleteFacility(int id)
        {
            if (Status == Status.AdminReview) return RedirectToAction("HotelFacilities");

            var facility = _unitOfWork.Facilities.GetOne(e => e.FacilityId == id);
            if (facility == null) return BadRequest();

            _unitOfWork.Facilities.Remove(facility);

            return RedirectToAction("HotelFacilities");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditFacility(Facility NewFacility)
        {
            if (Status == Status.AdminReview) return RedirectToAction("HotelFacilities");

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var currentFacility = _unitOfWork.Facilities.GetOne(e => e.FacilityId == NewFacility.FacilityId);

            if (currentFacility == null) return BadRequest();

            currentFacility.Name = NewFacility.Name;
            _unitOfWork.Facilities.Update(currentFacility);

            return RedirectToAction("HotelFacilities");
        }
        #endregion
        #region Gallery
        public IActionResult HotelGallery() =>
            View(
                new GalleyView { HotelGalleryList = 
                    _unitOfWork.HotelGalleries.Get(
                        e => e.HotelId == HotelId && e.IsMainImage == false).ToList(),
                    MainImageName = _unitOfWork.HotelGalleries.GetOne(
                        e => e.HotelId == HotelId && e.IsMainImage == true)?.ImageName
                }); 
        public IActionResult UploadMainImage(IFormFile MainImageFile)
        {
            if (Status == Status.AdminReview) return RedirectToAction("HotelGallery");

            if (MainImageFile == null) return BadRequest();

            #region Delete Old Main Image if Exists
            var oldMainImage = _unitOfWork.HotelGalleries.GetOne(
                e => e.IsMainImage == true && e.HotelId == HotelId);
            if (oldMainImage != null)
            {
                ImageManger.DeleteImage(oldMainImage.ImageName, ImageLocation.Hotels);
                _unitOfWork.HotelGalleries.Remove(oldMainImage);
            }
            #endregion
            #region Add New Main Image
            var mainimage = new HotelGallery
            {
                HotelId = HotelId,
                IsMainImage = true,
                ImageName = ImageManger.SaveImage(MainImageFile, ImageLocation.Hotels) ?? "error"
            };

            if(mainimage.ImageName != "error") 
                _unitOfWork.HotelGalleries.Add(mainimage);
            else
                return BadRequest();
            #endregion

            return RedirectToAction("HotelGallery");
        }
        public IActionResult UploadGalleryImages(List<IFormFile> ImageFiles)
        {
            if (Status == Status.AdminReview) return RedirectToAction("HotelGallery");

            var imagesNameList = ImageManger.SaveImageList(ImageFiles,ImageLocation.Hotels);
            foreach (var image in imagesNameList)
            {
                var newHotelImage = new HotelGallery
                {
                    HotelId = HotelId,
                    ImageName = image
                };
                _unitOfWork.HotelGalleries.Add(newHotelImage);
            }
            return RedirectToAction("HotelGallery");
        }
        public IActionResult DeleteGalleryImage(int id)
        {
            if (Status == Status.AdminReview) return RedirectToAction("HotelGallery");

            var hotelImage = _unitOfWork.HotelGalleries.GetOne(e => e.HotelGalleryId == id);

            if (hotelImage == null) return BadRequest();

            ImageManger.DeleteImage(hotelImage.ImageName, ImageLocation.Hotels);
            _unitOfWork.HotelGalleries.Remove(hotelImage);

            return RedirectToAction("HotelGallery");
        }
        #endregion
    }
}
