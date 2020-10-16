using Microsoft.AspNetCore.Mvc;

namespace MusicalInstrumentsShop.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
