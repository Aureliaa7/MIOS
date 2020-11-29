using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicalInstrumentsShop.BusinessLogic.Services.Interfaces;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.Controllers
{
    public class DashboardsController : Controller
    {
        private readonly ISupplierService supplierService;
        private readonly ICategoryService categoryService;
        private readonly IDeliveryService deliveryService;
        private readonly IProductService productService;

        public DashboardsController(ISupplierService supplierService, ICategoryService categoryService,
            IDeliveryService deliveryService, IProductService productService)
        {
            this.supplierService = supplierService;
            this.categoryService = categoryService;
            this.deliveryService = deliveryService;
            this.productService = productService;
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Admin()
        {
            ViewBag.Suppliers = await supplierService.GetNoSuppliersAsync();
            ViewBag.Categories = await categoryService.GetNoCategoriesAsync();
            ViewBag.Deliveries = await deliveryService.GetNoDeliveriesAsync();
            ViewBag.Products = await productService.GetNoProductsAsync();

            return View();
        }
    }
}
