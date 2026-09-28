using TicketDesk.Domain.Enums;

namespace TicketDesk.Domain.Entities;

public class Ticket
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TicketStatus Status { get; set; } = TicketStatus.Open;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // FK + navigation: the User who opened this Ticket
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    // Many-to-many with Category
    public ICollection<Category> Categories { get; set; } = new List<Category>();

    // One-to-many: a Ticket has many Comments
    public ICollection<TicketComment> Comments { get; set; } = new List<TicketComment>();

    // Maker-Checker audit trail (Week 7, Day 4)
    public int? ResolvedByUserId { get; set; }
    public User? ResolvedByUser { get; set; }
    public DateTime? ResolvedAt { get; set; }

    public int? ClosedByUserId { get; set; }
    public User? ClosedByUser { get; set; }
    public DateTime? ClosedAt { get; set; }

    public int? ReturnedByUserId { get; set; }
    public User? ReturnedByUser { get; set; }
    public DateTime? ReturnedAt { get; set; }
}