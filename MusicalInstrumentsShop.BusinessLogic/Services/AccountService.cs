using Microsoft.AspNetCore.Identity;
using MusicalInstrumentsShop.BusinessLogic.DTOs;
using MusicalInstrumentsShop.DataAccess.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.BusinessLogic.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly LoginResult loginResult;

        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            loginResult = new LoginResult();
        }

        public async Task<LoginResult> Login(LoginDto loginInfo)
        {
            var result = await signInManager.PasswordSignInAsync(loginInfo.Email, loginInfo.Password, false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                loginResult.ErrorMessages = null;
                var user = await userManager.FindByEmailAsync(loginInfo.Email);
                var roles = await userManager.GetRolesAsync(user);
                loginResult.UserRole = roles.FirstOrDefault();
            }
            else
            {
                loginResult.ErrorMessages.Add("Invalid login attempt");
            }
            return loginResult;
        }

        public async Task<List<string>> Register(RegistrationDto registrationInfo)
        {
            List<string> errorMessages = new List<string>();
            ApplicationUser newApplicationUser = new ApplicationUser
            {
                FirstName = registrationInfo.FirstName,
                LastName = registrationInfo.LastName,
                Email = registrationInfo.Email,
                UserName = registrationInfo.Email,
                Country = registrationInfo.Country,
                City = registrationInfo.City,
                Street = registrationInfo.Street,
                ZipCode = registrationInfo.ZipCode
            };

            var result = await userManager.CreateAsync(newApplicationUser, registrationInfo.Password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(newApplicationUser, "Customer");
            }
            else
            {
                foreach (var errorMessage in result.Errors)
                {
                    errorMessages.Add(errorMessage.Description);
                }
            }
            return errorMessages;
        }
    }
}
