using Microsoft.AspNetCore.Identity;
using MusicalInstrumentsShop.DataAccess.Entities;
using System;
using System.Linq;

namespace MusicalInstrumentsShop.DataAccess.Data
{
    public static class DatabaseSeeding
    {
        public static void AddRoles(RoleManager<ApplicationRole> roleManager)
        {
            if (roleManager.FindByNameAsync("Administrator").Result == null)
            {
                ApplicationRole administratorRole = new ApplicationRole
                {
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR"
                };
                IdentityResult resultAdmin = roleManager.CreateAsync(administratorRole).Result;
            }

            if (roleManager.FindByNameAsync("Customer").Result == null)
            {
                ApplicationRole customerRole = new ApplicationRole
                {
                    Name = "Customer",
                    NormalizedName = "CUSTOMER"
                };
                IdentityResult resultCustomer = roleManager.CreateAsync(customerRole).Result;
            }
        }

        public static void AddAdministrator(UserManager<ApplicationUser> userManager)
        {
            if (userManager.FindByNameAsync("admin@gmail.com").Result == null)
            {
                ApplicationUser administrator = new ApplicationUser
                {
                    Email = "admin@gmail.com",
                    FirstName = "David",
                    LastName = "Pop",
                    UserName = "admin@gmail.com",
                    NormalizedUserName = "DAVIDPOP@GMAIL.COM",
                    NormalizedEmail = "ADMIN@GMAIL.COM",
                    EmailConfirmed = true
                };
                IdentityResult resultAdministrator = userManager.CreateAsync(administrator, "Admin_miosce11@").Result;
                if (resultAdministrator.Succeeded)
                {
                    userManager.AddToRoleAsync(administrator, "Administrator").Wait();
                }
            }
        }

        public static void AddPaymentMethod(ApplicationDbContext context)
        {
            if(!context.PaymentMethods.Where(x => x.Name == "Cash on delivery").Any())
            {
                var paymentMethod = new PaymentMethod { Id = Guid.NewGuid(), Name = "Cash on delivery" };
                context.PaymentMethods.Add(paymentMethod);
                context.SaveChanges();
            }
        }
    }
}
