using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MusicalInstrumentsShop.BusinessLogic.Exceptions;
using MusicalInstrumentsShop.BusinessLogic.Services;
using MusicalInstrumentsShop.BusinessLogic.DTOs;

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
            return View(await categoryService.GetAll());
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return RedirectToAction("NotFound", "Error");
            }

            try
            {
                return View(await categoryService.GetById((Guid)id));
            }
            catch(ItemNotFoundException)
            {
                Response.StatusCode = 404;
                return RedirectToAction("NotFound", "Error");
            }
        }

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
                await categoryService.Add(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return RedirectToAction("NotFound", "Error");
            }

            try
            {
                return View(await categoryService.GetById((Guid)id));
            }
            catch (ItemNotFoundException)
            {
                Response.StatusCode = 404;
                return RedirectToAction("NotFound", "Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CategoryDto category)
        {
            if (id != category.Id)
            {
                Response.StatusCode = 404;
                return RedirectToAction("NotFound", "Error");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await categoryService.Update(category);
                }
                catch (ItemNotFoundException)
                {
                    Response.StatusCode = 404;
                    return RedirectToAction("NotFound", "Error");
                }
                return RedirectToAction("Index", "Categories");
            }
            return View(category);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return RedirectToAction("NotFound", "Error");
            }

            try
            {
                return View(await categoryService.GetById((Guid)id));
            }
            catch (ItemNotFoundException)
            {
                Response.StatusCode = 404;
                return RedirectToAction("NotFound", "Error");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return RedirectToAction("NotFound", "Error");
            }

            try
            {
                await categoryService.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (ItemNotFoundException)
            {
                Response.StatusCode = 404;
                return RedirectToAction("NotFound", "Error");
            }
        }

        public JsonResult GetExistingCategories()
        {
            var categories = categoryService.GetAll().Result;
            return new JsonResult(categories);
        }
    }
}
