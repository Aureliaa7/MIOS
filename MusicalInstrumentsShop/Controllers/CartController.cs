using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicalInstrumentsShop.BusinessLogic.DTOs;
using MusicalInstrumentsShop.BusinessLogic.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.Controllers
{
    [Authorize]
    public class CartController : Controller
    { 

        public IActionResult Index()
        {
            var cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            ViewBag.Cart = cart;
            if (cart != null)
            {
                ViewBag.Total = cart.Sum(x => x.Product.Price * x.Quantity);
            }
            return View();
        }

        public int GetProductIdFromCart(string id)
        {
            List<Item> cartProducts = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            for(int iterator = 0; iterator < cartProducts.Count; iterator++)
            {
                if(cartProducts[iterator].Product.Id == id)
                {
                    return iterator;
                }
            }
            return -1;
        }

        public async Task<IActionResult> Add([FromServices] IProductService productService, string id)
        {
            var product = await productService.GetByIdAsync(id);

            if(SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart") == null)
            {
                List<Item> cart = new List<Item>();
                cart.Add(new Item { Product = product, Quantity = 1 });
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
                int index = GetProductIdFromCart(id);
                if(index != -1)
                {
                    cart[index].Quantity++;
                }
                else
                {
                    cart.Add(new Item { Product = product, Quantity = 1 });  
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Remove(string id)
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            int index = GetProductIdFromCart(id);
            cart.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("Index");
        }

        public IActionResult UpdateQuantity(string id)
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            int index = GetProductIdFromCart(id);
            cart.ElementAt(index).Quantity++;
            ViewBag.QuantityUpdated = "Product quantity updated";
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("Index");
        }
    }
}
