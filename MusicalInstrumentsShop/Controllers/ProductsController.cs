using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MusicalInstrumentsShop.BusinessLogic.DTOs;
using MusicalInstrumentsShop.BusinessLogic.Exceptions;
using MusicalInstrumentsShop.BusinessLogic.Services.Interfaces;
using MusicalInstrumentsShop.DataAccess.Entities;
using ReflectionIT.Mvc.Paging;

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

        // browse products
        public async Task<IActionResult> Browse(int? pageNumber = 1)
        {
            var products = await productService.GetAllAsync();
            int pageSize = 6;
            return View(PaginatedList<ProductDto>.Create(products, pageNumber ?? 1, pageSize));
        }

        //TODO heree- sorting functionality
        [Authorize]
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["CategorySortParm"] = sortOrder == "CategoryName" ? "category_desc" : "CategoryName";
            ViewData["SupplierSortParm"] = sortOrder == "SupplierName" ? "supplier_desc" : "SupplierName";
            ViewData["CodeSortParm"] = sortOrder == "Id" ? "id_desc" : "Id";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var products = await productService.GetAllAsync();

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(x => x.CategoryName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    products = products.OrderByDescending(x => x.Name);
                    break;
                case "CategoryName":
                    products = products.OrderBy(x => x.CategoryName);
                    break;
                case "category_desc":
                    products = products.OrderByDescending(s => s.CategoryName);
                    break;
                case "SupplierName":
                    products = products.OrderBy(x => x.SupplierName);
                    break;
                case "supplier_desc":
                    products = products.OrderByDescending(s => s.SupplierName);
                    break;
                case "Id":
                    products = products.OrderBy(x => x.Id);
                    break;
                case "id_desc":
                    products = products.OrderByDescending(s => s.Id);
                    break;
                default:
                    products = products.OrderBy(s => s.Name);
                    break;
            }
            int pageSize = 4;
            return View(PaginatedList<ProductDto>.Create(products, pageNumber ?? 1, pageSize));
        }

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
                IEnumerable<string> fileNames = new List<string>();
                if (product.Photos != null && product.PhotoOption != PhotoOption.KeepCurrentPhotos)
                {
                    photos = imageService.SaveFiles(product.Photos);
                }
                fileNames = await productService.UpdateAsync(product, photos);
                if (fileNames.Count() > 0 && product.PhotoOption == PhotoOption.DeleteCurrentPhotos)
                {
                    imageService.DeleteFiles(fileNames);
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

        public JsonResult GetByCategory(string categoryId)
        {
            var products = productService.GetByCategoryAsync(new Guid(categoryId)).Result;
            return new JsonResult(products);
        }
    }
}
