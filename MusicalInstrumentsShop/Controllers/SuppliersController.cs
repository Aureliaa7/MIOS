using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MusicalInstrumentsShop.BusinessLogic.Exceptions;
using MusicalInstrumentsShop.BusinessLogic.Services;
using MusicalInstrumentsShop.DataAccess.Entities;

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
                return RedirectToAction("NotFound", "Errors");
            }

            try
            {
                return View(await supplierService.GetById((Guid)id));
            }
            catch (ItemNotFoundException)
            {
                return RedirectToAction("NotFound", "Errors");
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Address,Telephone")] Supplier supplier)
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
                return RedirectToAction("NotFound", "Errors");
            }

            try
            {
                return View(await supplierService.GetById((Guid)id));
            }
            catch (ItemNotFoundException)
            {
                return RedirectToAction("NotFound", "Errors");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Address,Telephone")] Supplier supplier)
        {
            if (id != supplier.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await supplierService.Update(supplier);
                }
                catch (ItemNotFoundException)
                {
                    return RedirectToAction("NotFound", "Errors");
                }
                return RedirectToAction("Index", "Suppliers");
            }
            return View(supplier);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                return View(await supplierService.GetById((Guid)id));
            }
            catch (ItemNotFoundException)
            {
                return RedirectToAction("NotFound", "Errors");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                await supplierService.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (ItemNotFoundException)
            {
                return RedirectToAction("NotFound", "Errors");
            }
        }
    }
}
