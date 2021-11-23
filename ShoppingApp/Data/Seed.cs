using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShoppingApp.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShoppingApp.Data
{
    public class Seed
    {
        public static async Task Seedusers(UserManager<AppUser> userManager,
                                           RoleManager<AppRole> roleManager)
        {
            if (await userManager.Users.AnyAsync()) return;

            var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);
            if (userManager == null) return;

            var roles = new List<AppRole>
            {
                new AppRole{Name = "Buyer"},
                new AppRole{Name = "GoldBuyer"},
                new AppRole{Name = "Supplier"},
                new AppRole{Name = "GoldSupplier"}
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            foreach (var user in users)
            {
                user.UserName = user.UserName;

                await userManager.CreateAsync(user, "Selvam1");

                await userManager.AddToRoleAsync(user, "Buyer");
            }

            var supplier = new AppUser
            {
                UserName = "SelvakumarSp",
                UserRole = "Supplier",
                DateOfBirth = Convert.ToDateTime("2000-01-03"),
                Gender = "Male",
                Fullname = "Selvakumar",
                Email = "sk2selva312000@gmail.com",
                PhoneNumber = "6374100161",
                City = "Madurai",
                State = "Tamilnadu",
                Country = "India"
            };

            await userManager.CreateAsync(supplier, "Selvam1");
            await userManager.AddToRoleAsync(supplier, "Supplier");
        }
    }
}
