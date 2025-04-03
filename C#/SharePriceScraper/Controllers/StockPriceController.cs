using Microsoft.AspNetCore.Mvc;
using SharePriceScraper.Services;
using System.Web;
using System.Linq;

namespace SharePriceScraper.Controllers
{
	public class StockPriceController : Controller
	{
		private readonly StockPriceService _stockPriceService;

		public StockPriceController(StockPriceService stockPriceService)
		{
			_stockPriceService = stockPriceService;
		}
		public async Task<IActionResult> Index()
		{
			var activeStocks = await _stockPriceService.GetActiveStockPriceAsync();
			if (activeStocks.Item1 != null) {
				var companies = activeStocks.Item1.Select(HttpUtility.HtmlDecode).ToList();
				var price = activeStocks.Item2.Select(HttpUtility.HtmlDecode).ToList();
                var priceChange = activeStocks.Item3.Select(HttpUtility.HtmlDecode).ToList(); ;


				ViewData["companies"] = companies;
				ViewData[ "activePrice"] = price;
				ViewData["activePriceChange"] = priceChange; 
			}

            var cryptoData = await _stockPriceService.GetCryptoPriceAsync();
            if (cryptoData.Item1 != null)
            {
                var crypto = cryptoData.Item1.Select(HttpUtility.HtmlDecode).ToList();
                var cryptoPrice = cryptoData.Item2.Select(HttpUtility.HtmlDecode).ToList();
                var cryptoPriceChange = cryptoData.Item3.Select(HttpUtility.HtmlDecode).ToList(); ;


                ViewData["crypto"] = crypto;
                ViewData["cryptoPrice"] = cryptoPrice;
                ViewData["cryptoPriceChange"] = cryptoPriceChange;
            }

            return View();
		}
	}
}
