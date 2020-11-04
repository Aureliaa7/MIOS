using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicalInstrumentsShop.BusinessLogic.DTOs;
using MusicalInstrumentsShop.BusinessLogic.Exceptions;
using MusicalInstrumentsShop.BusinessLogic.Services.Interfaces;

namespace MusicalInstrumentsShop.Controllers
{
    public class StocksController : Controller
    {
        private readonly IStockService stockService;

        public StocksController(IStockService stockService)
        {
            this.stockService = stockService;
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(StockDto stockDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await stockService.AddProductsInStockAsync(stockDto);
                    return RedirectToAction("Index", "Products");
                }
                catch (ItemNotFoundException)
                {
                    return RedirectToAction("NotFound", "Error");
                }
            }
            return View(stockDto);
        }
    }
}
