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
            // get variables from .env file
            Env.Load("C:\\Users\\mahmoud\\source\\repos\\Iti-Graduation-Project-Cars\\.env");

            // get Google ClientId and ClientSecret from .env
            var googleClientId = Environment.GetEnvironmentVariable("ClientId");
            var googleClientSecret = Environment.GetEnvironmentVariable("ClientSecret");

            if (string.IsNullOrWhiteSpace(googleClientId) || string.IsNullOrWhiteSpace(googleClientSecret))
            {
                throw new Exception("googleClientId and googleClientSecret are required");
            }

            // Set configuration values
            builder.Configuration["Google:ClientId"] = googleClientId;
            builder.Configuration["Google:ClientSecret"] = googleClientSecret;

            
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
            builder.Services.AddAuthentication(options =>
            {
                
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })

.AddCookie()

.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Google:ClientId"];
    options.ClientSecret = builder.Configuration["Google:ClientSecret"];
    options.SignInScheme = IdentityConstants.ExternalScheme;
});
            builder.Services.AddHangfire(x => x.UseSqlServerStorage(connectionString));
            builder.Services.AddHangfireServer();
            
            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();

                recurringJobManager.AddOrUpdate<IOfferService>("check-offer-date",service => service.CheckOfferDate(),"* * * * *");

                recurringJobManager.AddOrUpdate<IRentService>("check-rent-date",service => service.CheckRentDate(),"* * * * *");
            }

            // Middleware pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            //RecurringJob.AddOrUpdate<IOfferService>(service => service.CheckOfferDate(), "0 12 * * *");
            //RecurringJob.AddOrUpdate<IRentService>(service => service.CheckRentDate(), "0 12 * * *");
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
            app.UseHangfireDashboard("/BackGroundJob");
            app.Run();
        }
    }
}
