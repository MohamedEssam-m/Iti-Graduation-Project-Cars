using Cars.BLL.Mapper;
using Cars.BLL.Service.Abstraction;
using Cars.BLL.Service.Implementation;
using Cars.DAL.Database;
using Cars.DAL.Entities.Users;
using Cars.DAL.Repo.Abstraction;
using Cars.DAL.Repo.Implementation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Cors.Infrastructure;
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

            // Add services to the container.
            builder.Services.AddControllersWithViews()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
.AddDataAnnotationsLocalization(options =>
{
    options.DataAnnotationLocalizerProvider = (type, factory) =>
        factory.Create(typeof(SharedResource));
});
            ;
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<CarsDbContext>(options =>
            options.UseSqlServer(connectionString));

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
                            options =>
                            {
                                  options.LoginPath = new PathString("/Account/Login");
                                  options.AccessDeniedPath = new PathString("/Account/Login");
                            });
            builder.Services.AddIdentityCore<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<CarsDbContext>()
                .AddTokenProvider<DataProtectorTokenProvider<CarsDbContext>>(TokenOptions.DefaultProvider);


            builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                // Default Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;
            }).AddEntityFrameworkStores<CarsDbContext>();

            builder.Services.AddAutoMapper(x => x.AddProfile(new DomainProfile()));
            builder.Services.AddScoped<IAppUserRepo, AppUserRepo>();
            builder.Services.AddScoped<ICarRepo, CarRepo>();

            builder.Services.AddScoped<IAppUserService, AppUserService>();
            builder.Services.AddScoped<ICarService, CarService>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            var supportedCultures = new[] {
                      new CultureInfo("ar-EG"),
                      new CultureInfo("en-US"),
                };
            
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures,
                RequestCultureProviders = new List<IRequestCultureProvider>
                {
                new QueryStringRequestCultureProvider(),
                new CookieRequestCultureProvider()
                }
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
