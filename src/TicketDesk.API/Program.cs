using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TicketDesk.BL;
using TicketDesk.BL.Mapping;
using TicketDesk.DAL;
using TicketDesk.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add Controller services to the DI container
builder.Services.AddControllers();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {your token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Register TicketDeskDbContext with the DI container
builder.Services.AddDbContext<TicketDeskDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("TicketDeskConnection")));

// Register the generic repository (Day 2, Week 4)
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Register AutoMapper (Day 3, Week 4)
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<TicketProfile>());

// Register the ticket service (Day 3, Week 4)
builder.Services.AddScoped<ITicketService, TicketService>();

// Register API versioning + Swagger integration (Day 5, Week 4)
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

// Register CORS for the React dev server (Day 2, Week 5)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactDev", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Register password hashing + JWT token generation (Day 3, Week 6)
builder.Services.AddScoped<TicketDesk.BL.PasswordService>();
builder.Services.AddScoped<TicketDesk.BL.TokenService>();

// Register JWT authentication (Day 3, Week 6)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Global exception handling middleware (Day 2, Week 6)
app.UseMiddleware<TicketDesk.API.Middleware.ExceptionHandlingMiddleware>();

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

    db.Database.Migrate();

    var roles = db.Roles.OrderBy(r => r.Id).Select(r => r.Name).ToList();
    Console.WriteLine("Seeded roles: " + string.Join(", ", roles));

    var categories = db.Categories.OrderBy(c => c.Id).Select(c => c.Name).ToList();
    Console.WriteLine("Seeded categories: " + string.Join(", ", categories));
}

app.UseCors("AllowReactDev");

// Authentication must come before Authorization, and both before MapControllers (Day 3, Week 6)
app.UseAuthentication();
app.UseAuthorization();

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

// Map controller endpoints
app.MapControllers();

app.Run();