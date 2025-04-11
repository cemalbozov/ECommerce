using ECommerce.Business.Abstract;
using ECommerce.Business.Concrete;
using ECommerce.Data.Abstract;
using ECommerce.Data.Concrete.EfCore;
using ECommerce.WebUI.EmailServices;
using ECommerce.WebUI.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
            
        }

        private IConfiguration _configuration;
        

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(_configuration.GetConnectionString("MSSQLConnection")));
            services.AddDbContext<ShopContext>(options => options.UseSqlServer(_configuration.GetConnectionString("MSSQLConnection")));
            
            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                //password
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = true;

                //lockout
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.AllowedForNewUsers = true;

                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;


            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/account/login";
                options.LogoutPath = "/account/logout";
                options.AccessDeniedPath = "/acoount/accessdenied";
                options.SlidingExpiration = true; // her istekte verilen login süresinin sýfýrlanmasý için
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.Cookie = new CookieBuilder
                {
                    HttpOnly = true,
                    Name = ".ECommerce.Security.Cookie",
                    SameSite = SameSiteMode.Strict
                };
            });

            /*services.AddScoped<ICategoryRepository, EfCoreCategoryRepository>();
            services.AddScoped<IParentCategoryRepository, EfCoreParentCategoryRepository>();
            services.AddScoped<IProductRepository, EfCoreProductRepository>();
            services.AddScoped<ICartRepository, EfCoreCartRepository>();
            services.AddScoped<IOrderRepository, EfCoreOrderRepository>();*/
            services.AddScoped<IUnitOfWork, UnitOfWork>();


            services.AddScoped<ICategoryService, CategoryManager>();
            services.AddScoped<IParentCategoryService, ParentCategoryManager>();
            services.AddScoped<IProductService, ProductManager>();
            services.AddScoped<ICartService, CartManager>();
            services.AddScoped<IOrderService, OrderManager>();

            services.AddScoped<IEmailSender, SmtpEmailSender>(i =>
                new SmtpEmailSender(
                     _configuration["EmailSender:Host"],
                     _configuration.GetValue<int>("EmailSender:Port"),
                     _configuration.GetValue<bool>("EmailSender:EnableSSL"),
                     _configuration["EmailSender:UserName"],
                     _configuration["EmailSender:Password"])
                );

            services.AddControllersWithViews();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ICartService cartService)
        {
            app.UseStaticFiles();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "manage",
                    pattern: "account/manage",
                    defaults: new { controller = "Account", Action = "Manage" }
                    );
                endpoints.MapControllerRoute(
                    name: "accountorders",
                    pattern: "account/orders",
                    defaults: new { controller = "Account", Action = "GetOrders" }
                    );

                endpoints.MapControllerRoute(
                    name: "checkout",
                    pattern: "checkout",
                    defaults: new { controller = "Cart", Action = "Checkout" }
                    );

                endpoints.MapControllerRoute(
                    name: "cart",
                    pattern: "cart",
                    defaults: new { controller = "Cart", Action = "Index" }
                    );

                endpoints.MapControllerRoute(
                    name: "adminusers",
                    pattern: "admin/users",
                    defaults: new { controller = "Admin", Action = "UserList" }
                    );

                endpoints.MapControllerRoute(
                    name: "adminuseredit",
                    pattern: "admin/users/{id?}",
                    defaults: new { controller = "Admin", Action = "UserEdit" }
                    );

                endpoints.MapControllerRoute(
                    name: "adminroles",
                    pattern: "admin/roles",
                    defaults: new { controller = "Admin", Action = "RoleList" }
                    );

                endpoints.MapControllerRoute(
                    name: "adminrolecreate",
                    pattern: "admin/roles/create",
                    defaults: new { controller = "Admin", Action = "RoleCreate" }
                    );

                endpoints.MapControllerRoute(
                    name: "adminroleedit",
                    pattern: "admin/roles/{id?}",
                    defaults: new { controller = "Admin", Action = "RoleEdit" }
                    );

                endpoints.MapControllerRoute(
                    name: "adminproducts",
                    pattern: "admin/products",
                    defaults: new { controller = "Admin", Action = "ProductList" }
                    );

                endpoints.MapControllerRoute(
                    name: "adminproductcreate",
                    pattern: "admin/products/create",
                    defaults: new { controller = "Admin", Action = "ProductCreate" }
                    );

                endpoints.MapControllerRoute(
                    name: "adminproductedit",
                    pattern: "admin/products/{id?}",
                    defaults: new { controller = "Admin", Action = "ProductEdit" }
                    );

                endpoints.MapControllerRoute(
                    name: "admincategories",
                    pattern: "admin/categories",
                    defaults: new { controller = "Admin", Action = "CategoryList" }
                    );

                endpoints.MapControllerRoute(
                    name: "admincategorycreate",
                    pattern: "admin/categories/create",
                    defaults: new { controller = "Admin", Action = "CategoryCreate" }
                    );

                endpoints.MapControllerRoute(
                    name: "admincategoryedit",
                    pattern: "admin/categories/{id?}",
                    defaults: new { controller = "Admin", Action = "CategoryEdit" }
                    );

                endpoints.MapControllerRoute(
                    name: "search",
                    pattern: "search",
                    defaults: new { controller = "Shop", Action = "Search" }
                    );

                endpoints.MapControllerRoute(
                    name: "productdetails",
                    pattern: "{url}",
                    defaults: new { controller = "Shop", Action = "Details" }
                    );

                endpoints.MapControllerRoute(
                    name: "products",
                    pattern:"products/{category?}",
                    defaults: new {controller="Shop",Action="List"}
                    );

                endpoints.MapControllerRoute(
                   name: "default",
                   pattern:"{controller=Home}/{action=Index}/{id?}"
                   );
            });

            SeedIdentity.Seed(userManager,roleManager, cartService, _configuration).Wait();
        }
    }
}
