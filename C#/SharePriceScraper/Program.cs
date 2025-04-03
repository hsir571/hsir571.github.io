using SharePriceScraper;
using SharePriceScraper.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient<StockPriceService>();
builder.Services.AddRazorPages();
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=StockPrice}/{action=Index}/{id?}"
	);

app.Run();
