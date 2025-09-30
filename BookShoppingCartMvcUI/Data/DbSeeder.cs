using Microsoft.AspNetCore.Identity;
using BookShoppingCartMvcUI.Constants;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace BookShoppingCartMvcUI.Data
{
    public class DbSeeder
    {
        public static async Task SeedDefaultData(IServiceProvider service)
        {
            var userMgr = service.GetRequiredService<UserManager<IdentityUser>>();
            var roleMgr = service.GetRequiredService<RoleManager<IdentityRole>>();
            var config = service.GetRequiredService<IConfiguration>();

            // 1️ Create roles if not exist
            if (!await roleMgr.RoleExistsAsync("Admin"))
                await roleMgr.CreateAsync(new IdentityRole("Admin"));

            if (!await roleMgr.RoleExistsAsync("User"))
                await roleMgr.CreateAsync(new IdentityRole("User"));

            // 2  Get Email & Password from appsettings.json 
            var adminEmail = config["AdminUser:Email"];
            var adminPassword = config["AdminUser:Password"];

            // 3. check if admin user already exists,Create default Admin user
            var admin = await userMgr.FindByEmailAsync(adminEmail);
            if (admin == null)
            {
                var newAdmin = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userMgr.CreateAsync(newAdmin, adminPassword);
                if (result.Succeeded)
                {
                    await userMgr.AddToRoleAsync(newAdmin, "Admin");
                }
            }
        }
    }
}
