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

        public IActionResult Error404()
        {
            throw new ApplicationException("Отладочное исклбчение");
            _logger.LogWarning("Запрос страницы 404");
            _logger.LogCritical("Запрос страницы 404");
            _logger.LogDebug("Запрос страницы 404");
            _logger.LogError("Запрос страницы 404");
            return View();
        }
    }
}