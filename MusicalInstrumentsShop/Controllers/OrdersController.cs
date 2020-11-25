using Microsoft.AspNetCore.Mvc;
using MusicalInstrumentsShop.BusinessLogic.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace MusicalInstrumentsShop.Controllers
{
    public class OrdersController : Controller
    {
        public IActionResult Index()
        {
            var cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            ViewBag.cart = cart;
            ViewBag.total = cart.Sum(x => x.Product.Price * x.Quantity);
            return View();
        }
    }
}
