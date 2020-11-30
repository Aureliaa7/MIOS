using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicalInstrumentsShop.BusinessLogic.DTOs;
using MusicalInstrumentsShop.BusinessLogic.Exceptions;
using MusicalInstrumentsShop.BusinessLogic.Services.Interfaces;

namespace MusicalInstrumentsShop.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class SuppliersController : Controller
    {
        private readonly ISupplierService supplierService;

        public SuppliersController(ISupplierService supplierService)
        {
            this.supplierService = supplierService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await supplierService.GetAllAsync());
        }

        public async Task<IActionResult> Details(Guid? id)
        {

            if (id == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            try
            {
                return View(await supplierService.GetByIdAsync((Guid)id));
            }
            catch (ItemNotFoundException)
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
        public async Task<IActionResult> Create(SupplierDto supplier)
        {
            if (ModelState.IsValid)
            {
                await supplierService.AddAsync(supplier);
                return RedirectToAction(nameof(Index));
            }
            return View(supplier);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            try
            {
                return View(await supplierService.GetByIdAsync((Guid)id));
            }
            catch (ItemNotFoundException)
            {
                return RedirectToAction("NotFound", "Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, SupplierDto supplier)
        {
            if (id != supplier.Id)
            {
                return RedirectToAction("NotFound", "Error");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await supplierService.UpdateAsync(supplier);
                }
                catch (ItemNotFoundException)
                {
                    return RedirectToAction("NotFound", "Error");
                }
                return RedirectToAction("Index", "Suppliers");
            }
            return View(supplier);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            try
            {
                return View(await supplierService.GetByIdAsync((Guid)id));
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
                await supplierService.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (ItemNotFoundException)
            {
                return RedirectToAction("NotFound", "Error");
            }
        }

        public JsonResult GetExistingSuppliers()
        {
            var suppliers = supplierService.GetAllAsync().Result;
            return new JsonResult(suppliers.OrderBy(x => x.Name));
        }

        public JsonResult GetSupplierByProduct(string productId)
        {
            var supplier = supplierService.GetByProductAsync(productId).Result;
            return new JsonResult(supplier);
        }
    }
}
