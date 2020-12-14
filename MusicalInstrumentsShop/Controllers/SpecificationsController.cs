using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicalInstrumentsShop.BusinessLogic.DTOs;
using MusicalInstrumentsShop.BusinessLogic.Exceptions;
using MusicalInstrumentsShop.BusinessLogic.Services.Interfaces;

namespace MusicalInstrumentsShop.Controllers
{
    public class SpecificationsController : Controller
    {
        private readonly ISpecificationService specificationService;

        public SpecificationsController(ISpecificationService specificationService)
        {
            this.specificationService = specificationService;
        }

        [Authorize]
        public async Task<IActionResult> GetForProduct(string id)
        {
            try
            {
                return View(await specificationService.GetForProductAsync(id));
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
        public async Task<IActionResult> Create(string id, SpecificationDto specification)
        {
            if (specification.Key != null && specification.Value != null && id != null)
            {
                specification.ProductId = id;
                await specificationService.AddAsync(specification);
                return RedirectToAction("Details", "Products", new {id = specification.ProductId});
            }
            return View(specification);
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
                return View(await specificationService.GetByIdAsync((Guid)id));
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
                await specificationService.DeleteAsync(id);
                return RedirectToAction("Index", "Products");
            }
            catch (ItemNotFoundException)
            {
                return RedirectToAction("NotFound", "Error");
            }
        }
    }
}
