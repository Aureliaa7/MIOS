﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicalInstrumentsShop.BusinessLogic.DTOs;
using MusicalInstrumentsShop.BusinessLogic.Exceptions;
using MusicalInstrumentsShop.BusinessLogic.ProductFilteringEntities;
using MusicalInstrumentsShop.BusinessLogic.Services.Interfaces;
using MusicalInstrumentsShop.DataAccess.Entities;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.Controllers
{
    [Authorize(Roles = "Customer")]
    public class OrdersController : Controller
    {
        private readonly IOrderDetailsService orderService;

        public OrdersController(IOrderDetailsService orderService)
        {
            this.orderService = orderService;
        }

        public async Task<IActionResult> Index(int? pageNumber)
        {
            Guid currentUserId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var orders = await orderService.GetAllAsync(currentUserId);
            return View(PaginatedList<OrderDetailsDto>.Create(orders, pageNumber ?? 1, 5));
        }

        public async Task<IActionResult> Canceled(int? pageNumber)
        {
            Guid currentUserId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var orders = await orderService.GetByStatusAsync(currentUserId, OrderStatus.Canceled);
            return View(PaginatedList<OrderDetailsDto>.Create(orders, pageNumber ?? 1, 5));
        }

        public async Task<IActionResult> InProgress(int? pageNumber)
        {
            Guid currentUserId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var orders = await orderService.GetByStatusAsync(currentUserId, OrderStatus.InProgress);
            return View(PaginatedList<OrderDetailsDto>.Create(orders, pageNumber ?? 1, 5));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderDetailsDto orderDetails)
        {
            if (ModelState.IsValid)
            {
                orderDetails.CustomerId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
                await orderService.AddAsync(orderDetails);
                return RedirectToAction("Index", "Orders");
            }
            return View();
        }

        public async Task<IActionResult> Details(long id)
        {
            try
            {
                var order = await orderService.GetByIdAsync(id);
                return View(order);
            }
            catch(ItemNotFoundException)
            {
                return RedirectToAction("NotFound", "Error");
            }
        }

        public async Task<IActionResult> OrderedProducts(long id)
        {
            try
            {
                var order = await orderService.GetByIdAsync(id);
                return View(order);
            }
            catch (ItemNotFoundException)
            {
                return RedirectToAction("NotFound", "Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(long id)
        {
            try
            {
                await orderService.CancelAsync(id);
                return RedirectToAction("Details", new { id = id });
            }
            catch (ItemNotFoundException)
            {
                return RedirectToAction("NotFound", "Error");
            }
        }
    }
}
