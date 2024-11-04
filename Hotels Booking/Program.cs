using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Identity.UI.Services;
using Utility;
using Models;
using DataAccess.Repository.IRepository;
using DataAccess.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared;
using Stripe;

namespace Hotels_Booking
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            #region DataBase
            builder.Services.AddDbContext<ApplicationDbContext>(options
                => options.UseSqlServer(builder.Configuration
                .GetConnectionString("DefaultConnection")));
            #endregion
            #region Identity
            builder.Services.AddDefaultIdentity<ApplicationUser>(options
                => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            #endregion
            #region Email Sender
            builder.Services.AddScoped<IEmailSender, EmailSender>();
            #endregion
            #region Register With
            builder.Services.AddAuthentication().AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = 
                "667625120891-tu686hsp2q99u27up7qjc1jqmqlucf5a.apps.googleusercontent.com";
                googleOptions.ClientSecret =
                "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";
            });
            #endregion
            #region Models Services
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            //builder.Services.AddScoped<ICountryRepository, CountryRepository>();
            //builder.Services.AddScoped<IStateRepository, StateRepository>();
            //builder.Services.AddScoped<IContactRepository, ContactRepository>();
            //builder.Services.AddScoped<IFacilityRepository, FacilityRepository>();            
            //builder.Services.AddScoped<IHotelGalleryRepository, HotelGalleryRepository>();
            
            //builder.Services.AddScoped<IHotelRepository, HotelRepository>();
            //builder.Services.AddScoped<IHotelRequestRepository, HotelRequestRepository>();
            //builder.Services.AddScoped<ILocationViewRepository, LocationViewRepository>();
            //builder.Services.AddScoped<IReservationRepository, ReservationRepository>();            
            //builder.Services.AddScoped<IReservationRoomRepository, ReservationRoomRepository>();
            
            //builder.Services.AddScoped<IRoomGalleryRepository, RoomGalleryRepository>();
            //builder.Services.AddScoped<IRoomRepository, RoomRepository>();
            //builder.Services.AddScoped<IRoomTypeRepository, RoomTypeRepository>();
            //builder.Services.AddScoped<IRoomUnitRepository, RoomUnitRepository>();            
            //builder.Services.AddScoped<IRoomViewRepository, RoomViewRepository>();

            //builder.Services.AddScoped<IUserReviewRepository, UserReviewRepository>();
            #endregion
            #region Stripe
            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
            StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];
            #endregion

            var app = builder.Build();

            #region Inteal Create Roles And 1st Admin

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await IntailRolesAdmin.SeedRolesAsync(services);
                await IntailRolesAdmin.SeedAdminUserAsync(services);
            }
            #endregion

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();
            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }

    }
}
