using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UserAuth.Data;
using UserAuth.Models;
namespace UserAuth
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<UserAuthContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("UserAuthContext") 
                    ?? throw new InvalidOperationException("Connection string 'UserAuthContext' not found."));
            });

            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddDefaultTokenProviders()
                .AddDefaultUI()
                .AddEntityFrameworkStores<UserAuthContext>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Add Razor Pages support
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<UserAuthContext>();
                context.Database.Migrate(); // Applies any pending migrations or creates the database if it doesn't exist
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // Map Razor Pages for Identity
            app.MapRazorPages();

            /* When I browse to the app and don't supply any URL segments, 
             * it defaults to the "Home" controller and the "Index" method specified in the template line highlighted below */
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // Map a custom route for the Welcome action
            app.MapControllerRoute(
                name: "welcomeRoute",
                pattern: "HelloWorld/Welcome/{name}/{id?}",
                defaults: new { controller = "HelloWorld", action = "Welcome" });

            // initialize database
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                await SeedData.Initialize(services);
            }

            app.Run();
        }
    }
}
