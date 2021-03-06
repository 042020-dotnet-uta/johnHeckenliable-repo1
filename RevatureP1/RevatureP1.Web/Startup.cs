using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Revaturep1.Domain.Interfaces;
using Revaturep1.DataAccess;
using Revaturep1.DataAccess.Repositories;
using RevatureP1.Models;
using RevatureP1.DataAccess.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using RevatureP1.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace RevatureP1.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ShoppingDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("P1Context"));
            });

            services.AddControllersWithViews();

            services.AddLogging(logger =>
            {
                Host.CreateDefaultBuilder()
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                });
            });

            //services.AddScoped<IRepository<Customer>, CustomerRepository>();
            //services.AddScoped<IRepository<Store>, StoreRepository>();
            //services.AddScoped<IRepository<Order>, OrderRepository>();
            //services.AddScoped<IRepository<Product>, ProductRepository>();
            //services.AddScoped<IRepository<Inventory>, InventoryRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.Cookie.HttpOnly = true;
                        options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                        options.LoginPath = "/Login/UserLogin";
                        //options.AccessDeniedPath = "/Account/AccessDenied";
                        //options.SlidingExpiration = true;
                    });


            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    "Admin",
                    policyBuilder => policyBuilder.RequireRole("Admin"));
                options.AddPolicy(
                    "Customer",
                    policyBuilder => policyBuilder.RequireRole("Customer")); 
                options.AddPolicy(
                    "LoggedIn",
                    policyBuilder => policyBuilder.RequireRole(new string[] { "Customer", "Admin" }));
            });

            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
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

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapRazorPages();
            });
        }
    }
}