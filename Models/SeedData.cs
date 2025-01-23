using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserAuth.Data;

namespace UserAuth.Models
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            try
            {

                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var context = serviceProvider.GetRequiredService<UserAuthContext>();

                // Create roles if they don't exist
                string[] roleNames = { "Admin" };
                foreach (var roleName in roleNames)
                {
                    var roleExist = await roleManager.RoleExistsAsync(roleName);
                    if (!roleExist)
                    {
                        await roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }

                // Create user if not present
                var user = await userManager.FindByEmailAsync("userauth@gmail.com");

                if (user == null)
                {
                    user = new IdentityUser { UserName = "userauth@gmail.com", Email = "userauth@gmail.com" };
                    var result = await userManager.CreateAsync(user, "UserAuthApp_25");

                    if (result.Succeeded)
                    {
                        // Add the "Admin" role to the user
                        await userManager.AddToRoleAsync(user, "Admin");
                    }
                }
                else
                {
                    if (!await userManager.IsInRoleAsync(user, "Admin"))
                    {
                        // Add the "Admin" role to the user if not already assigned
                        await userManager.AddToRoleAsync(user, "Admin");
                    }
                }

                await context.SaveChangesAsync();

                // Create movies if they don't exist
                if (!context.Movie.Any())
                {
                    context.Movie.AddRange(
                        new Movie
                        {
                            Title = "When Harry Met Sally",
                            ReleaseDate = DateTime.Parse("1989-2-12"),
                            Genre = "Romantic Comedy",
                            Price = 7.99M,
                            Rating = "R",
                            UserId = user.Id
                        },
                        new Movie
                        {
                            Title = "Ghostbusters ",
                            ReleaseDate = DateTime.Parse("1984-3-13"),
                            Genre = "Comedy",
                            Price = 8.99M,
                            Rating = "R",
                            UserId = user.Id
                        },
                        new Movie
                        {
                            Title = "Ghostbusters 2",
                            ReleaseDate = DateTime.Parse("1986-2-23"),
                            Genre = "Comedy",
                            Price = 9.99M,
                            Rating = "R",
                            UserId = user.Id
                        },
                        new Movie
                        {
                            Title = "Rio Bravo",
                            ReleaseDate = DateTime.Parse("1959-4-15"),
                            Genre = "Western",
                            Price = 3.99M,
                            Rating = "R",
                            UserId = user.Id
                        }
                    );

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occured while seeding the data:\n{ex.Message}");
            }
        }
    }
}
