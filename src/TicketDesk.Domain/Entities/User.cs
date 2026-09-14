namespace TicketDesk.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    // FK + navigation: every User belongs to exactly one Role
    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;

    // One-to-many: a User raises many Tickets
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
