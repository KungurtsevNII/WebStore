using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //[ActionFilterAsync]
        public IActionResult Index() => View();

        public IActionResult ContactUs() => View();

        public IActionResult Checkout() => View();

        public IActionResult Blog() => View();

        public IActionResult BlogSingle() => View();

        public IActionResult ThrowException() => throw new ApplicationException("Отладочное исклбчение");

        public IActionResult NotFoundPage()
        {
            
            _logger.LogWarning("Запрос страницы 404");
            _logger.LogCritical("Запрос страницы 404");
            _logger.LogDebug("Запрос страницы 404");
            _logger.LogError("Запрос страницы 404");
            return View();
        }

        public IActionResult ErrorStatus(string id)
        {
            _logger.LogWarning("Запрос статусного кода ошибки {0}", id);

            switch (id)
            {
                default: return Content($"Статусный код ошибки { id }");
                case "404": return RedirectToAction(nameof(NotFoundPage));
            }
        }
    }
}