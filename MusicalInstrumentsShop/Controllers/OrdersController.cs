using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicalInstrumentsShop.BusinessLogic.DTOs;
using MusicalInstrumentsShop.BusinessLogic.Exceptions;
using MusicalInstrumentsShop.BusinessLogic.ProductFiltering;
using MusicalInstrumentsShop.BusinessLogic.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderDetailsService orderService;
        private readonly IPaymentService paymentService;

        public OrdersController(IOrderDetailsService orderService, IPaymentService paymentService)
        {
            this.orderService = orderService;
            this.paymentService = paymentService;
        }

        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Index(int? pageNumber)
        {
            Guid currentUserId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var orders = await orderService.GetAllAsync(currentUserId);
            return View(PaginatedList<OrderDetailsDto>.Create(orders, pageNumber ?? 1, 5));
        }

        [Authorize(Roles = "Customer")]
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
                orderDetails.Items = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
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

        public JsonResult GetPaymentMethods()
        {
            var paymentMethods = paymentService.GetAllAsync().Result;
            return new JsonResult(paymentMethods.OrderBy(x => x.Name));
        }
    }
}
