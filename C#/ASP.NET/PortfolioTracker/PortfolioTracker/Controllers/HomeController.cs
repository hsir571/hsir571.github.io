using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PortfolioTracker.Data;
using PortfolioTracker.Models;
using SQLitePCL;
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

        public async Task<IActionResult> IndexAsync()
        {
            await _repo.GetStockData("EA"); 
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

       

        public async Task<IActionResult> Stocks(string symbol)
        {

            if (string.IsNullOrEmpty(symbol))
            {
                symbol = "IBM";
            }

            
            var chartData = await _repo.GetChartData(symbol);
            var chartDataJson = JsonConvert.SerializeObject(chartData);
            ViewData["ChartData"] = chartDataJson;


            var stocks = await _repo.GetAllStock();
            ViewData["Stocks"] = stocks;
            ViewData["SelectedSymbol"] = symbol;
            
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
