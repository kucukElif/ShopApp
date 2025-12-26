using Microsoft.EntityFrameworkCore;
using ShopApp.Bussiness.Abstract;
using ShopApp.Bussiness.Concrete;
using ShopApp.DataAccess.Abstract;
using ShopApp.DataAccess.Concrete.EfCore;
using ShopApp.WebUI.Middlewares;

namespace ShopApp.WebUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<IProductDal, EfCoreProductDal>();
            builder.Services.AddScoped<ICategoryDal, EfCoreCategoryDal>();
            builder.Services.AddScoped<IProductService, ProductManager>();
            builder.Services.AddScoped<ICategoryService, CategoryManager>();
            builder.Services.AddMvc();
            builder.Services.AddControllersWithViews();
            // Add services to the container.
            builder.Services.AddRazorPages();

            builder.Services.AddDbContext<ShopContext>(options =>
     options.UseSqlServer(
         builder.Configuration.GetConnectionString("DefaultConnection")));

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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                SeedDatabase.Seed();
            }
            app.UseStaticFiles();
            app.CustomStaticFiles();
            app.UseRouting();
            app.UseEndpoints(routes =>
            {
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


            app.Run();
        }
    }
}
