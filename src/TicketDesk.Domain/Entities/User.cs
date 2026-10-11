namespace TicketDesk.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;

    public int? AgentId { get; set; }
    public User? Agent { get; set; }
    public ICollection<User> Managers { get; set; } = new List<User>();

    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}