using System;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Diagnostics;

namespace SharePriceScraper.Services
{
    public class StockPriceService
    {
        private readonly HttpClient _httpClient;

        public StockPriceService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<(List<string>, List<string>, List<string>)> GetCryptoPriceAsync()
        {
            string url = "https://www.google.com/finance/markets/cryptocurrencies?hl=en";
            List<string> crypto = [];
            List<string> price = [];
            List<string> priceChange = [];
            List<string> percentChange = [];
            try
            {
                var html = await _httpClient.GetStringAsync(url);
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);


                var cryptoNode = htmlDocument.DocumentNode.SelectNodes("//ul[@class='sbnBtf']//div[@class='ZvmM7']");
                var priceNode = htmlDocument.DocumentNode.SelectNodes("//ul[@class='sbnBtf']//div[@class='Bu4oXd']");
                var priceChangeNode = htmlDocument.DocumentNode.SelectNodes("//ul[@class='sbnBtf']//div[@class='SEGxAb']");
                // var percentChangeNode = htmlDocument.DocumentNode.SelectNodes("//ul[@class='sbnBtf']/div[@class='JwB6zf']");


                if (cryptoNode != null)
                {


                    for (int i = 0; i < cryptoNode.Count; i++)
                    {
                        crypto.Add(cryptoNode[i].InnerText);
                        price.Add(priceNode[i].InnerText);
                        priceChange.Add(priceChangeNode[i].InnerText);
                        //	percentChange.Add(percentChangeNode[i].InnerText);
                    }


                    return (crypto, price, priceChange);
                }
                else
                {
                    Debug.WriteLine("Stock price element not found.");
                    return (null, null, null);

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching stock price: {ex.Message}");
                return (null, null, null);
            }
        }


        public async Task<(List<string>, List<string>, List<string>)> GetActiveStockPriceAsync()
        {
            string url = "https://www.google.com/finance/markets/most-active?hl=en";
            List<string> companies = [];
            List<string> price = [];
            List<string> priceChange = [];
            List<string> percentChange = [];
            try
            {
                var html = await _httpClient.GetStringAsync(url);
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);


                var stockNode = htmlDocument.DocumentNode.SelectNodes("//ul[@class='sbnBtf']//div[@class='ZvmM7']");
                var priceNode = htmlDocument.DocumentNode.SelectNodes("//ul[@class='sbnBtf']//div[@class='Bu4oXd']");
                var priceChangeNode = htmlDocument.DocumentNode.SelectNodes("//ul[@class='sbnBtf']//div[@class='SEGxAb']");
                // var percentChangeNode = htmlDocument.DocumentNode.SelectNodes("//ul[@class='sbnBtf']/div[@class='JwB6zf']");


                if (stockNode != null)
                {


                    for (int i = 0; i < stockNode.Count; i++)
                    {
                        companies.Add(stockNode[i].InnerText);
                        price.Add(priceNode[i].InnerText);
                        priceChange.Add(priceChangeNode[i].InnerText);
                        //	percentChange.Add(percentChangeNode[i].InnerText);
                    }


                    return (companies, price, priceChange);
                }
                else
                {
                    Debug.WriteLine("Stock price element not found.");
                    return (null, null, null);

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching stock price: {ex.Message}");
                return (null, null, null);
            }
        }
    }
}
