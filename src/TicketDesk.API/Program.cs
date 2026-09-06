using Microsoft.EntityFrameworkCore;
using TicketDesk.DAL;

var builder = WebApplication.CreateBuilder(args);

// Registers TicketDeskDbContext with the DI container, pointed at SQL Server
// using the connection string from appsettings.json (Day 3).
builder.Services.AddDbContext<TicketDeskDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TicketDeskConnection")));

var app = builder.Build();

// ---- Day 5: small LINQ query proving the model + seed data work end-to-end ----
// Run once at startup so you can see it in the console without writing a controller yet.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TicketDeskDbContext>();

    var roles = db.Roles
        .OrderBy(r => r.Id)
        .Select(r => r.Name)
        .ToList();

    Console.WriteLine("Seeded roles: " + string.Join(", ", roles));

    var categories = db.Categories
        .OrderBy(c => c.Id)
        .Select(c => c.Name)
        .ToList();

    Console.WriteLine("Seeded categories: " + string.Join(", ", categories));
}

app.MapGet("/", () => "TicketDesk API is running.");

app.Run();
