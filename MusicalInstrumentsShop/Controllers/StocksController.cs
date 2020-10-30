﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MusicalInstrumentsShop.BusinessLogic.DTOs;
using MusicalInstrumentsShop.BusinessLogic.Services;

namespace MusicalInstrumentsShop.Controllers
{
    public class StocksController : Controller
    {
        private readonly IStockService stockService;

        public StocksController(IStockService stockService)
        {
            this.stockService = stockService;
        }

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
                await stockService.AddProductsInStock(stockDto);
                return RedirectToAction("Index", "Products");
            }
            return View(stockDto);
        }
    }
}
