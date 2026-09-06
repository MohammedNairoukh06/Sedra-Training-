namespace TicketDesk.Domain.Entities;

public class TicketComment
{
    public int Id { get; set; }
    public string Body { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // FK + navigation: which Ticket this comment belongs to
    public int TicketId { get; set; }
    public Ticket Ticket { get; set; } = null!;

    // FK + navigation: who wrote the comment (also a User, no inverse
    // collection needed on User for this one — see DbContext config)
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
