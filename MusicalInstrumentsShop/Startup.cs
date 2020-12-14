using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MusicalInstrumentsShop.BusinessLogic.Services.Interfaces;
using MusicalInstrumentsShop.DataAccess.Data;
using MusicalInstrumentsShop.DataAccess.Entities;
using System;
using Microsoft.AspNetCore.Http;
using MusicalInstrumentsShop.BusinessLogic.Mappings;
using AutoMapper;
using MusicalInstrumentsShop.BusinessLogic.Services;
using MusicalInstrumentsShop.DataAccess.UnitOfWork;
using MusicalInstrumentsShop.BusinessLogic.Mappings.Products;
using MusicalInstrumentsShop.BusinessLogic.ProductFiltering;

namespace MusicalInstrumentsShop
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(2);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddControllersWithViews();

            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
               b => b.MigrationsAssembly($"MusicalInstrumentsShop.DataAccess")));

            services.AddIdentity<ApplicationUser, ApplicationRole>()
             .AddEntityFrameworkStores<ApplicationDbContext>()
             .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 6;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
                options.User.RequireUniqueEmail = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/Error/AccessDenied");
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IStockService, StockService>();
            services.AddScoped<ISpecificationService, SpecificationService>();
            services.AddScoped<IDeliveryService, DeliveryService>();
            services.AddScoped<IProductFilterService, ProductFilterService>();
            services.AddScoped<IWishlistService, WishlistService>();
            services.AddScoped<IWishlistProductService, WishlistProductService>();
            services.AddScoped<IProductMappingService, ProductMappingService>();
            services.AddScoped<IOrderDetailsService, OrderDetailsService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<ICartProductService, CartProductService>();

            services.AddAutoMapper(typeof(CategoryProfile));
            services.AddAutoMapper(typeof(SupplierProfile));
            services.AddAutoMapper(typeof(ProductCreationProfile));
            services.AddAutoMapper(typeof(ProductEditingProfile));
            services.AddAutoMapper(typeof(ProductProfile));
            services.AddAutoMapper(typeof(SpecificationProfile));
            services.AddAutoMapper(typeof(DeliveryMethodProfile));
            services.AddAutoMapper(typeof(OrderDetailsProfile));
            services.AddAutoMapper(typeof(PaymentProfile));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager, ApplicationDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                app.UseHsts();
            }
        
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();

            DatabaseSeeding.AddRoles(roleManager);
            DatabaseSeeding.AddAdministrator(userManager);
            DatabaseSeeding.AddPaymentMethod(context);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Products}/{action=Browse}/{id?}");
            });
        }
    }
}
