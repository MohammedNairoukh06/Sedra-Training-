using Microsoft.EntityFrameworkCore;
using TicketDesk.Domain.Entities;
using TicketDesk.Domain.Enums;

namespace TicketDesk.DAL;

// Using C# 12 Primary Constructor
public class TicketDeskDbContext(DbContextOptions<TicketDeskDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<TicketComment> TicketComments => Set<TicketComment>();
    public DbSet<TicketAgent> TicketAgents => Set<TicketAgent>();
    public DbSet<TicketManager> TicketManagers => Set<TicketManager>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Ticket -> User (1:N)
        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.User)
            .WithMany(u => u.Tickets)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // TicketComment -> Ticket (1:N, Cascade)
        modelBuilder.Entity<TicketComment>()
            .HasOne(c => c.Ticket)
            .WithMany(t => t.Comments)
            .HasForeignKey(c => c.TicketId)
            .OnDelete(DeleteBehavior.Cascade);

        // TicketComment -> User (Author) (1:N, Restrict)
        modelBuilder.Entity<TicketComment>()
            .HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // User -> Role (1:N, Restrict)
        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        // User -> Agent (self-referencing, 1:N, Restrict) — Week 7 Day 1
        modelBuilder.Entity<User>()
            .HasOne(u => u.Agent)
            .WithMany(u => u.Managers)
            .HasForeignKey(u => u.AgentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Ticket <-> Category (M:N)
        modelBuilder.Entity<Ticket>()
            .HasMany(t => t.Categories)
            .WithMany(c => c.Tickets)
            .UsingEntity(j => j.ToTable("TicketCategories"));

        // Ticket <-> Agent (M:N via TicketAgent) — Week 7 Day 2
        modelBuilder.Entity<TicketAgent>()
            .HasKey(ta => new { ta.TicketId, ta.AgentId });

        modelBuilder.Entity<TicketAgent>()
            .HasOne(ta => ta.Ticket)
            .WithMany()
            .HasForeignKey(ta => ta.TicketId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TicketAgent>()
            .HasOne(ta => ta.Agent)
            .WithMany()
            .HasForeignKey(ta => ta.AgentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Ticket <-> Manager (M:N via TicketManager) — Week 7 Day 2
        modelBuilder.Entity<TicketManager>()
            .HasKey(tm => new { tm.TicketId, tm.ManagerId });

        modelBuilder.Entity<TicketManager>()
            .HasOne(tm => tm.Ticket)
            .WithMany()
            .HasForeignKey(tm => tm.TicketId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TicketManager>()
            .HasOne(tm => tm.Manager)
            .WithMany()
            .HasForeignKey(tm => tm.ManagerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Maker-Checker audit relationships — Week 7 Day 4
        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.ResolvedByUser)
            .WithMany()
            .HasForeignKey(t => t.ResolvedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.ClosedByUser)
            .WithMany()
            .HasForeignKey(t => t.ClosedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.ReturnedByUser)
            .WithMany()
            .HasForeignKey(t => t.ReturnedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Enum string conversion for Ticket Status
        modelBuilder.Entity<Ticket>()
            .Property(t => t.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        // Seed Roles
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "Admin" },
            new Role { Id = 2, Name = "Agent" },
            new Role { Id = 3, Name = "Customer" }
        );

        // Seed Categories
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Hardware" },
            new Category { Id = 2, Name = "Software" },
            new Category { Id = 3, Name = "Network" },
            new Category { Id = 4, Name = "Account" }
        );
    }
}