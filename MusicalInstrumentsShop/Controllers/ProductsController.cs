using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicalInstrumentsShop.BusinessLogic.ProductFilteringEntities;
using MusicalInstrumentsShop.BusinessLogic.DTOs;
using MusicalInstrumentsShop.BusinessLogic.Exceptions;
using MusicalInstrumentsShop.BusinessLogic.Services.Interfaces;
using MusicalInstrumentsShop.Caching;
using Microsoft.Extensions.Caching.Memory;

namespace MusicalInstrumentsShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService productService;
        private readonly IMemoryCache memoryCache;

        public ProductsController(IProductService productService, IMemoryCache memoryCache)
        {
            this.productService = productService;
            this.memoryCache = memoryCache;
        }

        public async Task<IActionResult> Browse([FromServices] IProductBrowsingService productFilterService, ProductsFilteringModel filteringModel, int? pageNumber = 1)
        {
            var productFilteringModel = await productFilterService.Filter(filteringModel, memoryCache, 4, pageNumber ?? 1);
            return View(productFilteringModel);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index([FromServices] IProductIndexService productIndexService, string sortOrder, string currentFilter, string searchString, int? pageNumber)
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
            var products = await productIndexService.OrderByCriteria(searchString, sortOrder, memoryCache);
            
            return View(PaginatedList<ProductDto>.Create(products, pageNumber ?? 1, 5));
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
                if (addProductModel.Photos != null && addProductModel.Photos.Count > 0)
                {
                    try
                    {
                        await productService.AddNewAsync(addProductModel);
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
                await productService.UpdateAsync(product);
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
                await productService.DeleteAsync(id);
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
            return new JsonResult(products.OrderBy(x => x.Name));
        }
    }
}
