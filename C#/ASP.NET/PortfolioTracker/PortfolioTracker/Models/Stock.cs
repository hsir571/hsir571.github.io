﻿using Microsoft.EntityFrameworkCore;

namespace PortfolioTracker.Models
{
    public class Stock
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        public ICollection<StockPriceDate> PriceDate { get; set; }

        

    }
}
