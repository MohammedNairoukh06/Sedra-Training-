namespace TicketDesk.Domain.Entities;

public class TicketManager
{
    public int TicketId { get; set; }
    public Ticket Ticket { get; set; } = null!;

    public int ManagerId { get; set; }
    public User Manager { get; set; } = null!;
}