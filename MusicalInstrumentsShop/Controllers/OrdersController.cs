using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using MusicalInstrumentsShop.BusinessLogic.DTOs;
using MusicalInstrumentsShop.BusinessLogic.Exceptions;
using MusicalInstrumentsShop.BusinessLogic.ProductFiltering;
using MusicalInstrumentsShop.BusinessLogic.Services.Interfaces;
using MusicalInstrumentsShop.DataAccess.Entities;
using System;
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
        public async Task<IActionResult> Canceled(int? pageNumber)
        {
            Guid currentUserId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var orders = await orderService.GetByStatusAsync(currentUserId, OrderStatus.Canceled);
            return View(PaginatedList<OrderDetailsDto>.Create(orders, pageNumber ?? 1, 5));
        }

        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> InProgress(int? pageNumber)
        {
            Guid currentUserId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var orders = await orderService.GetByStatusAsync(currentUserId, OrderStatus.InProgress);
            return View(PaginatedList<OrderDetailsDto>.Create(orders, pageNumber ?? 1, 5));
        }

        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Completed(int? pageNumber)
        {
            Guid currentUserId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var orders = await orderService.GetByStatusAsync(currentUserId, OrderStatus.Completed);
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
                orderDetails.CustomerId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
                await orderService.AddAsync(orderDetails);
                return RedirectToAction("Index", "Orders");
            }
            return View();
        }

        [Authorize(Roles = "Customer")]
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

        [Authorize(Roles = "Customer")]
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
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Cancel(long id)
        {
            try
            {
                await orderService.UpdateStatusAsync(id, OrderStatus.Canceled);
                return RedirectToAction("Details", new { id = id });
            }
            catch (ItemNotFoundException)
            {
                return RedirectToAction("NotFound", "Error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> MarkAsCompleted(long id)
        {
            try
            {
                await orderService.UpdateStatusAsync(id, OrderStatus.Completed);
                return RedirectToAction("Details", new { id = id });
            }
            catch (ItemNotFoundException)
            {
                return RedirectToAction("NotFound", "Error");
            }
        }

        //TODO move this method in PaymentMethodsController
        public JsonResult GetPaymentMethods()
        {
            var paymentMethods = paymentService.GetAllAsync().Result;
            return new JsonResult(paymentMethods.OrderBy(x => x.Name));
        }
    }
}
