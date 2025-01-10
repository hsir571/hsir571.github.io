namespace PortfolioTracker.Data
{
    public interface IRepo
    {

        Task GetStockData(string symbol); 
        Task<int> AddStock(string symbol);
        Task<List<(DateTime, double)>> GetChartData(string symbol);
        Task<List<string>> GetAllStock();
    }
}
