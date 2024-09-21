using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        ICountryRepository Countries { get; }
        IStateRepository States { get; }
        IContactRepository Contacts { get; }
        IFacilityRepository Facilities { get; }
        IHotelGalleryRepository HotelGalleries { get; }

        IHotelRepository Hotels { get; }
        IHotelRequestRepository HotelRequests { get; }
        ILocationViewRepository LocationViews { get; }
        IReservationRepository Reservations { get; }
        IReservationRoomRepository ReservationRooms { get; }

        IRoomGalleryRepository RoomGalleries { get; }
        IRoomRepository Rooms { get; }
        IRoomTypeRepository RoomTypes { get; }
        IRoomUnitRepository RoomUnits { get; }
        IRoomViewRepository RoomViews { get; }

        IUserReviewRepository UserReviews { get; }

        Task<int> CompleteAsync();
    }
}
