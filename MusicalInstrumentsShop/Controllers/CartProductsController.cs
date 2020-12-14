using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicalInstrumentsShop.BusinessLogic.DTOs;
using MusicalInstrumentsShop.BusinessLogic.Exceptions;
using MusicalInstrumentsShop.BusinessLogic.ProductFiltering;
using MusicalInstrumentsShop.BusinessLogic.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CartProductsController : Controller
    {
        private readonly ICartProductService cartProductService;

        public CartProductsController(ICartProductService cartProductService)
        {
            this.cartProductService = cartProductService;
        }

        public async Task<IActionResult> Index(int? pageNumber)
        {
            var cartProducts = await GetCartProducts();
            ViewBag.Total = cartProducts.Sum(x => x.Product.Price * x.NumberOfProducts);
            return View(PaginatedList<CartProductDto>.Create(cartProducts, pageNumber ?? 1, 8));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string id)
        {
            Guid currentUserId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var cartProductCreation = new CartProductCreationDto { UserId = currentUserId, ProductId = id, NumberOfProducts = 1 };
            await cartProductService.AddAsync(cartProductCreation);
            return RedirectToAction("Index", "CartProducts");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            try
            {
                await cartProductService.DeleteAsync(id);
                return RedirectToAction("Index", "CartProducts");
            }
            catch (ItemNotFoundException)
            {
                return RedirectToAction("NotFound", "Error");
            }
        }


        public JsonResult UpdateQuantity(string quantity, string id)
        {
            var cartProducts = GetCartProducts().Result;
            int intQuantity = Int16.Parse(quantity);
            bool updated = cartProductService.UpdateNumberOfProductsAsync(id, intQuantity).Result;
            if (updated)
            {
                var cartProductsNew = GetCartProducts().Result;
                ViewBag.Total = cartProductsNew.Sum(x => x.Product.Price * x.NumberOfProducts);
                return new JsonResult("updated");
            }
            else
            {
                return new JsonResult("not updated");
            }
        }

        public JsonResult UpdateTotalSum()
        {
            var cartProducts = GetCartProducts().Result;
            ViewBag.Total = cartProducts.Sum(x => x.Product.Price * x.NumberOfProducts);
            return new JsonResult(ViewBag.Total);
        }

        public JsonResult UpdateSubTotal(string quantity, string id)
        {
            var cartProducts = GetCartProducts().Result;
            int intQuantity = Int16.Parse(quantity);
            var cartProduct = cartProducts.Where(x => x.Product.Id == id).First();
            double subTotal = -1;
            if (cartProduct != null)
            {
                subTotal = intQuantity * cartProduct.Product.Price;
            }
            return new JsonResult(subTotal);
        }

        private async Task<IEnumerable<CartProductDto>> GetCartProducts()
        {
            Guid currentUserId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return await cartProductService.GetAllAsync(currentUserId);
        }
    }
}
