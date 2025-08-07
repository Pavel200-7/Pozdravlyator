using Microsoft.AspNetCore.Mvc;

namespace Pozdravlyator.Views
{
    public class BirthDayController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
