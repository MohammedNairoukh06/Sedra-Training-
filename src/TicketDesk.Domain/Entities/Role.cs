namespace TicketDesk.Domain.Entities;

// POCO: plain class, no EF/database code inside it.
public class Role
{
    public int Id { get; set; }                 // PK by convention (Id)
    public string Name { get; set; } = string.Empty;

    // Inverse navigation for the one-to-many Role -> Users
    public ICollection<User> Users { get; set; } = new List<User>();
}
