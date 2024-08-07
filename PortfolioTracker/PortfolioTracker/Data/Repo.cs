namespace PortfolioTracker.Data
{
    public class Repo: IRepo
    {
        private readonly ApplicationDbContext _context;

        public Repo(ApplicationDbContext context)
        {
            _context = context;
        }



        public async Task GetStocks()
        {

        }



    }
}
