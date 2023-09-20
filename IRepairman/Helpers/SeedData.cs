using IRepairman.Domain.Entities;
using IRepairman.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace IRepairman.Helpers
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            using (var scope = services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(Role.Admin.ToString()))
                {
                    var result = await roleManager.CreateAsync(new IdentityRole(Role.Admin.ToString()));
                    if (!result.Succeeded) throw new Exception(result.Errors.First().Description);
                }

                if (!await roleManager.RoleExistsAsync(Role.Master.ToString()))
                {
                    var result = await roleManager.CreateAsync(new IdentityRole(Role.Master.ToString()));
                    if (!result.Succeeded) throw new Exception(result.Errors.First().Description);
                }

                var user = await userManager.FindByEmailAsync("admin@admin.com");
                if (user is null)
                {
                    user = new AppUser
                    {
                        UserName = "admin@admin.com",
                        Email = "admin@admin.com",
                        FullName = "Admin",
                        CreatedTime = DateTime.Now,
                        EmailConfirmed = true,
                        Role = Role.Admin,
                    };
                    var result = await userManager.CreateAsync(user, "Admin12!");
                    if (!result.Succeeded) throw new Exception(result.Errors.First().Description);

                    result = await userManager.AddToRoleAsync(user, Role.Admin.ToString());
                }
            }
        }
    }
}
