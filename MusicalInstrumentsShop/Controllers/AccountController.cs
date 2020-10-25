using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MusicalInstrumentsShop.BusinessLogic.HelperEntities;
using MusicalInstrumentsShop.BusinessLogic.Services;
using MusicalInstrumentsShop.DataAccess.Entities;
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
        public async Task<IActionResult> Login(LoginModel loginInfo)
        {
            if (ModelState.IsValid)
            {
                var loginResult = await accountService.Login(loginInfo);
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
            }
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationModel registrationInfo)
        {
            if(ModelState.IsValid)
            {
                await accountService.Register(registrationInfo);
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
