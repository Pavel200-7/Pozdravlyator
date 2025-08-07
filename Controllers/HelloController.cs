using Microsoft.AspNetCore.Mvc;

namespace Pozdravlyator.Controllers
{
    public class HelloController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult sayHelloSomeOne(string person = "Незнакомец")
        {
            ViewData["Person"] = person;
            return View();
        }

        
    }
}
