using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TicketOffice.Database;
using TicketOffice.Domain.Repositories;
using TicketOffice.Domain.Services;
using TicketOffice.Infrastructure.Implementation;
using TicketOffice.Services;

namespace TicketOffice.Web
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
            services.AddDbContext<TicketOfficeDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<TicketOfficeDbContext>();
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddTransient<IShowRepository, ShowRepository>();
            services.AddTransient<ITicketRepository, TicketRepository>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<ITicketService, TicketService>();
            services.AddTransient<IShowService, ShowService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, SignInManager<IdentityUser> s, RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            if (roleManager.FindByNameAsync("Admin").Result == null)
            {
                var result = roleManager.CreateAsync(new IdentityRole() { Name = "Admin" }).Result;
                result = roleManager.CreateAsync(new IdentityRole() { Name = "User" }).Result;
                result = roleManager.CreateAsync(new IdentityRole() { Name = "Guest" }).Result;
            }

            if (s.UserManager.FindByNameAsync("admin@admin.com").Result == null)
            {
                var result = s.UserManager.CreateAsync(new IdentityUser
                {
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com",
                    EmailConfirmed = true
                }, "G12mnu!st").Result;

                s.UserManager.AddToRoleAsync(s.UserManager.FindByNameAsync("admin@admin.com").Result, "Admin");
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
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
