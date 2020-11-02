using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MusicalInstrumentsShop.BusinessLogic.DTOs;
using MusicalInstrumentsShop.BusinessLogic.Exceptions;
using MusicalInstrumentsShop.BusinessLogic.Services;

namespace MusicalInstrumentsShop.Controllers
{
    public class SpecificationsController : Controller
    {
        private readonly ISpecificationService specificationService;

        public SpecificationsController(ISpecificationService specificationService)
        {
            this.specificationService = specificationService;
        }

        public async Task<IActionResult> GetForProduct(string id)
        {
            try
            {
                return View(await specificationService.GetForProduct(id));
            }
            catch(ItemNotFoundException)
            {
                return RedirectToAction("NotFound", "Error");
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string id, SpecificationDto specification)
        {
            if (specification.Key != null && specification.Value != null && id != null)
            {
                specification.ProductId = id;
                await specificationService.Add(specification);
                return RedirectToAction("Details", "Products", new {id = specification.ProductId});
            }
            return View(specification);
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
                return View(await specificationService.GetById((Guid)id));
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
                await specificationService.Delete(id);
                return RedirectToAction("Index", "Products");
            }
            catch (ItemNotFoundException)
            {
                Response.StatusCode = 404;
                return RedirectToAction("NotFound", "Error");
            }
        }

    }
}
