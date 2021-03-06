﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicalInstrumentsShop.BusinessLogic.DTOs;
using MusicalInstrumentsShop.BusinessLogic.Exceptions;
using MusicalInstrumentsShop.BusinessLogic.Services.Interfaces;

namespace MusicalInstrumentsShop.Controllers
{
    public class DeliveryMethodsController : Controller
    {
        private readonly IDeliveryService deliveryService;

        public DeliveryMethodsController(IDeliveryService deliveryService)
        {
            this.deliveryService = deliveryService;
        }


        public async Task<IActionResult> Index()
        {
            return View(await deliveryService.GetAllAsync());
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DeliveryMethodDto deliveryMethod)
        {
            if (ModelState.IsValid)
            {
                deliveryMethod.Id = Guid.NewGuid();
                await deliveryService.AddAsync(deliveryMethod);
                return RedirectToAction(nameof(Index));
            }
            return View(deliveryMethod);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            try
            {
                return View(await deliveryService.GetByIdAsync((Guid)id));
            }
            catch (ItemNotFoundException)
            {
                return RedirectToAction("NotFound", "Error");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            try
            {
                await deliveryService.DeleteAsync((Guid)id);
                return RedirectToAction(nameof(Index));
            }
            catch (ItemNotFoundException)
            {
                return RedirectToAction("NotFound", "Error");
            }
        }

        public JsonResult GetDeliveryMethods()
        {
            var deliveryMethods = deliveryService.GetAllAsync().Result;
            return new JsonResult(deliveryMethods.OrderBy(x => x.Method));
        }

        public JsonResult GetDeliveryById(string id)
        {
            var deliveryMethod = deliveryService.GetByIdAsync(new Guid(id)).Result;
            return new JsonResult(deliveryMethod);
        }
    }
}
