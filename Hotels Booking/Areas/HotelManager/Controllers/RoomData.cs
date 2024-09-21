using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Models;
using Models.ViewModel;
using Utility;

namespace Hotels_Booking.Areas.HotelManager.Controllers
{
    [Area("HotelManager")]
    [Authorize(Roles = "HotelManager")]
    public class RoomData(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork) : Controller
    {
        #region Start Up
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private int HotelId;
        private Status Status;
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null) HotelId = user.HotelId ?? 0;

            Status = _unitOfWork.Hotels.GetOne(e => e.HotelId == HotelId)?.Status ?? Status;
            if (Status == Utility.Status.AdminReview)
                TempData["EditData"] = "No Changes Can Be Made";

            await next();
        }
        #endregion
        #region Rooms Data
        public IActionResult RoomGeneralData() => 
            View(new HotelRoomView
            {
                Rooms = _unitOfWork.Rooms.Get(e => e.HotelId ==  HotelId).ToList(),
                RoomsTypes = _unitOfWork.RoomTypes.Get(null).ToList()
            });
        public IActionResult AddRoom(Room Room)
        {
            if (Status == Status.AdminReview) return RedirectToAction("RoomGeneralData");

            if (!ModelState.IsValid) return BadRequest(ModelState);

            Room.HotelId = HotelId;
            _unitOfWork.Rooms.Add(Room);

            return RedirectToAction("RoomGeneralData");
        }
        public IActionResult EditRoom(Room Room)
        {
            if (Status == Status.AdminReview) return RedirectToAction("RoomGeneralData");

            if (!ModelState.IsValid) return BadRequest();

            Room.HotelId = HotelId;
            _unitOfWork?.Rooms.Update(Room);

            return RedirectToAction("RoomGeneralData");
        }
        public IActionResult DeleteRoom(int id)
        {
            if (Status == Status.AdminReview) return RedirectToAction("RoomGeneralData");

            var room = _unitOfWork.Rooms.GetOne(e => e.RoomID == id);
            if (room == null) return NotFound();

            _unitOfWork.Rooms.Remove(room);
            return RedirectToAction("RoomGeneralData");
        }
        #endregion
        #region Room Gallery
        public IActionResult RoomGallery() =>
            View(new GalleyView { 
            RoomList = _unitOfWork.Rooms.Get(
                e => e.HotelId == HotelId, e => e.RoomGalleries).ToList()
            });
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UploadRoomMainImage(GalleyView RG)
        {
            if (Status == Status.AdminReview) return RedirectToAction("RoomGallery");

            if (RG.MainImageFile == null || 
                _unitOfWork.Rooms.GetOne(
                    e => e.RoomID == RG.RoomId) == null) 
                                                           return BadRequest();

            #region Delete Old Main Image if Exists
            var oldMainImage = _unitOfWork.RoomGalleries.GetOne(
                e => e.IsMainImage == true && e.RoomID == RG.RoomId);
            if (oldMainImage != null)
            {
                ImageManger.DeleteImage(oldMainImage.ImageName, ImageLocation.Rooms);
                _unitOfWork.RoomGalleries.Remove(oldMainImage);
            }
            #endregion
            #region Add New Main Image
            var mainimage = new RoomGallery
            {
                RoomID = RG.RoomId,
                IsMainImage = true,
                ImageName = ImageManger.SaveImage(RG.MainImageFile, ImageLocation.Rooms) ?? "error"
            };

            if (mainimage.ImageName != "error")
                _unitOfWork.RoomGalleries.Add(mainimage);
            else
                return BadRequest();
            #endregion

            return RedirectToAction("RoomGallery");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UploadRoomGalleryImages(GalleyView RG)
        {
            if (Status == Status.AdminReview) return RedirectToAction("RoomGallery");

            if (RG.ImageFiles == null ||
                _unitOfWork.Rooms.GetOne(
                    e => e.RoomID == RG.RoomId) == null)
                                                return BadRequest();

            var imagesNameList = ImageManger.SaveImageList(RG.ImageFiles, ImageLocation.Rooms);
            foreach (var image in imagesNameList)
            {
                var newRoomImage = new RoomGallery
                {
                    RoomID = RG.RoomId,
                    ImageName = image
                };
                _unitOfWork.RoomGalleries.Add(newRoomImage);
            }
            return RedirectToAction("RoomGallery");
        }
        public IActionResult DeleteRoomGalleryImage(int id)
        {
            if (Status == Status.AdminReview) return RedirectToAction("RoomGallery");

            var roomImage = _unitOfWork.RoomGalleries.GetOne(e => e.RoomGalleryId == id);

            if (roomImage == null) return BadRequest();

            ImageManger.DeleteImage(roomImage.ImageName, ImageLocation.Rooms);
            _unitOfWork.RoomGalleries.Remove(roomImage);

            return RedirectToAction("RoomGallery");
        }
        #endregion
        #region Rooms with Views
        public IActionResult RoomViewManagement() =>
            View(new RoomUnitLocationView
            {
                Rooms = _unitOfWork.Rooms.Get(
                    e => e.HotelId == HotelId,
                    e => e.RoomViews,
                    e => e.LocationViews).ToList(),
                Locations = _unitOfWork.LocationViews.Get(null).ToList()
            });
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddRoomView(RoomView RoomView)
        {
            if (Status == Status.AdminReview) return RedirectToAction("RoomViewManagement");

            if (!ModelState.IsValid) return BadRequest();

            var RoomViewExist = _unitOfWork.RoomViews.GetOne(
                e => 
                e.LocationViewId == RoomView.LocationViewId 
                && e.RoomId == RoomView.RoomId);
            
            if(RoomViewExist == null)
            {
                _unitOfWork.RoomViews.Add(RoomView);
                
                for (int i = 0; i < RoomView.NumberRooms; i++)
                    _unitOfWork.RoomUnits.Add(new RoomUnit { RoomViewId = RoomView.RoomViewId });
                
                TempData["RoomViewStatus"] = "Successfully Add";
            }
            else
            {
                TempData["RoomViewStatus"] = "Can't Be Add Aready Exists";
            }
            return RedirectToAction("RoomViewManagement");
        }
        public IActionResult DeleteRoomView(int id)
        {
            if (Status == Status.AdminReview) return RedirectToAction("RoomViewManagement");

            var roomView = _unitOfWork.RoomViews.GetOne(e => e.RoomViewId == id);

            if(roomView == null) return NotFound();

            #region Delete Units 1st
            var roomUnitsForThisRoomView = _unitOfWork.RoomUnits.Get(e => e.RoomViewId == id);
            
            foreach(var roomUnit in roomUnitsForThisRoomView)
                _unitOfWork.RoomUnits.Remove(roomUnit);
            #endregion
            #region Delete RoomView
            _unitOfWork.RoomViews.Remove(roomView);
            #endregion

            return RedirectToAction("RoomViewManagement");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateRoomView(RoomView RoomView)
        {
            if (Status == Status.AdminReview) return RedirectToAction("RoomViewManagement");

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var curentRoomView = _unitOfWork.RoomViews.GetOne(e => e.RoomViewId == RoomView.RoomViewId);

            if (curentRoomView == null) return NotFound();
           
            #region Units Edits 1st
            var unitsRoomView = _unitOfWork.RoomUnits.Get(e => e.RoomViewId == RoomView.RoomViewId).ToList();

            if (curentRoomView.NumberRooms > RoomView.NumberRooms)
                for (int i = 0; i < curentRoomView.NumberRooms - RoomView.NumberRooms; i++)
                    _unitOfWork.RoomUnits.Remove(unitsRoomView[i]);
            else if(curentRoomView.NumberRooms < RoomView.NumberRooms)
                for (int i = 0; i < RoomView.NumberRooms - curentRoomView.NumberRooms; i++)
                    _unitOfWork.RoomUnits.Add(new RoomUnit { RoomViewId = RoomView.RoomViewId});
            #endregion

            #region RoomView Edits 2nd
            curentRoomView.NumberRooms = RoomView.NumberRooms;
            curentRoomView.Price = RoomView.Price;
            _unitOfWork.RoomViews.Update(curentRoomView);
            #endregion

            return RedirectToAction("RoomViewManagement");
        }
        #endregion
    }
}
