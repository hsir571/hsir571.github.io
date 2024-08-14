using Microsoft.AspNetCore.Mvc;
using PortfolioTracker.Data;
using PortfolioTracker.Models;
using System.Diagnostics;


namespace PortfolioTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepo _repo;

       
        public HomeController(ILogger<HomeController> logger, IRepo repo)
        {
            _logger = logger;
            _repo = repo;
            
        }

        public async Task<IActionResult> Index()
        {
            Console.WriteLine("Hello World");
            await _repo.GetStockData("IBM"); 
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Stocks()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
