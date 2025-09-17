using Cars.BLL.Mapper;
using Cars.BLL.Service.Abstraction;
using Cars.BLL.Service.Implementation;
using Cars.DAL.Database;
using Cars.DAL.Entities.Users;
using Cars.DAL.Repo.Abstraction;
using Cars.DAL.Repo.Implementation;
using Cars.DAL.Repo.Implementation.Cars.DAL.Repository;
using Cars.PL.Language;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Cars
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Get connection string
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // Add MVC + Localization
            builder.Services.AddControllersWithViews()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                        factory.Create(typeof(SharedResource));
                });

            // AutoMapper
            builder.Services.AddAutoMapper(x => x.AddProfile(new DomainProfile()));

            // DbContext
            builder.Services.AddDbContext<CarsDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Identity (Users + Roles)
            builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                // Password Policy
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;

                // User settings
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<CarsDbContext>()
            .AddDefaultTokenProviders();

            // Repositories
            builder.Services.AddScoped<IAppUserRepo, AppUserRepo>();
            builder.Services.AddScoped<ICarRepo, CarRepo>();
            builder.Services.AddScoped<IRentRepo, RentRepo>();
            builder.Services.AddScoped<IMechanicRepo, MechanicRepo>();
            builder.Services.AddScoped<IAccidentRepo, AccidentRepo>();
            builder.Services.AddScoped<IOfferRepo, OfferRepo>();

            // Services
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IAppUserService, AppUserService>();
            builder.Services.AddScoped<ICarService, CarService>();
            builder.Services.AddScoped<IPayPalService, PayPalService>();
            builder.Services.AddScoped<IRentService, RentService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IAccidentService, AccidentService>();
            builder.Services.AddScoped<IOfferService, OfferService>();
            builder.Services.Configure<EmailService>(builder.Configuration.GetSection("EmailSettings"));

            var app = builder.Build();

            // Middleware pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // Localization
            var supportedCultures = new[] {
                new CultureInfo("ar-EG"),
                new CultureInfo("en-US"),
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
