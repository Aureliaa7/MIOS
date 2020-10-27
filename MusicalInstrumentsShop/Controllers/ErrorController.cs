using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MusicalInstrumentsShop.Controllers
{
    public class ErrorController : Controller
    { 
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            string viewName = "NotFound";

            switch (statusCode)
            {
                case 403:
                    viewName = "AccessDenied";
                    break;

                case 404:
                    viewName = "NotFound";
                    break;
            }
            return View(viewName);
        }

        [Route("Error")]
        [AllowAnonymous]
        public IActionResult Error()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            ViewBag.ExceptionPath = exceptionDetails.Path;
            ViewBag.ExceptionMessage = exceptionDetails.Error.Message;
            ViewBag.StackTrace = exceptionDetails.Error.StackTrace;

            return View("Error");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
