using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QR_Code_Reader.Models.DAL;

namespace QR_Code_Reader
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddDbContext<MyContext>(options =>
            {
                options.UseSqlServer(_configuration["ConnectionStrings:Default"]);
            });
            services.AddIdentity<IdentityUser, IdentityRole>(options => options.Stores.MaxLengthForKeys = 128)
                  .AddEntityFrameworkStores<MyContext>()
                  .AddDefaultTokenProviders();

            services.AddControllersWithViews();
        }




        //create admin role
        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            //initializing custom roles 
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            string roleName = "Admin";
            IdentityResult roleResult;

            //foreach (var roleName in roleNames)
            //{
            var roleExist = await RoleManager.RoleExistsAsync(roleName);
            // ensure that the role does not exist
            if (!roleExist)
            {
                //create the roles and seed them to the database: 
                roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
            }
            //}

            // find the user with the admin email 
            var _user = await UserManager.FindByEmailAsync("admin@gmail.com");

            // check if the user exists
            if (_user == null)
            {
                //Here you could create the super admin who will maintain the web app
                var poweruser = new IdentityUser
                {
                    UserName = "Admin",
                    Email = "admin@gmail.com",
                };
                string adminPassword = "Admin123@@";

                var createPowerUser = await UserManager.CreateAsync(poweruser, adminPassword);
                if (createPowerUser.Succeeded)
                {
                    //here we tie the new user to the role
                    await UserManager.AddToRoleAsync(poweruser, "Admin");

                }
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=QRCode}/{action=Index}/{id?}");

                
                //endpoints.MapAreaControllerRoute(1
                //name: "areas", "WebCms",
                //pattern: "{area:exists}/{controller=Account}/{action=Login}/{id?}");

            });

            Task.Run(() => CreateRoles(serviceProvider)).Wait();

        }
    }
}
