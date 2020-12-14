using Microsoft.AspNetCore.Mvc;
using MusicalInstrumentsShop.BusinessLogic.Services.Interfaces;
using System.Linq;

namespace MusicalInstrumentsShop.Controllers
{
    public class PaymentMethodsController : Controller
    {
        private readonly IPaymentService paymentService;

        public PaymentMethodsController(IPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }

        public JsonResult GetPaymentMethods()
        {
            var paymentMethods = paymentService.GetAllAsync().Result;
            return new JsonResult(paymentMethods.OrderBy(x => x.Name));
        }
    }
}
