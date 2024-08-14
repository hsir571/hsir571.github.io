namespace PortfolioTracker.Data
{
    public interface IRepo
    {

        Task GetStockData(string symbol); 
        Task<int> AddStock(string symbol);
    }
}
