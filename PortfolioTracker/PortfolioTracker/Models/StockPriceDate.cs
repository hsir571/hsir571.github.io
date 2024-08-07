using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioTracker.Models
{
    public class StockPriceDate
    {
        public int Id { get; set; }
        [ForeignKey("Stock")]
        public int StockId { get; set; }
        public DateTime Date { get; set; }
        public double Price { get; set; }



    }
}
