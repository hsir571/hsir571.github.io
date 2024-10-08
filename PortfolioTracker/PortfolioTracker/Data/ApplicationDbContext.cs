﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using PortfolioTracker.Models;

namespace PortfolioTracker.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Stock> Stocks { get; set; }
        public DbSet<StockPriceDate> StockPriceDate { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Stock>().HasIndex(s => s.Name).IsUnique();
            builder.Entity<StockPriceDate>().HasIndex(s => new { s.StockId, s.Date }).IsUnique();
            base.OnModelCreating(builder);
        }
    }

    
}
