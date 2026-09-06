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
}
