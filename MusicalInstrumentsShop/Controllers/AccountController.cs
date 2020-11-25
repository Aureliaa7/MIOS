using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MusicalInstrumentsShop.BusinessLogic.DTOs;
using MusicalInstrumentsShop.BusinessLogic.Exceptions;
using MusicalInstrumentsShop.BusinessLogic.Services.Interfaces;
using MusicalInstrumentsShop.DataAccess.Entities;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService accountService;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(IAccountService accountService, SignInManager<ApplicationUser> signInManager)
        {
            this.accountService = accountService;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginInfo)
        {
            if (ModelState.IsValid)
            {
                var loginResult = await accountService.LoginAsync(loginInfo);
                if (loginResult.ErrorMessages == null)
                {
                    if (loginResult.UserRole.Equals("Administrator"))
                    {
                        return RedirectToAction("Admin", "Dashboards");
                    }
                    else if (loginResult.UserRole.Equals("Customer"))
                    {
                        return RedirectToAction("Customer", "Dashboards");
                    }
                }
                ViewBag.ErrorMessages = loginResult.ErrorMessages;
            }
            return View(loginInfo);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromServices] IWishlistService wishlistService, RegistrationDto registrationInfo)
        {
            if (ModelState.IsValid)
            {
                var registerResult = await accountService.RegisterAsync(registrationInfo);
                if (registerResult.Capacity == 0)
                {
                    await wishlistService.CreateAsync(registrationInfo.Email);
                    return RedirectToAction("Login", "Account");
                }
                ViewBag.RegisterResult = registerResult;
            }
            return View(registrationInfo);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            try
            {
                Guid currentUserId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var accountInfo = await accountService.GetAccountInfoAsync(currentUserId);
                return View(accountInfo);
            }
            catch (ItemNotFoundException) {
                return RedirectToAction("NotFound", "Error");
            }
        }


        [Authorize]
        public async Task<IActionResult> Edit()
        {
            Guid currentUserId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
            try
            {
                var accountInfo = await accountService.GetAccountInfoAsync(currentUserId);
                return View(accountInfo);
            }
            catch (ItemNotFoundException)
            {
                return RedirectToAction("NotFound", "Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AccountInfoDto accountInfo)
        {
            Guid currentUserId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
            try
            {
                await accountService.EditAsync(currentUserId, accountInfo);
                return RedirectToAction("Profile", "Account");
            }
            catch(ItemNotFoundException)
            {
                return RedirectToAction("NotFound", "Error");
            }
        }


        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(PasswordChangeDto passwordDto)
        {
            if (ModelState.IsValid)
            {
                Guid currentUserId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
                passwordDto.UserId = currentUserId;
                try
                {
                    var result = await accountService.ChangePasswordAsync(passwordDto);
                    TempData["Result"] = result;
                    return RedirectToAction("ChangePasswordResult", "Account");
                }
                catch (ItemNotFoundException)
                {
                    return RedirectToAction("NotFound", "Error");
                }
            }
            return View();
        }


        [Authorize]
        public IActionResult ChangePasswordResult()
        {
            ViewBag.Result = TempData["Result"];
            return View();
        }
    }
}
