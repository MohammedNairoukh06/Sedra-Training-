using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TicketDesk.BL;
using TicketDesk.BL.Mapping;
using TicketDesk.DAL;
using TicketDesk.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add Controller services to the DI container
builder.Services.AddControllers();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register TicketDeskDbContext with the DI container
builder.Services.AddDbContext<TicketDeskDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("TicketDeskConnection")));

// Register the generic repository (Day 2)
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Register AutoMapper (Day 3)
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<TicketProfile>());

// Register the ticket service (Day 3)
builder.Services.AddScoped<ITicketService, TicketService>();

// Register API versioning + Swagger integration (Day 5)
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
})
.AddMvc()
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Database Migration & Startup Verification
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TicketDeskDbContext>();

    // Apply pending EF Core migrations
    db.Database.Migrate();

    // Check seeded roles
    var roles = db.Roles
        .OrderBy(r => r.Id)
        .Select(r => r.Name)
        .ToList();

    Console.WriteLine("Seeded roles: " + string.Join(", ", roles));

    // Check seeded categories
    var categories = db.Categories
        .OrderBy(c => c.Id)
        .Select(c => c.Name)
        .ToList();

    Console.WriteLine("Seeded categories: " + string.Join(", ", categories));
}

// Simple root endpoint
app.MapGet("/", () => "TicketDesk API is running.");

// Health endpoint — confirms the DbContext resolves via DI and the DB is reachable
app.MapGet("/api/health", (TicketDeskDbContext db) =>
{
    var canConnect = db.Database.CanConnect();
    return Results.Ok(new
    {
        status = canConnect ? "healthy" : "unhealthy",
        database = canConnect ? "connected" : "unreachable",
        timestamp = DateTime.UtcNow
    });
});

// TEMPORARY — lists seeded users so we can find a valid UserId for testing ticket creation
app.MapGet("/api/test-users", async (TicketDeskDbContext db) =>
    Results.Ok(await db.Users.Select(u => new { u.Id, u.FullName, u.Email, u.RoleId }).ToListAsync()));

// TEMPORARY — creates one test user using the first seeded role, so we have a valid UserId to test with
app.MapPost("/api/test-create-user", async (TicketDeskDbContext db) =>
{
    var firstRole = await db.Roles.FirstOrDefaultAsync();
    if (firstRole is null)
        return Results.BadRequest("No roles found — seed Roles first.");

    var user = new User
    {
        FullName = "Test User",
        Email = "testuser@ticketdesk.com",
        PasswordHash = "temporary-hash-for-testing",
        RoleId = firstRole.Id
    };

    db.Users.Add(user);
    await db.SaveChangesAsync();

    return Results.Ok(new { user.Id, user.FullName, user.Email, user.RoleId });
});

// Map controller endpoints
app.MapControllers();

app.Run();