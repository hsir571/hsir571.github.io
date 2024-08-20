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
            //var options = new RestClientOptions(url);
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
                        await _context.StockPriceDate.AddAsync(stockPriceDate);
                        await _context.SaveChangesAsync();
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
                await _context.Stocks.AddAsync(stock);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
            }
            Stock newStock = await _context.Stocks.Where(s => s.Name.Equals(symbol)).FirstOrDefaultAsync();
            if (newStock != null)
            {
                int id = newStock.Id;
                return id;
            }
            return 0; 
        }
        public async Task<List<(DateTime, double)>> GetChartData(string symbol)
        {
            var values = new List<(DateTime, double)>();
            Stock stock = await _context.Stocks.Where(e => e.Name.Equals(symbol)).FirstOrDefaultAsync();
            int stockId = stock.Id; 
            List<StockPriceDate> stockPriceDates = new List<StockPriceDate>();                              
            stockPriceDates = await _context.StockPriceDate.Where(e=>e.StockId.Equals(stockId)).ToListAsync();
            foreach(var item in stockPriceDates) {
                (DateTime, double) data = (item.Date, item.Price);
                values.Add(data); 
            }

            return values;
        }

         public async Task<List<String>> GetAllStock()
        {
            List<String> result = await _context.Stocks.Select(s => s.Name).ToListAsync();
            return result;

        }

    }
}
