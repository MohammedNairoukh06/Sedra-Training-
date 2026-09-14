namespace TicketDesk.Domain.Entities;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // Many-to-many with Ticket: a Category can tag many Tickets
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
