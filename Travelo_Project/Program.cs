
using AutoMapper;
using DomainLayer.Models.Identity;
using DomainLayer.Models.User;
using DomainLayer.RepositoryInterface;
using DomainLayer.RepositoryInterface.Booking;
using DomainLayer.RepositoryInterface.Flights;
using DomainLayer.RepositoryInterface.Hotel___Accommodation;
using DomainLayer.RepositoryInterface.Tours;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistance;
using Persistance.DataSeed;
using Persistance.RepositoryImplementation;
using Persistance.RepositoryImplementation.Booking_Transaction;
using Persistance.RepositoryImplementation.Flights;
using Persistance.RepositoryImplementation.Hotel___Accommodation;
using Persistance.RepositoryImplementation.Hotel___Accomodation;
using Persistance.RepositoryImplementation.Tours;
using ServiceAbstraction;
using ServiceAbstraction.Booking;
using ServiceAbstraction.flight;
using ServiceAbstraction.Hotel___Accommodation;
using ServiceAbstraction.Tour;
using ServiceImplementation;
using ServiceImplementation.Booking;
using ServiceImplementation.flight;
using ServiceImplementation.Flight;
using ServiceImplementation.Hotel___Accommodation;
using ServiceImplementation.MappingProfile;
using ServiceImplementation.MappingProfile.Booking;
using ServiceImplementation.MappingProfile.Hotel___Accommodation;
using ServiceImplementation.Tour;
using Shared;
using System.Text;


namespace Travelo_Project
{
    
        public class Program
        {
            public static void Main(string[] args)
            {
                var builder = WebApplication.CreateBuilder(args);

            // Add services

                // Tour
                builder.Services.AddScoped<ITourRepository, TourRepository>();
                builder.Services.AddScoped<ITourService, TourService>();
                builder.Services.AddAutoMapper(typeof(TourProfile));

               //TourInclusion
                builder.Services.AddAutoMapper(typeof(TourInclusionProfile));
                builder.Services.AddScoped<ITourInclusionService, TourInclusionService>();
                builder.Services.AddScoped<ITourInclusionRepository, TourInclusionRepository>();


                //TourItinerary
                builder.Services.AddAutoMapper(typeof(TourItineraryProfile));
                builder.Services.AddScoped<ITourItineraryService, TourItineraryService>();
                builder.Services.AddScoped<ITourItineraryRepository, TourItineraryRepository>();

                //TourDestination
                builder.Services.AddAutoMapper(typeof(TourDestinationProfile));
                builder.Services.AddScoped<ITourDestinationService, TourDestinationService>();
                builder.Services.AddScoped<ITourDestinationRepository, TourDestinationRepository>();

                // TourDat
                builder.Services.AddScoped<ITourDateRepository, TourDateRepository>();
                builder.Services.AddScoped<ITourDateService, TourDateService>();
                builder.Services.AddAutoMapper(typeof(TourDateProfile));

                // Destination
                builder.Services.AddScoped<IDestinationRepository, DestinationRepository>();
                builder.Services.AddScoped<IDestinationService, DestinationService>();
                builder.Services.AddAutoMapper(typeof(DestinationProfile));

                // TourBooking 
                builder.Services.AddScoped<ITourBookingRepository, TourBookingRepository>();
                builder.Services.AddScoped<ITourBookingService, TourBookingService>();
                builder.Services.AddAutoMapper(typeof(TourBookingProfile));

            builder.Services.AddScoped<IAirlineService, AirlineService>();
            builder.Services.AddScoped<IAirlineRepository, AirLineRepository>();
            builder.Services.AddAutoMapper(typeof(AirlineProfile).Assembly);
            builder.Services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
            builder.Services.AddScoped<IAirportRepository, AirportRepository>();
            builder.Services.AddScoped<IAirportService, AirportService>();
            builder.Services.AddScoped<IFlightService, FlightService>();

            builder.Services.AddScoped<IFlightRepository, FlightRepository>();
            builder.Services.AddScoped<IFlightOfferRepository, FlightOfferRepository>();
            builder.Services.AddScoped<IFlightPriceRepository, FlightPriceRepository>();
            builder.Services.AddScoped<IFlightPriceRepository, FlightPriceRepository>();
            builder.Services.AddScoped<IFlightPriceService, FlightPriceService>();
            builder.Services.AddScoped<IFlightOfferService, FlightOfferService>();



            builder.Services.AddAutoMapper(typeof(AuthProfile));
            builder.Services.AddAutoMapper(typeof(BookingProfile));
            builder.Services.AddAutoMapper(typeof(ReviewProfile));



            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddScoped(typeof(IPaymentService), typeof(PaymentService));
            builder.Services.AddScoped(typeof(IReviewServices), typeof(ReviewServices));
            builder.Services.AddScoped(typeof(ITransactionService), typeof(TransactionService));

            //hotel
            builder.Services.AddScoped<IAmenityRepository, AmenityRepository>();
            builder.Services.AddScoped<IAmenityService, AmenityService>();
            builder.Services.AddAutoMapper(typeof(AmenityProfile));


            builder.Services.AddScoped<IRoomRepository, RoomRepository>();
            builder.Services.AddScoped<IRoomService, RoomService>();
            builder.Services.AddAutoMapper(typeof(RoomProfile));

            builder.Services.AddScoped<IHotelAmenityRepository, HotelAmenityRepository>();
            builder.Services.AddScoped<IHotelAmenityService, HotelAmenityService>();
            builder.Services.AddAutoMapper(typeof(HotelAmenityProfile));

            builder.Services.AddScoped<IHotelRepository, HotelRepository>();
            builder.Services.AddScoped<IHotelService, HotelService>();
            builder.Services.AddAutoMapper(typeof(HotelProfile));
            builder.Services.AddAutoMapper(typeof(HotelDetailsProfile));

            builder.Services.AddScoped<IPriceAndAvailabilityRepository, PriceAndAvailabilityRepository>();
            builder.Services.AddScoped<IPriceAndAvailbilityService, PriceAndAvailbilityService>();
            builder.Services.AddAutoMapper(typeof(PriceAndAvailabilityProfile));


            builder.Services.AddControllers();
                builder.Services.AddOpenApi();

                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseSqlServer(builder.Configuration.GetConnectionString("BaseConnection"));
                });

                builder.Services.AddScoped<IDataSeeding, DataSeeder>();

                builder.Services.AddAuthentication(config =>
                {
                    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration.GetSection("JwtOptions")["Issuer"],
                        ValidateAudience = true,
                        ValidAudience = builder.Configuration.GetSection("JwtOptions")["Audience"],
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JwtOptions")["SecretKey"]))
                    };
                });

                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Travelo API", Version = "v1" });
                });

                builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

                var app = builder.Build();

                // Run data seed
                using var scope = app.Services.CreateScope();
                var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeding>();
                seeder.DataSeed();

                // Swagger (يشتغل في كل البيئات)
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Travelo API v1");
                    c.RoutePrefix = string.Empty;
                });

                // Pipeline
                app.UseHttpsRedirection();

                app.UseRouting();

                app.UseAuthentication();
                app.UseAuthorization();

                app.MapControllers();

                app.Run();
            }
        }
    }
  
