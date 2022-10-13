using Microsoft.AspNetCore.Mvc;

namespace ValetAPI.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Reservations()
        {
            return View();
        }

        public IActionResult Venue()
        {
            return View();
        }

        public IActionResult Account()
        {
            return View();
        }
    }
}
