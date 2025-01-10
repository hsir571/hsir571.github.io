using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioTracker.Migrations
{
    /// <inheritdoc />
    public partial class uniquecomposite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StockPriceDate_StockId",
                table: "StockPriceDate");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_Name",
                table: "Stocks",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockPriceDate_StockId_Date",
                table: "StockPriceDate",
                columns: new[] { "StockId", "Date" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Stocks_Name",
                table: "Stocks");

            migrationBuilder.DropIndex(
                name: "IX_StockPriceDate_StockId_Date",
                table: "StockPriceDate");

            migrationBuilder.CreateIndex(
                name: "IX_StockPriceDate_StockId",
                table: "StockPriceDate",
                column: "StockId");
        }
    }
}
