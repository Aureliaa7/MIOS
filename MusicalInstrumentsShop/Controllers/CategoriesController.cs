using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MusicalInstrumentsShop.BusinessLogic.Exceptions;
using MusicalInstrumentsShop.BusinessLogic.Services.Interfaces;
using MusicalInstrumentsShop.BusinessLogic.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace MusicalInstrumentsShop.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await categoryService.GetAllAsync());
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            try
            {
                return View(await categoryService.GetByIdAsync((Guid)id));
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
        public async Task<IActionResult> Create(CategoryDto category)
        {
            if (ModelState.IsValid)
            {
                category.Id = Guid.NewGuid();
                await categoryService.AddAsync(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            try
            {
                return View(await categoryService.GetByIdAsync((Guid)id));
            }
            catch (ItemNotFoundException)
            {
                return RedirectToAction("NotFound", "Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CategoryDto category)
        {
            if (id != category.Id)
            {
                return RedirectToAction("NotFound", "Error");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await categoryService.UpdateAsync(category);
                }
                catch (ItemNotFoundException)
                {
                    return RedirectToAction("NotFound", "Error");
                }
                return RedirectToAction("Index", "Categories");
            }
            return View(category);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            try
            {
                return View(await categoryService.GetByIdAsync((Guid)id));
            }
            catch (ItemNotFoundException)
            {
                return RedirectToAction("NotFound", "Error");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            try
            {
                await categoryService.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (ItemNotFoundException)
            {
                return RedirectToAction("NotFound", "Error");
            }
        }

        public JsonResult GetExistingCategories()
        {
            var categories = categoryService.GetAllAsync().Result;
            return new JsonResult(categories);
        }
    }
}
