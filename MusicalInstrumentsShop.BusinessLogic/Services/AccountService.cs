﻿using Microsoft.AspNetCore.Identity;
using MusicalInstrumentsShop.BusinessLogic.DTOs;
using MusicalInstrumentsShop.BusinessLogic.Exceptions;
using MusicalInstrumentsShop.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MusicalInstrumentsShop.BusinessLogic.Services.Interfaces;

namespace MusicalInstrumentsShop.BusinessLogic.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly LoginResult loginResult;
        private readonly IWishlistService wishlistService;
        private readonly ICartService cartService;

        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            IWishlistService wishlistService, ICartService cartService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            loginResult = new LoginResult();
            this.wishlistService = wishlistService;
            this.cartService = cartService;
        }

        public async Task<string> ChangePasswordAsync(PasswordChangeDto passwordDto)
        {
            var user = await userManager.FindByIdAsync(passwordDto.UserId.ToString());
            if (user != null)
            {
                bool correctCurrentPassword = await userManager.CheckPasswordAsync(user, passwordDto.CurrentPassword);
                if (correctCurrentPassword)
                {
                    var result = await userManager.ChangePasswordAsync(user, passwordDto.CurrentPassword, passwordDto.NewPassword);
                    if (result.Succeeded)
                    {
                        return "Password changed successfully";
                    }
                }
                return "Something went wrong...";
            }
            throw new ItemNotFoundException("The user was not found...");
        }

        public async Task EditAsync(Guid userId, AccountInfoDto accountInfo)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user != null)
            {
                user.FirstName = accountInfo.FirstName;
                user.LastName = accountInfo.LastName;
                await userManager.UpdateAsync(user);
            }
            else
            {
                throw new ItemNotFoundException("The user was not found");
            }
        }

        public async Task<AccountInfoDto> GetAccountInfoAsync(Guid userId)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user != null)
            {
                var roles = await userManager.GetRolesAsync(user);
                var accountInfo = new AccountInfoDto
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Role = roles.First()
                };
                return accountInfo;
            }
            throw new ItemNotFoundException("The user was not found...");
        }

        public async Task<LoginResult> LoginAsync(LoginDto loginInfo)
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

        public async Task<List<string>> RegisterAsync(RegistrationDto registrationInfo)
        {
            List<string> errorMessages = new List<string>();
            ApplicationUser newApplicationUser = new ApplicationUser
            {
                FirstName = registrationInfo.FirstName,
                LastName = registrationInfo.LastName,
                Email = registrationInfo.Email,
                UserName = registrationInfo.Email
            };

            var result = await userManager.CreateAsync(newApplicationUser, registrationInfo.Password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(newApplicationUser, "Customer");
                await wishlistService.CreateAsync(registrationInfo.Email);
                await cartService.CreateAsync(registrationInfo.Email);
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
