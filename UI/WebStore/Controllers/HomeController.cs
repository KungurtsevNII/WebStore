using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
            _logger.LogWarning("Запрос страницы 404");
            return View();
        }
    }
}