using RestSharp;
using PortfolioTracker.Models;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;

namespace PortfolioTracker.Data
{
    public class Repo: IRepo
    {
        private readonly ApplicationDbContext _context;

        public Repo(ApplicationDbContext context)
        {
            _context = context;
        }



        public async Task GetStockData(string symbol)
        {
            
            string url = "https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol=" + symbol + "&apikey=CQ27XZY2SPPLIH1D";
            var options = new RestClientOptions("https://mocki.io/v1/a870ec85-53d0-472b-ba76-7f9ccff13f00"); 
            var client = new RestClient(options);
            var request = new RestRequest(); 
            var response = await client.GetAsync(request);

            int stockId = await AddStock(symbol); 
            if (response.Content != null)
            {

                var data = JObject.Parse(response.Content);
                var timeseries = data["Time Series (Daily)"] as JObject; 
                foreach(var item in timeseries)
                {
                    var date = item.Key;
                    var prices = item.Value as JObject;
                    var closePrice = prices["4. close"]?.ToString();
                    StockPriceDate stockPriceDate = new StockPriceDate
                    {
                        Date = DateTime.Parse(date),
                        StockId = stockId,
                        Price = Convert.ToDouble(closePrice)

                    };
                    try
                    {
                        _context.StockPriceDate.Add(stockPriceDate);
                        _context.SaveChanges();
                    }
                    catch (DbUpdateException e)
                    {

                    }                
                    }
            }


        }

        public async Task<int> AddStock(string symbol)
        {
            Stock stock = new Stock { 
            Name = symbol
            };
            try
            {
                _context.Stocks.Add(stock);
                _context.SaveChanges();
            }
            catch (DbUpdateException e)
            {
            }
            Stock newStock = _context.Stocks.Where(s => s.Name.Equals(symbol)).FirstOrDefault();
            if (newStock != null)
            {
                int id = newStock.Id;
                return id;
            }
            return 0; 
        }


    }
}
