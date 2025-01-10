
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using YourNamespace.Helper;


public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddDbContext<A2DbContext>(options =>
            options.UseSqlite(builder.Configuration["WebAPIConnection"]));
        builder.Services.AddControllers(options =>
        {
            options.OutputFormatters.Add(new CalendarOutputFormatter());
        });
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddScoped<IA2Repo, A2Repo>();
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("UserOnly",
                policy =>  policy.RequireClaim("userName"));

            options.AddPolicy("OrganizerOnly", 
                policy => policy.RequireClaim("Organizer"));



        });
        builder.Services
            .AddAuthentication()
            .AddScheme<AuthenticationSchemeOptions, A2AuthHandler>
                ("MyAuthentication", null);
            
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}