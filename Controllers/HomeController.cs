using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Pozdravlyator.Data;
using Pozdravlyator.Models;
using System.Diagnostics;

namespace Pozdravlyator.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PozdravlyatorContext _context;

        public HomeController(ILogger<HomeController> logger, PozdravlyatorContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            DateTime today = DateTime.UtcNow.Date;
            DateTime endDate = today.AddDays(30);
            var people = _context.Person.Where(p => p.BirthDay >= today)
                .Intersect(_context.Person.Where(p => p.BirthDay <= endDate));
            return View(await people.ToListAsync());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
