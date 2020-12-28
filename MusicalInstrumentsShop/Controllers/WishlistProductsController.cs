using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicalInstrumentsShop.BusinessLogic.DTOs;
using MusicalInstrumentsShop.BusinessLogic.Exceptions;
using MusicalInstrumentsShop.BusinessLogic.ProductFilteringEntities;
using MusicalInstrumentsShop.BusinessLogic.Services.Interfaces;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.Controllers
{
    [Authorize(Roles = "Customer")]
    public class WishlistProductsController : Controller
    {
        private readonly IWishlistProductService wishlistService;

        public WishlistProductsController(IWishlistProductService wishlistService)
        {
            this.wishlistService = wishlistService;
        }

        public async Task<IActionResult> Index(int? pageNumber)
        {
            Guid currentUserId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var wishlistProducts = await wishlistService.GetAllAsync(currentUserId);
            return View(PaginatedList<WishlistProductDto>.Create(wishlistProducts, pageNumber ?? 1, 4));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string id)
        {
            Guid currentUserId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var wishlistProductCreation = new WishlistProductCreationDto { UserId = currentUserId, ProductId = id };
            try
            {
                await wishlistService.AddAsync(wishlistProductCreation);
                return RedirectToAction("Index", "WishlistProducts");
            }
            catch (ItemNotFoundException)
            {
                return RedirectToAction("NotFound", "Error");
            }
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
                await wishlistService.DeleteAsync(id);
                return RedirectToAction("Index", "WishlistProducts");
            }
            catch (ItemNotFoundException)
            {
                return RedirectToAction("NotFound", "Error");
            }
        }
    }
}
