namespace TicketDesk.Domain.Entities;

public class TicketAgent
{
    public int TicketId { get; set; }
    public Ticket Ticket { get; set; } = null!;

    public int AgentId { get; set; }
    public User Agent { get; set; } = null!;
}