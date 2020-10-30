using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MusicalInstrumentsShop.BusinessLogic.DTOs;
using MusicalInstrumentsShop.BusinessLogic.Exceptions;
using MusicalInstrumentsShop.BusinessLogic.Services;

namespace MusicalInstrumentsShop.Controllers
{
    public class SuppliersController : Controller
    {
        private readonly ISupplierService supplierService;

        public SuppliersController(ISupplierService supplierService)
        {
            this.supplierService = supplierService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await supplierService.GetAll());
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
                return View(await supplierService.GetById((Guid)id));
            }
            catch (ItemNotFoundException)
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
        public async Task<IActionResult> Create(SupplierDto supplier)
        {
            if (ModelState.IsValid)
            {
                await supplierService.Add(supplier);
                return RedirectToAction(nameof(Index));
            }
            return View(supplier);
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
                return View(await supplierService.GetById((Guid)id));
            }
            catch (ItemNotFoundException)
            {
                Response.StatusCode = 404;
                return RedirectToAction("NotFound", "Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, SupplierDto supplier)
        {
            if (id != supplier.Id)
            {
                Response.StatusCode = 404;
                return RedirectToAction("NotFound", "Error");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await supplierService.Update(supplier);
                }
                catch (ItemNotFoundException)
                {
                    Response.StatusCode = 404;
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
                Response.StatusCode = 404;
                return RedirectToAction("NotFound", "Error");
            }

            try
            {
                return View(await supplierService.GetById((Guid)id));
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
                await supplierService.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (ItemNotFoundException)
            {
                Response.StatusCode = 404;
                return RedirectToAction("NotFound", "Error");
            }
        }

        public JsonResult GetExistingSuppliers()
        {
            var suppliers = supplierService.GetAll().Result;
            return new JsonResult(suppliers);
        }

        public JsonResult GetSupplierByProduct(string productId)
        {
            var supplier = supplierService.GetByProduct(productId).Result;
            return new JsonResult(supplier);
        }
    }
}
