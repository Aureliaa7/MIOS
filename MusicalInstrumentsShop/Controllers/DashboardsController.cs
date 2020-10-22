using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MusicalInstrumentsShop.Controllers
{
    public class DashboardsController : Controller
    {
        [Authorize(Roles = "Administrator")]
        public IActionResult Admin()
        {
            return View();
        }

        [Authorize(Roles = "Customer")]
        public IActionResult Customer()
        {
            return View();
        }
    }
}
