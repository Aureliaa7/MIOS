using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicalInstrumentsShop.BusinessLogic.DTOs;
using MusicalInstrumentsShop.BusinessLogic.Exceptions;
using MusicalInstrumentsShop.BusinessLogic.Services;
using MusicalInstrumentsShop.DataAccess.Entities;

namespace MusicalInstrumentsShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService productService;
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly IImageService imageService;

        public ProductsController(IProductService productService, IWebHostEnvironment hostEnvironment, IImageService imageService)
        {
            this.productService = productService;
            this.hostEnvironment = hostEnvironment;
            this.imageService = imageService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await productService.GetAll());
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }
            try
            {
                var product = await productService.GetById(id);
                return View(product);
            }
            catch(ItemNotFoundException)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddProductDto addProductModel)
        {
            if (ModelState.IsValid)
            {  
                IEnumerable<Photo> photos = new List<Photo>();
                if (addProductModel.Photos != null && addProductModel.Photos.Count > 0)
                {
                    photos = imageService.SaveFiles(addProductModel.Photos);
                    await productService.AddNew(addProductModel, photos);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(addProductModel);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }
            try
            {
                return View(await productService.GetForUpdate(id));
            }
            catch(ItemNotFoundException)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, UpdateProductDto product)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }

            if (ModelState.IsValid)
            {
                IEnumerable<Photo> photos = new List<Photo>();
                photos = imageService.SaveFiles(product.Photos);
                var fileNames = await productService.Update(product, photos);
                if (fileNames != null) {
                    imageService.DeleteFiles(fileNames);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }
            try {
                return View(await productService.GetById(id));
            }
            catch (ItemNotFoundException)
            {
                Response.StatusCode = 404;
                return View("/Error/NotFound");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                var fileNames = await productService.Delete(id);
                imageService.DeleteFiles(fileNames);

                return RedirectToAction(nameof(Index));
            }
            catch (ItemNotFoundException)
            {
                Response.StatusCode = 404;
                return View("/Error/NotFound");
            }
        }

        public JsonResult GetByCategory(string categoryId)
        {
            var products = productService.GetByCategory(new Guid(categoryId)).Result;
            return new JsonResult(products);
        }
    }
}
