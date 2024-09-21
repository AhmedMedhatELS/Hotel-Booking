using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccess
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<IdentityUser>(options)
    {
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<HotelRequest> HotelRequests { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<HotelGallery> HotelGalleries { get; set; }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<LocationView> LocationViews { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomGallery> RoomGalleries { get; set; }
        public DbSet<RoomView> RoomViews { get; set; }
        public DbSet<RoomUnit> RoomUnits { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<UserReview> UserReviews { get; set; }
        public DbSet<ReservationRoom> ReservationRooms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            #region OneToMany
            modelBuilder.Entity<Contact>()
                .HasOne(c => c.ApplicationUser)
                .WithMany(c => c.Contacts)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<HotelRequest>()
                .HasOne(c => c.ApplicationUser)
                .WithMany(c => c.HotelRequests)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reservation>()
               .HasOne(c => c.User)
               .WithMany(c => c.Reservations)
               .HasForeignKey(c => c.UserId)
               .OnDelete(DeleteBehavior.NoAction);
            #endregion
            #region ManyToMany
            modelBuilder.Entity<Room>(eb => {
                eb.HasMany(e => e.LocationViews).WithMany(e => e.Rooms).UsingEntity<RoomView>();
            });
            #endregion
        }

    }
}
