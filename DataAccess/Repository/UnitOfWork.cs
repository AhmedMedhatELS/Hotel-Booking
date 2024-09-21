using DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
    {
        private readonly ApplicationDbContext _context = context;

        #region private fields for repositories

        private ICountryRepository _countries = null!;
        private IStateRepository _states = null!;
        private IContactRepository _contacts = null!;
        private IFacilityRepository _facilities = null!;
        private IHotelGalleryRepository _hotelGalleries = null!;
        private IHotelRepository _hotels = null!;
        private IHotelRequestRepository _hotelRequests = null!;
        private ILocationViewRepository _locationViews = null!;
        private IReservationRepository _reservations = null!;
        private IReservationRoomRepository _reservationRooms = null!;
        private IRoomGalleryRepository _roomGalleries = null!;
        private IRoomRepository _rooms = null!;
        private IRoomTypeRepository _roomTypes = null!;
        private IRoomUnitRepository _roomUnits = null!;
        private IRoomViewRepository _roomViews = null!;
        private IUserReviewRepository _userReviews = null!;

        #endregion

        #region properties to access the repositories
        public ICountryRepository Countries => _countries ??= new CountryRepository(_context);
        public IStateRepository States => _states ??= new StateRepository(_context);
        public IContactRepository Contacts => _contacts ??= new ContactRepository(_context);
        public IFacilityRepository Facilities => _facilities ??= new FacilityRepository(_context);
        public IHotelGalleryRepository HotelGalleries => _hotelGalleries ??= new HotelGalleryRepository(_context);
        public IHotelRepository Hotels => _hotels ??= new HotelRepository(_context);
        public IHotelRequestRepository HotelRequests => _hotelRequests ??= new HotelRequestRepository(_context);
        public ILocationViewRepository LocationViews => _locationViews ??= new LocationViewRepository(_context);
        public IReservationRepository Reservations => _reservations ??= new ReservationRepository(_context);
        public IReservationRoomRepository ReservationRooms => _reservationRooms ??= new ReservationRoomRepository(_context);
        public IRoomGalleryRepository RoomGalleries => _roomGalleries ??= new RoomGalleryRepository(_context);
        public IRoomRepository Rooms => _rooms ??= new RoomRepository(_context);
        public IRoomTypeRepository RoomTypes => _roomTypes ??= new RoomTypeRepository(_context);
        public IRoomUnitRepository RoomUnits => _roomUnits ??= new RoomUnitRepository(_context);
        public IRoomViewRepository RoomViews => _roomViews ??= new RoomViewRepository(_context);
        public IUserReviewRepository UserReviews => _userReviews ??= new UserReviewRepository(_context);
        #endregion

        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();
        public void Dispose() => _context.Dispose();
    }

}
