using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using ShopApp.Bussiness.Abstract;
using ShopApp.Bussiness.Concrete;
using ShopApp.DataAccess.Abstract;
using ShopApp.DataAccess.Concrete.EfCore;
using ShopApp.WebUI.EmailServices;
using ShopApp.WebUI.Identity;
using ShopApp.WebUI.Middlewares;

namespace ShopApp.WebUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddScoped<ICartDal, EfCoreCartDal>();
            builder.Services.AddScoped<ICartService, CartManager>();
            builder.Services.AddScoped<IProductDal, EfCoreProductDal>();
            builder.Services.AddScoped<ICategoryDal, EfCoreCategoryDal>();
            builder.Services.AddScoped<IProductService, ProductManager>();
            builder.Services.AddScoped<ICategoryService, CategoryManager>();
            builder.Services.AddTransient<IEmailSender, EmailSender>();
            builder.Services.AddMvc();
            builder.Services.AddControllersWithViews();
            // Add services to the container.
            builder.Services.AddRazorPages();

            builder.Services.AddDbContext<ShopContext>(options =>
     options.UseSqlServer(
         builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
   options.UseSqlServer(
       builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
                .AddDefaultTokenProviders();
            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase= true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric= true;
                options.Password.RequireUppercase= true;

                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan= TimeSpan.FromMinutes(5);
                options.Lockout.AllowedForNewUsers = true;

                //options.User.AllowedUserNameCharacters = "";
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            });
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/account/login";
                options.LogoutPath = "/account/logout";
                options.AccessDeniedPath = "/account/accessdenied";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.SlidingExpiration = true;
                options.Cookie = new CookieBuilder
                {
                    HttpOnly = true,
                    Name = "ShopApp.Security.Cookie",
                    SameSite =SameSiteMode.Strict
                };
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                SeedDatabase.Seed();
            }
            
            app.CustomStaticFiles();
     
           
            app.UseEndpoints(routes =>
            {

                routes.MapControllerRoute(
             name: "cart",
             pattern: "cart",
             defaults: new { controller = "Cart", action = "Index" }
           );


                routes.MapControllerRoute(
              name: "adminCategories",
              pattern: "admin/categories/{id?}",
              defaults: new { controller = "Admin", action = "EditCategory" }
            );
                routes.MapControllerRoute(
                 name: "adminProducts",
                 pattern: "admin/products",
                 defaults: new { controller = "Admin", action = "ProductList" }
               );

                routes.MapControllerRoute(
                 name: "adminProducts",
                 pattern: "admin/products/{id?}",
                 defaults: new { controller = "Admin", action = "EditProduct" }
               );

                routes.MapControllerRoute(
                  name: "products",
                  pattern: "products/{category?}",
                  defaults: new { controller = "Shop", action = "List" }
                );

                routes.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            using (var scope= app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var userManager = services.GetRequiredService <UserManager<ApplicationUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var configuration = services.GetRequiredService<IConfiguration>();
                SeedIdentity.Seed(userManager, roleManager, configuration).Wait();
            }

                app.Run();

        }
    }
}
