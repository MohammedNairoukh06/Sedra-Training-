using Microsoft.EntityFrameworkCore;
using TicketDesk.Domain.Entities;

namespace TicketDesk.DAL;

public class TicketDeskDbContext : DbContext
{
    public TicketDeskDbContext(DbContextOptions<TicketDeskDbContext> options)
        : base(options)
    {
    }

    // One DbSet per table
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<TicketComment> TicketComments => Set<TicketComment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ---- Role (1) -> Users (many) ----
        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict); // don't let deleting a Role cascade-delete Users

        // ---- User (1) -> Tickets (many) ----
        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.User)
            .WithMany(u => u.Tickets)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // ---- Ticket (1) -> Comments (many) ----
        modelBuilder.Entity<TicketComment>()
            .HasOne(c => c.Ticket)
            .WithMany(t => t.Comments)
            .HasForeignKey(c => c.TicketId)
            .OnDelete(DeleteBehavior.Cascade); // deleting a Ticket deletes its comments

        // ---- Comment (1) -> Author User ----
        // No inverse collection on User (a User doesn't need "MyComments" for this ERD),
        // so WithMany() is called with no argument — EF still creates the FK correctly.
        modelBuilder.Entity<TicketComment>()
            .HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // ---- Ticket <-> Category (many-to-many) ----
        // EF Core 5+ can do implicit many-to-many; naming the join table explicitly
        // keeps it readable and matches the Week 2 ERD's join table.
        modelBuilder.Entity<Ticket>()
            .HasMany(t => t.Categories)
            .WithMany(c => c.Tickets)
            .UsingEntity(j => j.ToTable("TicketCategories"));

        // Store the enum as its text name (e.g. "Open") instead of an int,
        // so the column is self-explanatory when you look at the table directly.
        modelBuilder.Entity<Ticket>()
            .Property(t => t.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        // ---- Day 5: seed starter data ----
        // HasData rows need fixed, hand-assigned keys (no auto-generated IDs),
        // because EF has to know the exact row to insert/compare on every migration.
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "Admin" },
            new Role { Id = 2, Name = "Agent" },
            new Role { Id = 3, Name = "Customer" }
        );

        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Hardware" },
            new Category { Id = 2, Name = "Software" },
            new Category { Id = 3, Name = "Network" },
            new Category { Id = 4, Name = "Account" }
        );
    }
}
