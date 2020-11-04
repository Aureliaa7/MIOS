using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MusicalInstrumentsShop.BusinessLogic.DTOs;
using MusicalInstrumentsShop.BusinessLogic.Exceptions;
using MusicalInstrumentsShop.BusinessLogic.Services.Interfaces;
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

        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await productService.GetAllAsync());
        }

        [Authorize]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            try
            {
                var product = await productService.GetByIdAsync(id);
                return View(product);
            }
            catch(ItemNotFoundException)
            {
                return RedirectToAction("NotFound", "Error");
            }
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreationDto addProductModel)
        {
            if (ModelState.IsValid)
            {  
                IEnumerable<Photo> photos = new List<Photo>();
                if (addProductModel.Photos != null && addProductModel.Photos.Count > 0)
                {
                    try
                    {
                        photos = imageService.SaveFiles(addProductModel.Photos);
                        await productService.AddNewAsync(addProductModel, photos);
                    } 
                    catch(ProductAlreadyExistsException e)
                    {
                        ViewBag.ErrorMessage = e.Message;
                        return View(addProductModel);
                    }
                }
                return RedirectToAction("Create", "Specifications", new { id = addProductModel.Id });
            }
            return View(addProductModel);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            try
            {
                return View(await productService.GetForUpdateAsync(id));
            }
            catch(ItemNotFoundException)
            {
                return RedirectToAction("NotFound", "Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ProductEditingDto product)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            if (ModelState.IsValid)
            {
                IEnumerable<Photo> photos = new List<Photo>();
                if (product.Photos != null)
                {
                    photos = imageService.SaveFiles(product.Photos);
                    var fileNames = await productService.UpdateAsync(product, photos);
                    if (fileNames != null)
                    {
                        imageService.DeleteFiles(fileNames);
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            try {
                return View(await productService.GetByIdAsync(id));
            }
            catch (ItemNotFoundException)
            {
                return RedirectToAction("NotFound", "Error");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                var fileNames = await productService.DeleteAsync(id);
                imageService.DeleteFiles(fileNames);

                return RedirectToAction(nameof(Index));
            }
            catch (ItemNotFoundException)
            {
                return RedirectToAction("NotFound", "Error");
            }
        }

        public IActionResult FilterByCategory()
        {
            return View();
        }

        public async Task<IActionResult> FilteredByCategory(Guid id)
        {
            try
            {
                var products = await productService.GetByCategoryAsync(id);
                return View(products);
            }
            catch(ItemNotFoundException)
            {
                return RedirectToAction("NotFound", "Error");
            }
        }

        public JsonResult GetByCategory(string categoryId)
        {
            var products = productService.GetByCategoryAsync(new Guid(categoryId)).Result;
            return new JsonResult(products);
        }
    }
}
