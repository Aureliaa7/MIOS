using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicalInstrumentsShop.BusinessLogic.DTOs;
using MusicalInstrumentsShop.BusinessLogic.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IStockService stockService;

        public CartController(IStockService stockService)
        {
            this.stockService = stockService;
        }

        public IActionResult Index()
        {
            var cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "Cart");
            ViewBag.Cart = cart;
            if (cart != null)
            {
                ViewBag.Total = cart.Sum(x => x.Product.Price * x.Quantity);
            }
            return View();
        }

        private int GetProductIndexFromCart(string id)
        {
            List<Item> cartProducts = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "Cart");
            for (int iterator = 0; iterator < cartProducts.Count; iterator++)
            {
                if (cartProducts[iterator].Product.Id == id)
                {
                    return iterator;
                }
            }
            return -1;
        }

        public async Task<IActionResult> Add([FromServices] IProductService productService, string id)
        {
            var product = await productService.GetByIdAsync(id);

            if (SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "Cart") == null)
            {
                List<Item> cart = new List<Item>();
                if (product.NumberOfProducts > 0)
                {
                    cart.Add(new Item { Product = product, Quantity = 1 });
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "Cart", cart);
                }
            }
            else
            {
                List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "Cart");
                int index = GetProductIndexFromCart(id);
                if (index != -1)
                {
                    cart[index].Quantity++;
                }
                else
                {
                    cart.Add(new Item { Product = product, Quantity = 1 });
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "Cart", cart);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Remove(string id)
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "Cart");
            int index = GetProductIndexFromCart(id);
            cart.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "Cart", cart);
            return RedirectToAction("Index");
        }

        public JsonResult UpdateQuantity(string quantity, string id)
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "Cart");
            int index = GetProductIndexFromCart(id);
            int intQuantity = Int16.Parse(quantity);
            int quantityDifference = intQuantity - cart.ElementAt(index).Quantity;
            if (intQuantity > 0 && (quantityDifference > 0)
                && stockService.CanTakeAsync(quantityDifference, id).Result)
            {
                cart.ElementAt(index).Quantity = intQuantity;
                ViewBag.Total = cart.Sum(x => x.Product.Price * x.Quantity);
                SessionHelper.SetObjectAsJson(HttpContext.Session, "Cart", cart);
                return new JsonResult("updated");
            }
            else
            {
                return new JsonResult("not updated");
            }
        }

        public JsonResult UpdateTotalSum()
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "Cart");
            ViewBag.Total = cart.Sum(x => x.Product.Price * x.Quantity);
            return new JsonResult(ViewBag.Total);
        }
    }
}
