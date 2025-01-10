using LOLStats.Data;
using LOLStats.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LOLStats.Controllers
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

        public IActionResult IndexAsync()
        {
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Profile()
        {
            
            return View();
        }

        public async Task<IActionResult> Champions(string champion)
        {

            var allChampions = await _repo.GetAllChampions();
            ViewData["allChampions"] = allChampions;
            if (champion != null && champion != "")
            {
                var selectedChampion = await _repo.GetChampionInfo(champion);
                ViewData["selectedChampion"] = selectedChampion;
            }


			return View();
        }

        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
